using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class DetalleCurativoConfig : IEntityTypeConfiguration<DetalleCurativo>
{
    public void Configure(EntityTypeBuilder<DetalleCurativo> b)
    {
        b.ToTable("DetallesCurativos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdDetalle")
            .HasConversion(id => id.Value, v => DetalleCurativoId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdTratamiento).HasColumnName("IdTratamiento")
            .HasConversion(id => id.Value, v => HistorialCurativoId.From(v));
        b.Property(x => x.IdMedicamento).HasColumnName("IdMedicamento")
            .HasConversion(id => id.Value, v => MedicamentoId.From(v));
        b.Property(x => x.Dosis).HasColumnType("decimal(10,3)");
        b.Property(x => x.CostoUnitario).HasColumnType("decimal(12,4)");
    }
}
