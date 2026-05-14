using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Costos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class TransferenciaCostoConfig : IEntityTypeConfiguration<TransferenciaCosto>
{
    public void Configure(EntityTypeBuilder<TransferenciaCosto> b)
    {
        b.ToTable("TransferenciasCosto");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdTransferencia")
            .HasConversion(id => id.Value, v => TransferenciaCostoId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdCentroOrigen).HasColumnName("IdCentroOrigen")
            .HasConversion(id => id.Value, v => CentroCostoId.From(v));
        b.Property(x => x.IdCentroDestino).HasColumnName("IdCentroDestino")
            .HasConversion(id => id.Value, v => CentroCostoId.From(v));
        b.Property(x => x.Concepto).HasMaxLength(200).IsRequired();
        b.Property(x => x.Valor).HasColumnType("decimal(15,2)").IsRequired();
    }
}
