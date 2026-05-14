using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class HistorialMastitisConfig : IEntityTypeConfiguration<HistorialMastitis>
{
    public void Configure(EntityTypeBuilder<HistorialMastitis> b)
    {
        b.ToTable("HistorialMastitis");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdMastitis")
            .HasConversion(id => id.Value, v => HistorialMastitisId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.Cuarto).HasMaxLength(20);
        b.Property(x => x.GradoInfeccion).HasMaxLength(20);
        b.Property(x => x.IdTratamiento).HasColumnName("IdTratamiento")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? HistorialCurativoId.From(v.Value) : null);
    }
}
