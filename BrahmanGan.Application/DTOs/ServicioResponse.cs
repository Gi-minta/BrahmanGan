namespace BrahmanGan.Application.DTOs;

public record ServicioResponse(int Id, int IdHembra, string TipoServicio, DateOnly Fecha,
    int? IdToro, int? IdSemen, bool? ResultadoPreniez, DateOnly? FechaConfirmacion);
