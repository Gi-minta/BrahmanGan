using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportServiciosReproductivosService(
    IServicioReproductivoService servicioService,
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
                var codigoHembra = fila.Valores.Req("CodigoHembra");
                if (!animales.TryGetValue(codigoHembra, out var idHembra))
                    throw new InvalidOperationException($"Hembra '{codigoHembra}' no encontrada.");

                var codigoToro = fila.Valores.Req("CodigoToro");
                if (!animales.TryGetValue(codigoToro, out var idToro))
                    throw new InvalidOperationException($"Toro '{codigoToro}' no encontrado.");

                var req = new RegistrarMontaRequest(
                    IdHembra: idHembra,
                    IdToro: idToro,
                    Fecha: fila.Valores.Fecha("Fecha"),
                    Responsable: fila.Valores.Opt("Responsable"));

                await servicioService.RegistrarMontaAsync(req, ct);
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

public sealed class ImportGestacionesService(
    IGestacionService gestacionService,
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

                var req = new IniciarGestacionRequest(
                    IdAnimal: idAnimal,
                    FechaInicio: fila.Valores.Fecha("FechaInicio"),
                    Observaciones: fila.Valores.Opt("Observaciones"));

                await gestacionService.IniciarAsync(req, ct);
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
