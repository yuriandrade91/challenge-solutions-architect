using FluxoCaixa.Consolidado.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace FluxoCaixa.Consolidado.UnitTests;

public class ConsolidadoDiarioTests
{
    [Fact]
    public void Criar_DeveInicializarComValoresZero()
    {
        var data = new DateOnly(2024, 1, 15);
        var consolidado = ConsolidadoDiario.Criar(data);

        consolidado.Should().NotBeNull();
        consolidado.Data.Should().Be(data);
        consolidado.TotalCreditos.Should().Be(0);
        consolidado.TotalDebitos.Should().Be(0);
        consolidado.Saldo.Should().Be(0);
        consolidado.QuantidadeLancamentos.Should().Be(0);
    }

    [Fact]
    public void AdicionarCredito_DeveSomarCreditos()
    {
        var consolidado = ConsolidadoDiario.Criar(DateOnly.FromDateTime(DateTime.UtcNow));
        consolidado.AdicionarCredito(100m);
        consolidado.AdicionarCredito(50m);

        consolidado.TotalCreditos.Should().Be(150m);
        consolidado.QuantidadeLancamentos.Should().Be(2);
    }

    [Fact]
    public void AdicionarDebito_DeveSomarDebitos()
    {
        var consolidado = ConsolidadoDiario.Criar(DateOnly.FromDateTime(DateTime.UtcNow));
        consolidado.AdicionarDebito(30m);

        consolidado.TotalDebitos.Should().Be(30m);
        consolidado.QuantidadeLancamentos.Should().Be(1);
    }

    [Fact]
    public void Saldo_DeveSer_CreditosMenosDebitos()
    {
        var consolidado = ConsolidadoDiario.Criar(DateOnly.FromDateTime(DateTime.UtcNow));
        consolidado.AdicionarCredito(200m);
        consolidado.AdicionarDebito(80m);

        consolidado.Saldo.Should().Be(120m);
    }

    [Fact]
    public void Saldo_ComMaisDebitos_DeveSerNegativo()
    {
        var consolidado = ConsolidadoDiario.Criar(DateOnly.FromDateTime(DateTime.UtcNow));
        consolidado.AdicionarCredito(50m);
        consolidado.AdicionarDebito(100m);

        consolidado.Saldo.Should().Be(-50m);
    }

    [Fact]
    public void RemoverCredito_DeveSubtrairDoCreditoTotal()
    {
        var consolidado = ConsolidadoDiario.Criar(DateOnly.FromDateTime(DateTime.UtcNow));
        consolidado.AdicionarCredito(200m);
        consolidado.RemoverCredito(100m);

        consolidado.TotalCreditos.Should().Be(100m);
        consolidado.QuantidadeLancamentos.Should().Be(0);
    }

    [Fact]
    public void AdicionarCredito_ComValorZero_DeveLancarArgumentException()
    {
        var consolidado = ConsolidadoDiario.Criar(DateOnly.FromDateTime(DateTime.UtcNow));
        var act = () => consolidado.AdicionarCredito(0m);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AdicionarDebito_ComValorNegativo_DeveLancarArgumentException()
    {
        var consolidado = ConsolidadoDiario.Criar(DateOnly.FromDateTime(DateTime.UtcNow));
        var act = () => consolidado.AdicionarDebito(-10m);
        act.Should().Throw<ArgumentException>();
    }
}
