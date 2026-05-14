using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Application.DTOs;

// ===== Fase 1: Inventario =====
public record CrearAnimalRequest(
    string Codigo, Sexo Sexo, int IdRaza, int IdFinca,
    string? Nombre = null, DateOnly? FechaNacimiento = null, decimal? PesoNacimiento = null,
    int? IdMadre = null, int? IdPadre = null, int? IdOrigen = null,
    DateOnly? FechaIngreso = null, string? Observaciones = null);
