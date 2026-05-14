using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Costos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class GastoGeneralConfig : IEntityTypeConfiguration<GastoGeneral>
{
    public void Configure(EntityTypeBuilder<GastoGeneral> b)
    {
        b.ToTable("GastosGenerales");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdGasto")
            .HasConversion(id => id.Value, v => GastoGeneralId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdCentro).HasColumnName("IdCentro")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? CentroCostoId.From(v.Value) : null);
        b.Property(x => x.Concepto).HasMaxLength(200).IsRequired();
        b.Property(x => x.Valor).HasColumnType("decimal(15,2)").IsRequired();
        b.Property(x => x.Proveedor).HasMaxLength(100);
        b.Property(x => x.Comprobante).HasMaxLength(50);
    }
}
