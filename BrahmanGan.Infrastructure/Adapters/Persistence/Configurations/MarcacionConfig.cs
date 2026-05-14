using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class MarcacionConfig : IEntityTypeConfiguration<Marcacion>
{
    public void Configure(EntityTypeBuilder<Marcacion> b)
    {
        b.ToTable("Marcaciones");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdMarcacion")
            .HasConversion(id => id.Value, v => MarcacionId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdAnimal).HasColumnName("IdAnimal")
            .HasConversion(id => id.Value, v => AnimalId.From(v));
        b.Property(x => x.TipoMarcacion).HasConversion<string>().HasMaxLength(30).IsRequired();
        b.Property(x => x.Codigo).HasMaxLength(60).IsRequired();
        b.Property(x => x.Responsable).HasMaxLength(80);
        b.Property(x => x.MotivoBaja).HasMaxLength(200);
        b.HasIndex(x => x.IdAnimal);
        b.HasIndex(x => x.Codigo);
    }
}
