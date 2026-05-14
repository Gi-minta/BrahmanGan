using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ─────────────────────────────────────────────────────────────
//  Usuario
// ─────────────────────────────────────────────────────────────
internal class UsuarioConfig : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> b)
    {
        b.ToTable("Usuarios");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id)
            .HasConversion(id => id.Value, v => UsuarioId.From(v))
            .ValueGeneratedOnAdd();

        b.Property(x => x.Email).HasMaxLength(200).IsRequired();
        b.Property(x => x.NombreCompleto).HasMaxLength(200).IsRequired();
        b.Property(x => x.PasswordHash).HasMaxLength(500);
        b.Property(x => x.RefreshToken).HasMaxLength(500);
        b.Property(x => x.RefreshTokenExpira);
        b.Property(x => x.Proveedor).HasConversion<string>().HasMaxLength(30);
        b.Property(x => x.IdExterno).HasMaxLength(200);
        b.Property(x => x.EmailConfirmado);
        b.Property(x => x.Activo);
        b.Property(x => x.FechaCreacion);
        b.Property(x => x.UltimoAcceso);

        b.HasIndex(x => x.Email).IsUnique();
    }
}
