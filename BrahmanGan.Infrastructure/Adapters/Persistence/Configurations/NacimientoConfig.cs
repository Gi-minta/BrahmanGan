using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class NacimientoConfig : IEntityTypeConfiguration<Nacimiento>
{
    public void Configure(EntityTypeBuilder<Nacimiento> b)
    {
        b.ToTable("Nacimientos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdNacimiento")
            .HasConversion(id => id.Value, v => NacimientoId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdGestacion).HasColumnName("IdGestacion")
            .HasConversion(id => id.Value, v => GestacionId.From(v));
        b.Property(x => x.IdAnimalCria).HasColumnName("IdAnimalCria")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? AnimalId.From(v.Value) : null);
        b.Property(x => x.Sexo).HasColumnType("char(1)");
        b.Property(x => x.PesoNacimiento).HasColumnType("decimal(7,2)");
        b.Property(x => x.Condicion).HasMaxLength(20);
        b.Property(x => x.Observaciones).HasMaxLength(300);
    }
}
