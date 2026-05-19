using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Exceptions;

namespace BrahmanGan.Domain.Modulos.Sanidad;

/// <summary>
/// Medicamento veterinario. Importante: TiempoCarne y TiempoLeche definen
/// los días de retiro tras la aplicación (regulación sanitaria).
/// </summary>
public sealed class Medicamento : Entity<MedicamentoId>
{
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string? Principio { get; private set; }
    public string? TipoUso { get; private set; }
    public string? Unidad { get; private set; }
    public decimal? PrecioUnitario { get; private set; }
    public int? TiempoCarne { get; private set; }
    public int? TiempoLeche { get; private set; }
    public bool Activo { get; private set; } = true;

    private Medicamento() { }

    public static Medicamento Crear(string codigo, string nombre, string? principio = null,
        string? tipoUso = null, string? unidad = null, decimal? precioUnitario = null,
        int? tiempoCarne = null, int? tiempoLeche = null)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("Código requerido");
        if (string.IsNullOrWhiteSpace(nombre)) throw new DomainException("Nombre requerido");
        if (precioUnitario is < 0) throw new DomainException("Precio no puede ser negativo");
        if (tiempoCarne is < 0 || tiempoLeche is < 0) throw new DomainException("Tiempos de retiro no negativos");
        return new Medicamento
        {
            Id = MedicamentoId.New(),
            Codigo = codigo.Trim(),
            Nombre = nombre.Trim(),
            Principio = principio,
            TipoUso = tipoUso,
            Unidad = unidad,
            PrecioUnitario = precioUnitario,
            TiempoCarne = tiempoCarne,
            TiempoLeche = tiempoLeche,
            Activo = true
        };
    }

    public DateOnly? FechaLiberacionCarne(DateOnly fechaAplicacion) =>
        TiempoCarne.HasValue ? fechaAplicacion.AddDays(TiempoCarne.Value) : null;

    public DateOnly? FechaLiberacionLeche(DateOnly fechaAplicacion) =>
        TiempoLeche.HasValue ? fechaAplicacion.AddDays(TiempoLeche.Value) : null;
}
