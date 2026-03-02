using FluentValidation;
using FluxoCaixa.Lancamentos.Application.DTOs;
using FluxoCaixa.Lancamentos.Domain.Enums;

namespace FluxoCaixa.Lancamentos.Application.Validators;

public class CriarLancamentoValidator : AbstractValidator<CriarLancamentoRequest>
{
    public CriarLancamentoValidator()
    {
        RuleFor(x => x.IdempotencyKey).NotEmpty().WithMessage("IdempotencyKey é obrigatório.");
        RuleFor(x => x.Valor).GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(500).WithMessage("A descrição é obrigatória e deve ter no máximo 500 caracteres.");
        RuleFor(x => x.Categoria).MaximumLength(100).When(x => x.Categoria != null);
        RuleFor(x => x.Tipo).IsInEnum().WithMessage("Tipo inválido. Use CREDIT ou DEBIT.");
    }
}
