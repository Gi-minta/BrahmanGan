using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Costos;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class CostoAnimalDiarioConfig : IEntityTypeConfiguration<CostoAnimalDiario>
{
    public void Configure(EntityTypeBuilder<CostoAnimalDiario> b)
    {
        b.ToTable("CostosAnimalesDiarios");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdCostoAnimal")
            .HasConversion(id => id.Value, v => CostoAnimalDiarioId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.TipoCosto).HasMaxLength(50).IsRequired();
        b.Property(x => x.Valor).HasColumnType("decimal(12,4)").IsRequired();
    }
}
