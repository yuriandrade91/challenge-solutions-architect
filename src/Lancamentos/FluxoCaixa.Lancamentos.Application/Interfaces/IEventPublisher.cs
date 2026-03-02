using FluxoCaixa.Shared.Events;

namespace FluxoCaixa.Lancamentos.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IntegrationEvent;
}
