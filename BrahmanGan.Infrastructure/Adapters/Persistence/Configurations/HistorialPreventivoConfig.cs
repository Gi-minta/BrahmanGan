using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class HistorialPreventivoConfig : IEntityTypeConfiguration<HistorialPreventivo>
{
    public void Configure(EntityTypeBuilder<HistorialPreventivo> b)
    {
        b.ToTable("HistorialPreventivo");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdHistorial")
            .HasConversion(id => id.Value, v => HistorialPreventivoId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.IdControl).HasColumnName("IdControl")
            .HasConversion(id => id.Value, v => ControlPreventivoId.From(v));
        b.Property(x => x.IdMedicamento).HasColumnName("IdMedicamento")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? MedicamentoId.From(v.Value) : null);
        b.Property(x => x.Dosis).HasColumnType("decimal(10,3)");
        b.Property(x => x.Responsable).HasMaxLength(80);
    }
}
