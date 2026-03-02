using FluxoCaixa.Lancamentos.Domain.Enums;

namespace FluxoCaixa.Lancamentos.Application.DTOs;

public record CriarLancamentoRequest(
    Guid IdempotencyKey,
    TipoLancamento Tipo,
    decimal Valor,
    string Descricao,
    string? Categoria,
    DateOnly? DataLancamento
);

public record AtualizarLancamentoRequest(
    decimal Valor,
    string Descricao,
    string? Categoria
);
