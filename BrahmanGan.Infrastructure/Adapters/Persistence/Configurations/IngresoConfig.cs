using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Costos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class IngresoConfig : IEntityTypeConfiguration<Ingreso>
{
    public void Configure(EntityTypeBuilder<Ingreso> b)
    {
        b.ToTable("Ingresos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdIngreso")
            .HasConversion(id => id.Value, v => IngresoId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdCentro).HasColumnName("IdCentro")
            .HasConversion(id => id.Value, v => CentroCostoId.From(v));
        b.Property(x => x.TipoIngreso).HasMaxLength(50).IsRequired();
        b.Property(x => x.Concepto).HasMaxLength(200);
        b.Property(x => x.Valor).HasColumnType("decimal(15,2)").IsRequired();
        b.Property(x => x.Comprobante).HasMaxLength(50);
    }
}
