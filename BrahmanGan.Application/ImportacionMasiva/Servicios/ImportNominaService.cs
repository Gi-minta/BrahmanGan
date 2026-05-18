using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportTrabajadoresService(ITrabajadorService trabajadorService)
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
                var req = new ContratarTrabajadorRequest(
                    Cedula: fila.Valores.Req("Cedula"),
                    Nombres: fila.Valores.Req("Nombres"),
                    Apellidos: fila.Valores.Req("Apellidos"),
                    FechaIngreso: fila.Valores.Fecha("FechaIngreso"),
                    Cargo: fila.Valores.Opt("Cargo"),
                    SalarioBase: fila.Valores.DecOpt("SalarioBase"),
                    TipoContrato: fila.Valores.Opt("TipoContrato"));

                await trabajadorService.ContratarAsync(req, ct);
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

public sealed class ImportPagosJornalService(
    ITrabajadorService trabajadorService,
    ICentroCostoService centroCostoService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var trabajadores = (await trabajadorService.ListarAsync(ct))
            .ToDictionary(t => t.Cedula.Trim(), t => t.Id, StringComparer.OrdinalIgnoreCase);
        var centros = (await centroCostoService.ListarAsync(ct))
            .ToDictionary(c => c.Codigo.Trim(), c => c.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                var cedula = fila.Valores.Req("CedulaTrabajador");
                if (!trabajadores.TryGetValue(cedula, out var idTrabajador))
                    throw new InvalidOperationException($"Trabajador con cédula '{cedula}' no encontrado.");

                int? idCentro = null;
                var codigoCentro = fila.Valores.Opt("CodigoCentroCosto");
                if (codigoCentro is not null)
                {
                    if (!centros.TryGetValue(codigoCentro, out var cId))
                        throw new InvalidOperationException($"Centro de costo '{codigoCentro}' no encontrado.");
                    idCentro = cId;
                }

                var req = new RegistrarPagoJornalRequest(
                    IdTrabajador: idTrabajador,
                    Fecha: fila.Valores.Fecha("Fecha"),
                    ValorJornal: fila.Valores.Dec("ValorJornal"),
                    HorasTrabajadas: fila.Valores.DecOpt("HorasTrabajadas"),
                    Concepto: fila.Valores.Opt("Concepto"),
                    IdCentro: idCentro);

                await trabajadorService.RegistrarPagoAsync(req, ct);
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
