using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sostenibilidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ===== Sostenibilidad =====
internal sealed class CapturaCarbonoConfig : IEntityTypeConfiguration<CapturaCarbono>
{
    public void Configure(EntityTypeBuilder<CapturaCarbono> b)
    {
        b.ToTable("CapturaCarbono");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdRegistro")
            .HasConversion(id => id.Value, v => CapturaCarbonoId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdFinca).HasColumnName("IdFinca")
            .HasConversion(id => id.Value, v => FincaId.From(v));
        b.Property(x => x.EmisionesGanadoTCO2).HasColumnType("decimal(10,4)");
        b.Property(x => x.CapturaForestal).HasColumnType("decimal(10,4)");
        b.Ignore(x => x.HuellaNeta);
        b.Property(x => x.Certificacion).HasMaxLength(100);
        b.Property(x => x.Observaciones).HasMaxLength(300);
        b.HasIndex(x => new { x.IdFinca, x.Anio, x.Mes }).IsUnique();
    }
}
