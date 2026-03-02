using FluxoCaixa.Lancamentos.Application.Interfaces;
using FluxoCaixa.Lancamentos.Domain.Interfaces;
using FluxoCaixa.Lancamentos.Infrastructure.Messaging;
using FluxoCaixa.Lancamentos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoCaixa.Lancamentos.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddLancamentosInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<LancamentosDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(LancamentosDbContext).Assembly.FullName)));

        services.AddScoped<ILancamentoRepository, LancamentoRepository>();
        services.AddSingleton<IEventPublisher, RabbitMqPublisher>();

        return services;
    }
}
