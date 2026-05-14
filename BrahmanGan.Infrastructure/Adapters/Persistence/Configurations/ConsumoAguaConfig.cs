using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sostenibilidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ConsumoAguaConfig : IEntityTypeConfiguration<ConsumoAgua>
{
    public void Configure(EntityTypeBuilder<ConsumoAgua> b)
    {
        b.ToTable("ConsumosAgua");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdConsumo")
            .HasConversion(id => id.Value, v => ConsumoAguaId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdFinca).HasColumnName("IdFinca")
            .HasConversion(id => id.Value, v => FincaId.From(v));
        b.Property(x => x.IdPotrero).HasColumnName("IdPotrero")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? PotreroId.From(v.Value) : null);
        b.Property(x => x.FuenteAgua).HasMaxLength(50);
        b.Property(x => x.VolumenM3).HasColumnType("decimal(10,3)").IsRequired();
        b.Ignore(x => x.LitrosAnimalDia);
        b.Property(x => x.Observaciones).HasMaxLength(200);
        b.HasIndex(x => new { x.IdFinca, x.Fecha });
    }
}
