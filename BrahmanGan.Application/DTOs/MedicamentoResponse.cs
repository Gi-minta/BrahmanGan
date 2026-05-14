namespace BrahmanGan.Application.DTOs;

public record MedicamentoResponse(int Id, string Codigo, string Nombre, int? TiempoCarne, int? TiempoLeche, bool Activo);
