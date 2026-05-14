using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ServicioConfig : IEntityTypeConfiguration<Servicio>
{
    public void Configure(EntityTypeBuilder<Servicio> b)
    {
        b.ToTable("Servicios");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdServicio")
            .HasConversion(id => id.Value, v => ServicioId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdHembra).HasColumnName("IdHembra")
            .HasConversion(id => id.Value, v => AnimalId.From(v)).IsRequired();
        b.Property(x => x.TipoServicio).HasConversion<string>().HasMaxLength(15).IsRequired();
        b.Property(x => x.IdToro).HasColumnName("IdToro")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? AnimalId.From(v.Value) : null);
        b.Property(x => x.IdSemen).HasColumnName("IdSemen")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? SemenId.From(v.Value) : null);
        b.Property(x => x.Responsable).HasMaxLength(80);
    }
}
