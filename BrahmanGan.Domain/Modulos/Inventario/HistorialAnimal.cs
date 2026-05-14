using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Inventario;

/// <summary>Registro histórico de eventos relevantes del animal (compras, ventas, muertes, etc.).</summary>
public sealed class HistorialAnimal : Entity<HistorialAnimalId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public TipoEventoHistorial TipoEvento { get; private set; }
    public DateOnly Fecha { get; private set; }
    public string? Descripcion { get; private set; }
    public decimal? Valor { get; private set; }
    public string? UsuarioRegistro { get; private set; }
    public DateTime FechaRegistro { get; private set; } = DateTime.UtcNow;

    private HistorialAnimal() { }

    public static HistorialAnimal Crear(AnimalId idAnimal, TipoEventoHistorial tipo, DateOnly fecha,
        string? descripcion = null, decimal? valor = null, string? usuarioRegistro = null)
    {
        if (idAnimal is null) throw new DomainException("Animal requerido");
        return new HistorialAnimal
        {
            Id = HistorialAnimalId.New(),
            IdAnimal = idAnimal,
            TipoEvento = tipo,
            Fecha = fecha,
            Descripcion = descripcion,
            Valor = valor,
            UsuarioRegistro = usuarioRegistro
        };
    }
}
