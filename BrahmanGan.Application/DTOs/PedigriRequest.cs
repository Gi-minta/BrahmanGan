namespace BrahmanGan.Application.DTOs;

// ===== Pedigri =====
public record CrearPedigriRequest(
    int IdAnimal,
    int? IdAbuelo1 = null, int? IdAbuela1 = null,
    int? IdAbuelo2 = null, int? IdAbuela2 = null,
    decimal? PuntajeMorfologia = null, string? Observaciones = null);

public record PedigriResponse(
    int Id, int IdAnimal,
    int? IdAbuelo1, int? IdAbuela1,
    int? IdAbuelo2, int? IdAbuela2,
    decimal? PuntajeMorfologia, string? Observaciones);
