using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Comercial;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class DetalleCotizacionConfig : IEntityTypeConfiguration<DetalleCotizacion>
{
    public void Configure(EntityTypeBuilder<DetalleCotizacion> b)
    {
        b.ToTable("DetalleCotizacion");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdDetalle")
            .HasConversion(id => id.Value, v => DetalleCotizacionId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdCotizacion).HasColumnName("IdCotizacion")
            .HasConversion(id => id.Value, v => CotizacionVentaId.From(v));
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.PesoEstimadoKg).HasColumnType("decimal(7,2)");
        b.Property(x => x.PrecioUnitario).HasColumnType("decimal(12,4)");
    }
}
