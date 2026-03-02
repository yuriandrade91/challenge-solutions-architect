using FluxoCaixa.Lancamentos.Domain.Entities;

namespace FluxoCaixa.Lancamentos.Domain.Interfaces;

public interface ILancamentoRepository
{
    Task<Lancamento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Lancamento?> GetByIdempotencyKeyAsync(Guid key, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Lancamento> Items, int TotalCount)> GetByPeriodAsync(
        DateOnly? dataInicio,
        DateOnly? dataFim,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task AddAsync(Lancamento lancamento, CancellationToken cancellationToken = default);
    Task UpdateAsync(Lancamento lancamento, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
