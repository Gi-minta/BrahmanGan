using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Almacen;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class KardexInsumoConfig : IEntityTypeConfiguration<KardexInsumo>
{
    public void Configure(EntityTypeBuilder<KardexInsumo> b)
    {
        b.ToTable("KardexInsumos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdMovimiento")
            .HasConversion(id => id.Value, v => KardexInsumoId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdInsumo).HasColumnName("IdInsumo")
            .HasConversion(id => id.Value, v => InsumoId.From(v));
        b.Property(x => x.TipoMovimiento).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(x => x.Cantidad).HasColumnType("decimal(10,3)").IsRequired();
        b.Property(x => x.CostoUnitario).HasColumnType("decimal(12,4)");
        b.Property(x => x.Concepto).HasMaxLength(200);
        b.Property(x => x.Referencia).HasMaxLength(50);
        b.Property(x => x.SaldoAnterior).HasColumnType("decimal(10,3)");
        b.Property(x => x.SaldoNuevo).HasColumnType("decimal(10,3)");
        b.HasIndex(x => new { x.IdInsumo, x.Fecha });
    }
}
