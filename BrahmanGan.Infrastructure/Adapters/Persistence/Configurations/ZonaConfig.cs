using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ZonaConfig : IEntityTypeConfiguration<Zona>
{
    public void Configure(EntityTypeBuilder<Zona> b)
    {
        b.ToTable("Zonas");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdZona")
            .HasConversion(id => id.Value, v => ZonaId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.Codigo).HasMaxLength(10).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasMaxLength(80).IsRequired();
        b.Property(x => x.Tipo).HasMaxLength(50);
        b.Property(x => x.Descripcion).HasMaxLength(200);
        b.HasIndex(x => x.Tipo);
    }
}
