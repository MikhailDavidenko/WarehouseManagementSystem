using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Business.Pallets;
using WarehouseManagementSystem.Web.Contracts;
using WarehouseManagementSystem.Web.Contracts.Pallets;
using WarehouseManagementSystem.Web.Contracts.Routes;

namespace WarehouseManagementSystem.Web.Pallets;

[Route(ApiV1.Warehouse.Pallets)]
[ApiController]
 public sealed class PalletController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IValidator<PalletRequest> requestValidator;
    private readonly IPalletService palletsService;

    public PalletController(
        IPalletService palletsService,
        IMapper mapper,
        IValidator<PalletRequest> requestValidator)
    {
        this.palletsService = palletsService;
        this.mapper = mapper;
        this.requestValidator = requestValidator;
    }

    [ProducesResponseType<PalletResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [HttpGet("{palletId}")]
    public async Task<IActionResult> GetPallet(
        [FromRoute] string palletId,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(palletId, out var palletIdGuid))
        {
            return BadRequest($"Id {palletId} не корректен");
        }

        var pallet = await palletsService.GetPalletAsync(palletIdGuid, cancellationToken);

        var response = mapper.Map<PalletResponse>(pallet);

        return Ok(response);
    }

    [ProducesResponseType<PalletResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<IActionResult> GetPallets(
        [FromQuery] PaginationParams paginationParams,
        CancellationToken cancellationToken = default)
    {
        var pallet = await palletsService.GetPalletsWithPaginationAsync
            (paginationParams.Limit ?? 10,
            paginationParams.Offset ?? 0,
            cancellationToken);

        var response = mapper.Map<IReadOnlyList<PalletResponse>>(pallet);

        return Ok(response);
    }

    [ProducesResponseType<PalletResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<IActionResult> CreatePallet(
        [FromBody] PalletRequest palletRequest,
        CancellationToken cancellationToken = default)
    {
        await requestValidator.ValidateAndThrowAsync(palletRequest, cancellationToken);

        var palletCommand = mapper.Map<CreatePalletCommand>(palletRequest);
        var pallet = await palletsService.CreatePalletAsync(palletCommand, cancellationToken);

        var palletResponse = mapper.Map<PalletResponse>(pallet);

        return Created($"{ApiV1.Warehouse.Pallets}/{palletResponse.Id}", palletResponse);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [HttpPut("{palletId}")]
    public async Task<IActionResult> EditPallet(
        [FromRoute] string palletId,
        [FromBody] PalletRequest palletRequest,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(palletId, out var palletIdGuid))
        {
            return BadRequest($"Id {palletId} не корректен");
        }
        await requestValidator.ValidateAndThrowAsync(palletRequest, cancellationToken);

        var palletCommand = mapper.Map<UpdatePalletCommand>(palletRequest);

        await palletsService.UpdatePalletAsync(palletIdGuid, palletCommand, cancellationToken);

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [HttpDelete("{palletId}")]
    public async Task<IActionResult> DeletePallet(
        [FromRoute] string palletId,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(palletId, out var palletIdGuid))
        {
            return BadRequest($"Id {palletId} не корректен");
        }

        await palletsService.DeletePalletAsync(palletIdGuid, cancellationToken);

        return NoContent();
    }
}
