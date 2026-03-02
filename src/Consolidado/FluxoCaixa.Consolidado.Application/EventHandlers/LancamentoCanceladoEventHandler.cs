using FluxoCaixa.Consolidado.Application.Interfaces;
using FluxoCaixa.Consolidado.Domain.Interfaces;
using FluxoCaixa.Lancamentos.Domain.Enums;
using FluxoCaixa.Lancamentos.Domain.Events;
using Microsoft.Extensions.Logging;

namespace FluxoCaixa.Consolidado.Application.EventHandlers;

public class LancamentoCanceladoEventHandler
{
    private readonly IConsolidadoRepository _repository;
    private readonly ICacheService _cache;
    private readonly ILogger<LancamentoCanceladoEventHandler> _logger;

    public LancamentoCanceladoEventHandler(
        IConsolidadoRepository repository,
        ICacheService cache,
        ILogger<LancamentoCanceladoEventHandler> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task HandleAsync(LancamentoCanceladoEvent evt, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing LancamentoCanceladoEvent for date {Data}", evt.DataLancamento);

        var consolidado = await _repository.GetByDataAsync(evt.DataLancamento, cancellationToken);
        if (consolidado == null) return;

        if (evt.Tipo == TipoLancamento.CREDIT)
            consolidado.RemoverCredito(evt.Valor);
        else
            consolidado.RemoverDebito(evt.Valor);

        await _repository.UpdateAsync(consolidado, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var cacheKey = $"consolidado:{evt.DataLancamento:yyyy-MM-dd}";
        await _cache.RemoveAsync(cacheKey, cancellationToken);
    }
}
