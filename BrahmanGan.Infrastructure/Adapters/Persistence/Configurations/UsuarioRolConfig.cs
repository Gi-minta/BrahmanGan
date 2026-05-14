using BrahmanGan.Domain.Modulos.Seguridad;
using BrahmanGan.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ─────────────────────────────────────────────────────────────
//  UsuarioRol  (junction table)
// ─────────────────────────────────────────────────────────────
public class UsuarioRolConfig : IEntityTypeConfiguration<UsuarioRol>
{
    public void Configure(EntityTypeBuilder<UsuarioRol> builder)
    {
        builder.ToTable("UsuariosRoles");

        builder.HasKey("Id");

        builder.Property<int>("Id").ValueGeneratedOnAdd();

        builder.Property< int >("UsuarioId")
            .HasConversion(v => (int)v, v => UsuarioId.From(v));

        builder.Property< int >("RolId")
            .HasConversion(v => (int)v, v => RolId.From(v));

        builder.HasOne(ur => ur.Usuario)
            .WithMany("UsuariosRol")
            .HasForeignKey("UsuarioId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ur => ur.Rol)
            .WithMany("UsuariosRol")
            .HasForeignKey("RolId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
