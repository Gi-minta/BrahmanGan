using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ControlPreventivoConfig : IEntityTypeConfiguration<ControlPreventivo>
{
    public void Configure(EntityTypeBuilder<ControlPreventivo> b)
    {
        b.ToTable("ControlesPreventivos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdControl")
            .HasConversion(id => id.Value, v => ControlPreventivoId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        b.Property(x => x.Periodicidad).HasMaxLength(50);
        b.Property(x => x.Descripcion).HasMaxLength(300);
    }
}
