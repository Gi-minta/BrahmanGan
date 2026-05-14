using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ParametroLactanciaConfig : IEntityTypeConfiguration<ParametroLactancia>
{
    public void Configure(EntityTypeBuilder<ParametroLactancia> b)
    {
        b.ToTable("ParametrosLactancia");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdParametro")
            .HasConversion(id => id.Value, v => ParametroLactanciaId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.LitrosTotales).HasColumnType("decimal(10,2)");
        b.HasIndex(x => new { x.IdAnimal, x.NumeroParto }).IsUnique();
    }
}
