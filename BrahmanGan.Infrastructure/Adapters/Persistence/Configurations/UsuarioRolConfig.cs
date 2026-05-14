using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ─────────────────────────────────────────────────────────────
//  UsuarioRol  (junction table)
// ─────────────────────────────────────────────────────────────
internal class UsuarioRolConfig : IEntityTypeConfiguration<UsuarioRol>
{
    public void Configure(EntityTypeBuilder<UsuarioRol> b)
    {
        b.ToTable("UsuariosRoles");
        b.HasKey(x => new { x.UsuarioId, x.RolId });

        b.Property(x => x.UsuarioId)
            .HasConversion(id => id.Value, v => UsuarioId.From(v));
        b.Property(x => x.RolId)
            .HasConversion(id => id.Value, v => RolId.From(v));

        b.HasOne(x => x.Rol)
            .WithMany(r => r.UsuariosRol)
            .HasForeignKey(x => x.RolId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
