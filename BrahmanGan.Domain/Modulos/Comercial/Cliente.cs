using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.DomainEvents;
using BrahmanGan.Domain.Exceptions;
using BrahmanGan.Domain.ValueObjects;

namespace BrahmanGan.Domain.Modulos.Comercial;

/// <summary>Cliente comercial (compra leche, animales, servicios).</summary>
public sealed class Cliente : AggregateRoot<ClienteId>
{
    public string TipoDocumento { get; private set; } = "NIT";
    public string Documento { get; private set; } = string.Empty;
    public string RazonSocial { get; private set; } = string.Empty;
    public string? Contacto { get; private set; }
    public string? Telefono { get; private set; }
    public Email? Email { get; private set; }
    public string? Direccion { get; private set; }
    public MunicipioId? IdMunicipio { get; private set; }
    public string? TipoCliente { get; private set; }
    public bool Activo { get; private set; } = true;

    private Cliente() { }

    public static Cliente Crear(string documento, string razonSocial,
        string tipoDocumento = "NIT", string? contacto = null, string? telefono = null,
        Email? email = null, string? direccion = null, MunicipioId? idMunicipio = null,
        string? tipoCliente = null)
    {
        if (string.IsNullOrWhiteSpace(documento)) throw new DomainException("Documento requerido");
        if (string.IsNullOrWhiteSpace(razonSocial)) throw new DomainException("Razón social requerida");
        var c = new Cliente
        {
            Id = ClienteId.New(),
            TipoDocumento = tipoDocumento.Trim().ToUpperInvariant(),
            Documento = documento.Trim(),
            RazonSocial = razonSocial.Trim(),
            Contacto = contacto, Telefono = telefono, Email = email,
            Direccion = direccion, IdMunicipio = idMunicipio, TipoCliente = tipoCliente,
            Activo = true
        };
        c.AddDomainEvent(new ClienteCreadoEvent(c.Id, c.Documento, c.RazonSocial));
        return c;
    }

    public void Activar() { Activo = true; IncrementVersion(); }
    public void Desactivar() { Activo = false; IncrementVersion(); }

    public void ActualizarContacto(string? contacto, string? telefono, Email? email)
    {
        Contacto = contacto; Telefono = telefono; Email = email;
        IncrementVersion();
    }
}
