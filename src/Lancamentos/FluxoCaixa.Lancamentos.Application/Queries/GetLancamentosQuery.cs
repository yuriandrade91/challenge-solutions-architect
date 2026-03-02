using FluxoCaixa.Lancamentos.Application.DTOs;
using FluxoCaixa.Lancamentos.Domain.Interfaces;
using MediatR;

namespace FluxoCaixa.Lancamentos.Application.Queries;

public record GetLancamentosQuery(
    DateOnly? DataInicio,
    DateOnly? DataFim,
    int Page = 1,
    int PageSize = 20) : IRequest<(IEnumerable<LancamentoResponse> Items, int TotalCount)>;

public class GetLancamentosQueryHandler : IRequestHandler<GetLancamentosQuery, (IEnumerable<LancamentoResponse> Items, int TotalCount)>
{
    private readonly ILancamentoRepository _repository;

    public GetLancamentosQueryHandler(ILancamentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<(IEnumerable<LancamentoResponse> Items, int TotalCount)> Handle(
        GetLancamentosQuery query, CancellationToken cancellationToken)
    {
        var (items, total) = await _repository.GetByPeriodAsync(
            query.DataInicio, query.DataFim, query.Page, query.PageSize, cancellationToken);

        var responses = items.Select(l => new LancamentoResponse(
            l.Id, l.IdempotencyKey, l.Tipo, l.Valor, l.Descricao, l.Categoria,
            l.DataLancamento, l.Status, l.CreatedAt, l.UpdatedAt));

        return (responses, total);
    }
}
