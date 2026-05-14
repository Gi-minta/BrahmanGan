using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sanidad;

/// <summary>Episodio de mastitis (puede asociar tratamiento curativo).</summary>
public sealed class HistorialMastitis : Entity<HistorialMastitisId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public string? Cuarto { get; private set; }
    public string? GradoInfeccion { get; private set; }
    public HistorialCurativoId? IdTratamiento { get; private set; }

    private HistorialMastitis() { }

    public static HistorialMastitis Registrar(AnimalId idAnimal, DateOnly fecha,
        string? cuarto = null, string? grado = null, HistorialCurativoId? idTratamiento = null)
    {
        if (idAnimal is null) throw new DomainException("Animal requerido");
        return new HistorialMastitis
        {
            Id = HistorialMastitisId.New(),
            IdAnimal = idAnimal,
            Fecha = fecha,
            Cuarto = cuarto,
            GradoInfeccion = grado,
            IdTratamiento = idTratamiento
        };
    }
}
