using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportRegistrosICAService(
    IRegistroICAService registroICAService,
    IAnimalService animalService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var animales = (await animalService.ListarActivosAsync(ct))
            .ToDictionary(a => a.Codigo.Trim(), a => a.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                var codigo = fila.Valores.Req("CodigoAnimal");
                if (!animales.TryGetValue(codigo, out var idAnimal))
                    throw new InvalidOperationException($"Animal '{codigo}' no encontrado.");

                var req = new EmitirRegistroICARequest(
                    IdAnimal: idAnimal,
                    TipoDocumento: fila.Valores.Req("TipoDocumento"),
                    NumeroDocumento: fila.Valores.Req("NumeroDocumento"),
                    FechaExpedicion: fila.Valores.Fecha("FechaExpedicion"),
                    FechaVencimiento: fila.Valores.FechaOpt("FechaVencimiento"),
                    EntidadEmisora: fila.Valores.Opt("EntidadEmisora"),
                    Observaciones: fila.Valores.Opt("Observaciones"));

                await registroICAService.EmitirAsync(req, ct);
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
