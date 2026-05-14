using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PermisoCreatedAtDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modulo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", rowVersion: true, nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    EsSistema = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", rowVersion: true, nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefreshTokenExpira = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Proveedor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IdExterno = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailConfirmado = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolesPermisos",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "int", nullable: false),
                    PermisoId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Version = table.Column<int>(type: "int", rowVersion: true, nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesPermisos", x => new { x.RolId, x.PermisoId });
                    table.ForeignKey(
                        name: "FK_RolesPermisos_Permisos_PermisoId",
                        column: x => x.PermisoId,
                        principalTable: "Permisos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesPermisos_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosRoles",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosRoles", x => new { x.UsuarioId, x.RolId });
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permisos",
                columns: new[] { "Id", "Accion", "Activo", "Descripcion", "Modulo", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Leer", true, "Inventario - Leer", "Inventario", null },
                    { 2, "Crear", true, "Inventario - Crear", "Inventario", null },
                    { 3, "Editar", true, "Inventario - Editar", "Inventario", null },
                    { 4, "Eliminar", true, "Inventario - Eliminar", "Inventario", null },
                    { 5, "Exportar", true, "Inventario - Exportar", "Inventario", null },
                    { 6, "Administrar", true, "Inventario - Administrar", "Inventario", null },
                    { 7, "Leer", true, "Finca - Leer", "Finca", null },
                    { 8, "Crear", true, "Finca - Crear", "Finca", null },
                    { 9, "Editar", true, "Finca - Editar", "Finca", null },
                    { 10, "Eliminar", true, "Finca - Eliminar", "Finca", null },
                    { 11, "Exportar", true, "Finca - Exportar", "Finca", null },
                    { 12, "Administrar", true, "Finca - Administrar", "Finca", null },
                    { 13, "Leer", true, "Reproduccion - Leer", "Reproduccion", null },
                    { 14, "Crear", true, "Reproduccion - Crear", "Reproduccion", null },
                    { 15, "Editar", true, "Reproduccion - Editar", "Reproduccion", null },
                    { 16, "Eliminar", true, "Reproduccion - Eliminar", "Reproduccion", null },
                    { 17, "Exportar", true, "Reproduccion - Exportar", "Reproduccion", null },
                    { 18, "Administrar", true, "Reproduccion - Administrar", "Reproduccion", null },
                    { 19, "Leer", true, "Sanidad - Leer", "Sanidad", null },
                    { 20, "Crear", true, "Sanidad - Crear", "Sanidad", null },
                    { 21, "Editar", true, "Sanidad - Editar", "Sanidad", null },
                    { 22, "Eliminar", true, "Sanidad - Eliminar", "Sanidad", null },
                    { 23, "Exportar", true, "Sanidad - Exportar", "Sanidad", null },
                    { 24, "Administrar", true, "Sanidad - Administrar", "Sanidad", null },
                    { 25, "Leer", true, "Leche - Leer", "Leche", null },
                    { 26, "Crear", true, "Leche - Crear", "Leche", null },
                    { 27, "Editar", true, "Leche - Editar", "Leche", null },
                    { 28, "Eliminar", true, "Leche - Eliminar", "Leche", null },
                    { 29, "Exportar", true, "Leche - Exportar", "Leche", null },
                    { 30, "Administrar", true, "Leche - Administrar", "Leche", null },
                    { 31, "Leer", true, "Comercial - Leer", "Comercial", null },
                    { 32, "Crear", true, "Comercial - Crear", "Comercial", null },
                    { 33, "Editar", true, "Comercial - Editar", "Comercial", null },
                    { 34, "Eliminar", true, "Comercial - Eliminar", "Comercial", null },
                    { 35, "Exportar", true, "Comercial - Exportar", "Comercial", null },
                    { 36, "Administrar", true, "Comercial - Administrar", "Comercial", null },
                    { 37, "Leer", true, "Costos - Leer", "Costos", null },
                    { 38, "Crear", true, "Costos - Crear", "Costos", null },
                    { 39, "Editar", true, "Costos - Editar", "Costos", null },
                    { 40, "Eliminar", true, "Costos - Eliminar", "Costos", null },
                    { 41, "Exportar", true, "Costos - Exportar", "Costos", null },
                    { 42, "Administrar", true, "Costos - Administrar", "Costos", null },
                    { 43, "Leer", true, "Nomina - Leer", "Nomina", null },
                    { 44, "Crear", true, "Nomina - Crear", "Nomina", null },
                    { 45, "Editar", true, "Nomina - Editar", "Nomina", null },
                    { 46, "Eliminar", true, "Nomina - Eliminar", "Nomina", null },
                    { 47, "Exportar", true, "Nomina - Exportar", "Nomina", null },
                    { 48, "Administrar", true, "Nomina - Administrar", "Nomina", null },
                    { 49, "Leer", true, "Almacen - Leer", "Almacen", null },
                    { 50, "Crear", true, "Almacen - Crear", "Almacen", null },
                    { 51, "Editar", true, "Almacen - Editar", "Almacen", null },
                    { 52, "Eliminar", true, "Almacen - Eliminar", "Almacen", null },
                    { 53, "Exportar", true, "Almacen - Exportar", "Almacen", null },
                    { 54, "Administrar", true, "Almacen - Administrar", "Almacen", null },
                    { 55, "Leer", true, "Equipos - Leer", "Equipos", null },
                    { 56, "Crear", true, "Equipos - Crear", "Equipos", null },
                    { 57, "Editar", true, "Equipos - Editar", "Equipos", null },
                    { 58, "Eliminar", true, "Equipos - Eliminar", "Equipos", null },
                    { 59, "Exportar", true, "Equipos - Exportar", "Equipos", null },
                    { 60, "Administrar", true, "Equipos - Administrar", "Equipos", null },
                    { 61, "Leer", true, "Trazabilidad - Leer", "Trazabilidad", null },
                    { 62, "Crear", true, "Trazabilidad - Crear", "Trazabilidad", null },
                    { 63, "Editar", true, "Trazabilidad - Editar", "Trazabilidad", null },
                    { 64, "Eliminar", true, "Trazabilidad - Eliminar", "Trazabilidad", null },
                    { 65, "Exportar", true, "Trazabilidad - Exportar", "Trazabilidad", null },
                    { 66, "Administrar", true, "Trazabilidad - Administrar", "Trazabilidad", null },
                    { 67, "Leer", true, "Sostenibilidad - Leer", "Sostenibilidad", null },
                    { 68, "Crear", true, "Sostenibilidad - Crear", "Sostenibilidad", null },
                    { 69, "Editar", true, "Sostenibilidad - Editar", "Sostenibilidad", null },
                    { 70, "Eliminar", true, "Sostenibilidad - Eliminar", "Sostenibilidad", null },
                    { 71, "Exportar", true, "Sostenibilidad - Exportar", "Sostenibilidad", null },
                    { 72, "Administrar", true, "Sostenibilidad - Administrar", "Sostenibilidad", null },
                    { 73, "Leer", true, "Seguridad - Leer", "Seguridad", null },
                    { 74, "Crear", true, "Seguridad - Crear", "Seguridad", null },
                    { 75, "Editar", true, "Seguridad - Editar", "Seguridad", null },
                    { 76, "Eliminar", true, "Seguridad - Eliminar", "Seguridad", null },
                    { 77, "Exportar", true, "Seguridad - Exportar", "Seguridad", null },
                    { 78, "Administrar", true, "Seguridad - Administrar", "Seguridad", null },
                    { 79, "Leer", true, "Reportes - Leer", "Reportes", null },
                    { 80, "Crear", true, "Reportes - Crear", "Reportes", null },
                    { 81, "Editar", true, "Reportes - Editar", "Reportes", null },
                    { 82, "Eliminar", true, "Reportes - Eliminar", "Reportes", null },
                    { 83, "Exportar", true, "Reportes - Exportar", "Reportes", null },
                    { 84, "Administrar", true, "Reportes - Administrar", "Reportes", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Activo", "Descripcion", "EsSistema", "Nombre", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, true, "Acceso total al sistema", true, "Administrador", null },
                    { 2, true, "Gestión y reportes", true, "Gerente", null },
                    { 3, true, "Salud y reproducción animal", true, "Veterinario", null },
                    { 4, true, "Registro de actividades diarias", true, "Operador", null },
                    { 5, true, "Consulta y trazabilidad (solo lectura)", true, "Auditor", null }
                });

            migrationBuilder.InsertData(
                table: "RolesPermisos",
                columns: new[] { "PermisoId", "RolId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 },
                    { 8, 1 },
                    { 9, 1 },
                    { 10, 1 },
                    { 11, 1 },
                    { 12, 1 },
                    { 13, 1 },
                    { 14, 1 },
                    { 15, 1 },
                    { 16, 1 },
                    { 17, 1 },
                    { 18, 1 },
                    { 19, 1 },
                    { 20, 1 },
                    { 21, 1 },
                    { 22, 1 },
                    { 23, 1 },
                    { 24, 1 },
                    { 25, 1 },
                    { 26, 1 },
                    { 27, 1 },
                    { 28, 1 },
                    { 29, 1 },
                    { 30, 1 },
                    { 31, 1 },
                    { 32, 1 },
                    { 33, 1 },
                    { 34, 1 },
                    { 35, 1 },
                    { 36, 1 },
                    { 37, 1 },
                    { 38, 1 },
                    { 39, 1 },
                    { 40, 1 },
                    { 41, 1 },
                    { 42, 1 },
                    { 43, 1 },
                    { 44, 1 },
                    { 45, 1 },
                    { 46, 1 },
                    { 47, 1 },
                    { 48, 1 },
                    { 49, 1 },
                    { 50, 1 },
                    { 51, 1 },
                    { 52, 1 },
                    { 53, 1 },
                    { 54, 1 },
                    { 55, 1 },
                    { 56, 1 },
                    { 57, 1 },
                    { 58, 1 },
                    { 59, 1 },
                    { 60, 1 },
                    { 61, 1 },
                    { 62, 1 },
                    { 63, 1 },
                    { 64, 1 },
                    { 65, 1 },
                    { 66, 1 },
                    { 67, 1 },
                    { 68, 1 },
                    { 69, 1 },
                    { 70, 1 },
                    { 71, 1 },
                    { 72, 1 },
                    { 73, 1 },
                    { 74, 1 },
                    { 75, 1 },
                    { 76, 1 },
                    { 77, 1 },
                    { 78, 1 },
                    { 79, 1 },
                    { 80, 1 },
                    { 81, 1 },
                    { 82, 1 },
                    { 83, 1 },
                    { 84, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_Modulo_Accion",
                table: "Permisos",
                columns: new[] { "Modulo", "Accion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Nombre",
                table: "Roles",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolesPermisos_PermisoId",
                table: "RolesPermisos",
                column: "PermisoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_RolId",
                table: "UsuariosRoles",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolesPermisos");

            migrationBuilder.DropTable(
                name: "UsuariosRoles");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
