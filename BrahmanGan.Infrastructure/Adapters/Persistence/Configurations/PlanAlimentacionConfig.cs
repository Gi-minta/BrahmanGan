using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Alimentacion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ===== Alimentación =====
internal sealed class PlanAlimentacionConfig : IEntityTypeConfiguration<PlanAlimentacion>
{
    public void Configure(EntityTypeBuilder<PlanAlimentacion> b)
    {
        b.ToTable("PlanesAlimentacion");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdPlanAlimentacion")
            .HasConversion(id => id.Value, v => PlanAlimentacionId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdFinca).HasColumnName("IdFinca")
            .HasConversion(id => id.Value, v => FincaId.From(v));
        b.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
        b.Property(x => x.Observaciones).HasMaxLength(500);
    }
}

internal sealed class DetallePlanAlimentacionConfig : IEntityTypeConfiguration<DetallePlanAlimentacion>
{
    public void Configure(EntityTypeBuilder<DetallePlanAlimentacion> b)
    {
        b.ToTable("DetallePlanAlimentacion");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdDetallePlan")
            .HasConversion(id => id.Value, v => DetallePlanAlimentacionId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdPlan).HasColumnName("IdPlanAlimentacion")
            .HasConversion(id => id.Value, v => PlanAlimentacionId.From(v));
        b.Property(x => x.IdInsumo).HasColumnName("IdInsumo")
            .HasConversion(id => id!.Value, v => InsumoId.From(v)).IsRequired(false);
        b.Property(x => x.Alimento).HasMaxLength(150).IsRequired();
        b.Property(x => x.CantidadDiaria).HasColumnType("decimal(10,3)");
        b.Property(x => x.UnidadMedida).HasMaxLength(30);
        b.Property(x => x.Observaciones).HasMaxLength(300);
    }
}
