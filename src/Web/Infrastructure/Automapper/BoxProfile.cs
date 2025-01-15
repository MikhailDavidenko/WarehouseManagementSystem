using AutoMapper;
using WarehouseManagementSystem.Business.Boxes;
using WarehouseManagementSystem.Domain;
using WarehouseManagementSystem.Web.Contracts.Boxes;

namespace WarehouseManagementSystem.Web.Infrastructure.Automapper;

internal sealed class BoxProfile : Profile
{
    public BoxProfile()
    {
        CreateMap<Box, BoxResponse>();

        CreateMap<BoxRequest, CreateBoxCommand>()
            .ConvertUsing((source, target) =>
            new CreateBoxCommand(
                source.Width ?? throw new ArgumentNullException(),
                source.Height ?? throw new ArgumentNullException(),
                source.Depth ?? throw new ArgumentNullException(),
                source.Weight ?? throw new ArgumentNullException(),
                source.ProductionDate?.ToUniversalTime(),
                source.ExpirationDate?.ToUniversalTime()));
        CreateMap<BoxRequest, UpdateBoxCommand>()
            .ConvertUsing((source, target) =>
                new UpdateBoxCommand(
                    source.Width ?? throw new ArgumentNullException(),
                    source.Height ?? throw new ArgumentNullException(),
                    source.Depth ?? throw new ArgumentNullException(),
                    source.Weight ?? throw new ArgumentNullException(),
                    source.ProductionDate?.ToUniversalTime(),
                    source.ExpirationDate?.ToUniversalTime()));
    }
}
