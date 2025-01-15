using System.Net;
using FluentAssertions;
using FluentAssertions.Execution;
using WarehouseManagementSystem.IntegrationTests.Infrastructure;
using WarehouseManagementSystem.Web.Contracts;
using WarehouseManagementSystem.Web.Contracts.Pallets;
using WarehouseManagementSystem.Web.ManualClient.Pallets;

namespace WarehouseManagementSystem.IntegrationTests;

[Collection(ServerCollection.Name)]
public sealed class PalletTest
{
    private readonly IPalletClient palletClient;
    private const int MaxPalletsForPagination = 100;

    public PalletTest(ServerFixture serverFixture)
    {
        palletClient = serverFixture.GetManualPalletClient();
    }

    private static IReadOnlyList<int> PaginationValues()
        =>
        [
            0,
            1,
            Random.Shared.Next(MaxPalletsForPagination)
        ];

    [Fact]
    public async Task CreateAndGetPalletByIdAsync_ResponseShouldBeEqualToRequest()
    {
        // Arrange
        var palletRequest = SampleStorageItemRequestBuilder.BuildPalletRequest();

        // Act
        var pallet = await palletClient
            .CreatePalletAsync(palletRequest);
        pallet.Should().NotBeNull();

        var palletResponse = await palletClient
            .GetPalletByIdAsync(pallet.Id);

        // Assert
        using (new AssertionScope())
        {
            palletResponse.Should().NotBeNull();
            ToRequest(palletResponse!).Should().BeEquivalentTo(palletRequest);
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task CreateAndGetPalletsWithPagination_ShouldSamePalletsCount(
        [CombinatorialMemberData(nameof(PaginationValues))]
        int limit,
        [CombinatorialMemberData(nameof(PaginationValues))]
        int offset
        )
    {
        // Arrange
        var palletRequest = SampleStorageItemRequestBuilder.BuildPalletRequest();
        var paginationParams = new PaginationParams( offset,  limit );

        // Act
        List<Task<PalletResponse?>> tasks = new List<Task<PalletResponse?>>(limit + offset);
        for (var i = 0; i < limit + offset; i++)
        {
            tasks.Add(palletClient.CreatePalletAsync(palletRequest));
        }
        await Task.WhenAll(tasks);

        var palletResponse = await palletClient.GetPalletsAsync(paginationParams);

        // Assert
        palletResponse.Count.Should().Be(paginationParams.Limit);
    }

    [Fact]
    public async Task UpdateAndGetPalletAsync_GetShouldBeEqualToUpdatedPallet()
    {
        // Arrange
        var palletRequest = SampleStorageItemRequestBuilder.BuildPalletRequest();
        var createdPalletResponse = await palletClient.CreatePalletAsync(palletRequest);
        createdPalletResponse.Should().NotBeNull();
        createdPalletResponse.Width = 10;

        // Act
        await palletClient.UpdatePalletAsync(createdPalletResponse.Id, ToRequest(createdPalletResponse));
        var updatedPalletResponse = await palletClient.GetPalletByIdAsync(createdPalletResponse.Id);

        // Assert
        using (new AssertionScope())
        {
            updatedPalletResponse.Should().NotBeNull();
            updatedPalletResponse.Should().BeEquivalentTo(createdPalletResponse);
        }
    }

    [Fact]
    public async Task DeleteAndGetPalletAsync_ShouldBeNotFound()
    {
        // Arrange
        var palletRequest = SampleStorageItemRequestBuilder.BuildPalletRequest();
        var createdPalletResponse = await palletClient.CreatePalletAsync(palletRequest);
        createdPalletResponse.Should().NotBeNull();

        // Act
        await palletClient.DeletePalletAsync(createdPalletResponse.Id);
        Func<Task> act = () => palletClient.GetPalletByIdAsync(createdPalletResponse.Id);

        // Assert
        await act.Should().ThrowExactlyAsync<HttpRequestException>();
        var exception = await act.Should().ThrowAsync<HttpRequestException>();
        exception.And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static PalletRequest ToRequest(PalletResponse pallet)
        => new PalletRequest() { Depth = pallet.Depth, Height = pallet.Height, Width = pallet.Width };
}
