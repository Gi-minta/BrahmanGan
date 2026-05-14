using BrahmanGan.Domain.Modulos.Seguridad;
using BrahmanGan.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ─────────────────────────────────────────────────────────────
//  Usuario
// ─────────────────────────────────────────────────────────────
public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        // Configuración del Id tipado como UsuarioId (int en el DB)
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(v => (int)v, v => UsuarioId.From(v))
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.NombreCompleto)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.PasswordHash).HasMaxLength(500);

        builder.Property(u => u.RefreshToken).HasMaxLength(500);

        builder.Property(u => u.Proveedor)
            .IsRequired();

        builder.Property(u => u.IdExterno).HasMaxLength(200);

        builder.Property(u => u.EmailConfirmado).IsRequired();
        builder.Property(u => u.Activo).IsRequired();

        builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        // Relaciones
        builder.HasMany(typeof(UsuarioRol), "_usuariosRol")
            .WithOne("Usuario")
            .HasForeignKey("UsuarioId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
