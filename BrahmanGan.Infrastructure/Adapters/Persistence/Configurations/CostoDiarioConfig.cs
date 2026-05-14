using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Costos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class CostoDiarioConfig : IEntityTypeConfiguration<CostoDiario>
{
    public void Configure(EntityTypeBuilder<CostoDiario> b)
    {
        b.ToTable("CostosDiarios");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdCosto")
            .HasConversion(id => id.Value, v => CostoDiarioId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdCentro).HasColumnName("IdCentro")
            .HasConversion(id => id.Value, v => CentroCostoId.From(v));
        b.Property(x => x.TipoCosto).HasMaxLength(50).IsRequired();
        b.Property(x => x.Descripcion).HasMaxLength(200);
        b.Property(x => x.Valor).HasColumnType("decimal(15,2)").IsRequired();
        b.HasIndex(x => new { x.Fecha, x.IdCentro });
    }
}
