using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportCentrosCostoService(
    ICentroCostoService centroCostoService,
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
                int? idFinca = null;
                var nombreFinca = fila.Valores.Opt("NombreFinca");
                if (nombreFinca is not null)
                {
                    if (!fincas.TryGetValue(nombreFinca, out var fId))
                        throw new InvalidOperationException($"Finca '{nombreFinca}' no encontrada.");
                    idFinca = fId;
                }

                var req = new CrearCentroCostoRequest(
                    Codigo: fila.Valores.Req("Codigo"),
                    Nombre: fila.Valores.Req("Nombre"),
                    IdFinca: idFinca);

                await centroCostoService.CrearAsync(req, ct);
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

public sealed class ImportGastosService(
    IGastoGeneralService gastoService,
    ICentroCostoService centroCostoService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var centros = (await centroCostoService.ListarAsync(ct))
            .ToDictionary(c => c.Codigo.Trim(), c => c.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                int? idCentro = null;
                var codigoCentro = fila.Valores.Opt("CodigoCentroCosto");
                if (codigoCentro is not null)
                {
                    if (!centros.TryGetValue(codigoCentro, out var cId))
                        throw new InvalidOperationException($"Centro de costo '{codigoCentro}' no encontrado.");
                    idCentro = cId;
                }

                var req = new CrearGastoGeneralRequest(
                    Fecha: fila.Valores.Fecha("Fecha"),
                    Concepto: fila.Valores.Req("Concepto"),
                    Valor: fila.Valores.Dec("Valor"),
                    IdCentro: idCentro,
                    Proveedor: fila.Valores.Opt("Proveedor"),
                    Comprobante: fila.Valores.Opt("Comprobante"));

                await gastoService.CrearAsync(req, ct);
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

public sealed class ImportIngresosService(
    IIngresoService ingresoService,
    ICentroCostoService centroCostoService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var centros = (await centroCostoService.ListarAsync(ct))
            .ToDictionary(c => c.Codigo.Trim(), c => c.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                var codigoCentro = fila.Valores.Req("CodigoCentroCosto");
                if (!centros.TryGetValue(codigoCentro, out var idCentro))
                    throw new InvalidOperationException($"Centro de costo '{codigoCentro}' no encontrado.");

                var req = new CrearIngresoRequest(
                    Fecha: fila.Valores.Fecha("Fecha"),
                    IdCentro: idCentro,
                    TipoIngreso: fila.Valores.Req("TipoIngreso"),
                    Valor: fila.Valores.Dec("Valor"),
                    Concepto: fila.Valores.Opt("Concepto"),
                    Comprobante: fila.Valores.Opt("Comprobante"));

                await ingresoService.CrearAsync(req, ct);
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
