namespace BrahmanGan.Application.DTOs;

// ===== Fase 4: Sanidad =====
public record CrearMedicamentoRequest(string Codigo, string Nombre, string? Principio,
    string? TipoUso, string? Unidad, decimal? PrecioUnitario, int? TiempoCarne, int? TiempoLeche);
