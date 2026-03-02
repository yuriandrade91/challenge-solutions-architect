using FluxoCaixa.Lancamentos.Domain.Entities;
using FluxoCaixa.Lancamentos.Domain.Enums;
using FluxoCaixa.Lancamentos.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace FluxoCaixa.Lancamentos.UnitTests;

public class LancamentoEntityTests
{
    [Fact]
    public void Criar_ComValorPositivo_DeveCriarLancamento()
    {
        var key = Guid.NewGuid();
        var lancamento = Lancamento.Criar(key, TipoLancamento.CREDIT, 100m, "Teste", null, null);
        lancamento.Should().NotBeNull();
        lancamento.Valor.Should().Be(100m);
        lancamento.Tipo.Should().Be(TipoLancamento.CREDIT);
        lancamento.Status.Should().Be(StatusLancamento.ACTIVE);
    }

    [Fact]
    public void Criar_ComValorZero_DeveLancarDomainException()
    {
        var act = () => Lancamento.Criar(Guid.NewGuid(), TipoLancamento.DEBIT, 0m, "Teste", null, null);
        act.Should().Throw<DomainException>().WithMessage("*zero*");
    }

    [Fact]
    public void Criar_ComValorNegativo_DeveLancarDomainException()
    {
        var act = () => Lancamento.Criar(Guid.NewGuid(), TipoLancamento.DEBIT, -10m, "Teste", null, null);
        act.Should().Throw<DomainException>().WithMessage("*zero*");
    }

    [Fact]
    public void Criar_SemDescricao_DeveLancarDomainException()
    {
        var act = () => Lancamento.Criar(Guid.NewGuid(), TipoLancamento.CREDIT, 100m, "", null, null);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Criar_ComDataLancamento_DeveUsarDataInformada()
    {
        var data = new DateOnly(2024, 1, 15);
        var lancamento = Lancamento.Criar(Guid.NewGuid(), TipoLancamento.CREDIT, 100m, "Teste", null, data);
        lancamento.DataLancamento.Should().Be(data);
    }

    [Fact]
    public void Criar_SemDataLancamento_DeveUsarDataAtual()
    {
        var lancamento = Lancamento.Criar(Guid.NewGuid(), TipoLancamento.DEBIT, 50m, "Teste", null, null);
        lancamento.DataLancamento.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
    }

    [Fact]
    public void Cancelar_LancamentoAtivo_DeveCancelar()
    {
        var lancamento = Lancamento.Criar(Guid.NewGuid(), TipoLancamento.CREDIT, 100m, "Teste", null, null);
        lancamento.Cancelar();
        lancamento.Status.Should().Be(StatusLancamento.CANCELLED);
        lancamento.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public void Cancelar_LancamentoCancelado_DeveLancarDomainException()
    {
        var lancamento = Lancamento.Criar(Guid.NewGuid(), TipoLancamento.CREDIT, 100m, "Teste", null, null);
        lancamento.Cancelar();
        var act = () => lancamento.Cancelar();
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Atualizar_LancamentoCancelado_DeveLancarDomainException()
    {
        var lancamento = Lancamento.Criar(Guid.NewGuid(), TipoLancamento.CREDIT, 100m, "Teste", null, null);
        lancamento.Cancelar();
        var act = () => lancamento.Atualizar(200m, "Novo", null);
        act.Should().Throw<DomainException>().WithMessage("*cancelados*");
    }

    [Fact]
    public void Criar_ComTipoDebit_DeveSetarTipoDebit()
    {
        var lancamento = Lancamento.Criar(Guid.NewGuid(), TipoLancamento.DEBIT, 75m, "Débito", null, null);
        lancamento.Tipo.Should().Be(TipoLancamento.DEBIT);
    }

    [Fact]
    public void Criar_ComCategoria_DeveSetarCategoria()
    {
        var lancamento = Lancamento.Criar(Guid.NewGuid(), TipoLancamento.CREDIT, 100m, "Teste", "Alimentação", null);
        lancamento.Categoria.Should().Be("Alimentação");
    }
}
