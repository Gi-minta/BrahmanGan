using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class GestacionConfig : IEntityTypeConfiguration<Gestacion>
{
    public void Configure(EntityTypeBuilder<Gestacion> b)
    {
        b.ToTable("Gestaciones");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdGestacion")
            .HasConversion(id => id.Value, v => GestacionId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.IdServicio).HasColumnName("IdServicio")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? ServicioId.From(v.Value) : null);
        b.Property(x => x.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(x => x.Observaciones).HasMaxLength(300);
        b.HasIndex(x => new { x.IdAnimal, x.Estado });
    }
}
