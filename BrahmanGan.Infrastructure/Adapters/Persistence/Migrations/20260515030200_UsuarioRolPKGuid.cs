using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioRolPKGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuariosRoles",
                table: "UsuariosRoles");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UsuariosRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "AsignadoEn",
                table: "UsuariosRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AsignadoPor",
                table: "UsuariosRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UsuariosRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EliminadoEn",
                table: "UsuariosRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UsuariosRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Proveedor",
                table: "Usuarios",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Usuarios",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuariosRoles",
                table: "UsuariosRoles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_UsuarioId",
                table: "UsuariosRoles",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuariosRoles",
                table: "UsuariosRoles");

            migrationBuilder.DropIndex(
                name: "IX_UsuariosRoles_UsuarioId",
                table: "UsuariosRoles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsuariosRoles");

            migrationBuilder.DropColumn(
                name: "AsignadoEn",
                table: "UsuariosRoles");

            migrationBuilder.DropColumn(
                name: "AsignadoPor",
                table: "UsuariosRoles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UsuariosRoles");

            migrationBuilder.DropColumn(
                name: "EliminadoEn",
                table: "UsuariosRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UsuariosRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Proveedor",
                table: "Usuarios",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Usuarios",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuariosRoles",
                table: "UsuariosRoles",
                columns: new[] { "UsuarioId", "RolId" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }
    }
}
