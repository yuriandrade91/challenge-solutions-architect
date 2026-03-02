using FluxoCaixa.Consolidado.Domain.Entities;
using FluxoCaixa.Consolidado.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FluxoCaixa.Consolidado.Infrastructure.Persistence;

public class ConsolidadoRepository : IConsolidadoRepository
{
    private readonly ConsolidadoDbContext _context;

    public ConsolidadoRepository(ConsolidadoDbContext context)
    {
        _context = context;
    }

    public async Task<ConsolidadoDiario?> GetByDataAsync(DateOnly data, CancellationToken cancellationToken = default)
        => await _context.ConsolidadoDiario.FirstOrDefaultAsync(c => c.Data == data, cancellationToken);

    public async Task<IEnumerable<ConsolidadoDiario>> GetByPeriodAsync(
        DateOnly dataInicio, DateOnly dataFim, CancellationToken cancellationToken = default)
        => await _context.ConsolidadoDiario
            .Where(c => c.Data >= dataInicio && c.Data <= dataFim)
            .OrderByDescending(c => c.Data)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(ConsolidadoDiario consolidado, CancellationToken cancellationToken = default)
        => await _context.ConsolidadoDiario.AddAsync(consolidado, cancellationToken);

    public Task UpdateAsync(ConsolidadoDiario consolidado, CancellationToken cancellationToken = default)
    {
        _context.ConsolidadoDiario.Update(consolidado);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
