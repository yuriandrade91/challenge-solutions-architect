using FluxoCaixa.Lancamentos.Application.DTOs;
using FluxoCaixa.Lancamentos.Application.Interfaces;
using FluxoCaixa.Lancamentos.Domain.Entities;
using FluxoCaixa.Lancamentos.Domain.Events;
using FluxoCaixa.Lancamentos.Domain.Interfaces;
using MediatR;

namespace FluxoCaixa.Lancamentos.Application.Commands;

public record CriarLancamentoCommand(CriarLancamentoRequest Request) : IRequest<LancamentoResponse>;

public class CriarLancamentoCommandHandler : IRequestHandler<CriarLancamentoCommand, LancamentoResponse>
{
    private readonly ILancamentoRepository _repository;
    private readonly IEventPublisher _eventPublisher;

    public CriarLancamentoCommandHandler(ILancamentoRepository repository, IEventPublisher eventPublisher)
    {
        _repository = repository;
        _eventPublisher = eventPublisher;
    }

    public async Task<LancamentoResponse> Handle(CriarLancamentoCommand command, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdempotencyKeyAsync(command.Request.IdempotencyKey, cancellationToken);
        if (existing != null)
            return MapToResponse(existing);

        var lancamento = Lancamento.Criar(
            command.Request.IdempotencyKey,
            command.Request.Tipo,
            command.Request.Valor,
            command.Request.Descricao,
            command.Request.Categoria,
            command.Request.DataLancamento);

        await _repository.AddAsync(lancamento, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var evt = new LancamentoCriadoEvent
        {
            LancamentoId = lancamento.Id,
            Tipo = lancamento.Tipo,
            Valor = lancamento.Valor,
            DataLancamento = lancamento.DataLancamento,
            Descricao = lancamento.Descricao,
            Categoria = lancamento.Categoria
        };
        await _eventPublisher.PublishAsync(evt, cancellationToken);

        return MapToResponse(lancamento);
    }

    private static LancamentoResponse MapToResponse(Lancamento l) => new(
        l.Id, l.IdempotencyKey, l.Tipo, l.Valor, l.Descricao, l.Categoria,
        l.DataLancamento, l.Status, l.CreatedAt, l.UpdatedAt);
}
