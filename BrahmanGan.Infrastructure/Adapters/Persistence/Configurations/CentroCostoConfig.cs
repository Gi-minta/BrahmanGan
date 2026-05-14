using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Costos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ===== Costos =====
internal sealed class CentroCostoConfig : IEntityTypeConfiguration<CentroCosto>
{
    public void Configure(EntityTypeBuilder<CentroCosto> b)
    {
        b.ToTable("CentrosCosto");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdCentro")
            .HasConversion(id => id.Value, v => CentroCostoId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.Codigo).HasMaxLength(15).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        b.Property(x => x.IdFinca).HasColumnName("IdFinca")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? FincaId.From(v.Value) : null);
    }
}
