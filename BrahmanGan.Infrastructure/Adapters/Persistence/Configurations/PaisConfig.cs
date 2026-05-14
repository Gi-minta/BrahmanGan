using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class PaisConfig : IEntityTypeConfiguration<Pais>
{
    public void Configure(EntityTypeBuilder<Pais> b)
    {
        b.ToTable("Paises");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdPais")
            .HasConversion(id => id.Value, v => PaisId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.Codigo).HasMaxLength(5).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasMaxLength(80).IsRequired();
    }
}
