using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class HistorialCurativoConfig : IEntityTypeConfiguration<HistorialCurativo>
{
    public void Configure(EntityTypeBuilder<HistorialCurativo> b)
    {
        b.ToTable("HistorialCurativo");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdTratamiento")
            .HasConversion(id => id.Value, v => HistorialCurativoId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.Diagnostico).HasMaxLength(200).IsRequired();
        b.Property(x => x.Veterinario).HasMaxLength(100);
        b.Property(x => x.Resultado).HasMaxLength(50);
        b.Property(x => x.CostoTotal).HasColumnType("decimal(12,2)");
        // Relación con detalles
        b.HasMany(x => x.Detalles).WithOne()
            .HasForeignKey(d => d.IdTratamiento)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
