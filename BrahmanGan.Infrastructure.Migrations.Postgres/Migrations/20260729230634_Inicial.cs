using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BrahmanGan.Infrastructure.Migrations.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activos",
                columns: table => new
                {
                    IdActivo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCentro = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    FechaCompra = table.Column<DateOnly>(type: "date", nullable: true),
                    ValorCompra = table.Column<decimal>(type: "numeric(15,2)", nullable: true),
                    VidaUtilAnios = table.Column<int>(type: "integer", nullable: true),
                    ValorResidual = table.Column<decimal>(type: "numeric(15,2)", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activos", x => x.IdActivo);
                });

            migrationBuilder.CreateTable(
                name: "AcumulacionInsumosPotrero",
                columns: table => new
                {
                    IdAcumulacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPotrero = table.Column<int>(type: "integer", nullable: false),
                    IdInsumo = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "numeric(12,4)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcumulacionInsumosPotrero", x => x.IdAcumulacion);
                });

            migrationBuilder.CreateTable(
                name: "Animales",
                columns: table => new
                {
                    IdAnimal = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    IdRaza = table.Column<int>(type: "integer", nullable: false),
                    Sexo = table.Column<string>(type: "char(1)", nullable: false),
                    FechaNacimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    IdMadre = table.Column<int>(type: "integer", nullable: true),
                    IdPadre = table.Column<int>(type: "integer", nullable: true),
                    IdOrigen = table.Column<int>(type: "integer", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PesoNacimiento = table.Column<decimal>(type: "numeric(7,2)", nullable: true),
                    IdFinca = table.Column<int>(type: "integer", nullable: false),
                    FechaIngreso = table.Column<DateOnly>(type: "date", nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "LOCALTIMESTAMP"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animales", x => x.IdAnimal);
                });

            migrationBuilder.CreateTable(
                name: "AnimalPotrero",
                columns: table => new
                {
                    IdAsignacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    IdPotrero = table.Column<int>(type: "integer", nullable: false),
                    FechaIngreso = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaSalida = table.Column<DateOnly>(type: "date", nullable: true),
                    IdGrupo = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalPotrero", x => x.IdAsignacion);
                });

            migrationBuilder.CreateTable(
                name: "Autoconsumos",
                columns: table => new
                {
                    IdAutoconsumo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    IdCentro = table.Column<int>(type: "integer", nullable: false),
                    Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    ValorUnitario = table.Column<decimal>(type: "numeric(12,4)", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autoconsumos", x => x.IdAutoconsumo);
                });

            migrationBuilder.CreateTable(
                name: "CalidadLeche",
                columns: table => new
                {
                    IdMuestra = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: true),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    CelSomaticas = table.Column<int>(type: "integer", nullable: true),
                    GrasaPct = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    ProteinaPct = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    LactozaPct = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    UreaMgDL = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    Laboratorio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Resultado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalidadLeche", x => x.IdMuestra);
                });

            migrationBuilder.CreateTable(
                name: "CapturaCarbono",
                columns: table => new
                {
                    IdRegistro = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdFinca = table.Column<int>(type: "integer", nullable: false),
                    Anio = table.Column<int>(type: "integer", nullable: false),
                    Mes = table.Column<int>(type: "integer", nullable: false),
                    EmisionesGanadoTCO2 = table.Column<decimal>(type: "numeric(10,4)", nullable: true),
                    CapturaForestal = table.Column<decimal>(type: "numeric(10,4)", nullable: true),
                    Certificacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapturaCarbono", x => x.IdRegistro);
                });

            migrationBuilder.CreateTable(
                name: "CentrosCosto",
                columns: table => new
                {
                    IdCentro = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IdFinca = table.Column<int>(type: "integer", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentrosCosto", x => x.IdCentro);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoDocumento = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Documento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RazonSocial = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Contacto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IdMunicipio = table.Column<int>(type: "integer", nullable: true),
                    TipoCliente = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Complementos",
                columns: table => new
                {
                    IdComplemento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTratamiento = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Costo = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complementos", x => x.IdComplemento);
                });

            migrationBuilder.CreateTable(
                name: "ConsumosAgua",
                columns: table => new
                {
                    IdConsumo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdFinca = table.Column<int>(type: "integer", nullable: false),
                    IdPotrero = table.Column<int>(type: "integer", nullable: true),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    FuenteAgua = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VolumenM3 = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    NumAnimales = table.Column<int>(type: "integer", nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumosAgua", x => x.IdConsumo);
                });

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    IdContrato = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCliente = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    PrecioAcordado = table.Column<decimal>(type: "numeric(12,4)", nullable: true),
                    UnidadPrecio = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    VolumenEstimado = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    Condiciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.IdContrato);
                });

            migrationBuilder.CreateTable(
                name: "ControlesPreventivos",
                columns: table => new
                {
                    IdControl = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Periodicidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlesPreventivos", x => x.IdControl);
                });

            migrationBuilder.CreateTable(
                name: "ControlLecheAnimal",
                columns: table => new
                {
                    IdControl = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Ordeno = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    LitrosMañana = table.Column<decimal>(type: "numeric(7,3)", nullable: true),
                    LitrosTarde = table.Column<decimal>(type: "numeric(7,3)", nullable: true),
                    LitrosNoche = table.Column<decimal>(type: "numeric(7,3)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlLecheAnimal", x => x.IdControl);
                });

            migrationBuilder.CreateTable(
                name: "CostosAnimalesDiarios",
                columns: table => new
                {
                    IdCostoAnimal = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    TipoCosto = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(12,4)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostosAnimalesDiarios", x => x.IdCostoAnimal);
                });

            migrationBuilder.CreateTable(
                name: "CostosDiarios",
                columns: table => new
                {
                    IdCosto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    IdCentro = table.Column<int>(type: "integer", nullable: false),
                    TipoCosto = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Valor = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostosDiarios", x => x.IdCosto);
                });

            migrationBuilder.CreateTable(
                name: "CotizacionesVenta",
                columns: table => new
                {
                    IdCotizacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCliente = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaVigencia = table.Column<DateOnly>(type: "date", nullable: true),
                    PrecioOfertado = table.Column<decimal>(type: "numeric(12,4)", nullable: false),
                    UnidadPrecio = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CotizacionesVenta", x => x.IdCotizacion);
                });

            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    IdDepto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPais = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.IdDepto);
                });

            migrationBuilder.CreateTable(
                name: "DetallePlanAlimentacion",
                columns: table => new
                {
                    IdDetallePlan = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPlanAlimentacion = table.Column<int>(type: "integer", nullable: false),
                    IdInsumo = table.Column<int>(type: "integer", nullable: true),
                    Alimento = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CantidadDiaria = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallePlanAlimentacion", x => x.IdDetallePlan);
                });

            migrationBuilder.CreateTable(
                name: "EventosMedioambientales",
                columns: table => new
                {
                    IdEvento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdFinca = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    TipoEvento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Intensidad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PrecipitacionMM = table.Column<decimal>(type: "numeric(6,1)", nullable: true),
                    TempMaxC = table.Column<decimal>(type: "numeric(4,1)", nullable: true),
                    TempMinC = table.Column<decimal>(type: "numeric(4,1)", nullable: true),
                    ImpactoEstimado = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosMedioambientales", x => x.IdEvento);
                });

            migrationBuilder.CreateTable(
                name: "Finca",
                columns: table => new
                {
                    IdFinca = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NIT = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Propietario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IdMunicipio = table.Column<int>(type: "integer", nullable: true),
                    AreaHectareas = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    Activa = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "LOCALTIMESTAMP"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finca", x => x.IdFinca);
                });

            migrationBuilder.CreateTable(
                name: "GastosGenerales",
                columns: table => new
                {
                    IdGasto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    IdCentro = table.Column<int>(type: "integer", nullable: true),
                    Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    Proveedor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Comprobante = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastosGenerales", x => x.IdGasto);
                });

            migrationBuilder.CreateTable(
                name: "Gestaciones",
                columns: table => new
                {
                    IdGestacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    IdServicio = table.Column<int>(type: "integer", nullable: true),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaPartoEstimado = table.Column<DateOnly>(type: "date", nullable: true),
                    FechaPartoReal = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gestaciones", x => x.IdGestacion);
                });

            migrationBuilder.CreateTable(
                name: "GruposManejo",
                columns: table => new
                {
                    IdGrupo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    TipoAnimal = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruposManejo", x => x.IdGrupo);
                });

            migrationBuilder.CreateTable(
                name: "HistorialAnimales",
                columns: table => new
                {
                    IdHistorial = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    TipoEvento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Valor = table.Column<decimal>(type: "numeric(15,2)", nullable: true),
                    UsuarioRegistro = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "LOCALTIMESTAMP"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialAnimales", x => x.IdHistorial);
                });

            migrationBuilder.CreateTable(
                name: "HistorialCurativo",
                columns: table => new
                {
                    IdTratamiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    Diagnostico = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    Veterinario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Resultado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CostoTotal = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialCurativo", x => x.IdTratamiento);
                });

            migrationBuilder.CreateTable(
                name: "HistorialDesparasitacion",
                columns: table => new
                {
                    IdDesparasitacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    IdMedicamento = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Dosis = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    TipoParasito = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ProximaFecha = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialDesparasitacion", x => x.IdDesparasitacion);
                });

            migrationBuilder.CreateTable(
                name: "HistorialMastitis",
                columns: table => new
                {
                    IdMastitis = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Cuarto = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    GradoInfeccion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IdTratamiento = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialMastitis", x => x.IdMastitis);
                });

            migrationBuilder.CreateTable(
                name: "HistorialPreventivo",
                columns: table => new
                {
                    IdHistorial = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    IdControl = table.Column<int>(type: "integer", nullable: false),
                    IdMedicamento = table.Column<int>(type: "integer", nullable: true),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Dosis = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    Responsable = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    ProximaFecha = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialPreventivo", x => x.IdHistorial);
                });

            migrationBuilder.CreateTable(
                name: "HistorialVacunacion",
                columns: table => new
                {
                    IdVacunacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    IdMedicamento = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Dosis = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    Lote = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Responsable = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    ProximaFecha = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialVacunacion", x => x.IdVacunacion);
                });

            migrationBuilder.CreateTable(
                name: "Ingresos",
                columns: table => new
                {
                    IdIngreso = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    IdCentro = table.Column<int>(type: "integer", nullable: false),
                    TipoIngreso = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Valor = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    Comprobante = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingresos", x => x.IdIngreso);
                });

            migrationBuilder.CreateTable(
                name: "Insumos",
                columns: table => new
                {
                    IdInsumo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UnidadMedida = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PrecioUnitario = table.Column<decimal>(type: "numeric(12,4)", nullable: true),
                    StockMinimo = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    StockActual = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insumos", x => x.IdInsumo);
                });

            migrationBuilder.CreateTable(
                name: "KardexInsumos",
                columns: table => new
                {
                    IdMovimiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdInsumo = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Cantidad = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "numeric(12,4)", nullable: true),
                    Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Referencia = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SaldoAnterior = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    SaldoNuevo = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KardexInsumos", x => x.IdMovimiento);
                });

            migrationBuilder.CreateTable(
                name: "MantenimientoEquipos",
                columns: table => new
                {
                    IdMantenimiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdMaquinaria = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    TipoMantenimiento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Tecnico = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Proveedor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CostoManoObra = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    CostoRepuestos = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    ProximoMantenimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    HorasAlMomento = table.Column<decimal>(type: "numeric(10,1)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MantenimientoEquipos", x => x.IdMantenimiento);
                });

            migrationBuilder.CreateTable(
                name: "Maquinaria",
                columns: table => new
                {
                    IdMaquinaria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCentro = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Marca = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Modelo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Anio = table.Column<int>(type: "integer", nullable: true),
                    NumeroSerie = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    FechaCompra = table.Column<DateOnly>(type: "date", nullable: true),
                    ValorCompra = table.Column<decimal>(type: "numeric(15,2)", nullable: true),
                    HorasUso = table.Column<decimal>(type: "numeric(10,1)", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquinaria", x => x.IdMaquinaria);
                });

            migrationBuilder.CreateTable(
                name: "Marcaciones",
                columns: table => new
                {
                    IdMarcacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    TipoMarcacion = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Codigo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    FechaAplicacion = table.Column<DateOnly>(type: "date", nullable: false),
                    Responsable = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Activa = table.Column<bool>(type: "boolean", nullable: false),
                    FechaBaja = table.Column<DateOnly>(type: "date", nullable: true),
                    MotivoBaja = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcaciones", x => x.IdMarcacion);
                });

            migrationBuilder.CreateTable(
                name: "Medicamentos",
                columns: table => new
                {
                    IdMedicamento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Principio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TipoUso = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Unidad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PrecioUnitario = table.Column<decimal>(type: "numeric(12,4)", nullable: true),
                    TiempoCarne = table.Column<int>(type: "integer", nullable: true),
                    TiempoLeche = table.Column<int>(type: "integer", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicamentos", x => x.IdMedicamento);
                });

            migrationBuilder.CreateTable(
                name: "MovimientosAnimales",
                columns: table => new
                {
                    IdMovimiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(15,2)", nullable: true),
                    IdCentro = table.Column<int>(type: "integer", nullable: true),
                    PesoKg = table.Column<decimal>(type: "numeric(7,2)", nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosAnimales", x => x.IdMovimiento);
                });

            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    IdMunicipio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdDepto = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.IdMunicipio);
                });

            migrationBuilder.CreateTable(
                name: "Nacimientos",
                columns: table => new
                {
                    IdNacimiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdGestacion = table.Column<int>(type: "integer", nullable: false),
                    IdAnimalCria = table.Column<int>(type: "integer", nullable: true),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Sexo = table.Column<char>(type: "char(1)", nullable: true),
                    PesoNacimiento = table.Column<decimal>(type: "numeric(7,2)", nullable: true),
                    Condicion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nacimientos", x => x.IdNacimiento);
                });

            migrationBuilder.CreateTable(
                name: "Origen",
                columns: table => new
                {
                    IdOrigen = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origen", x => x.IdOrigen);
                });

            migrationBuilder.CreateTable(
                name: "PagosJornales",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTrabajador = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    HorasTrabajadas = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    ValorJornal = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IdCentro = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosJornales", x => x.IdPago);
                });

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    IdPais = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.IdPais);
                });

            migrationBuilder.CreateTable(
                name: "ParametrosLactancia",
                columns: table => new
                {
                    IdParametro = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    NumeroParto = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    LitrosTotales = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametrosLactancia", x => x.IdParametro);
                });

            migrationBuilder.CreateTable(
                name: "Pedigri",
                columns: table => new
                {
                    IdPedigri = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    IdAbuelo1 = table.Column<int>(type: "integer", nullable: true),
                    IdAbuela1 = table.Column<int>(type: "integer", nullable: true),
                    IdAbuelo2 = table.Column<int>(type: "integer", nullable: true),
                    IdAbuela2 = table.Column<int>(type: "integer", nullable: true),
                    PuntajeMorfologia = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedigri", x => x.IdPedigri);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Modulo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Accion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", rowVersion: true, nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pesajes",
                columns: table => new
                {
                    IdPesaje = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    PesoKg = table.Column<decimal>(type: "numeric(7,2)", nullable: false),
                    CondicionCorporal = table.Column<decimal>(type: "numeric(3,1)", nullable: true),
                    MetodoPesaje = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Responsable = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pesajes", x => x.IdPesaje);
                });

            migrationBuilder.CreateTable(
                name: "PlanesAlimentacion",
                columns: table => new
                {
                    IdPlanAlimentacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdFinca = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesAlimentacion", x => x.IdPlanAlimentacion);
                });

            migrationBuilder.CreateTable(
                name: "PlanesPastoreo",
                columns: table => new
                {
                    IdPlanPastoreo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPotrero = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    NumAnimales = table.Column<int>(type: "integer", nullable: true),
                    CapacidadCarga = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesPastoreo", x => x.IdPlanPastoreo);
                });

            migrationBuilder.CreateTable(
                name: "Potreros",
                columns: table => new
                {
                    IdPotrero = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdFinca = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    AreaHectareas = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    TipoPasto = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Potreros", x => x.IdPotrero);
                });

            migrationBuilder.CreateTable(
                name: "PrestacionesSociales",
                columns: table => new
                {
                    IdPrestacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTrabajador = table.Column<int>(type: "integer", nullable: false),
                    Anio = table.Column<int>(type: "integer", nullable: false),
                    Mes = table.Column<int>(type: "integer", nullable: false),
                    SalarioBase = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    Cesantias = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    Vacaciones = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    PrimaServicio = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    SaludEmpleador = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    PensionEmpleador = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    ARL = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    CajaCompensacion = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    SENA = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    ICBF = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestacionesSociales", x => x.IdPrestacion);
                });

            migrationBuilder.CreateTable(
                name: "ProduccionLeche",
                columns: table => new
                {
                    IdProduccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdFinca = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalLitros = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    LitrosVendidos = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    LitrosAutoconsumo = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    LitrosMerma = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduccionLeche", x => x.IdProduccion);
                });

            migrationBuilder.CreateTable(
                name: "Razas",
                columns: table => new
                {
                    IdRaza = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Proposito = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Razas", x => x.IdRaza);
                });

            migrationBuilder.CreateTable(
                name: "RegistroICA",
                columns: table => new
                {
                    IdRegistro = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    TipoDocumento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumeroDocumento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FechaExpedicion = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaVencimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    EntidadEmisora = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IdMunicipio = table.Column<int>(type: "integer", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UrlDocumento = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroICA", x => x.IdRegistro);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    EsSistema = table.Column<bool>(type: "boolean", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", rowVersion: true, nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Semen",
                columns: table => new
                {
                    IdSemen = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NombreToro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IdRaza = table.Column<int>(type: "integer", nullable: true),
                    Casa = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    StockDosis = table.Column<int>(type: "integer", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semen", x => x.IdSemen);
                });

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    IdServicio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdHembra = table.Column<int>(type: "integer", nullable: false),
                    TipoServicio = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    IdToro = table.Column<int>(type: "integer", nullable: true),
                    IdSemen = table.Column<int>(type: "integer", nullable: true),
                    ResultadoPreniez = table.Column<bool>(type: "boolean", nullable: true),
                    FechaConfirmacion = table.Column<DateOnly>(type: "date", nullable: true),
                    Responsable = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.IdServicio);
                });

            migrationBuilder.CreateTable(
                name: "Trabajadores",
                columns: table => new
                {
                    IdTrabajador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cedula = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nombres = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Apellidos = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Cargo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    FechaIngreso = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaRetiro = table.Column<DateOnly>(type: "date", nullable: true),
                    SalarioBase = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    TipoContrato = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajadores", x => x.IdTrabajador);
                });

            migrationBuilder.CreateTable(
                name: "TransferenciasCosto",
                columns: table => new
                {
                    IdTransferencia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    IdCentroOrigen = table.Column<int>(type: "integer", nullable: false),
                    IdCentroDestino = table.Column<int>(type: "integer", nullable: false),
                    Concepto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(15,2)", nullable: false),
                    Aprobado = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferenciasCosto", x => x.IdTransferencia);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NombreCompleto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RefreshToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RefreshTokenExpira = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Proveedor = table.Column<int>(type: "integer", nullable: false),
                    IdExterno = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EmailConfirmado = table.Column<bool>(type: "boolean", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VentasLeche",
                columns: table => new
                {
                    IdVenta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    IdCliente = table.Column<int>(type: "integer", nullable: false),
                    IdContrato = table.Column<int>(type: "integer", nullable: true),
                    LitrosVendidos = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    PrecioLitro = table.Column<decimal>(type: "numeric(10,4)", nullable: false),
                    Factura = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentasLeche", x => x.IdVenta);
                });

            migrationBuilder.CreateTable(
                name: "ZonaFinca",
                columns: table => new
                {
                    IdZonaFinca = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdZona = table.Column<int>(type: "integer", nullable: false),
                    IdFinca = table.Column<int>(type: "integer", nullable: false),
                    FechaIngreso = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaSalida = table.Column<DateOnly>(type: "date", nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonaFinca", x => x.IdZonaFinca);
                });

            migrationBuilder.CreateTable(
                name: "Zonas",
                columns: table => new
                {
                    IdZona = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Activa = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zonas", x => x.IdZona);
                });

            migrationBuilder.CreateTable(
                name: "DetalleCotizacion",
                columns: table => new
                {
                    IdDetalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCotizacion = table.Column<int>(type: "integer", nullable: false),
                    IdAnimal = table.Column<int>(type: "integer", nullable: false),
                    PesoEstimadoKg = table.Column<decimal>(type: "numeric(7,2)", nullable: true),
                    PrecioUnitario = table.Column<decimal>(type: "numeric(12,4)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleCotizacion", x => x.IdDetalle);
                    table.ForeignKey(
                        name: "FK_DetalleCotizacion_CotizacionesVenta_IdCotizacion",
                        column: x => x.IdCotizacion,
                        principalTable: "CotizacionesVenta",
                        principalColumn: "IdCotizacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesCurativos",
                columns: table => new
                {
                    IdDetalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTratamiento = table.Column<int>(type: "integer", nullable: false),
                    IdMedicamento = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Dosis = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    CostoUnitario = table.Column<decimal>(type: "numeric(12,4)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesCurativos", x => x.IdDetalle);
                    table.ForeignKey(
                        name: "FK_DetallesCurativos_HistorialCurativo_IdTratamiento",
                        column: x => x.IdTratamiento,
                        principalTable: "HistorialCurativo",
                        principalColumn: "IdTratamiento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolesPermisos",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    PermisoId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    Version = table.Column<int>(type: "integer", rowVersion: true, nullable: false, defaultValue: 1)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    AsignadoEn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AsignadoPor = table.Column<string>(type: "text", nullable: true),
                    EliminadoEn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosRoles", x => x.Id);
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
                name: "IX_Animales_Codigo",
                table: "Animales",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animales_Estado",
                table: "Animales",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Animales_IdFinca",
                table: "Animales",
                column: "IdFinca");

            migrationBuilder.CreateIndex(
                name: "IX_Animales_IdRaza",
                table: "Animales",
                column: "IdRaza");

            migrationBuilder.CreateIndex(
                name: "IX_CalidadLeche_Fecha",
                table: "CalidadLeche",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_CalidadLeche_IdAnimal_Fecha",
                table: "CalidadLeche",
                columns: new[] { "IdAnimal", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_CapturaCarbono_IdFinca_Anio_Mes",
                table: "CapturaCarbono",
                columns: new[] { "IdFinca", "Anio", "Mes" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CentrosCosto_Codigo",
                table: "CentrosCosto",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Documento",
                table: "Clientes",
                column: "Documento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsumosAgua_IdFinca_Fecha",
                table: "ConsumosAgua",
                columns: new[] { "IdFinca", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Contratos_IdCliente_Estado",
                table: "Contratos",
                columns: new[] { "IdCliente", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_ControlLecheAnimal_Fecha",
                table: "ControlLecheAnimal",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_ControlLecheAnimal_IdAnimal_Fecha",
                table: "ControlLecheAnimal",
                columns: new[] { "IdAnimal", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_CostosDiarios_Fecha_IdCentro",
                table: "CostosDiarios",
                columns: new[] { "Fecha", "IdCentro" });

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_Codigo",
                table: "Departamentos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_IdPais",
                table: "Departamentos",
                column: "IdPais");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCotizacion_IdCotizacion",
                table: "DetalleCotizacion",
                column: "IdCotizacion");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesCurativos_IdTratamiento",
                table: "DetallesCurativos",
                column: "IdTratamiento");

            migrationBuilder.CreateIndex(
                name: "IX_Finca_IdMunicipio",
                table: "Finca",
                column: "IdMunicipio");

            migrationBuilder.CreateIndex(
                name: "IX_Gestaciones_IdAnimal_Estado",
                table: "Gestaciones",
                columns: new[] { "IdAnimal", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_GruposManejo_Codigo",
                table: "GruposManejo",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialVacunacion_IdAnimal_Fecha",
                table: "HistorialVacunacion",
                columns: new[] { "IdAnimal", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_Codigo",
                table: "Insumos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KardexInsumos_IdInsumo_Fecha",
                table: "KardexInsumos",
                columns: new[] { "IdInsumo", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_MantenimientoEquipos_IdMaquinaria_Fecha",
                table: "MantenimientoEquipos",
                columns: new[] { "IdMaquinaria", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Maquinaria_Codigo",
                table: "Maquinaria",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marcaciones_Codigo",
                table: "Marcaciones",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_Marcaciones_IdAnimal",
                table: "Marcaciones",
                column: "IdAnimal");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_Codigo",
                table: "Medicamentos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosAnimales_Fecha",
                table: "MovimientosAnimales",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosAnimales_IdAnimal",
                table: "MovimientosAnimales",
                column: "IdAnimal");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_Codigo",
                table: "Municipios",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_IdDepto",
                table: "Municipios",
                column: "IdDepto");

            migrationBuilder.CreateIndex(
                name: "IX_Origen_Codigo",
                table: "Origen",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paises_Codigo",
                table: "Paises",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParametrosLactancia_IdAnimal_NumeroParto",
                table: "ParametrosLactancia",
                columns: new[] { "IdAnimal", "NumeroParto" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_Modulo_Accion",
                table: "Permisos",
                columns: new[] { "Modulo", "Accion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pesajes_IdAnimal_Fecha",
                table: "Pesajes",
                columns: new[] { "IdAnimal", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Potreros_IdFinca_Codigo",
                table: "Potreros",
                columns: new[] { "IdFinca", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProduccionLeche_IdFinca_Fecha",
                table: "ProduccionLeche",
                columns: new[] { "IdFinca", "Fecha" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Razas_Codigo",
                table: "Razas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistroICA_FechaVencimiento_Estado",
                table: "RegistroICA",
                columns: new[] { "FechaVencimiento", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_RegistroICA_IdAnimal",
                table: "RegistroICA",
                column: "IdAnimal");

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
                name: "IX_Semen_Codigo",
                table: "Semen",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_Cedula",
                table: "Trabajadores",
                column: "Cedula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_RolId",
                table: "UsuariosRoles",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_UsuarioId",
                table: "UsuariosRoles",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_VentasLeche_Fecha",
                table: "VentasLeche",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_VentasLeche_IdCliente",
                table: "VentasLeche",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_ZonaFinca_IdFinca",
                table: "ZonaFinca",
                column: "IdFinca");

            migrationBuilder.CreateIndex(
                name: "IX_ZonaFinca_IdZona",
                table: "ZonaFinca",
                column: "IdZona");

            migrationBuilder.CreateIndex(
                name: "IX_ZonaFinca_IdZona_IdFinca",
                table: "ZonaFinca",
                columns: new[] { "IdZona", "IdFinca" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zonas_Codigo",
                table: "Zonas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zonas_Tipo",
                table: "Zonas",
                column: "Tipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activos");

            migrationBuilder.DropTable(
                name: "AcumulacionInsumosPotrero");

            migrationBuilder.DropTable(
                name: "Animales");

            migrationBuilder.DropTable(
                name: "AnimalPotrero");

            migrationBuilder.DropTable(
                name: "Autoconsumos");

            migrationBuilder.DropTable(
                name: "CalidadLeche");

            migrationBuilder.DropTable(
                name: "CapturaCarbono");

            migrationBuilder.DropTable(
                name: "CentrosCosto");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Complementos");

            migrationBuilder.DropTable(
                name: "ConsumosAgua");

            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropTable(
                name: "ControlesPreventivos");

            migrationBuilder.DropTable(
                name: "ControlLecheAnimal");

            migrationBuilder.DropTable(
                name: "CostosAnimalesDiarios");

            migrationBuilder.DropTable(
                name: "CostosDiarios");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropTable(
                name: "DetalleCotizacion");

            migrationBuilder.DropTable(
                name: "DetallePlanAlimentacion");

            migrationBuilder.DropTable(
                name: "DetallesCurativos");

            migrationBuilder.DropTable(
                name: "EventosMedioambientales");

            migrationBuilder.DropTable(
                name: "Finca");

            migrationBuilder.DropTable(
                name: "GastosGenerales");

            migrationBuilder.DropTable(
                name: "Gestaciones");

            migrationBuilder.DropTable(
                name: "GruposManejo");

            migrationBuilder.DropTable(
                name: "HistorialAnimales");

            migrationBuilder.DropTable(
                name: "HistorialDesparasitacion");

            migrationBuilder.DropTable(
                name: "HistorialMastitis");

            migrationBuilder.DropTable(
                name: "HistorialPreventivo");

            migrationBuilder.DropTable(
                name: "HistorialVacunacion");

            migrationBuilder.DropTable(
                name: "Ingresos");

            migrationBuilder.DropTable(
                name: "Insumos");

            migrationBuilder.DropTable(
                name: "KardexInsumos");

            migrationBuilder.DropTable(
                name: "MantenimientoEquipos");

            migrationBuilder.DropTable(
                name: "Maquinaria");

            migrationBuilder.DropTable(
                name: "Marcaciones");

            migrationBuilder.DropTable(
                name: "Medicamentos");

            migrationBuilder.DropTable(
                name: "MovimientosAnimales");

            migrationBuilder.DropTable(
                name: "Municipios");

            migrationBuilder.DropTable(
                name: "Nacimientos");

            migrationBuilder.DropTable(
                name: "Origen");

            migrationBuilder.DropTable(
                name: "PagosJornales");

            migrationBuilder.DropTable(
                name: "Paises");

            migrationBuilder.DropTable(
                name: "ParametrosLactancia");

            migrationBuilder.DropTable(
                name: "Pedigri");

            migrationBuilder.DropTable(
                name: "Pesajes");

            migrationBuilder.DropTable(
                name: "PlanesAlimentacion");

            migrationBuilder.DropTable(
                name: "PlanesPastoreo");

            migrationBuilder.DropTable(
                name: "Potreros");

            migrationBuilder.DropTable(
                name: "PrestacionesSociales");

            migrationBuilder.DropTable(
                name: "ProduccionLeche");

            migrationBuilder.DropTable(
                name: "Razas");

            migrationBuilder.DropTable(
                name: "RegistroICA");

            migrationBuilder.DropTable(
                name: "RolesPermisos");

            migrationBuilder.DropTable(
                name: "Semen");

            migrationBuilder.DropTable(
                name: "Servicios");

            migrationBuilder.DropTable(
                name: "Trabajadores");

            migrationBuilder.DropTable(
                name: "TransferenciasCosto");

            migrationBuilder.DropTable(
                name: "UsuariosRoles");

            migrationBuilder.DropTable(
                name: "VentasLeche");

            migrationBuilder.DropTable(
                name: "ZonaFinca");

            migrationBuilder.DropTable(
                name: "Zonas");

            migrationBuilder.DropTable(
                name: "CotizacionesVenta");

            migrationBuilder.DropTable(
                name: "HistorialCurativo");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
