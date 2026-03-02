using FluxoCaixa.Lancamentos.Domain.Enums;
using FluxoCaixa.Lancamentos.Domain.Exceptions;

namespace FluxoCaixa.Lancamentos.Domain.Entities;

public class Lancamento
{
    public Guid Id { get; private set; }
    public Guid IdempotencyKey { get; private set; }
    public TipoLancamento Tipo { get; private set; }
    public decimal Valor { get; private set; }
    public string Descricao { get; private set; }
    public string? Categoria { get; private set; }
    public DateOnly DataLancamento { get; private set; }
    public StatusLancamento Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private Lancamento() { }

    public static Lancamento Criar(
        Guid idempotencyKey,
        TipoLancamento tipo,
        decimal valor,
        string descricao,
        string? categoria,
        DateOnly? dataLancamento)
    {
        if (valor <= 0)
            throw new DomainException("RN01", "O valor do lançamento deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("RN_DESCRICAO", "A descrição é obrigatória.");

        return new Lancamento
        {
            Id = Guid.NewGuid(),
            IdempotencyKey = idempotencyKey,
            Tipo = tipo,
            Valor = valor,
            Descricao = descricao,
            Categoria = categoria,
            DataLancamento = dataLancamento ?? DateOnly.FromDateTime(DateTime.UtcNow),
            Status = StatusLancamento.ACTIVE,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Atualizar(decimal valor, string descricao, string? categoria)
    {
        if (Status == StatusLancamento.CANCELLED)
            throw new DomainException("RN05", "Lançamentos cancelados não podem ser editados.");

        if (valor <= 0)
            throw new DomainException("RN01", "O valor do lançamento deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("RN_DESCRICAO", "A descrição é obrigatória.");

        Valor = valor;
        Descricao = descricao;
        Categoria = categoria;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancelar()
    {
        if (Status == StatusLancamento.CANCELLED)
            throw new DomainException("RN_ALREADY_CANCELLED", "O lançamento já está cancelado.");

        Status = StatusLancamento.CANCELLED;
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
