using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using WarehouseManagementSystem.Data.Engine;
using WarehouseManagementSystem.Web.ManualClient;
using WarehouseManagementSystem.Web.ManualClient.Boxes;
using WarehouseManagementSystem.Web.ManualClient.Pallets;

namespace WarehouseManagementSystem.IntegrationTests.Infrastructure;

public sealed class ServerFixture : IAsyncLifetime
{
    private TestWebApplication? server;
    public TestWebApplication RequiredServer => server ?? throw new ArgumentNullException(nameof(server));

    private volatile bool isDisposed;

    private IServiceProvider? localServices;
    private IServiceProvider RequiredServices => localServices ?? throw new ArgumentNullException(nameof(localServices));

    public IPalletClient GetManualPalletClient() => RequiredServices.GetRequiredService<IPalletClient>();
    public IBoxClient GetManualBoxClient() => RequiredServices.GetRequiredService<IBoxClient>();

    public async Task InitializeAsync()
    {
        server = new TestWebApplication();

        try
        {
            await server.InitializeAsync();
            var host = server.Server;
            host.Should().NotBeNull();
        }
        catch
        {
            await server.DisposeAsync().ConfigureAwait(false);
            server = null;
            throw;
        }

        localServices = ConfigureLocalServices();
    }

    public async Task DisposeAsync()
    {
        if (server == null || isDisposed)
        {
            return;
        }

        isDisposed = true;
        await server.DisposeAsync().ConfigureAwait(false);
    }

    private IServiceProvider ConfigureLocalServices()
    {
        var configurationValues = new Dictionary<string, string?>()
        {
            {
                $"{WebClientOptions.OptionsKey}:{nameof(WebClientOptions.ServerUrl)}",
                RequiredServer.ClientOptions.BaseAddress.ToString()
            },
        };

        var services = new ServiceCollection();

        // Перенаправляем все запросы HttpClient'ов к тестируемому серверу на TestServer
        services.TryAddSingleton(RequiredServer.Server);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, TestServerMessageFilter>());

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationValues)
            .Build();
        services.AddTransient<IConfiguration>(_ => configuration);

        services.AddManualClient();

        return services.BuildServiceProvider();
    }
}
