using FluxoCaixa.Lancamentos.Domain.Entities;
using FluxoCaixa.Lancamentos.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FluxoCaixa.Lancamentos.Infrastructure.Persistence;

public class LancamentosDbContext : DbContext
{
    public LancamentosDbContext(DbContextOptions<LancamentosDbContext> options) : base(options) { }

    public DbSet<Lancamento> Lancamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lancamento>(entity =>
        {
            entity.ToTable("lancamentos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdempotencyKey).HasColumnName("idempotency_key").IsRequired();
            entity.HasIndex(e => e.IdempotencyKey).IsUnique();
            entity.Property(e => e.Tipo).HasColumnName("tipo")
                .HasConversion(new EnumToStringConverter<TipoLancamento>()).IsRequired();
            entity.Property(e => e.Valor).HasColumnName("valor").HasPrecision(15, 2).IsRequired();
            entity.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(500).IsRequired();
            entity.Property(e => e.Categoria).HasColumnName("categoria").HasMaxLength(100);
            entity.Property(e => e.DataLancamento).HasColumnName("data_lancamento").IsRequired();
            entity.Property(e => e.Status).HasColumnName("status")
                .HasConversion(new EnumToStringConverter<StatusLancamento>()).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        });
    }
}
