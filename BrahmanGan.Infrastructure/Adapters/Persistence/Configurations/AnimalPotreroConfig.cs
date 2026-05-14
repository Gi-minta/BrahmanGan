using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class AnimalPotreroConfig : IEntityTypeConfiguration<AnimalPotrero>
{
    public void Configure(EntityTypeBuilder<AnimalPotrero> b)
    {
        b.ToTable("AnimalPotrero");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdAsignacion")
            .HasConversion(id => id.Value, v => AnimalPotreroId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.IdPotrero).HasColumnName("IdPotrero")
            .HasConversion(id => id.Value, v => PotreroId.From(v));
        b.Property(x => x.IdGrupo).HasColumnName("IdGrupo")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? GrupoManejoId.From(v.Value) : null);
    }
}
