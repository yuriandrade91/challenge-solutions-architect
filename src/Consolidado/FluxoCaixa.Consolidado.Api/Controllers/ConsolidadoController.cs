using FluxoCaixa.Consolidado.Application.DTOs;
using FluxoCaixa.Consolidado.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FluxoCaixa.Consolidado.Api.Controllers;

[ApiController]
[Route("api/v1/consolidado")]
public class ConsolidadoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConsolidadoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetByPeriodo(
        [FromQuery] DateOnly dataInicio,
        [FromQuery] DateOnly dataFim,
        CancellationToken cancellationToken)
    {
        var requestId = HttpContext.TraceIdentifier;
        var result = await _mediator.Send(new GetConsolidadoPorPeriodoQuery(dataInicio, dataFim), cancellationToken);
        return Ok(ApiResponse<IEnumerable<ConsolidadoResponse>>.Ok(result, new MetadataInfo(requestId, DateTime.UtcNow)));
    }

    [HttpGet("{data}")]
    public async Task<IActionResult> GetByData(DateOnly data, CancellationToken cancellationToken)
    {
        var requestId = HttpContext.TraceIdentifier;
        var result = await _mediator.Send(new GetConsolidadoByDataQuery(data), cancellationToken);
        if (result == null)
            return NotFound(ApiResponse<ConsolidadoResponse>.Fail("NOT_FOUND", $"Consolidado para a data {data} não encontrado."));
        return Ok(ApiResponse<ConsolidadoResponse>.Ok(result, new MetadataInfo(requestId, DateTime.UtcNow)));
    }
}
