using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ComplementoConfig : IEntityTypeConfiguration<Complemento>
{
    public void Configure(EntityTypeBuilder<Complemento> b)
    {
        b.ToTable("Complementos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdComplemento")
            .HasConversion(id => id.Value, v => ComplementoId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdTratamiento).HasColumnName("IdTratamiento")
            .HasConversion(id => id.Value, v => HistorialCurativoId.From(v));
        b.Property(x => x.Descripcion).HasMaxLength(500).IsRequired();
        b.Property(x => x.Tipo).HasMaxLength(80);
        b.Property(x => x.Costo).HasColumnType("decimal(12,2)");
    }
}
