using FluxoCaixa.Lancamentos.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoCaixa.Lancamentos.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddLancamentosApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceExtensions).Assembly));
        services.AddTransient<IValidator<DTOs.CriarLancamentoRequest>, CriarLancamentoValidator>();
        return services;
    }
}
