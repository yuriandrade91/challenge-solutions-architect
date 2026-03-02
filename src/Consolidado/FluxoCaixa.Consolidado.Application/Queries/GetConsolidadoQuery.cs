using FluxoCaixa.Consolidado.Application.DTOs;
using FluxoCaixa.Consolidado.Application.Interfaces;
using FluxoCaixa.Consolidado.Domain.Interfaces;
using MediatR;

namespace FluxoCaixa.Consolidado.Application.Queries;

public record GetConsolidadoByDataQuery(DateOnly Data) : IRequest<ConsolidadoResponse?>;

public class GetConsolidadoByDataQueryHandler : IRequestHandler<GetConsolidadoByDataQuery, ConsolidadoResponse?>
{
    private readonly IConsolidadoRepository _repository;
    private readonly ICacheService _cache;

    public GetConsolidadoByDataQueryHandler(IConsolidadoRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<ConsolidadoResponse?> Handle(GetConsolidadoByDataQuery query, CancellationToken cancellationToken)
    {
        var cacheKey = $"consolidado:{query.Data:yyyy-MM-dd}";
        var cached = await _cache.GetAsync<ConsolidadoResponse>(cacheKey, cancellationToken);
        if (cached != null) return cached;

        var consolidado = await _repository.GetByDataAsync(query.Data, cancellationToken);
        if (consolidado == null) return null;

        var response = new ConsolidadoResponse(
            consolidado.Id, consolidado.Data, consolidado.TotalCreditos,
            consolidado.TotalDebitos, consolidado.Saldo, consolidado.QuantidadeLancamentos,
            consolidado.UpdatedAt);

        await _cache.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);
        return response;
    }
}

public record GetConsolidadoPorPeriodoQuery(DateOnly DataInicio, DateOnly DataFim) : IRequest<IEnumerable<ConsolidadoResponse>>;

public class GetConsolidadoPorPeriodoQueryHandler : IRequestHandler<GetConsolidadoPorPeriodoQuery, IEnumerable<ConsolidadoResponse>>
{
    private readonly IConsolidadoRepository _repository;

    public GetConsolidadoPorPeriodoQueryHandler(IConsolidadoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ConsolidadoResponse>> Handle(GetConsolidadoPorPeriodoQuery query, CancellationToken cancellationToken)
    {
        var consolidados = await _repository.GetByPeriodAsync(query.DataInicio, query.DataFim, cancellationToken);
        return consolidados.Select(c => new ConsolidadoResponse(
            c.Id, c.Data, c.TotalCreditos, c.TotalDebitos, c.Saldo, c.QuantidadeLancamentos, c.UpdatedAt));
    }
}
