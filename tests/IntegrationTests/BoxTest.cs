using System.Net;
using FluentAssertions;
using FluentAssertions.Execution;
using WarehouseManagementSystem.IntegrationTests.Infrastructure;
using WarehouseManagementSystem.Web.Contracts.Boxes;
using WarehouseManagementSystem.Web.ManualClient.Boxes;
using WarehouseManagementSystem.Web.ManualClient.Pallets;

namespace WarehouseManagementSystem.IntegrationTests;

[Collection(ServerCollection.Name)]
public sealed class BoxTest
{
    private readonly IBoxClient boxClient;
    private readonly IPalletClient palletClient;

    public BoxTest(ServerFixture serverFixture)
    {
        boxClient = serverFixture.GetManualBoxClient();
        palletClient = serverFixture.GetManualPalletClient();
    }

    [Fact]
    public async Task CreateBoxOnPalletAsync_ResponseShouldBeEqualToRequest()
    {
        // Arrange
        var palletRequest = SampleStorageItemRequestBuilder.BuildPalletRequest();
        var boxRequest = SampleStorageItemRequestBuilder.BuildBoxRequest();
        var createdPalletResponse = await palletClient.CreatePalletAsync(palletRequest);
        createdPalletResponse.Should().NotBeNull();

        // Act
        var boxResponse = await boxClient.CreateBoxOnPalletAsync(createdPalletResponse.Id, boxRequest);

        // Assert
        using (new AssertionScope())
        {
            boxResponse.Should().NotBeNull();
            ToRequest(boxResponse).Should().BeEquivalentTo(boxRequest);
        }
    }

    [Fact]
    public async Task CreateBoxesOnPalletAndGetAsync_ShouldSameBoxesCountOnPallet()
    {
        // Arrange
        var palletRequest = SampleStorageItemRequestBuilder.BuildPalletRequest();
        var boxRequest = SampleStorageItemRequestBuilder.BuildBoxRequest();
        var createdPalletResponse = await palletClient.CreatePalletAsync(palletRequest);
        createdPalletResponse.Should().NotBeNull();

        // Act
        var tasks = new[]
        {
            boxClient.CreateBoxOnPalletAsync(createdPalletResponse.Id, boxRequest),
            boxClient.CreateBoxOnPalletAsync(createdPalletResponse.Id, boxRequest),
            boxClient.CreateBoxOnPalletAsync(createdPalletResponse.Id, boxRequest)
        };

        await Task.WhenAll(tasks);
        var palletResponse = await palletClient.GetPalletByIdAsync(createdPalletResponse.Id);

        // Assert
        using (new AssertionScope())
        {
            palletResponse.Should().NotBeNull();
            palletResponse.Boxes.Count.Should().Be(3);
        }
    }

    [Fact]
    public async Task UpdateBoxOnPalletAndGetAsync_GetShouldBeEqualToUpdatedBox()
    {
        // Arrange
        var palletRequest = SampleStorageItemRequestBuilder.BuildPalletRequest();
        var boxRequest = SampleStorageItemRequestBuilder.BuildBoxRequest();
        var createdPalletResponse = await palletClient.CreatePalletAsync(palletRequest);
        createdPalletResponse.Should().NotBeNull();

        // Act
        var createdBoxResponse = await boxClient.CreateBoxOnPalletAsync(createdPalletResponse.Id, boxRequest);
        createdBoxResponse.Weight = 10;
        await boxClient.UpdateBoxOnPalletAsync(createdPalletResponse.Id, createdBoxResponse.Id, ToRequest(createdBoxResponse));
        var palletResponse = await palletClient.GetPalletByIdAsync(createdPalletResponse.Id);

        // Assert
        using (new AssertionScope())
        {
            palletResponse.Should().NotBeNull();
            palletResponse.Boxes.Count.Should().Be(1);
            palletResponse.Boxes[0].Should().BeEquivalentTo(createdBoxResponse);
        }
    }

    [Fact]
    public async Task DeleteAndGetPalletAsync_GetShouldBeNull()
    {
        // Arrange
        var palletRequest = SampleStorageItemRequestBuilder.BuildPalletRequest();
        var boxRequest = SampleStorageItemRequestBuilder.BuildBoxRequest();
        var createdPalletResponse = await palletClient.CreatePalletAsync(palletRequest);
        createdPalletResponse.Should().NotBeNull();
        var createdBoxResponse = await boxClient.CreateBoxOnPalletAsync(createdPalletResponse.Id, boxRequest);

        // Act
        await boxClient.DeleteBoxOnPalletAsync(createdPalletResponse.Id, createdBoxResponse.Id);
        var palletResponse = await palletClient.GetPalletByIdAsync(createdPalletResponse.Id);

        // Assert
        palletResponse.Should().NotBeNull();
        palletResponse.Boxes.Count.Should().Be(0);
    }

    private static BoxRequest ToRequest(BoxResponse box)
        => new BoxRequest() {
            Depth = box.Depth,
            Height = box.Height,
            Width = box.Width,
            Weight = box.Weight,
            ExpirationDate = box.ExpirationDate,
            ProductionDate = box.ProductionDate
        };
}
