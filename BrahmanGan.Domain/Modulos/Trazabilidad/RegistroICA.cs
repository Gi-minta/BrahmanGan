using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Trazabilidad;

/// <summary>
/// Registro oficial de trazabilidad ante el ICA (u otra entidad emisora).
/// Documento con vigencia y posible vencimiento.
/// </summary>
public sealed class RegistroICA : AggregateRoot<RegistroICAId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public string TipoDocumento { get; private set; } = string.Empty;
    public string NumeroDocumento { get; private set; } = string.Empty;
    public DateOnly FechaExpedicion { get; private set; }
    public DateOnly? FechaVencimiento { get; private set; }
    public string? EntidadEmisora { get; private set; }
    public MunicipioId? IdMunicipio { get; private set; }
    public EstadoRegistroICA Estado { get; private set; } = EstadoRegistroICA.VIGENTE;
    public string? UrlDocumento { get; private set; }
    public string? Observaciones { get; private set; }

    private RegistroICA() { }
    public static RegistroICA Emitir(AnimalId idAnimal, string tipoDocumento, string numeroDocumento,
        DateOnly fechaExpedicion, DateOnly? fechaVencimiento = null, string? entidadEmisora = null,
        MunicipioId? idMunicipio = null, string? urlDocumento = null, string? observaciones = null)
    {
        if (idAnimal is null) throw new DomainException("Animal requerido");
        if (string.IsNullOrWhiteSpace(tipoDocumento)) throw new DomainException("Tipo documento requerido");
        if (string.IsNullOrWhiteSpace(numeroDocumento)) throw new DomainException("Número documento requerido");
        if (fechaVencimiento.HasValue && fechaVencimiento.Value < fechaExpedicion)
            throw new BusinessRuleException("Vencimiento anterior a expedición");
        return new RegistroICA { Id = RegistroICAId.New(), IdAnimal = idAnimal,
            TipoDocumento = tipoDocumento.Trim(), NumeroDocumento = numeroDocumento.Trim(),
            FechaExpedicion = fechaExpedicion, FechaVencimiento = fechaVencimiento,
            EntidadEmisora = entidadEmisora, IdMunicipio = idMunicipio,
            Estado = EstadoRegistroICA.VIGENTE, UrlDocumento = urlDocumento, Observaciones = observaciones };
    }

    public void Cancelar()
    {
        if (Estado == EstadoRegistroICA.CANCELADO) throw new BusinessRuleException("Ya cancelado");
        Estado = EstadoRegistroICA.CANCELADO;
        IncrementVersion();
    }

    public void EvaluarVencimiento(DateOnly hoy)
    {
        if (Estado != EstadoRegistroICA.VIGENTE) return;
        if (FechaVencimiento.HasValue && FechaVencimiento.Value < hoy)
        {
            Estado = EstadoRegistroICA.VENCIDO;
            IncrementVersion();
        }
    }

    public bool RequiereAlerta(DateOnly hoy, int diasUmbral = 30) =>
        FechaVencimiento.HasValue
        && (FechaVencimiento.Value.DayNumber - hoy.DayNumber) <= diasUmbral
        && Estado == EstadoRegistroICA.VIGENTE;
}
