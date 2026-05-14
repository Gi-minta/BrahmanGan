using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Finca;

/// <summary>
/// Finca / unidad productiva (raíz de agregado del módulo Finca).
/// Su ubicación geográfica se referencia por IdMunicipio (FK normalizada).
/// La pertenencia a zonas operativas se gestiona desde ZonaFinca.
/// </summary>
public sealed class Finca : AggregateRoot<FincaId>
{
    public string Nombre { get; private set; } = string.Empty;
    public string? NIT { get; private set; }
    public string? Propietario { get; private set; }
    public string? Direccion { get; private set; }
    public string? Telefono { get; private set; }
    public string? Email { get; private set; }
    public MunicipioId? IdMunicipio { get; private set; }
    public decimal? AreaHectareas { get; private set; }
    public bool Activa { get; private set; } = true;
    public DateTime FechaRegistro { get; private set; } = DateTime.UtcNow;

    private Finca() { }

    public static Finca Crear(string nombre, MunicipioId? idMunicipio = null,
        string? nit = null, string? propietario = null, string? direccion = null,
        string? telefono = null, string? email = null, decimal? areaHectareas = null)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre de finca requerido");
        if (areaHectareas is < 0) throw new DomainException("Área no puede ser negativa");
        return new Finca
        {
            Id = FincaId.New(),
            Nombre = nombre.Trim(),
            NIT = nit,
            Propietario = propietario,
            Direccion = direccion,
            Telefono = telefono,
            Email = email,
            IdMunicipio = idMunicipio,
            AreaHectareas = areaHectareas,
            Activa = true
        };
    }

    public void Activar() { Activa = true; IncrementVersion(); }
    public void Desactivar() { Activa = false; IncrementVersion(); }

    public void ActualizarUbicacion(MunicipioId? idMunicipio, string? direccion)
    {
        IdMunicipio = idMunicipio;
        Direccion = direccion;
        IncrementVersion();
    }
}
