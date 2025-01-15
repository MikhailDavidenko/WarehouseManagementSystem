using Xunit;

namespace WarehouseManagementSystem.IntegrationTests.Infrastructure;

[CollectionDefinition(Name)]
public sealed class ServerCollection : ICollectionFixture<ServerFixture>
{
    public const string Name = "Integration tests collection";
}
