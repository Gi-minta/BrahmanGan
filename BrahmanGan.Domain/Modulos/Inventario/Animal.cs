using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.DomainEvents;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Inventario;

/// <summary>
/// Raíz de agregado del módulo de Inventario Animal.
///
/// Reglas de negocio (extraídas de BrahmanGan_Corregido.sql):
///  - Codigo único en todo el sistema.
///  - Sexo obligatorio (M|H).
///  - Raza y Finca obligatorias.
///  - Genealogía opcional: madre debe ser Hembra, padre debe ser Macho.
///  - Estado por defecto: ACTIVO. No se permite reactivar un animal MUERTO.
///  - Cambios de estado y movimientos importantes generan registros en HistorialAnimal.
/// </summary>
public sealed class Animal : AggregateRoot<AnimalId>
{
    public string Codigo { get; private set; } = string.Empty;
    public string? Nombre { get; private set; }
    public RazaId IdRaza { get; private set; } = null!;
    public Sexo Sexo { get; private set; }
    public DateOnly? FechaNacimiento { get; private set; }
    public AnimalId? IdMadre { get; private set; }
    public AnimalId? IdPadre { get; private set; }
    public OrigenId? IdOrigen { get; private set; }
    public EstadoAnimal Estado { get; private set; } = EstadoAnimal.ACTIVO;
    public decimal? PesoNacimiento { get; private set; }
    public FincaId IdFinca { get; private set; } = null!;
    public DateOnly? FechaIngreso { get; private set; }
    public string? Observaciones { get; private set; }
    public DateTime FechaRegistro { get; private set; } = DateTime.UtcNow;

    private Animal() { }

    public static Animal Registrar(
        string codigo,
        Sexo sexo,
        RazaId idRaza,
        FincaId idFinca,
        string? nombre = null,
        DateOnly? fechaNacimiento = null,
        decimal? pesoNacimiento = null,
        AnimalId? idMadre = null,
        AnimalId? idPadre = null,
        OrigenId? idOrigen = null,
        DateOnly? fechaIngreso = null,
        string? observaciones = null)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new DomainException("Código del animal requerido");
        if (codigo.Length > 20)
            throw new DomainException("El código no puede exceder 20 caracteres");
        if (idRaza is null) throw new DomainException("Raza requerida");
        if (idFinca is null) throw new DomainException("Finca requerida");
        if (pesoNacimiento is < 0) throw new DomainException("Peso de nacimiento no puede ser negativo");
        if (fechaNacimiento.HasValue && fechaNacimiento.Value > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DomainException("La fecha de nacimiento no puede ser futura");

        var a = new Animal
        {
            Id = AnimalId.New(),
            Codigo = codigo.Trim(),
            Nombre = nombre?.Trim(),
            IdRaza = idRaza,
            Sexo = sexo,
            FechaNacimiento = fechaNacimiento,
            PesoNacimiento = pesoNacimiento,
            IdMadre = idMadre,
            IdPadre = idPadre,
            IdOrigen = idOrigen,
            IdFinca = idFinca,
            FechaIngreso = fechaIngreso ?? DateOnly.FromDateTime(DateTime.UtcNow),
            Observaciones = observaciones,
            Estado = EstadoAnimal.ACTIVO
        };
        a.AddDomainEvent(new AnimalRegistradoEvent(a.Id, a.Codigo, a.IdFinca));
        return a;
    }

    public void AsignarMadre(Animal madre)
    {
        if (madre.Sexo != Sexo.H) throw new BusinessRuleException("La madre debe ser Hembra");
        if (madre.Id.Value == Id.Value) throw new BusinessRuleException("Un animal no puede ser su propia madre");
        IdMadre = madre.Id;
        IncrementVersion();
    }

    public void AsignarPadre(Animal padre)
    {
        if (padre.Sexo != Sexo.M) throw new BusinessRuleException("El padre debe ser Macho");
        if (padre.Id.Value == Id.Value) throw new BusinessRuleException("Un animal no puede ser su propio padre");
        IdPadre = padre.Id;
        IncrementVersion();
    }

    public void CambiarEstado(EstadoAnimal nuevo)
    {
        if (Estado == EstadoAnimal.MUERTO && nuevo != EstadoAnimal.MUERTO)
            throw new BusinessRuleException("Un animal MUERTO no puede cambiar de estado");
        if (Estado == nuevo) return;
        var anterior = Estado;
        Estado = nuevo;
        AddDomainEvent(new AnimalEstadoCambiadoEvent(Id, anterior, nuevo));
        IncrementVersion();
    }

    public void Trasladar(FincaId nuevaFinca)
    {
        if (nuevaFinca is null) throw new DomainException("Finca destino requerida");
        if (Estado != EstadoAnimal.ACTIVO)
            throw new BusinessRuleException("Solo animales ACTIVOS pueden trasladarse");
        if (IdFinca.Value == nuevaFinca.Value) return;
        var origen = IdFinca;
        IdFinca = nuevaFinca;
        Estado = EstadoAnimal.TRANSFERIDO;
        AddDomainEvent(new AnimalTrasladadoEvent(Id, origen, nuevaFinca));
        IncrementVersion();
    }

    public void ActualizarObservaciones(string? obs)
    {
        if (obs is { Length: > 500 }) throw new DomainException("Observaciones no pueden exceder 500 caracteres");
        Observaciones = obs?.Trim();
        IncrementVersion();
    }
}
