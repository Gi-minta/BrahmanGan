using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Nomina;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class PagoJornalConfig : IEntityTypeConfiguration<PagoJornal>
{
    public void Configure(EntityTypeBuilder<PagoJornal> b)
    {
        b.ToTable("PagosJornales");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdPago")
            .HasConversion(id => id.Value, v => PagoJornalId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdTrabajador).HasColumnName("IdTrabajador")
            .HasConversion(id => id.Value, v => TrabajadorId.From(v));
        b.Property(x => x.HorasTrabajadas).HasColumnType("decimal(5,2)");
        b.Property(x => x.ValorJornal).HasColumnType("decimal(10,2)").IsRequired();
        b.Property(x => x.Concepto).HasMaxLength(200);
        b.Property(x => x.IdCentro).HasColumnName("IdCentro")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? CentroCostoId.From(v.Value) : null);
    }
}
