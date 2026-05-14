using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class SemenConfig : IEntityTypeConfiguration<Semen>
{
    public void Configure(EntityTypeBuilder<Semen> b)
    {
        b.ToTable("Semen");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdSemen")
            .HasConversion(id => id.Value, v => SemenId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.Codigo).HasMaxLength(20).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.NombreToro).HasMaxLength(100).IsRequired();
        b.Property(x => x.IdRaza).HasColumnName("IdRaza")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? RazaId.From(v.Value) : null);
        b.Property(x => x.Casa).HasMaxLength(80);
    }
}
