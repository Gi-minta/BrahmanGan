using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class MunicipioConfig : IEntityTypeConfiguration<Municipio>
{
    public void Configure(EntityTypeBuilder<Municipio> b)
    {
        b.ToTable("Municipios");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdMunicipio")
            .HasConversion(id => id.Value, v => MunicipioId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdDepto).HasColumnName("IdDepto")
            .HasConversion(id => id.Value, v => DepartamentoId.From(v)).IsRequired();
        b.Property(x => x.Codigo).HasMaxLength(10).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasMaxLength(80).IsRequired();
        b.HasIndex(x => x.IdDepto);
    }
}
