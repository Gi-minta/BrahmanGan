using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using FincaEntity = BrahmanGan.Domain.Modulos.Finca.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class FincaConfig : IEntityTypeConfiguration<FincaEntity>
{
    public void Configure(EntityTypeBuilder<FincaEntity> b)
    {
        b.ToTable("Finca");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdFinca")
            .HasConversion(id => id.Value, v => FincaId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        b.Property(x => x.NIT).HasMaxLength(20);
        b.Property(x => x.Propietario).HasMaxLength(100);
        b.Property(x => x.Direccion).HasMaxLength(200);
        b.Property(x => x.Telefono).HasMaxLength(30);
        b.Property(x => x.Email).HasMaxLength(100);
        b.Property(x => x.IdMunicipio).HasColumnName("IdMunicipio")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? MunicipioId.From(v.Value) : null);
        b.Property(x => x.AreaHectareas).HasColumnType("decimal(10,2)");
        b.Property(x => x.FechaRegistro).HasDefaultValueSql("GETDATE()");
        b.HasIndex(x => x.IdMunicipio);
    }
}
