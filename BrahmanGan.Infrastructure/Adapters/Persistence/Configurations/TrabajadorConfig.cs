using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Nomina;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ===== Nómina =====
internal sealed class TrabajadorConfig : IEntityTypeConfiguration<Trabajador>
{
    public void Configure(EntityTypeBuilder<Trabajador> b)
    {
        b.ToTable("Trabajadores");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdTrabajador")
            .HasConversion(id => id.Value, v => TrabajadorId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.Cedula).HasMaxLength(20).IsRequired();
        b.HasIndex(x => x.Cedula).IsUnique();
        b.Property(x => x.Nombres).HasMaxLength(80).IsRequired();
        b.Property(x => x.Apellidos).HasMaxLength(80).IsRequired();
        b.Property(x => x.Cargo).HasMaxLength(80);
        b.Property(x => x.SalarioBase).HasColumnType("decimal(12,2)");
        b.Property(x => x.TipoContrato).HasMaxLength(30);
    }
}
