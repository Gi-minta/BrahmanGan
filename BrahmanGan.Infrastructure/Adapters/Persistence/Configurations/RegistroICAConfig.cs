using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Trazabilidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ===== Trazabilidad =====
internal sealed class RegistroICAConfig : IEntityTypeConfiguration<RegistroICA>
{
    public void Configure(EntityTypeBuilder<RegistroICA> b)
    {
        b.ToTable("RegistroICA");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdRegistro")
            .HasConversion(id => id.Value, v => RegistroICAId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.TipoDocumento).HasMaxLength(50).IsRequired();
        b.Property(x => x.NumeroDocumento).HasMaxLength(50).IsRequired();
        b.Property(x => x.EntidadEmisora).HasMaxLength(100);
        b.Property(x => x.IdMunicipio).HasColumnName("IdMunicipio")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? MunicipioId.From(v.Value) : null);
        b.Property(x => x.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(x => x.UrlDocumento).HasMaxLength(300);
        b.Property(x => x.Observaciones).HasMaxLength(300);
        b.HasIndex(x => x.IdAnimal);
        b.HasIndex(x => new { x.FechaVencimiento, x.Estado });
    }
}
