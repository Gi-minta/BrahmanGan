using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class VentaLecheConfig : IEntityTypeConfiguration<VentaLeche>
{
    public void Configure(EntityTypeBuilder<VentaLeche> b)
    {
        b.ToTable("VentasLeche");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdVenta")
            .HasConversion(id => id.Value, v => VentaLecheId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdCliente).HasColumnName("IdCliente")
            .HasConversion(id => id.Value, v => ClienteId.From(v)).IsRequired();
        b.Property(x => x.IdContrato).HasColumnName("IdContrato")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? ContratoId.From(v.Value) : null);
        b.Property(x => x.LitrosVendidos).HasColumnType("decimal(10,3)").IsRequired();
        b.Property(x => x.PrecioLitro).HasColumnType("decimal(10,4)").IsRequired();
        b.Ignore(x => x.TotalVenta);
        b.Property(x => x.Factura).HasMaxLength(50);
        b.HasIndex(x => x.Fecha);
        b.HasIndex(x => x.IdCliente);
    }
}
