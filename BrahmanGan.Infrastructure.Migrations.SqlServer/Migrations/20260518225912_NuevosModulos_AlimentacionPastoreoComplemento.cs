using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NuevosModulos_AlimentacionPastoreoComplemento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Complementos",
                columns: table => new
                {
                    IdComplemento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTratamiento = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Costo = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complementos", x => x.IdComplemento);
                });

            migrationBuilder.CreateTable(
                name: "DetallePlanAlimentacion",
                columns: table => new
                {
                    IdDetallePlan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPlanAlimentacion = table.Column<int>(type: "int", nullable: false),
                    IdInsumo = table.Column<int>(type: "int", nullable: true),
                    Alimento = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CantidadDiaria = table.Column<decimal>(type: "decimal(10,3)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallePlanAlimentacion", x => x.IdDetallePlan);
                });

            migrationBuilder.CreateTable(
                name: "PlanesAlimentacion",
                columns: table => new
                {
                    IdPlanAlimentacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFinca = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesAlimentacion", x => x.IdPlanAlimentacion);
                });

            migrationBuilder.CreateTable(
                name: "PlanesPastoreo",
                columns: table => new
                {
                    IdPlanPastoreo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPotrero = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    NumAnimales = table.Column<int>(type: "int", nullable: true),
                    CapacidadCarga = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesPastoreo", x => x.IdPlanPastoreo);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Complementos");

            migrationBuilder.DropTable(
                name: "DetallePlanAlimentacion");

            migrationBuilder.DropTable(
                name: "PlanesAlimentacion");

            migrationBuilder.DropTable(
                name: "PlanesPastoreo");
        }
    }
}
