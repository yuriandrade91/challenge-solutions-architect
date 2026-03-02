using FluxoCaixa.Lancamentos.Application.Interfaces;
using FluxoCaixa.Lancamentos.Domain.Events;
using FluxoCaixa.Lancamentos.Domain.Exceptions;
using FluxoCaixa.Lancamentos.Domain.Interfaces;
using MediatR;

namespace FluxoCaixa.Lancamentos.Application.Commands;

public record CancelarLancamentoCommand(Guid Id) : IRequest<bool>;

public class CancelarLancamentoCommandHandler : IRequestHandler<CancelarLancamentoCommand, bool>
{
    private readonly ILancamentoRepository _repository;
    private readonly IEventPublisher _eventPublisher;

    public CancelarLancamentoCommandHandler(ILancamentoRepository repository, IEventPublisher eventPublisher)
    {
        _repository = repository;
        _eventPublisher = eventPublisher;
    }

    public async Task<bool> Handle(CancelarLancamentoCommand command, CancellationToken cancellationToken)
    {
        var lancamento = await _repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new DomainException("NOT_FOUND", $"Lançamento {command.Id} não encontrado.");

        lancamento.Cancelar();
        await _repository.UpdateAsync(lancamento, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var evt = new LancamentoCanceladoEvent
        {
            LancamentoId = lancamento.Id,
            Tipo = lancamento.Tipo,
            Valor = lancamento.Valor,
            DataLancamento = lancamento.DataLancamento
        };
        await _eventPublisher.PublishAsync(evt, cancellationToken);

        return true;
    }
}
