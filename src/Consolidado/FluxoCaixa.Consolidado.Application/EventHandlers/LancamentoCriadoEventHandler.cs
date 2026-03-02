using FluxoCaixa.Consolidado.Application.Interfaces;
using FluxoCaixa.Consolidado.Domain.Entities;
using FluxoCaixa.Consolidado.Domain.Interfaces;
using FluxoCaixa.Lancamentos.Domain.Enums;
using FluxoCaixa.Lancamentos.Domain.Events;
using Microsoft.Extensions.Logging;

namespace FluxoCaixa.Consolidado.Application.EventHandlers;

public class LancamentoCriadoEventHandler
{
    private readonly IConsolidadoRepository _repository;
    private readonly ICacheService _cache;
    private readonly ILogger<LancamentoCriadoEventHandler> _logger;

    public LancamentoCriadoEventHandler(
        IConsolidadoRepository repository,
        ICacheService cache,
        ILogger<LancamentoCriadoEventHandler> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task HandleAsync(LancamentoCriadoEvent evt, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing LancamentoCriadoEvent for date {Data}", evt.DataLancamento);

        var consolidado = await _repository.GetByDataAsync(evt.DataLancamento, cancellationToken);
        if (consolidado == null)
        {
            consolidado = ConsolidadoDiario.Criar(evt.DataLancamento);
            await _repository.AddAsync(consolidado, cancellationToken);
        }

        if (evt.Tipo == TipoLancamento.CREDIT)
            consolidado.AdicionarCredito(evt.Valor);
        else
            consolidado.AdicionarDebito(evt.Valor);

        await _repository.UpdateAsync(consolidado, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var cacheKey = $"consolidado:{evt.DataLancamento:yyyy-MM-dd}";
        await _cache.RemoveAsync(cacheKey, cancellationToken);
    }
}
