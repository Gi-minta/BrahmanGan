using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class AnimalConfig : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> b)
    {
        b.ToTable("Animales");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.Codigo).HasMaxLength(20).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasMaxLength(80);
        b.Property(x => x.IdRaza).HasColumnName("IdRaza")
            .HasConversion(id => id.Value, v => RazaId.From(v));
        b.Property(x => x.Sexo)
            .HasConversion(s => s == Sexo.M ? "M" : "H", s => s == "M" ? Sexo.M : Sexo.H)
            .HasColumnType("char(1)").IsRequired();
        b.Property(x => x.IdMadre).HasColumnName("IdMadre")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? AnimalId.From(v.Value) : null);
        b.Property(x => x.IdPadre).HasColumnName("IdPadre")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? AnimalId.From(v.Value) : null);
        b.Property(x => x.IdOrigen).HasColumnName("IdOrigen")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? OrigenId.From(v.Value) : null);
        b.Property(x => x.IdFinca).HasColumnName("IdFinca")
            .HasConversion(id => id.Value, v => FincaId.From(v)).IsRequired();
        b.Property(x => x.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(x => x.PesoNacimiento).HasColumnType("decimal(7,2)");
        b.Property(x => x.Observaciones).HasMaxLength(500);
        b.Property(x => x.FechaRegistro).HasDefaultValueSql("GETDATE()");
        b.HasIndex(x => x.IdFinca);
        b.HasIndex(x => x.Estado);
        b.HasIndex(x => x.IdRaza);
    }
}
