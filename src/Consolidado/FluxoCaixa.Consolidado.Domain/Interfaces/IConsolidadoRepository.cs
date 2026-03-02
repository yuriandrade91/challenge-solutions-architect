using FluxoCaixa.Consolidado.Domain.Entities;

namespace FluxoCaixa.Consolidado.Domain.Interfaces;

public interface IConsolidadoRepository
{
    Task<ConsolidadoDiario?> GetByDataAsync(DateOnly data, CancellationToken cancellationToken = default);
    Task<IEnumerable<ConsolidadoDiario>> GetByPeriodAsync(DateOnly dataInicio, DateOnly dataFim, CancellationToken cancellationToken = default);
    Task AddAsync(ConsolidadoDiario consolidado, CancellationToken cancellationToken = default);
    Task UpdateAsync(ConsolidadoDiario consolidado, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
