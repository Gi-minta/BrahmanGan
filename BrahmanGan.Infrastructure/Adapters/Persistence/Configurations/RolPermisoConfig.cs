using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ─────────────────────────────────────────────────────────────
//  RolPermiso  (junction table)
// ─────────────────────────────────────────────────────────────

// ─────────────────────────────────────────────────────────────
//  RolPermisoConfig
// ─────────────────────────────────────────────────────────────
internal class RolPermisoConfig : IEntityTypeConfiguration<RolPermiso>
{
    public void Configure(EntityTypeBuilder<RolPermiso> b)
    {
        b.ToTable("RolesPermisos");
        b.HasKey(x => new { x.RolId, x.PermisoId });

        // Configura claves foráneas con ValueObject converters
        b.Property(x => x.RolId)
            .HasConversion(id => id.Value, v => RolId.From(v));

        b.Property(x => x.PermisoId)
            .HasConversion(id => id.Value, v => PermisoId.From(v));

        // Relación con Permiso
        b.HasOne(x => x.Permiso)
            .WithMany(p => p.RolesPermiso)
            .HasForeignKey(x => x.PermisoId)
            .OnDelete(DeleteBehavior.Cascade);

        // 👇 FIX: Configurar Version como int con concurrencia + default
        b.Property<int>("Version")
            .IsConcurrencyToken()
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValue(1);

        // 👇 FIX: Configurar CreatedAt con default en BD para seed
        b.Property<DateTime>(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        // Seed: Administrador (RolId=1) con todos los permisos
        // ✅ No incluir Version ni CreatedAt: usan HasDefaultValue
        var seeds = Enumerable.Range(1, 84)
            .Select(pid => new
            {
                RolId = RolId.From(1),
                PermisoId = PermisoId.From(pid)
                // 🎯 Version y CreatedAt se asignan automáticamente por los defaults
            })
            .ToArray();

        b.HasData(seeds);
    }
}

//internal class RolPermisoConfig : IEntityTypeConfiguration<RolPermiso>
//{
//    public void Configure(EntityTypeBuilder<RolPermiso> b)
//    {
//        b.ToTable("RolesPermisos");
//        b.HasKey(x => new { x.RolId, x.PermisoId });

//        b.Property(x => x.RolId)
//            .HasConversion(id => id.Value, v => RolId.From(v));
//        b.Property(x => x.PermisoId)
//            .HasConversion(id => id.Value, v => PermisoId.From(v));

//        b.HasOne(x => x.Permiso)
//            .WithMany(p => p.RolesPermiso)
//            .HasForeignKey(x => x.PermisoId)
//            .OnDelete(DeleteBehavior.Cascade);

//        // El Administrador (RolId=1) recibe todos los permisos en seed.
//        // Calculamos: 14 módulos × 6 acciones = 84 permisos (ids 1..84)
//        var seeds = Enumerable.Range(1, 84)
//            .Select(pid => new { RolId = RolId.From(1), PermisoId = PermisoId.From(pid) })
//            .ToArray();
//        b.HasData(seeds);
//    }
//}
