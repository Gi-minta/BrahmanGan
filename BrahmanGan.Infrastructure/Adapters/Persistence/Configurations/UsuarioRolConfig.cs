using BrahmanGan.Domain.Modulos.Seguridad;
using BrahmanGan.Domain.Common;
using Common;
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

        builder.HasKey(ur => ur.Id);

        builder.Property(ur => ur.Id)
            .HasConversion(
                id => id.Value,
                value => new UsuarioRolId(value));

        builder.Property(ur => ur.UsuarioId)
            .HasConversion(
                id => id.Value,
                v => UsuarioId.From(v));

        builder.Property(ur => ur.RolId)
            .HasConversion(
                id => id.Value,
                v => RolId.From(v));

    }
}
