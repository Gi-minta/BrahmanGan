using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class PedigriConfig : IEntityTypeConfiguration<Pedigri>
{
    public void Configure(EntityTypeBuilder<Pedigri> b)
    {
        b.ToTable("Pedigri");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdPedigri")
            .HasConversion(id => id.Value, v => PedigriId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.IdAbuelo1).HasColumnName("IdAbuelo1")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? AnimalId.From(v.Value) : null);
        b.Property(x => x.IdAbuela1).HasColumnName("IdAbuela1")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? AnimalId.From(v.Value) : null);
        b.Property(x => x.IdAbuelo2).HasColumnName("IdAbuelo2")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? AnimalId.From(v.Value) : null);
        b.Property(x => x.IdAbuela2).HasColumnName("IdAbuela2")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? AnimalId.From(v.Value) : null);
        b.Property(x => x.PuntajeMorfologia).HasColumnType("decimal(5,2)");
        b.Property(x => x.Observaciones).HasMaxLength(300);
    }
}
