using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportMedicamentosService(IMedicamentoService medicamentoService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        foreach (var fila in filas)
        {
            try
            {
                var req = new CrearMedicamentoRequest(
                    Codigo: fila.Valores.Req("Codigo"),
                    Nombre: fila.Valores.Req("Nombre"),
                    Principio: fila.Valores.Opt("Principio"),
                    TipoUso: fila.Valores.Opt("TipoUso"),
                    Unidad: fila.Valores.Opt("Unidad"),
                    PrecioUnitario: fila.Valores.DecOpt("PrecioUnitario"),
                    TiempoCarne: fila.Valores.Int32Opt("TiempoCarne"),
                    TiempoLeche: fila.Valores.Int32Opt("TiempoLeche"));

                await medicamentoService.CrearAsync(req, ct);
                exitosos++;
            }
            catch (Exception ex)
            {
                errores.Add(new ImportError(fila.NumeroFila, null, ex.Message));
            }
        }

        return new ImportResult(filas.Count, exitosos, errores.Count, errores);
    }
}

public sealed class ImportVacunacionesService(
    IVacunacionService vacunacionService,
    IAnimalService animalService,
    IMedicamentoService medicamentoService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var animales = (await animalService.ListarActivosAsync(ct))
            .ToDictionary(a => a.Codigo.Trim(), a => a.Id, StringComparer.OrdinalIgnoreCase);
        var medicamentos = (await medicamentoService.ListarAsync(ct))
            .ToDictionary(m => m.Codigo.Trim(), m => m.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                var codigoAnimal = fila.Valores.Req("CodigoAnimal");
                if (!animales.TryGetValue(codigoAnimal, out var idAnimal))
                    throw new InvalidOperationException($"Animal '{codigoAnimal}' no encontrado.");

                var codigoMed = fila.Valores.Req("CodigoMedicamento");
                if (!medicamentos.TryGetValue(codigoMed, out var idMed))
                    throw new InvalidOperationException($"Medicamento '{codigoMed}' no encontrado.");

                var req = new AplicarVacunaRequest(
                    IdAnimal: idAnimal,
                    IdMedicamento: idMed,
                    Fecha: fila.Valores.Fecha("Fecha"),
                    Dosis: fila.Valores.DecOpt("Dosis"),
                    Lote: fila.Valores.Opt("Lote"),
                    Responsable: fila.Valores.Opt("Responsable"),
                    ProximaFecha: fila.Valores.FechaOpt("ProximaFecha"));

                await vacunacionService.AplicarAsync(req, ct);
                exitosos++;
            }
            catch (Exception ex)
            {
                errores.Add(new ImportError(fila.NumeroFila, null, ex.Message));
            }
        }

        return new ImportResult(filas.Count, exitosos, errores.Count, errores);
    }
}
