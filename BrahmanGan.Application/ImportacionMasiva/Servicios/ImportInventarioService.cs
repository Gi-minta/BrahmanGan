using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

public sealed class ImportRazasService(IRazaService razaService)
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
                var req = new CrearRazaRequest(
                    fila.Valores.Req("Codigo"),
                    fila.Valores.Req("Nombre"),
                    fila.Valores.EnumOpt<PropositoRaza>("PropositoRaza"));
                await razaService.CrearAsync(req, ct);
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

public sealed class ImportAnimalesService(
    IAnimalService animalService,
    IRazaService razaService,
    IFincaService fincaService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var razas = (await razaService.ListarAsync(ct))
            .ToDictionary(r => r.Nombre.Trim(), r => r.Id, StringComparer.OrdinalIgnoreCase);
        var fincas = (await fincaService.ListarAsync(ct))
            .ToDictionary(f => f.Nombre.Trim(), f => f.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                var nombreRaza = fila.Valores.Req("NombreRaza");
                if (!razas.TryGetValue(nombreRaza, out var idRaza))
                    throw new InvalidOperationException($"Raza '{nombreRaza}' no encontrada.");

                var nombreFinca = fila.Valores.Req("NombreFinca");
                if (!fincas.TryGetValue(nombreFinca, out var idFinca))
                    throw new InvalidOperationException($"Finca '{nombreFinca}' no encontrada.");

                var req = new CrearAnimalRequest(
                    Codigo: fila.Valores.Req("Codigo"),
                    Sexo: fila.Valores.Enum<Sexo>("Sexo"),
                    IdRaza: idRaza,
                    IdFinca: idFinca,
                    Nombre: fila.Valores.Opt("Nombre"),
                    FechaNacimiento: fila.Valores.FechaOpt("FechaNacimiento"),
                    PesoNacimiento: fila.Valores.DecOpt("PesoNacimiento"),
                    FechaIngreso: fila.Valores.FechaOpt("FechaIngreso"),
                    Observaciones: fila.Valores.Opt("Observaciones"));

                await animalService.RegistrarAsync(req, ct);
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

public sealed class ImportPesajesService(
    IPesajeService pesajeService,
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

                var req = new RegistrarPesajeRequest(
                    IdAnimal: idAnimal,
                    Fecha: fila.Valores.Fecha("Fecha"),
                    PesoKg: fila.Valores.Dec("PesoKg"),
                    CondicionCorporal: fila.Valores.DecOpt("CondicionCorporal"),
                    MetodoPesaje: fila.Valores.Opt("MetodoPesaje"),
                    Responsable: fila.Valores.Opt("Responsable"));

                await pesajeService.RegistrarAsync(req, ct);
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
