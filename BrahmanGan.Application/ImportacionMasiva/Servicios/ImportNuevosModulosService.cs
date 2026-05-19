using BrahmanGan.Application.DTOs;
using BrahmanGan.Application.Ports.Input;

namespace BrahmanGan.Application.ImportacionMasiva.Servicios;

// ===== Planes de Alimentación =====
public sealed class ImportPlanesAlimentacionService(
    IAlimentacionService alimentacionService,
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

                var req = new CrearPlanAlimentacionRequest(
                    IdFinca:       idFinca,
                    Nombre:        fila.Valores.Req("NombrePlan"),
                    FechaInicio:   fila.Valores.Fecha("FechaInicio"),
                    FechaFin:      fila.Valores.FechaOpt("FechaFin"),
                    Observaciones: fila.Valores.Opt("Observaciones"));

                await alimentacionService.CrearPlanAsync(req, ct);
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

// ===== Detalles / Componentes de Plan de Alimentación =====
public sealed class ImportDetallesAlimentacionService(IAlimentacionService alimentacionService)
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
                var req = new AgregarDetallePlanRequest(
                    IdPlan:         fila.Valores.Int32("IdPlan"),
                    Alimento:       fila.Valores.Req("Alimento"),
                    CantidadDiaria: fila.Valores.Dec("CantidadDiaria"),
                    UnidadMedida:   fila.Valores.Opt("UnidadMedida"),
                    IdInsumo:       fila.Valores.Int32Opt("IdInsumo"),
                    Observaciones:  fila.Valores.Opt("Observaciones"));

                await alimentacionService.AgregarDetalleAsync(req, ct);
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

// ===== Planes de Pastoreo =====
public sealed class ImportPlanesPastoreoService(
    IPastoreoService pastoreoService,
    IFincaService fincaService,
    IPotreroService potreroService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var fincas = (await fincaService.ListarAsync(ct))
            .ToDictionary(f => f.Nombre.Trim(), f => f.Id, StringComparer.OrdinalIgnoreCase);

        // Key: (nombreFinca, codigoPotrero) → idPotrero
        var potreros = new Dictionary<(string, string), int>();
        foreach (var (nombreFinca, idFinca) in fincas)
        {
            var lista = await potreroService.ListarPorFincaAsync(idFinca, ct);
            foreach (var p in lista)
                potreros[(nombreFinca.ToUpperInvariant(), p.Codigo.Trim().ToUpperInvariant())] = p.Id;
        }

        foreach (var fila in filas)
        {
            try
            {
                var nombreFinca    = fila.Valores.Req("NombreFinca").Trim();
                var codigoPotrero  = fila.Valores.Req("CodigoPotrero").Trim();
                var key            = (nombreFinca.ToUpperInvariant(), codigoPotrero.ToUpperInvariant());

                if (!potreros.TryGetValue(key, out var idPotrero))
                    throw new InvalidOperationException(
                        $"Potrero '{codigoPotrero}' no encontrado en finca '{nombreFinca}'.");

                var req = new CrearPlanPastoreoRequest(
                    IdPotrero:      idPotrero,
                    FechaInicio:    fila.Valores.Fecha("FechaInicio"),
                    FechaFin:       fila.Valores.FechaOpt("FechaFin"),
                    NumAnimales:    fila.Valores.Int32Opt("NumAnimales"),
                    CapacidadCarga: fila.Valores.DecOpt("CapacidadCarga"),
                    Observaciones:  fila.Valores.Opt("Observaciones"));

                await pastoreoService.CrearPlanAsync(req, ct);
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

// ===== Complementos de Tratamiento Curativo =====
public sealed class ImportComplementosService(IComplementoService complementoService)
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
                var req = new RegistrarComplementoRequest(
                    IdTratamiento: fila.Valores.Int32("IdTratamiento"),
                    Fecha:         fila.Valores.Fecha("Fecha"),
                    Descripcion:   fila.Valores.Req("Descripcion"),
                    Tipo:          fila.Valores.Opt("Tipo"),
                    Costo:         fila.Valores.DecOpt("Costo"));

                await complementoService.RegistrarAsync(req, ct);
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

// ===== Desparasitaciones =====
public sealed class ImportDesparasitacionesService(
    IVacunacionService vacunacionService,
    IAnimalService animalService,
    IMedicamentoService medicamentoService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var animales    = (await animalService.ListarActivosAsync(ct))
            .ToDictionary(a => a.Codigo.Trim(), a => a.Id, StringComparer.OrdinalIgnoreCase);
        var medicamentos = (await medicamentoService.ListarAsync(ct))
            .ToDictionary(m => m.Codigo.Trim(), m => m.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                var codigoAnimal = fila.Valores.Req("CodigoAnimal");
                if (!animales.TryGetValue(codigoAnimal, out var idAnimal))
                    throw new InvalidOperationException($"Animal '{codigoAnimal}' no encontrado.");

                var codigoMed = fila.Valores.Req("CodigoMedicamento");
                if (!medicamentos.TryGetValue(codigoMed, out var idMed))
                    throw new InvalidOperationException($"Medicamento '{codigoMed}' no encontrado.");

                var req = new AplicarDesparasitacionRequest(
                    IdAnimal:      idAnimal,
                    IdMedicamento: idMed,
                    Fecha:         fila.Valores.Fecha("Fecha"),
                    Dosis:         fila.Valores.DecOpt("Dosis"),
                    TipoParasito:  fila.Valores.Opt("TipoParasito"),
                    ProximaFecha:  fila.Valores.FechaOpt("ProximaFecha"));

                await vacunacionService.AplicarDesparasitacionAsync(req, ct);
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

// ===== Aplicación de Controles Preventivos =====
public sealed class ImportControlesPreventivosService(
    IMedicamentoService medicamentoService,
    IAnimalService animalService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var animales    = (await animalService.ListarActivosAsync(ct))
            .ToDictionary(a => a.Codigo.Trim(), a => a.Id, StringComparer.OrdinalIgnoreCase);
        var controles   = (await medicamentoService.ListarControlesAsync(ct))
            .ToDictionary(c => c.Nombre.Trim(), c => c.Id, StringComparer.OrdinalIgnoreCase);
        var medicamentos = (await medicamentoService.ListarAsync(ct))
            .ToDictionary(m => m.Codigo.Trim(), m => m.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                var codigoAnimal = fila.Valores.Req("CodigoAnimal");
                if (!animales.TryGetValue(codigoAnimal, out var idAnimal))
                    throw new InvalidOperationException($"Animal '{codigoAnimal}' no encontrado.");

                var nombreControl = fila.Valores.Req("NombreControl");
                if (!controles.TryGetValue(nombreControl, out var idControl))
                    throw new InvalidOperationException($"Control '{nombreControl}' no encontrado.");

                int? idMed = null;
                var codigoMed = fila.Valores.Opt("CodigoMedicamento");
                if (codigoMed is not null)
                {
                    if (!medicamentos.TryGetValue(codigoMed, out var mId))
                        throw new InvalidOperationException($"Medicamento '{codigoMed}' no encontrado.");
                    idMed = mId;
                }

                var req = new AplicarControlPreventivoRequest(
                    IdAnimal:      idAnimal,
                    IdControl:     idControl,
                    Fecha:         fila.Valores.Fecha("Fecha"),
                    IdMedicamento: idMed,
                    Dosis:         fila.Valores.DecOpt("Dosis"),
                    Responsable:   fila.Valores.Opt("Responsable"),
                    ProximaFecha:  fila.Valores.FechaOpt("ProximaFecha"));

                await medicamentoService.AplicarControlAsync(req, ct);
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

// ===== Lactancias (Parámetros de Lactancia) =====
public sealed class ImportLactanciasService(
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
                var codigoAnimal = fila.Valores.Req("CodigoAnimal");
                if (!animales.TryGetValue(codigoAnimal, out var idAnimal))
                    throw new InvalidOperationException($"Animal '{codigoAnimal}' no encontrado.");

                var req = new IniciarParametroLactanciaRequest(
                    IdAnimal:    idAnimal,
                    NumeroParto: fila.Valores.Int32("NumeroParto"),
                    FechaInicio: fila.Valores.Fecha("FechaInicio"));

                await controlLecheService.IniciarLactanciaAsync(req, ct);
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

// ===== Calidad de Leche =====
public sealed class ImportCalidadLecheService(
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
                int? idAnimal = null;
                var codigoAnimal = fila.Valores.Opt("CodigoAnimal");
                if (codigoAnimal is not null)
                {
                    if (!animales.TryGetValue(codigoAnimal, out var aId))
                        throw new InvalidOperationException($"Animal '{codigoAnimal}' no encontrado.");
                    idAnimal = aId;
                }

                var req = new RegistrarCalidadLecheRequest(
                    Fecha:         fila.Valores.Fecha("Fecha"),
                    IdAnimal:      idAnimal,
                    CelSomaticas:  fila.Valores.Int32Opt("CelSomaticas"),
                    GrasaPct:      fila.Valores.DecOpt("GrasaPct"),
                    ProteinaPct:   fila.Valores.DecOpt("ProteinaPct"),
                    LactozaPct:    fila.Valores.DecOpt("LactozaPct"),
                    UreaMgDL:      fila.Valores.DecOpt("UreaMgDL"),
                    Laboratorio:   fila.Valores.Opt("Laboratorio"),
                    Resultado:     fila.Valores.Opt("Resultado"),
                    Observaciones: fila.Valores.Opt("Observaciones"));

                await controlLecheService.RegistrarCalidadAsync(req, ct);
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

// ===== Banco de Semen =====
public sealed class ImportSemenService(
    IServicioReproductivoService servicioService,
    IRazaService razaService)
{
    public async Task<ImportResult> ImportarAsync(Stream csv, CancellationToken ct = default)
    {
        var filas = await CsvPipeParser.LeerAsync(csv, ct);
        var errores = new List<ImportError>();
        int exitosos = 0;

        var razas = (await razaService.ListarAsync(ct))
            .ToDictionary(r => r.Nombre.Trim(), r => r.Id, StringComparer.OrdinalIgnoreCase);

        foreach (var fila in filas)
        {
            try
            {
                int? idRaza = null;
                var nombreRaza = fila.Valores.Opt("NombreRaza");
                if (nombreRaza is not null)
                {
                    if (!razas.TryGetValue(nombreRaza, out var rId))
                        throw new InvalidOperationException($"Raza '{nombreRaza}' no encontrada.");
                    idRaza = rId;
                }

                var req = new CrearSemenRequest(
                    Codigo:       fila.Valores.Req("Codigo"),
                    NombreToro:   fila.Valores.Req("NombreToro"),
                    IdRaza:       idRaza,
                    Casa:         fila.Valores.Opt("Casa"),
                    StockInicial: fila.Valores.Int32("StockInicial"));

                await servicioService.CrearSemenAsync(req, ct);
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
