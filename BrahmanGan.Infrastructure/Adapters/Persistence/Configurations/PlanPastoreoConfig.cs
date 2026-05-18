using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Pastoreo;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ===== Pastoreo =====
internal sealed class PlanPastoreoConfig : IEntityTypeConfiguration<PlanPastoreo>
{
    public void Configure(EntityTypeBuilder<PlanPastoreo> b)
    {
        b.ToTable("PlanesPastoreo");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdPlanPastoreo")
            .HasConversion(id => id.Value, v => PlanPastoreoId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdPotrero).HasColumnName("IdPotrero")
            .HasConversion(id => id.Value, v => PotreroId.From(v));
        b.Property(x => x.CapacidadCarga).HasColumnType("decimal(8,2)");
        b.Property(x => x.Observaciones).HasMaxLength(500);
    }
}
