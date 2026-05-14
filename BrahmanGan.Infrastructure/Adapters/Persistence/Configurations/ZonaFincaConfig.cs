using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ZonaFincaConfig : IEntityTypeConfiguration<ZonaFinca>
{
    public void Configure(EntityTypeBuilder<ZonaFinca> b)
    {
        b.ToTable("ZonaFinca");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdZonaFinca")
            .HasConversion(id => id.Value, v => ZonaFincaId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdZona).HasColumnName("IdZona")
            .HasConversion(id => id.Value, v => ZonaId.From(v));
        b.Property(x => x.IdFinca).HasColumnName("IdFinca")
            .HasConversion(id => id.Value, v => FincaId.From(v));
        b.Property(x => x.Observaciones).HasMaxLength(200);
        b.HasIndex(x => new { x.IdZona, x.IdFinca }).IsUnique();
        b.HasIndex(x => x.IdZona);
        b.HasIndex(x => x.IdFinca);
    }
}
