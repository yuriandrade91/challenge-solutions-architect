using FluxoCaixa.Consolidado.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FluxoCaixa.Consolidado.Infrastructure.Persistence;

public class ConsolidadoDbContext : DbContext
{
    public ConsolidadoDbContext(DbContextOptions<ConsolidadoDbContext> options) : base(options) { }

    public DbSet<ConsolidadoDiario> ConsolidadoDiario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConsolidadoDiario>(entity =>
        {
            entity.ToTable("consolidado_diario");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Data).HasColumnName("data").IsRequired();
            entity.HasIndex(e => e.Data).IsUnique();
            entity.Property(e => e.TotalCreditos).HasColumnName("total_creditos").HasPrecision(15, 2).IsRequired();
            entity.Property(e => e.TotalDebitos).HasColumnName("total_debitos").HasPrecision(15, 2).IsRequired();
            entity.Property(e => e.Saldo).HasColumnName("saldo").HasPrecision(15, 2).IsRequired();
            entity.Property(e => e.QuantidadeLancamentos).HasColumnName("quantidade_lancamentos").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });
    }
}
