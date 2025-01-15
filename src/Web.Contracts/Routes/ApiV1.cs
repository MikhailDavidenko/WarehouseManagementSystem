using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Web.Contracts.Routes;

public static class ApiV1
{
    private const string Root = "/api/v1";

    public static class Warehouse
    {
        public const string Pallets = Root + "/pallets";
        public const string Boxes = Root + "/pallets/{palletId}/boxes";

        public static string PalletsWithPagination( PaginationParams paginationParams)
            => $"{Pallets}?offset={paginationParams.Offset}&limit={paginationParams.Limit}";

        public static string PalletWithPalletId( Guid palletId )
            => $"{Pallets}/{palletId}";

        public static string BoxOnPallet( Guid palletId )
            => $"{Pallets}/{palletId}/boxes";

        public static string BoxOnPalletWithBoxId( Guid palletId, Guid boxId)
            => $"{Pallets}/{palletId}/boxes/{boxId}";
    }
}
