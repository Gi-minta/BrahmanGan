namespace BrahmanGan.Application.DTOs;

public record RegistrarAbortoRequest(DateOnly Fecha, string? Motivo);
