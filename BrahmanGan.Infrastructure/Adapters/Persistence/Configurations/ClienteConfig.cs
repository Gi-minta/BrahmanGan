using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Comercial;
using BrahmanGan.Domain.ValueObjects;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ClienteConfig : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> b)
    {
        b.ToTable("Clientes");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdCliente")
            .HasConversion(id => id.Value, v => ClienteId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.TipoDocumento).HasMaxLength(10).IsRequired();
        b.Property(x => x.Documento).HasMaxLength(20).IsRequired();
        b.HasIndex(x => x.Documento).IsUnique();
        b.Property(x => x.RazonSocial).HasMaxLength(150).IsRequired();
        b.Property(x => x.Contacto).HasMaxLength(100);
        b.Property(x => x.Telefono).HasMaxLength(30);
        b.Property(x => x.Email)
            .HasConversion(e => e == null ? null : (string)e, s => string.IsNullOrEmpty(s) ? null : Email.Create(s))
            .HasMaxLength(100);
        b.Property(x => x.Direccion).HasMaxLength(200);
        b.Property(x => x.IdMunicipio).HasColumnName("IdMunicipio")
            .HasConversion(id => id == null ? (int?)null : id.Value, v => v.HasValue ? MunicipioId.From(v.Value) : null);
        b.Property(x => x.TipoCliente).HasMaxLength(30);
    }
}
