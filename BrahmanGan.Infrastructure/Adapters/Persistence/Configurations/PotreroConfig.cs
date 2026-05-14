using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class PotreroConfig : IEntityTypeConfiguration<Potrero>
{
    public void Configure(EntityTypeBuilder<Potrero> b)
    {
        b.ToTable("Potreros");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdPotrero")
            .HasConversion(id => id.Value, v => PotreroId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdFinca).HasColumnName("IdFinca")
            .HasConversion(id => id.Value, v => FincaId.From(v)).IsRequired();
        b.Property(x => x.Codigo).HasMaxLength(15).IsRequired();
        b.Property(x => x.Nombre).HasMaxLength(80).IsRequired();
        b.Property(x => x.AreaHectareas).HasColumnType("decimal(8,2)");
        b.Property(x => x.TipoPasto).HasMaxLength(80);
        b.HasIndex(x => new { x.IdFinca, x.Codigo }).IsUnique();
    }
}
