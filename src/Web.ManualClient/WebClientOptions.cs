namespace WarehouseManagementSystem.Web.ManualClient;

public sealed class WebClientOptions
{
    public const string OptionsKey = "ServerOptions";

    public Uri? ServerUrl { get; set; }

    public Uri RequiredServerUrl => ServerUrl
                                    ?? throw new ArgumentNullException(nameof(ServerUrl), "ServerUrl не может быть null");
}
