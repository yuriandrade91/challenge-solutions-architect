namespace FluxoCaixa.Consolidado.Domain.Entities;

public class ConsolidadoDiario
{
    public Guid Id { get; private set; }
    public DateOnly Data { get; private set; }
    public decimal TotalCreditos { get; private set; }
    public decimal TotalDebitos { get; private set; }
    public decimal Saldo => TotalCreditos - TotalDebitos;
    public int QuantidadeLancamentos { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private ConsolidadoDiario() { }

    public static ConsolidadoDiario Criar(DateOnly data)
    {
        return new ConsolidadoDiario
        {
            Id = Guid.NewGuid(),
            Data = data,
            TotalCreditos = 0,
            TotalDebitos = 0,
            QuantidadeLancamentos = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void AdicionarCredito(decimal valor)
    {
        if (valor <= 0) throw new ArgumentException("Valor deve ser maior que zero.", nameof(valor));
        TotalCreditos += valor;
        QuantidadeLancamentos++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AdicionarDebito(decimal valor)
    {
        if (valor <= 0) throw new ArgumentException("Valor deve ser maior que zero.", nameof(valor));
        TotalDebitos += valor;
        QuantidadeLancamentos++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoverCredito(decimal valor)
    {
        TotalCreditos = Math.Max(0, TotalCreditos - valor);
        QuantidadeLancamentos = Math.Max(0, QuantidadeLancamentos - 1);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoverDebito(decimal valor)
    {
        TotalDebitos = Math.Max(0, TotalDebitos - valor);
        QuantidadeLancamentos = Math.Max(0, QuantidadeLancamentos - 1);
        UpdatedAt = DateTime.UtcNow;
    }
}
