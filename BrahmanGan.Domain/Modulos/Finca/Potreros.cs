using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Finca;

/// <summary>
/// Potrero perteneciente a una finca. Codigo es único dentro de la finca.
/// </summary>
public sealed class Potrero : Entity<PotreroId>
{
    public FincaId IdFinca { get; private set; } = null!;
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public decimal? AreaHectareas { get; private set; }
    public string? TipoPasto { get; private set; }
    public bool Activo { get; private set; } = true;

    private Potrero() { }

    public static Potrero Crear(FincaId idFinca, string codigo, string nombre,
        decimal? areaHectareas = null, string? tipoPasto = null)
    {
        if (idFinca is null) throw new DomainException("Finca requerida");
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código de potrero requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre de potrero requerido");
        if (areaHectareas is < 0) throw new DomainException("Área no puede ser negativa");
        return new Potrero
        {
            Id = PotreroId.New(),
            IdFinca = idFinca,
            Codigo = codigo.Trim().ToUpperInvariant(),
            Nombre = nombre.Trim(),
            AreaHectareas = areaHectareas,
            TipoPasto = tipoPasto,
            Activo = true
        };
    }

    public void Activar() { Activo = true; MarkAsModified(); }
    public void Desactivar() { Activo = false; MarkAsModified(); }
}

/// <summary>Grupo de manejo (lotes, categorías productivas como TERNERAS_DESTETE, VACAS_LACTANCIA, etc.).</summary>
public sealed class GrupoManejo : Entity<GrupoManejoId>
{
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string? Descripcion { get; private set; }
    public string? TipoAnimal { get; private set; }
    public bool Activo { get; private set; } = true;

    private GrupoManejo() { }

    public static GrupoManejo Crear(string codigo, string nombre, string? descripcion = null, string? tipoAnimal = null)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre requerido");
        return new GrupoManejo
        {
            Id = GrupoManejoId.New(),
            Codigo = codigo.Trim().ToUpperInvariant(),
            Nombre = nombre.Trim(),
            Descripcion = descripcion,
            TipoAnimal = tipoAnimal,
            Activo = true
        };
    }
}

/// <summary>Asignación temporal de un animal a un potrero, opcionalmente bajo un grupo de manejo.</summary>
public sealed class AnimalPotrero : Entity<AnimalPotreroId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public PotreroId IdPotrero { get; private set; } = null!;
    public DateOnly FechaIngreso { get; private set; }
    public DateOnly? FechaSalida { get; private set; }
    public GrupoManejoId? IdGrupo { get; private set; }

    private AnimalPotrero() { }

    public static AnimalPotrero Asignar(AnimalId idAnimal, PotreroId idPotrero, DateOnly fechaIngreso,
        GrupoManejoId? idGrupo = null)
    {
        if (idAnimal is null) throw new DomainException("Animal requerido");
        if (idPotrero is null) throw new DomainException("Potrero requerido");
        return new AnimalPotrero
        {
            Id = AnimalPotreroId.New(),
            IdAnimal = idAnimal,
            IdPotrero = idPotrero,
            FechaIngreso = fechaIngreso,
            IdGrupo = idGrupo
        };
    }

    public void Cerrar(DateOnly fechaSalida)
    {
        if (fechaSalida < FechaIngreso) throw new BusinessRuleException("Fecha salida < Fecha ingreso");
        FechaSalida = fechaSalida;
        MarkAsModified();
    }

    public bool EstaVigente() => !FechaSalida.HasValue;
}
