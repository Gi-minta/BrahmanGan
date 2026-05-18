using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportClientesService(IClienteService clienteService)
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
                var req = new CrearClienteRequest(
                    Documento: fila.Valores.Req("Documento"),
                    RazonSocial: fila.Valores.Req("RazonSocial"),
                    TipoDocumento: fila.Valores.Opt("TipoDocumento") ?? "NIT",
                    Contacto: fila.Valores.Opt("Contacto"),
                    Telefono: fila.Valores.Opt("Telefono"),
                    Email: fila.Valores.Opt("Email"),
                    Direccion: fila.Valores.Opt("Direccion"),
                    IdMunicipio: fila.Valores.Int32Opt("IdMunicipio"),
                    TipoCliente: fila.Valores.Opt("TipoCliente"));

                await clienteService.CrearAsync(req, ct);
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
