using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Almacen;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ===== Almacén =====
internal sealed class InsumoConfig : IEntityTypeConfiguration<Insumo>
{
    public void Configure(EntityTypeBuilder<Insumo> b)
    {
        b.ToTable("Insumos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdInsumo")
            .HasConversion(id => id.Value, v => InsumoId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.Codigo).HasMaxLength(20).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        b.Property(x => x.Tipo).HasMaxLength(50);
        b.Property(x => x.UnidadMedida).HasMaxLength(20);
        b.Property(x => x.PrecioUnitario).HasColumnType("decimal(12,4)");
        b.Property(x => x.StockMinimo).HasColumnType("decimal(10,3)");
        b.Property(x => x.StockActual).HasColumnType("decimal(10,3)");
    }
}
