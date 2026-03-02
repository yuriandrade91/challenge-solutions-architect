using FluxoCaixa.Lancamentos.Domain.Enums;
using FluxoCaixa.Shared.Events;

namespace FluxoCaixa.Lancamentos.Domain.Events;

public record LancamentoAtualizadoEvent : IntegrationEvent
{
    public Guid LancamentoId { get; init; }
    public TipoLancamento Tipo { get; init; }
    public decimal ValorAnterior { get; init; }
    public decimal ValorNovo { get; init; }
    public DateOnly DataLancamento { get; init; }
}
