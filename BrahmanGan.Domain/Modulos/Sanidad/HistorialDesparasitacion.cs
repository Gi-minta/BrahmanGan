using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sanidad;

/// <summary>Aplicación de desparasitación a un animal.</summary>
public sealed class HistorialDesparasitacion : Entity<HistorialDesparasitacionId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public MedicamentoId IdMedicamento { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public decimal? Dosis { get; private set; }
    public string? TipoParasito { get; private set; }
    public DateOnly? ProximaFecha { get; private set; }

    private HistorialDesparasitacion() { }

    public static HistorialDesparasitacion Aplicar(AnimalId idAnimal, MedicamentoId idMedicamento,
        DateOnly fecha, decimal? dosis = null, string? tipoParasito = null, DateOnly? proximaFecha = null)
    {
        if (dosis is < 0) throw new DomainException("Dosis no negativa");
        return new HistorialDesparasitacion
        {
            Id = HistorialDesparasitacionId.New(),
            IdAnimal = idAnimal, IdMedicamento = idMedicamento, Fecha = fecha,
            Dosis = dosis, TipoParasito = tipoParasito, ProximaFecha = proximaFecha
        };
    }
}
