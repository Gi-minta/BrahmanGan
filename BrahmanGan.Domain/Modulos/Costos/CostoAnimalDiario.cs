using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Costos;

/// <summary>Costo diario imputado a un animal específico.</summary>
public sealed class CostoAnimalDiario : Entity<CostoAnimalDiarioId>
{
    public AnimalId IdAnimal { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public string TipoCosto { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }

    private CostoAnimalDiario() { }
    public static CostoAnimalDiario Crear(AnimalId idAnimal, DateOnly fecha, string tipoCosto, decimal valor)
    {
        if (string.IsNullOrWhiteSpace(tipoCosto)) throw new DomainException("Tipo de costo requerido");
        if (valor < 0) throw new DomainException("Valor no negativo");
        return new CostoAnimalDiario { Id = CostoAnimalDiarioId.New(), IdAnimal = idAnimal, Fecha = fecha, TipoCosto = tipoCosto.Trim(), Valor = valor };
    }
}
