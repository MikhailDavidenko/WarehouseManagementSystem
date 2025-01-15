using AutoMapper;
using WarehouseManagementSystem.Business.Pallets;
using WarehouseManagementSystem.Domain;
using WarehouseManagementSystem.Web.Contracts.Pallets;

namespace WarehouseManagementSystem.Web.Infrastructure.Automapper;

internal sealed class PalletProfile : Profile
{
    public PalletProfile()
    {
        CreateMap<Pallet, PalletResponse>();

        CreateMap<PalletRequest, CreatePalletCommand>()
            .ConvertUsing((source, target) =>
                new CreatePalletCommand(
                    source.Width ?? throw new ArgumentNullException(),
                    source.Height ?? throw new ArgumentNullException(),
                    source.Depth ?? throw new ArgumentNullException()));

        CreateMap<PalletRequest, UpdatePalletCommand>()
            .ConvertUsing((source, target) =>
                new UpdatePalletCommand(
                    source.Width ?? throw new ArgumentNullException(),
                    source.Height ?? throw new ArgumentNullException(),
                    source.Depth ?? throw new ArgumentNullException()));
    }
}
