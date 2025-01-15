using WarehouseManagementSystem.Web.Contracts.Boxes;
using WarehouseManagementSystem.Web.Contracts.Pallets;

namespace WarehouseManagementSystem.IntegrationTests.Infrastructure;

public static class SampleStorageItemRequestBuilder
{
    public static BoxRequest BuildBoxRequest()
    {
        return new BoxRequest()
        {
            Width = 10,
            Height = 12,
            Depth = 3,
            Weight = 12,
            ExpirationDate = DateTime.Now.AddDays(5).ToUniversalTime().Date,
        };
    }

    public static PalletRequest BuildPalletRequest()
    {
        return new PalletRequest()
        {
            Width = 23,
            Height = 23,
            Depth = 22,
        };
    }
}
