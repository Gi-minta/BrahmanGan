using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class OrigenConfig : IEntityTypeConfiguration<Origen>
{
    public void Configure(EntityTypeBuilder<Origen> b)
    {
        b.ToTable("Origen");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdOrigen")
            .HasConversion(id => id.Value, v => OrigenId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.Codigo).HasMaxLength(10).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Descripcion).HasMaxLength(100).IsRequired();
    }
}
