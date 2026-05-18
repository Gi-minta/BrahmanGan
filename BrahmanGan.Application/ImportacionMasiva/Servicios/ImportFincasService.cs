using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportFincasService(IFincaService fincaService)
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
                var req = new CrearFincaRequest(
                    Nombre: fila.Valores.Req("Nombre"),
                    IdMunicipio: fila.Valores.Int32Opt("IdMunicipio"),
                    NIT: fila.Valores.Opt("NIT"),
                    Propietario: fila.Valores.Opt("Propietario"),
                    Direccion: fila.Valores.Opt("Direccion"),
                    Telefono: fila.Valores.Opt("Telefono"),
                    Email: fila.Valores.Opt("Email"),
                    AreaHectareas: fila.Valores.DecOpt("AreaHectareas"));

                await fincaService.CrearAsync(req, ct);
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

public sealed class ImportPotrerosService(
    IPotreroService potreroService,
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

                var req = new CrearPotreroRequest(
                    IdFinca: idFinca,
                    Codigo: fila.Valores.Req("Codigo"),
                    Nombre: fila.Valores.Req("Nombre"),
                    AreaHectareas: fila.Valores.DecOpt("AreaHectareas"),
                    TipoPasto: fila.Valores.Opt("TipoPasto"));

                await potreroService.CrearAsync(req, ct);
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
