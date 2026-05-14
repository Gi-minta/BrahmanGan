using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Almacen;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class AcumulacionInsumoPotreroConfig : IEntityTypeConfiguration<AcumulacionInsumoPotrero>
{
    public void Configure(EntityTypeBuilder<AcumulacionInsumoPotrero> b)
    {
        b.ToTable("AcumulacionInsumosPotrero");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdAcumulacion")
            .HasConversion(id => id.Value, v => AcumulacionInsumoPotreroId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdPotrero).HasColumnName("IdPotrero")
            .HasConversion(id => id.Value, v => PotreroId.From(v));
        b.Property(x => x.IdInsumo).HasColumnName("IdInsumo")
            .HasConversion(id => id.Value, v => InsumoId.From(v));
        b.Property(x => x.Cantidad).HasColumnType("decimal(10,3)").IsRequired();
        b.Property(x => x.CostoUnitario).HasColumnType("decimal(12,4)");
    }
}
