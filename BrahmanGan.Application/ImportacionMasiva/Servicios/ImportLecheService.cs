using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportControlLecheService(
    IControlLecheService controlLecheService,
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

                var req = new RegistrarControlLecheRequest(
                    IdAnimal: idAnimal,
                    Fecha: fila.Valores.Fecha("Fecha"),
                    Maniana: fila.Valores.DecOpt("Maniana"),
                    Tarde: fila.Valores.DecOpt("Tarde"),
                    Noche: fila.Valores.DecOpt("Noche"),
                    Ordeno: fila.Valores.Opt("Ordeno"));

                await controlLecheService.RegistrarAsync(req, ct);
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

public sealed class ImportProduccionLecheService(
    IProduccionLecheService produccionService,
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

                var req = new RegistrarProduccionLecheRequest(
                    IdFinca: idFinca,
                    Fecha: fila.Valores.Fecha("Fecha"),
                    TotalLitros: fila.Valores.Dec("TotalLitros"),
                    Vendidos: fila.Valores.DecOpt("Vendidos"),
                    Autoconsumo: fila.Valores.DecOpt("Autoconsumo"),
                    Merma: fila.Valores.DecOpt("Merma"));

                await produccionService.RegistrarAsync(req, ct);
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

public sealed class ImportVentasLecheService(
    IVentaLecheService ventaService,
    IClienteService clienteService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var clientes = (await clienteService.ListarAsync(ct))
            .ToDictionary(c => c.Documento.Trim(), c => c.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                var docCliente = fila.Valores.Req("DocumentoCliente");
                if (!clientes.TryGetValue(docCliente, out var idCliente))
                    throw new InvalidOperationException($"Cliente con documento '{docCliente}' no encontrado.");

                var req = new RegistrarVentaLecheRequest(
                    Fecha: fila.Valores.Fecha("Fecha"),
                    IdCliente: idCliente,
                    Litros: fila.Valores.Dec("Litros"),
                    PrecioLitro: fila.Valores.Dec("PrecioLitro"),
                    Factura: fila.Valores.Opt("Factura"));

                await ventaService.RegistrarAsync(req, ct);
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
