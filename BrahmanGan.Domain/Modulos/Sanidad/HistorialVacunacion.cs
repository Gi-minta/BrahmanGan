using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sanidad;

/// <summary>Aplicación de una vacuna a un animal.</summary>
public sealed class HistorialVacunacion : Entity<HistorialVacunacionId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public MedicamentoId IdMedicamento { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public decimal? Dosis { get; private set; }
    public string? Lote { get; private set; }
    public string? Responsable { get; private set; }
    public DateOnly? ProximaFecha { get; private set; }

    private HistorialVacunacion() { }

    public static HistorialVacunacion Aplicar(AnimalId idAnimal, MedicamentoId idMedicamento, DateOnly fecha,
        decimal? dosis = null, string? lote = null, string? responsable = null, DateOnly? proximaFecha = null)
    {
        if (dosis is < 0) throw new DomainException("Dosis no negativa");
        if (proximaFecha.HasValue && proximaFecha.Value < fecha)
            throw new BusinessRuleException("Próxima vacunación no puede ser anterior");
        return new HistorialVacunacion
        {
            Id = HistorialVacunacionId.New(),
            IdAnimal = idAnimal, IdMedicamento = idMedicamento, Fecha = fecha,
            Dosis = dosis, Lote = lote, Responsable = responsable, ProximaFecha = proximaFecha
        };
    }

    public bool RequiereAlerta(DateOnly hoy, int diasUmbral = 7) =>
        ProximaFecha.HasValue && (ProximaFecha.Value.DayNumber - hoy.DayNumber) <= diasUmbral;
}
