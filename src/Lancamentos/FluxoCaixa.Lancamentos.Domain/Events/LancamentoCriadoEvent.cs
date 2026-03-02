using FluxoCaixa.Lancamentos.Domain.Enums;
using FluxoCaixa.Shared.Events;

namespace FluxoCaixa.Lancamentos.Domain.Events;

public record LancamentoCriadoEvent : IntegrationEvent
{
    public Guid LancamentoId { get; init; }
    public TipoLancamento Tipo { get; init; }
    public decimal Valor { get; init; }
    public DateOnly DataLancamento { get; init; }
    public string Descricao { get; init; } = string.Empty;
    public string? Categoria { get; init; }
}
