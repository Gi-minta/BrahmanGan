using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class DepartamentoConfig : IEntityTypeConfiguration<Departamento>
{
    public void Configure(EntityTypeBuilder<Departamento> b)
    {
        b.ToTable("Departamentos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdDepto")
            .HasConversion(id => id.Value, v => DepartamentoId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdPais).HasColumnName("IdPais")
            .HasConversion(id => id.Value, v => PaisId.From(v)).IsRequired();
        b.Property(x => x.Codigo).HasMaxLength(5).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasMaxLength(80).IsRequired();
        b.HasIndex(x => x.IdPais);
    }
}
