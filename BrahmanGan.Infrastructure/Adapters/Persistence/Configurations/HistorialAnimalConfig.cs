using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class HistorialAnimalConfig : IEntityTypeConfiguration<HistorialAnimal>
{
    public void Configure(EntityTypeBuilder<HistorialAnimal> b)
    {
        b.ToTable("HistorialAnimales");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdHistorial")
            .HasConversion(id => id.Value, v => HistorialAnimalId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.TipoEvento).HasConversion<string>().HasMaxLength(50).IsRequired();
        b.Property(x => x.Descripcion).HasMaxLength(500);
        b.Property(x => x.Valor).HasColumnType("decimal(15,2)");
        b.Property(x => x.UsuarioRegistro).HasMaxLength(50);
        b.Property(x => x.FechaRegistro).HasDefaultValueSql("GETDATE()");
    }
}
