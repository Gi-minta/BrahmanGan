using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Nomina;

/// <summary>Trabajador / empleado de la finca.</summary>
public sealed class Trabajador : AggregateRoot<TrabajadorId>
{
    public string Cedula { get; private set; } = string.Empty;
    public string Nombres { get; private set; } = string.Empty;
    public string Apellidos { get; private set; } = string.Empty;
    public string? Cargo { get; private set; }
    public DateOnly FechaIngreso { get; private set; }
    public DateOnly? FechaRetiro { get; private set; }
    public decimal? SalarioBase { get; private set; }
    public string? TipoContrato { get; private set; }
    public bool Activo { get; private set; } = true;

    private Trabajador() { }
    public static Trabajador Contratar(string cedula, string nombres, string apellidos, DateOnly fechaIngreso,
        string? cargo = null, decimal? salarioBase = null, string? tipoContrato = null)
    {
        if (string.IsNullOrWhiteSpace(cedula)) throw new DomainException("Cédula requerida");
        if (string.IsNullOrWhiteSpace(nombres)) throw new DomainException("Nombres requeridos");
        if (string.IsNullOrWhiteSpace(apellidos)) throw new DomainException("Apellidos requeridos");
        if (salarioBase is < 0) throw new DomainException("Salario no negativo");
        return new Trabajador { Id = TrabajadorId.New(), Cedula = cedula.Trim(), Nombres = nombres.Trim(),
            Apellidos = apellidos.Trim(), Cargo = cargo, FechaIngreso = fechaIngreso,
            SalarioBase = salarioBase, TipoContrato = tipoContrato, Activo = true };
    }

    public void Retirar(DateOnly fechaRetiro)
    {
        if (!Activo) throw new BusinessRuleException("El trabajador ya está retirado");
        if (fechaRetiro < FechaIngreso) throw new BusinessRuleException("FechaRetiro < FechaIngreso");
        FechaRetiro = fechaRetiro;
        Activo = false;
        IncrementVersion();
    }
}
