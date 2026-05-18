using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportCapturaCarbonoService(
    ISostenibilidadService sostenibilidadService,
    IFincaService fincaService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var fincas = (await fincaService.ListarAsync(ct))
            .ToDictionary(f => f.Nombre.Trim(), f => f.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                var nombreFinca = fila.Valores.Req("NombreFinca");
                if (!fincas.TryGetValue(nombreFinca, out var idFinca))
                    throw new InvalidOperationException($"Finca '{nombreFinca}' no encontrada.");

                var req = new RegistrarCapturaCarbonoRequest(
                    IdFinca: idFinca,
                    Anio: fila.Valores.Int32("Anio"),
                    Mes: fila.Valores.Int32("Mes"),
                    EmisionesGanadoTCO2: fila.Valores.DecOpt("EmisionesGanadoTCO2"),
                    CapturaForestal: fila.Valores.DecOpt("CapturaForestal"),
                    Certificacion: fila.Valores.Opt("Certificacion"),
                    Observaciones: fila.Valores.Opt("Observaciones"));

                await sostenibilidadService.RegistrarCapturaAsync(req, ct);
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

public sealed class ImportConsumoAguaService(
    ISostenibilidadService sostenibilidadService,
    IFincaService fincaService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var fincas = (await fincaService.ListarAsync(ct))
            .ToDictionary(f => f.Nombre.Trim(), f => f.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                var nombreFinca = fila.Valores.Req("NombreFinca");
                if (!fincas.TryGetValue(nombreFinca, out var idFinca))
                    throw new InvalidOperationException($"Finca '{nombreFinca}' no encontrada.");

                var req = new RegistrarConsumoAguaRequest(
                    IdFinca: idFinca,
                    Fecha: fila.Valores.Fecha("Fecha"),
                    VolumenM3: fila.Valores.Dec("VolumenM3"),
                    FuenteAgua: fila.Valores.Opt("FuenteAgua"),
                    NumAnimales: fila.Valores.Int32Opt("NumAnimales"),
                    Observaciones: fila.Valores.Opt("Observaciones"));

                await sostenibilidadService.RegistrarConsumoAguaAsync(req, ct);
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
