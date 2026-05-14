using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Inventario;

/// <summary>
/// Marcación física u oficial aplicada al animal.
/// Una marcación puede darse de baja con fecha y motivo.
/// </summary>
public sealed class Marcacion : Entity<MarcacionId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public TipoMarcacion TipoMarcacion { get; private set; }
    public string Codigo { get; private set; } = string.Empty;
    public DateOnly FechaAplicacion { get; private set; }
    public string? Responsable { get; private set; }
    public bool Activa { get; private set; } = true;
    public DateOnly? FechaBaja { get; private set; }
    public string? MotivoBaja { get; private set; }

    private Marcacion() { }

    public static Marcacion Aplicar(AnimalId idAnimal, TipoMarcacion tipo, string codigo,
        DateOnly fechaAplicacion, string? responsable = null)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código de marcación requerido");
        return new Marcacion
        {
            Id = MarcacionId.New(),
            IdAnimal = idAnimal,
            TipoMarcacion = tipo,
            Codigo = codigo.Trim(),
            FechaAplicacion = fechaAplicacion,
            Responsable = responsable,
            Activa = true
        };
    }

    public void DarDeBaja(DateOnly fechaBaja, string motivo)
    {
        if (!Activa) throw new BusinessRuleException("La marcación ya está dada de baja");
        if (string.IsNullOrWhiteSpace(motivo)) throw new DomainException("Motivo de baja requerido");
        Activa = false;
        FechaBaja = fechaBaja;
        MotivoBaja = motivo.Trim();
        MarkAsModified();
    }
}
