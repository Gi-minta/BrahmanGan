using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Comercial;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class CotizacionVentaConfig : IEntityTypeConfiguration<CotizacionVenta>
{
    public void Configure(EntityTypeBuilder<CotizacionVenta> b)
    {
        b.ToTable("CotizacionesVenta");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdCotizacion")
            .HasConversion(id => id.Value, v => CotizacionVentaId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdCliente).HasColumnName("IdCliente")
            .HasConversion(id => id.Value, v => ClienteId.From(v));
        b.Property(x => x.PrecioOfertado).HasColumnType("decimal(12,4)").IsRequired();
        b.Property(x => x.UnidadPrecio).HasMaxLength(20);
        b.Property(x => x.Estado).HasMaxLength(20).IsRequired();
        b.Property(x => x.Observaciones).HasMaxLength(300);
        b.HasMany(x => x.Detalles).WithOne()
            .HasForeignKey(d => d.IdCotizacion)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
