using FluxoCaixa.Lancamentos.Application.DTOs;
using FluxoCaixa.Lancamentos.Domain.Exceptions;
using FluxoCaixa.Lancamentos.Domain.Interfaces;
using MediatR;

namespace FluxoCaixa.Lancamentos.Application.Queries;

public record GetLancamentoByIdQuery(Guid Id) : IRequest<LancamentoResponse>;

public class GetLancamentoByIdQueryHandler : IRequestHandler<GetLancamentoByIdQuery, LancamentoResponse>
{
    private readonly ILancamentoRepository _repository;

    public GetLancamentoByIdQueryHandler(ILancamentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<LancamentoResponse> Handle(GetLancamentoByIdQuery query, CancellationToken cancellationToken)
    {
        var lancamento = await _repository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new DomainException("NOT_FOUND", $"Lançamento {query.Id} não encontrado.");

        return new LancamentoResponse(
            lancamento.Id, lancamento.IdempotencyKey, lancamento.Tipo, lancamento.Valor,
            lancamento.Descricao, lancamento.Categoria, lancamento.DataLancamento,
            lancamento.Status, lancamento.CreatedAt, lancamento.UpdatedAt);
    }
}
