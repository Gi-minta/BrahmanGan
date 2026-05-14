using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class RazaConfig : IEntityTypeConfiguration<Raza>
{
    public void Configure(EntityTypeBuilder<Raza> b)
    {
        b.ToTable("Razas");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdRaza")
            .HasConversion(id => id.Value, v => RazaId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.Codigo).HasMaxLength(10).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasMaxLength(80).IsRequired();
        b.Property(x => x.Proposito).HasConversion<string?>().HasMaxLength(20);
    }
}
