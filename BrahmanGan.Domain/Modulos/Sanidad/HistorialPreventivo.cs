using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sanidad;

/// <summary>Aplicación de un control preventivo a un animal.</summary>
public sealed class HistorialPreventivo : Entity<HistorialPreventivoId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public ControlPreventivoId IdControl { get; private set; } = null!;
    public MedicamentoId? IdMedicamento { get; private set; }
    public DateOnly Fecha { get; private set; }
    public decimal? Dosis { get; private set; }
    public string? Responsable { get; private set; }
    public DateOnly? ProximaFecha { get; private set; }

    private HistorialPreventivo() { }

    public static HistorialPreventivo Aplicar(AnimalId idAnimal, ControlPreventivoId idControl,
        DateOnly fecha, MedicamentoId? idMedicamento = null, decimal? dosis = null,
        string? responsable = null, DateOnly? proximaFecha = null)
    {
        if (idAnimal is null) throw new DomainException("Animal requerido");
        if (idControl is null) throw new DomainException("Control requerido");
        if (dosis is < 0) throw new DomainException("Dosis no negativa");
        if (proximaFecha.HasValue && proximaFecha.Value < fecha)
            throw new BusinessRuleException("Próxima fecha no puede ser anterior a la aplicación");
        return new HistorialPreventivo
        {
            Id = HistorialPreventivoId.New(),
            IdAnimal = idAnimal, IdControl = idControl, IdMedicamento = idMedicamento,
            Fecha = fecha, Dosis = dosis, Responsable = responsable, ProximaFecha = proximaFecha
        };
    }
}
