namespace BrahmanGan.Application.DTOs;

public record RegistrarIaRequest(int IdHembra, int IdSemen, DateOnly Fecha, string? Responsable = null);
