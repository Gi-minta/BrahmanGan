using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportInsumosService(IInsumoService insumoService)
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
                var req = new CrearInsumoRequest(
                    Codigo: fila.Valores.Req("Codigo"),
                    Nombre: fila.Valores.Req("Nombre"),
                    Tipo: fila.Valores.Opt("Tipo"),
                    UnidadMedida: fila.Valores.Opt("UnidadMedida"),
                    PrecioUnitario: fila.Valores.DecOpt("PrecioUnitario"),
                    StockMinimo: fila.Valores.DecOpt("StockMinimo") ?? 0,
                    StockInicial: fila.Valores.DecOpt("StockInicial") ?? 0);

                await insumoService.CrearAsync(req, ct);
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
