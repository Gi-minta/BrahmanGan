using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Comercial;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ContratoConfig : IEntityTypeConfiguration<Contrato>
{
    public void Configure(EntityTypeBuilder<Contrato> b)
    {
        b.ToTable("Contratos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdContrato")
            .HasConversion(id => id.Value, v => ContratoId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdCliente).HasColumnName("IdCliente")
            .HasConversion(id => id.Value, v => ClienteId.From(v));
        b.Property(x => x.Tipo).HasMaxLength(20).IsRequired();
        b.Property(x => x.PrecioAcordado).HasColumnType("decimal(12,4)");
        b.Property(x => x.UnidadPrecio).HasMaxLength(20);
        b.Property(x => x.VolumenEstimado).HasColumnType("decimal(12,2)");
        b.Property(x => x.Condiciones).HasMaxLength(500);
        b.Property(x => x.Estado).HasMaxLength(20).IsRequired();
        b.HasIndex(x => new { x.IdCliente, x.Estado });
    }
}
