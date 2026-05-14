using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ControlLecheAnimalConfig : IEntityTypeConfiguration<ControlLecheAnimal>
{
    public void Configure(EntityTypeBuilder<ControlLecheAnimal> b)
    {
        b.ToTable("ControlLecheAnimal");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdControl")
            .HasConversion(id => id.Value, v => ControlLecheAnimalId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.Ordeno).HasMaxLength(10);
        b.Property(x => x.LitrosManiana).HasColumnName("LitrosMañana").HasColumnType("decimal(7,3)");
        b.Property(x => x.LitrosTarde).HasColumnType("decimal(7,3)");
        b.Property(x => x.LitrosNoche).HasColumnType("decimal(7,3)");
        b.Ignore(x => x.TotalLitros); // Se calcula como columna computed en BD
        b.HasIndex(x => x.Fecha);
        b.HasIndex(x => new { x.IdAnimal, x.Fecha });
    }
}
