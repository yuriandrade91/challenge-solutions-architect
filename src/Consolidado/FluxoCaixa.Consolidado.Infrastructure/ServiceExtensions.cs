using FluxoCaixa.Consolidado.Application.EventHandlers;
using FluxoCaixa.Consolidado.Application.Interfaces;
using FluxoCaixa.Consolidado.Domain.Interfaces;
using FluxoCaixa.Consolidado.Infrastructure.Cache;
using FluxoCaixa.Consolidado.Infrastructure.Messaging;
using FluxoCaixa.Consolidado.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace FluxoCaixa.Consolidado.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddConsolidadoInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ConsolidadoDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ConsolidadoDbContext).Assembly.FullName)));

        services.AddScoped<IConsolidadoRepository, ConsolidadoRepository>();

        var redisConnection = configuration["Redis:ConnectionString"] ?? "localhost:6379";
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
        services.AddScoped<ICacheService, RedisCacheService>();

        services.AddScoped<LancamentoCriadoEventHandler>();
        services.AddScoped<LancamentoCanceladoEventHandler>();
        services.AddHostedService<RabbitMqConsumer>();

        return services;
    }
}
