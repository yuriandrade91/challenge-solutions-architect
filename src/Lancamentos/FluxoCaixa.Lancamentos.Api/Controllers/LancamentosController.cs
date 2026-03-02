using FluxoCaixa.Lancamentos.Application.Commands;
using FluxoCaixa.Lancamentos.Application.DTOs;
using FluxoCaixa.Lancamentos.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FluxoCaixa.Lancamentos.Api.Controllers;

[ApiController]
[Route("api/v1/lancamentos")]
public class LancamentosController : ControllerBase
{
    private readonly IMediator _mediator;

    public LancamentosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarLancamentoRequest request, CancellationToken cancellationToken)
    {
        var requestId = HttpContext.TraceIdentifier;
        var result = await _mediator.Send(new CriarLancamentoCommand(request), cancellationToken);
        var response = ApiResponse<LancamentoResponse>.Ok(result, metadata: new MetadataInfo(requestId, DateTime.UtcNow));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
    }

    [HttpGet]
    public async Task<IActionResult> Listar(
        [FromQuery] DateOnly? dataInicio,
        [FromQuery] DateOnly? dataFim,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var requestId = HttpContext.TraceIdentifier;
        var (items, total) = await _mediator.Send(new GetLancamentosQuery(dataInicio, dataFim, page, pageSize), cancellationToken);
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);
        var pagination = new PaginationInfo(page, pageSize, total, totalPages);
        var response = ApiResponse<IEnumerable<LancamentoResponse>>.Ok(items, pagination, new MetadataInfo(requestId, DateTime.UtcNow));
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var requestId = HttpContext.TraceIdentifier;
        var result = await _mediator.Send(new GetLancamentoByIdQuery(id), cancellationToken);
        return Ok(ApiResponse<LancamentoResponse>.Ok(result, metadata: new MetadataInfo(requestId, DateTime.UtcNow)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarLancamentoRequest request, CancellationToken cancellationToken)
    {
        var requestId = HttpContext.TraceIdentifier;
        var result = await _mediator.Send(new AtualizarLancamentoCommand(id, request), cancellationToken);
        return Ok(ApiResponse<LancamentoResponse>.Ok(result, metadata: new MetadataInfo(requestId, DateTime.UtcNow)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Cancelar(Guid id, CancellationToken cancellationToken)
    {
        var requestId = HttpContext.TraceIdentifier;
        await _mediator.Send(new CancelarLancamentoCommand(id), cancellationToken);
        return Ok(ApiResponse<bool>.Ok(true, metadata: new MetadataInfo(requestId, DateTime.UtcNow)));
    }
}
