using System.Text;
using System.Text.Json;
using FluxoCaixa.Consolidado.Application.EventHandlers;
using FluxoCaixa.Lancamentos.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FluxoCaixa.Consolidado.Infrastructure.Messaging;

public class RabbitMqConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMqConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string ExchangeName = "fluxocaixa.lancamentos";
    private const string QueueName = "consolidado.lancamentos";

    public RabbitMqConsumer(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<RabbitMqConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest",
                VirtualHost = _configuration["RabbitMQ:VirtualHost"] ?? "/"
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(QueueName, ExchangeName, "lancamentocriadoevent", cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(QueueName, ExchangeName, "lancamentocanceladoevent", cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(QueueName, ExchangeName, "lancamentoatualizadoevent", cancellationToken: stoppingToken);
            await _channel.BasicQosAsync(0, 10, false, stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    await ProcessMessageAsync(ea.RoutingKey, message, stoppingToken);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message with routing key {RoutingKey}", ea.RoutingKey);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            _logger.LogInformation("RabbitMQ consumer started, listening on queue {Queue}", QueueName);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("RabbitMQ consumer stopping...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RabbitMQ consumer failed");
        }
    }

    private async Task ProcessMessageAsync(string routingKey, string message, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        if (routingKey == "lancamentocriadoevent")
        {
            var evt = JsonSerializer.Deserialize<LancamentoCriadoEvent>(message);
            if (evt != null)
            {
                var handler = scope.ServiceProvider.GetRequiredService<LancamentoCriadoEventHandler>();
                await handler.HandleAsync(evt, cancellationToken);
            }
        }
        else if (routingKey == "lancamentocanceladoevent")
        {
            var evt = JsonSerializer.Deserialize<LancamentoCanceladoEvent>(message);
            if (evt != null)
            {
                var handler = scope.ServiceProvider.GetRequiredService<LancamentoCanceladoEventHandler>();
                await handler.HandleAsync(evt, cancellationToken);
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        if (_channel != null) await _channel.CloseAsync(cancellationToken);
        if (_connection != null) await _connection.CloseAsync(cancellationToken);
    }
}
