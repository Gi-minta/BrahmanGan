namespace BrahmanGan.Application.DTOs;

// ===== Grupos de Manejo =====
public record CrearGrupoManejoRequest(
    string Codigo, string Nombre,
    string? Descripcion = null, string? TipoAnimal = null);

public record GrupoManejoResponse(
    int Id, string Codigo, string Nombre,
    string? Descripcion, string? TipoAnimal, bool Activo);

// ===== Animal por Potrero =====
public record AsignarAnimalPotreroRequest(
    int IdAnimal, int IdPotrero, DateOnly FechaIngreso,
    int? IdGrupo = null);

public record CerrarAnimalPotreroRequest(DateOnly FechaSalida);

public record AnimalPotreroResponse(
    int Id, int IdAnimal, int IdPotrero, DateOnly FechaIngreso,
    DateOnly? FechaSalida, int? IdGrupo, bool Vigente);

// ===== Acumulación de Insumos en Potrero =====
public record RegistrarAcumulacionInsumoRequest(
    int IdPotrero, int IdInsumo, DateOnly Fecha,
    decimal Cantidad, decimal? CostoUnitario = null);

public record AcumulacionInsumoPotreroResponse(
    int Id, int IdPotrero, int IdInsumo, DateOnly Fecha,
    decimal Cantidad, decimal? CostoUnitario);
