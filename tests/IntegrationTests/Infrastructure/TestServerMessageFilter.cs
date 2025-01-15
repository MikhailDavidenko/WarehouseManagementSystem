using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Http;

namespace WarehouseManagementSystem.IntegrationTests.Infrastructure;


// Взято из ресурсов/опсхаба, чтобы не подменять HttpClient в сервисах клиентов

/// <summary>
/// Фильтр проверяет все запросы HttpClient'ов с целью перенаправить нужные на TestServer
/// </summary>
internal sealed class TestServerMessageFilter : IHttpMessageHandlerBuilderFilter
{
    private readonly TestServer server;

    public TestServerMessageFilter(TestServer server)
    {
        this.server = server;
    }

    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        return builder =>
        {
            // Сначала добавим другие обработчики
            next(builder);

            // Мы должны быть в конце
            var serverHandler = new TestServerHandler(server);
            builder.AdditionalHandlers.Add(serverHandler);
        };
    }

    private sealed class TestServerHandler : DelegatingHandler
    {
        private readonly string serverAuthority;
        private readonly HttpMessageHandlerInvoker serverHandler;

        public TestServerHandler(TestServer server)
        {
            serverAuthority = GetFullAuthority(server.BaseAddress) ?? throw new ArgumentNullException();
            var testServerHandler = server.CreateHandler();
            // HACK: We can't use the direct HttpMessageHandler type because the sending methods are internal
            serverHandler = new HttpMessageHandlerInvoker(testServerHandler);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (IsRequestToTestServer(request))
            {
                // Перенаправляем на специальный обработчик-заглушку
                return serverHandler.ExecuteAsync(request, cancellationToken);
            }

            // Отправляем в настоящую сеть
            return base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Если запрос направляется к тестовому серверу
        /// </summary>
        private bool IsRequestToTestServer(HttpRequestMessage request)
        {
            return GetFullAuthority(request.RequestUri) == serverAuthority;
        }

        private static string? GetFullAuthority(Uri? uri) => uri?.GetLeftPart(UriPartial.Authority);
    }

    /// <summary>
    /// Класс-обёртка, открывающая методы <see cref="HttpMessageHandler"/> для всеобщего использования.
    /// </summary>
    /// <remarks>
    /// Ибо оригинальные методы internal и мы не можем просто так их использовать.
    /// Design Pattern: Павлик Морозов
    /// </remarks>
    private sealed class HttpMessageHandlerInvoker : DelegatingHandler
    {
        public HttpMessageHandlerInvoker(HttpMessageHandler messageHandler)
        {
            InnerHandler = messageHandler;
        }

        public Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage request, CancellationToken cancellationToken) => SendAsync(request, cancellationToken);
    }
}
