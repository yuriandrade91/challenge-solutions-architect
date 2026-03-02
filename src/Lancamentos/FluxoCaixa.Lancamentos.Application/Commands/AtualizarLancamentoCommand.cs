using FluxoCaixa.Lancamentos.Application.DTOs;
using FluxoCaixa.Lancamentos.Application.Interfaces;
using FluxoCaixa.Lancamentos.Domain.Events;
using FluxoCaixa.Lancamentos.Domain.Exceptions;
using FluxoCaixa.Lancamentos.Domain.Interfaces;
using MediatR;

namespace FluxoCaixa.Lancamentos.Application.Commands;

public record AtualizarLancamentoCommand(Guid Id, AtualizarLancamentoRequest Request) : IRequest<LancamentoResponse>;

public class AtualizarLancamentoCommandHandler : IRequestHandler<AtualizarLancamentoCommand, LancamentoResponse>
{
    private readonly ILancamentoRepository _repository;
    private readonly IEventPublisher _eventPublisher;

    public AtualizarLancamentoCommandHandler(ILancamentoRepository repository, IEventPublisher eventPublisher)
    {
        _repository = repository;
        _eventPublisher = eventPublisher;
    }

    public async Task<LancamentoResponse> Handle(AtualizarLancamentoCommand command, CancellationToken cancellationToken)
    {
        var lancamento = await _repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new DomainException("NOT_FOUND", $"Lançamento {command.Id} não encontrado.");

        var valorAnterior = lancamento.Valor;
        lancamento.Atualizar(command.Request.Valor, command.Request.Descricao, command.Request.Categoria);

        await _repository.UpdateAsync(lancamento, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var evt = new LancamentoAtualizadoEvent
        {
            LancamentoId = lancamento.Id,
            Tipo = lancamento.Tipo,
            ValorAnterior = valorAnterior,
            ValorNovo = lancamento.Valor,
            DataLancamento = lancamento.DataLancamento
        };
        await _eventPublisher.PublishAsync(evt, cancellationToken);

        return new(lancamento.Id, lancamento.IdempotencyKey, lancamento.Tipo, lancamento.Valor,
            lancamento.Descricao, lancamento.Categoria, lancamento.DataLancamento,
            lancamento.Status, lancamento.CreatedAt, lancamento.UpdatedAt);
    }
}
