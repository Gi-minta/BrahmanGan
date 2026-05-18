using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportMaquinariaService(
    IMaquinariaService maquinariaService,
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

                var req = new CrearMaquinariaRequest(
                    IdCentro: idCentro,
                    Codigo: fila.Valores.Req("Codigo"),
                    Nombre: fila.Valores.Req("Nombre"),
                    Marca: fila.Valores.Opt("Marca"),
                    Modelo: fila.Valores.Opt("Modelo"),
                    Anio: fila.Valores.Int32Opt("Anio"),
                    NumeroSerie: fila.Valores.Opt("NumeroSerie"),
                    FechaCompra: fila.Valores.FechaOpt("FechaCompra"),
                    ValorCompra: fila.Valores.DecOpt("ValorCompra"));

                await maquinariaService.CrearAsync(req, ct);
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
