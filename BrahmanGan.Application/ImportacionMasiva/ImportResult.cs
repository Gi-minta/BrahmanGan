namespace BrahmanGan.Application.ImportacionMasiva;

public sealed record ImportResult(
    int TotalFilas,
    int Exitosos,
    int Fallidos,
    IReadOnlyList<ImportError> Errores);

public sealed record ImportError(int Fila, string? Campo, string Mensaje);
