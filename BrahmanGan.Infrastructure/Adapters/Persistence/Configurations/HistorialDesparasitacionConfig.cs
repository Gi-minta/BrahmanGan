using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class HistorialDesparasitacionConfig : IEntityTypeConfiguration<HistorialDesparasitacion>
{
    public void Configure(EntityTypeBuilder<HistorialDesparasitacion> b)
    {
        b.ToTable("HistorialDesparasitacion");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdDesparasitacion")
            .HasConversion(id => id.Value, v => HistorialDesparasitacionId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.IdMedicamento).HasColumnName("IdMedicamento")
            .HasConversion(id => id.Value, v => MedicamentoId.From(v));
        b.Property(x => x.Dosis).HasColumnType("decimal(10,3)");
        b.Property(x => x.TipoParasito).HasMaxLength(50);
    }
}
