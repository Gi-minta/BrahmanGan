using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class GrupoManejoConfig : IEntityTypeConfiguration<GrupoManejo>
{
    public void Configure(EntityTypeBuilder<GrupoManejo> b)
    {
        b.ToTable("GruposManejo");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdGrupo")
            .HasConversion(id => id.Value, v => GrupoManejoId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.Codigo).HasMaxLength(15).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        b.Property(x => x.Descripcion).HasMaxLength(300);
        b.Property(x => x.TipoAnimal).HasMaxLength(30);
    }
}
