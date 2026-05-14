using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class CalidadLecheConfig : IEntityTypeConfiguration<CalidadLeche>
{
    public void Configure(EntityTypeBuilder<CalidadLeche> b)
    {
        b.ToTable("CalidadLeche");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdMuestra")
            .HasConversion(id => id.Value, v => CalidadLecheId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? AnimalId.From(v.Value) : null);
        b.Property(x => x.GrasaPct).HasColumnType("decimal(5,2)");
        b.Property(x => x.ProteinaPct).HasColumnType("decimal(5,2)");
        b.Property(x => x.LactozaPct).HasColumnType("decimal(5,2)");
        b.Property(x => x.UreaMgDL).HasColumnType("decimal(6,2)");
        b.Property(x => x.Laboratorio).HasMaxLength(100);
        b.Property(x => x.Resultado).HasMaxLength(20);
        b.Property(x => x.Observaciones).HasMaxLength(300);
        b.HasIndex(x => x.Fecha);
        b.HasIndex(x => new { x.IdAnimal, x.Fecha });
    }
}
