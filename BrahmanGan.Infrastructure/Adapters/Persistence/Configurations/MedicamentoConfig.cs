using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class MedicamentoConfig : IEntityTypeConfiguration<Medicamento>
{
    public void Configure(EntityTypeBuilder<Medicamento> b)
    {
        b.ToTable("Medicamentos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdMedicamento")
            .HasConversion(id => id.Value, v => MedicamentoId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.Codigo).HasMaxLength(20).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
        b.Property(x => x.Principio).HasMaxLength(100);
        b.Property(x => x.TipoUso).HasMaxLength(20);
        b.Property(x => x.Unidad).HasMaxLength(20);
        b.Property(x => x.PrecioUnitario).HasColumnType("decimal(12,4)");
    }
}
