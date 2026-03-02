using FluxoCaixa.Lancamentos.Domain.Entities;
using FluxoCaixa.Lancamentos.Domain.Enums;
using FluxoCaixa.Lancamentos.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FluxoCaixa.Lancamentos.Infrastructure.Persistence;

public class LancamentoRepository : ILancamentoRepository
{
    private readonly LancamentosDbContext _context;

    public LancamentoRepository(LancamentosDbContext context)
    {
        _context = context;
    }

    public async Task<Lancamento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Lancamentos
            .Where(l => l.Status != StatusLancamento.CANCELLED || l.DeletedAt == null)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

    public async Task<Lancamento?> GetByIdempotencyKeyAsync(Guid key, CancellationToken cancellationToken = default)
        => await _context.Lancamentos.FirstOrDefaultAsync(l => l.IdempotencyKey == key, cancellationToken);

    public async Task<(IEnumerable<Lancamento> Items, int TotalCount)> GetByPeriodAsync(
        DateOnly? dataInicio, DateOnly? dataFim, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Lancamentos
            .Where(l => l.Status == StatusLancamento.ACTIVE);

        if (dataInicio.HasValue)
            query = query.Where(l => l.DataLancamento >= dataInicio.Value);
        if (dataFim.HasValue)
            query = query.Where(l => l.DataLancamento <= dataFim.Value);

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(l => l.DataLancamento)
            .ThenByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public async Task AddAsync(Lancamento lancamento, CancellationToken cancellationToken = default)
        => await _context.Lancamentos.AddAsync(lancamento, cancellationToken);

    public Task UpdateAsync(Lancamento lancamento, CancellationToken cancellationToken = default)
    {
        _context.Lancamentos.Update(lancamento);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
