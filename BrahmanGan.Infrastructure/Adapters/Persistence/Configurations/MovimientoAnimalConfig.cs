using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class MovimientoAnimalConfig : IEntityTypeConfiguration<MovimientoAnimal>
{
    public void Configure(EntityTypeBuilder<MovimientoAnimal> b)
    {
        b.ToTable("MovimientosAnimales");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdMovimiento")
            .HasConversion(id => id.Value, v => MovimientoAnimalId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.TipoMovimiento).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(x => x.Valor).HasColumnType("decimal(15,2)");
        b.Property(x => x.PesoKg).HasColumnType("decimal(7,2)");
        b.Property(x => x.Observaciones).HasMaxLength(300);
        b.HasIndex(x => x.Fecha);
        b.HasIndex(x => x.IdAnimal);
    }
}
