using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Costos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class AutoconsumoConfig : IEntityTypeConfiguration<Autoconsumo>
{
    public void Configure(EntityTypeBuilder<Autoconsumo> b)
    {
        b.ToTable("Autoconsumos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdAutoconsumo")
            .HasConversion(id => id.Value, v => AutoconsumoId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdCentro).HasColumnName("IdCentro")
            .HasConversion(id => id.Value, v => CentroCostoId.From(v));
        b.Property(x => x.Concepto).HasMaxLength(200).IsRequired();
        b.Property(x => x.Cantidad).HasColumnType("decimal(10,3)");
        b.Property(x => x.ValorUnitario).HasColumnType("decimal(12,4)");
        b.Property(x => x.ValorTotal).HasColumnType("decimal(15,2)").IsRequired();
    }
}
