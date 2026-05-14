using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class HistorialVacunacionConfig : IEntityTypeConfiguration<HistorialVacunacion>
{
    public void Configure(EntityTypeBuilder<HistorialVacunacion> b)
    {
        b.ToTable("HistorialVacunacion");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdVacunacion")
            .HasConversion(id => id.Value, v => HistorialVacunacionId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.IdMedicamento).HasColumnName("IdMedicamento")
            .HasConversion(id => id.Value, v => MedicamentoId.From(v));
        b.Property(x => x.Dosis).HasColumnType("decimal(10,3)");
        b.Property(x => x.Lote).HasMaxLength(50);
        b.Property(x => x.Responsable).HasMaxLength(80);
        b.HasIndex(x => new { x.IdAnimal, x.Fecha });
    }
}
