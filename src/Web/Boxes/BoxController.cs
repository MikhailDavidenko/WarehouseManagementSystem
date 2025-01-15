using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Business.Boxes;
using WarehouseManagementSystem.Domain;
using WarehouseManagementSystem.Web.Contracts.Boxes;
using WarehouseManagementSystem.Web.Contracts.Routes;

namespace WarehouseManagementSystem.Web.Boxes;

[Route(ApiV1.Warehouse.Boxes)]
[ApiController]
public sealed class BoxController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IValidator<BoxRequest> requestValidator;
    private readonly IBoxService boxService;

    public BoxController(IBoxService boxService,
        IMapper mapper,
        IValidator<BoxRequest> requestValidator)
    {
        this.boxService = boxService;
        this.mapper = mapper;
        this.requestValidator = requestValidator;
    }

    [ProducesResponseType<BoxResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<IActionResult> AddBoxToPallet(
        [FromRoute] string palletId,
        [FromBody] BoxRequest boxRequest,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(palletId, out var palletIdGuid))
        {
            return BadRequest($"Id {palletId} не корректен");
        }

        await requestValidator.ValidateAndThrowAsync(boxRequest, cancellationToken);

        var boxCommand = mapper.Map<CreateBoxCommand>(boxRequest);

        var box = await boxService.AddBoxToPalletAsync(palletIdGuid, boxCommand, cancellationToken);

        var boxResponse = mapper.Map<BoxResponse>(box);

        return Created($"{ApiV1.Warehouse.Boxes}/{boxResponse.Id}", boxResponse);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [HttpPut("{boxId}")]
    public async Task<IActionResult> UpdateBoxInPallet(
        [FromRoute] string palletId,
        [FromRoute] string boxId,
        [FromBody] BoxRequest boxRequest,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(palletId, out var palletIdGuid) || !Guid.TryParse(boxId, out var boxIdGuid))
        {
            return BadRequest($"Id {palletId} или {boxId} не корректны");
        }

        await requestValidator.ValidateAndThrowAsync(boxRequest, cancellationToken);

        var boxCommand = mapper.Map<UpdateBoxCommand>(boxRequest);

        await boxService.UpdateOnPalletAsync(palletIdGuid, boxIdGuid, boxCommand, cancellationToken);

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [HttpDelete("{boxId}")]
    public async Task<IActionResult> DeleteBox(
        [FromRoute] string boxId,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(boxId, out var boxIdGuid))
        {
            return BadRequest($"Id {boxId} не корректен");
        }

        await boxService.DeleteBoxAsync(boxIdGuid, cancellationToken);

        return NoContent();
    }
}
