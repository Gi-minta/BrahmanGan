using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Nomina;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class PrestacionSocialConfig : IEntityTypeConfiguration<PrestacionSocial>
{
    public void Configure(EntityTypeBuilder<PrestacionSocial> b)
    {
        b.ToTable("PrestacionesSociales");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdPrestacion")
            .HasConversion(id => id.Value, v => PrestacionSocialId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdTrabajador).HasColumnName("IdTrabajador")
            .HasConversion(id => id.Value, v => TrabajadorId.From(v));
        foreach (var col in new[] { "SalarioBase", "Cesantias", "Vacaciones", "PrimaServicio",
            "SaludEmpleador", "PensionEmpleador", "ARL", "CajaCompensacion", "SENA", "ICBF" })
        {
            b.Property(col).HasColumnType("decimal(12,2)");
        }
    }
}
