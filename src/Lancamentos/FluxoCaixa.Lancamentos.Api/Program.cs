using FluxoCaixa.Lancamentos.Api.Middleware;
using FluxoCaixa.Lancamentos.Application.Commands;
using FluxoCaixa.Lancamentos.Application.Validators;
using FluxoCaixa.Lancamentos.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FluxoCaixa Lancamentos API", Version = "v1" });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CriarLancamentoCommand).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<CriarLancamentoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddLancamentosInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { }
