using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Equipos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ===== Equipos =====
internal sealed class MaquinariaConfig : IEntityTypeConfiguration<Maquinaria>
{
    public void Configure(EntityTypeBuilder<Maquinaria> b)
    {
        b.ToTable("Maquinaria");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdMaquinaria")
            .HasConversion(id => id.Value, v => MaquinariaId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdCentro).HasColumnName("IdCentro")
            .HasConversion(id => id.Value, v => CentroCostoId.From(v));
        b.Property(x => x.Codigo).HasMaxLength(20).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        b.Property(x => x.Marca).HasMaxLength(80);
        b.Property(x => x.Modelo).HasMaxLength(80);
        b.Property(x => x.NumeroSerie).HasMaxLength(80);
        b.Property(x => x.ValorCompra).HasColumnType("decimal(15,2)");
        b.Property(x => x.HorasUso).HasColumnType("decimal(10,1)");
        b.Property(x => x.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();
        b.Property(x => x.Observaciones).HasMaxLength(300);
    }
}
