using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

// ─────────────────────────────────────────────────────────────
//  RolConfig
// ─────────────────────────────────────────────────────────────
internal class RolConfig : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> b)
    {
        b.ToTable("Roles");
        b.HasKey(x => x.Id);

        // Configura Id con ValueObject converter
        b.Property(x => x.Id)
            .HasConversion(id => id.Value, v => RolId.From(v))
            .ValueGeneratedOnAdd();

        // Propiedades de dominio
        b.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        b.Property(x => x.Descripcion)
            .HasMaxLength(300);

        b.Property(x => x.EsSistema);
        b.Property(x => x.Activo);

        // Índice único para Nombre
        b.HasIndex(x => x.Nombre).IsUnique();

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

        // Seed roles de sistema
        b.HasMany(r => r.UsuariosRol)
            .WithOne(ur => ur.Rol)
            .HasForeignKey(ur => ur.RolId)
            .OnDelete(DeleteBehavior.Cascade);

        // ✅ No incluir Version ni CreatedAt: usan HasDefaultValue
        b.HasData(
            new { Id = RolId.From(1), Nombre = "Administrador", Descripcion = "Acceso total al sistema", EsSistema = true, Activo = true },
            new { Id = RolId.From(2), Nombre = "Gerente", Descripcion = "Gestión y reportes", EsSistema = true, Activo = true },
            new { Id = RolId.From(3), Nombre = "Veterinario", Descripcion = "Salud y reproducción animal", EsSistema = true, Activo = true },
            new { Id = RolId.From(4), Nombre = "Operador", Descripcion = "Registro de actividades diarias", EsSistema = true, Activo = true },
            new { Id = RolId.From(5), Nombre = "Auditor", Descripcion = "Consulta y trazabilidad (solo lectura)", EsSistema = true, Activo = true }
        );
    }
}

//// ─────────────────────────────────────────────────────────────
////  Rol
//// ─────────────────────────────────────────────────────────────
//internal class RolConfig : IEntityTypeConfiguration<Rol>
//{
//    public void Configure(EntityTypeBuilder<Rol> b)
//    {
//        b.ToTable("Roles");
//        b.HasKey(x => x.Id);
//        b.Property(x => x.Id)
//            .HasConversion(id => id.Value, v => RolId.From(v))
//            .ValueGeneratedOnAdd();

//        b.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
//        b.Property(x => x.Descripcion).HasMaxLength(300);
//        b.Property(x => x.EsSistema);
//        b.Property(x => x.Activo);

//        b.HasIndex(x => x.Nombre).IsUnique();

//        // Seed roles de sistema
//        b.HasData(
//            new { Id = RolId.From(1), Nombre = "Administrador", Descripcion = "Acceso total al sistema", EsSistema = true, Activo = true },
//            new { Id = RolId.From(2), Nombre = "Gerente",        Descripcion = "Gestión y reportes", EsSistema = true, Activo = true },
//            new { Id = RolId.From(3), Nombre = "Veterinario",    Descripcion = "Salud y reproducción animal", EsSistema = true, Activo = true },
//            new { Id = RolId.From(4), Nombre = "Operador",       Descripcion = "Registro de actividades diarias", EsSistema = true, Activo = true },
//            new { Id = RolId.From(5), Nombre = "Auditor",        Descripcion = "Consulta y trazabilidad (solo lectura)", EsSistema = true, Activo = true }
//        );
//    }
//}
