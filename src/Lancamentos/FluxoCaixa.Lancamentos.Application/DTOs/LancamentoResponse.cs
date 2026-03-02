using FluxoCaixa.Lancamentos.Domain.Enums;

namespace FluxoCaixa.Lancamentos.Application.DTOs;

public record LancamentoResponse(
    Guid Id,
    Guid IdempotencyKey,
    TipoLancamento Tipo,
    decimal Valor,
    string Descricao,
    string? Categoria,
    DateOnly DataLancamento,
    StatusLancamento Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
