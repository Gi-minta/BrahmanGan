using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sanidad;

/// <summary>
/// Tratamiento curativo: agrega un diagnóstico y agrega detalles (medicamentos aplicados).
/// </summary>
public sealed class HistorialCurativo : AggregateRoot<HistorialCurativoId>
{
    private readonly List<DetalleCurativo> _detalles = new();

    public AnimalId IdAnimal { get; private set; } = null!;
    public string Diagnostico { get; private set; } = string.Empty;
    public DateOnly FechaInicio { get; private set; }
    public DateOnly? FechaFin { get; private set; }
    public string? Veterinario { get; private set; }
    public string? Resultado { get; private set; }
    public decimal? CostoTotal { get; private set; }
    public IReadOnlyCollection<DetalleCurativo> Detalles => _detalles.AsReadOnly();

    private HistorialCurativo() { }

    public static HistorialCurativo Iniciar(AnimalId idAnimal, string diagnostico, DateOnly fechaInicio, string? veterinario = null)
    {
        if (idAnimal is null) throw new DomainException("Animal requerido");
        if (string.IsNullOrWhiteSpace(diagnostico)) throw new DomainException("Diagnóstico requerido");
        return new HistorialCurativo
        {
            Id = HistorialCurativoId.New(),
            IdAnimal = idAnimal,
            Diagnostico = diagnostico.Trim(),
            FechaInicio = fechaInicio,
            Veterinario = veterinario
        };
    }

    public void AgregarDetalle(MedicamentoId idMedicamento, DateOnly fecha, decimal? dosis = null, decimal? costoUnitario = null)
    {
        if (FechaFin.HasValue) throw new BusinessRuleException("El tratamiento ya está cerrado");
        if (idMedicamento is null) throw new DomainException("Medicamento requerido");
        if (fecha < FechaInicio) throw new BusinessRuleException("Fecha del detalle anterior al inicio");
        var d = DetalleCurativo.Crear(Id, idMedicamento, fecha, dosis, costoUnitario);
        _detalles.Add(d);
        RecalcularCosto();
        IncrementVersion();
    }

    public void Cerrar(DateOnly fechaFin, string resultado)
    {
        if (FechaFin.HasValue) throw new BusinessRuleException("Tratamiento ya cerrado");
        if (fechaFin < FechaInicio) throw new BusinessRuleException("FechaFin < FechaInicio");
        FechaFin = fechaFin;
        Resultado = resultado;
        IncrementVersion();
    }

    private void RecalcularCosto()
    {
        CostoTotal = _detalles.Sum(d => (d.Dosis ?? 0m) * (d.CostoUnitario ?? 0m));
    }
}
