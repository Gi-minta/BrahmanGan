using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Costos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ActivoConfig : IEntityTypeConfiguration<Activo>
{
    public void Configure(EntityTypeBuilder<Activo> b)
    {
        b.ToTable("Activos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdActivo")
            .HasConversion(id => id.Value, v => ActivoId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdCentro).HasColumnName("IdCentro")
            .HasConversion(id => id.Value, v => CentroCostoId.From(v));
        b.Property(x => x.Descripcion).HasMaxLength(150).IsRequired();
        b.Property(x => x.ValorCompra).HasColumnType("decimal(15,2)");
        b.Property(x => x.ValorResidual).HasColumnType("decimal(15,2)");
        b.Property(x => x.EstaActivo).HasColumnName("Activo");
    }
}
