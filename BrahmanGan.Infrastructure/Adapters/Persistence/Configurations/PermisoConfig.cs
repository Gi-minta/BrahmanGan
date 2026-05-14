using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ─────────────────────────────────────────────────────────────
//  Permiso
// ─────────────────────────────────────────────────────────────

// ─────────────────────────────────────────────────────────────
//  PermisoConfig
// ─────────────────────────────────────────────────────────────
internal class PermisoConfig : IEntityTypeConfiguration<Permiso>
{
    public void Configure(EntityTypeBuilder<Permiso> b)
    {
        b.ToTable("Permisos");
        b.HasKey(x => x.Id);

        // Configura Id con ValueObject converter
        b.Property(x => x.Id)
            .HasConversion(id => id.Value, v => PermisoId.From(v))
            .ValueGeneratedOnAdd();

        // Propiedades de dominio
        b.Property(x => x.Modulo)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        b.Property(x => x.Accion)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        b.Property(x => x.Descripcion)
            .HasMaxLength(200);

        b.Property(x => x.Activo);

        // Ignorar propiedad calculada
        b.Ignore(x => x.Clave);

        // Índice único para clave lógica Módulo+Acción
        b.HasIndex(x => new { x.Modulo, x.Accion }).IsUnique();

        // 👇 Configurar Version como int con concurrencia + default
        b.Property<int>("Version")
            .IsConcurrencyToken()
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValue(1);

        // Auditoría: CreatedAt con default en BD
        b.Property<DateTime>(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        // Seed data
        SeedPermisos(b);
    }

    private static void SeedPermisos(EntityTypeBuilder<Permiso> b)
    {
        int id = 1;
        var modulos = Enum.GetValues<ModuloSistema>();
        var acciones = Enum.GetValues<AccionPermiso>();

        foreach (var mod in modulos)
        {
            foreach (var acc in acciones)
            {
                // ✅ FIX: Usar PermisoId.From() explícitamente en el seed
                // Los ValueConverters NO se aplican automáticamente en tipos anónimos
                b.HasData(new
                {
                    Id = PermisoId.From(id++),  // 👈 ValueObject completo, no solo el int
                    Modulo = mod,
                    Accion = acc,
                    Descripcion = $"{mod} - {acc}",
                    Activo = true
                    // Version usa HasDefaultValue(1), no necesita seed explícito
                });
            }
        }
    }
}
//internal class PermisoConfig : IEntityTypeConfiguration<Permiso>
//{
//    public void Configure(EntityTypeBuilder<Permiso> b)
//    {
//        b.ToTable("Permisos");
//        b.HasKey(x => x.Id);
//        b.Property(x => x.Id)
//            .HasConversion(id => id.Value, v => PermisoId.From(v))
//            .ValueGeneratedOnAdd();

//        b.Property(x => x.Modulo)
//            .HasConversion<string>().HasMaxLength(50).IsRequired();
//        b.Property(x => x.Accion)
//            .HasConversion<string>().HasMaxLength(50).IsRequired();
//        b.Property(x => x.Descripcion).HasMaxLength(200);
//        b.Property(x => x.Activo);
//        b.Ignore(x => x.Clave);

//        b.HasIndex(x => new { x.Modulo, x.Accion }).IsUnique();

//        // Asegurar valor por defecto en BD para CreatedAt
//        b.Property<DateTime>(x => x.CreatedAt)
//            .IsRequired()
//            .HasDefaultValueSql("GETUTCDATE()")
//            .ValueGeneratedOnAdd();

//        // Seed: todos los permisos del sistema
//        SeedPermisos(b);
//    }

//    private static void SeedPermisos(EntityTypeBuilder<Permiso> b)
//    {
//        // Generamos permisos para cada módulo × cada acción.
//        // Se usan shadow-property seeds (sin navigation cargada).
//        int id = 1;
//        var modulos = Enum.GetValues<ModuloSistema>();
//        var acciones = Enum.GetValues<AccionPermiso>();
//        foreach (var mod in modulos)
//            foreach (var acc in acciones)
//                b.HasData(new { Id = PermisoId.From(id++), Modulo = mod, Accion = acc,
//                                Descripcion = $"{mod} - {acc}", Activo = true });
//    }
//}
