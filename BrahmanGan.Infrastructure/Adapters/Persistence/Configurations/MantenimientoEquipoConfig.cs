using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Equipos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class MantenimientoEquipoConfig : IEntityTypeConfiguration<MantenimientoEquipo>
{
    public void Configure(EntityTypeBuilder<MantenimientoEquipo> b)
    {
        b.ToTable("MantenimientoEquipos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdMantenimiento")
            .HasConversion(id => id.Value, v => MantenimientoEquipoId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdMaquinaria).HasColumnName("IdMaquinaria")
            .HasConversion(id => id.Value, v => MaquinariaId.From(v));
        b.Property(x => x.TipoMantenimiento).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(x => x.Descripcion).HasMaxLength(400).IsRequired();
        b.Property(x => x.Tecnico).HasMaxLength(100);
        b.Property(x => x.Proveedor).HasMaxLength(100);
        b.Property(x => x.CostoManoObra).HasColumnType("decimal(12,2)");
        b.Property(x => x.CostoRepuestos).HasColumnType("decimal(12,2)");
        b.Ignore(x => x.CostoTotal);
        b.Property(x => x.HorasAlMomento).HasColumnType("decimal(10,1)");
        b.HasIndex(x => new { x.IdMaquinaria, x.Fecha });
    }
}
