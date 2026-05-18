namespace BrahmanGan.Application.ImportacionMasiva;

public static class Plantillas
{
    private static readonly Dictionary<string, string> _headers = new(StringComparer.OrdinalIgnoreCase)
    {
        ["razas"]                   = "Codigo|Nombre|PropositoRaza",
        ["animales"]                = "Codigo|NombreRaza|Sexo|FechaNacimiento|NombreFinca|Nombre|PesoNacimiento|FechaIngreso|Observaciones",
        ["pesajes"]                 = "CodigoAnimal|Fecha|PesoKg|CondicionCorporal|MetodoPesaje|Responsable",
        ["fincas"]                  = "Nombre|NIT|Propietario|Direccion|Telefono|Email|AreaHectareas|IdMunicipio",
        ["potreros"]                = "NombreFinca|Codigo|Nombre|AreaHectareas|TipoPasto",
        ["medicamentos"]            = "Codigo|Nombre|Principio|TipoUso|Unidad|PrecioUnitario|TiempoCarne|TiempoLeche",
        ["vacunaciones"]            = "CodigoAnimal|CodigoMedicamento|Fecha|Dosis|Lote|Responsable|ProximaFecha",
        ["servicios-reproductivos"] = "CodigoHembra|CodigoToro|Fecha|Responsable",
        ["gestaciones"]             = "CodigoAnimal|FechaInicio|Observaciones",
        ["control-leche"]           = "CodigoAnimal|Fecha|Maniana|Tarde|Noche|Ordeno",
        ["produccion-leche"]        = "NombreFinca|Fecha|TotalLitros|Vendidos|Autoconsumo|Merma",
        ["ventas-leche"]            = "DocumentoCliente|Fecha|Litros|PrecioLitro|Factura",
        ["clientes"]                = "Documento|RazonSocial|TipoDocumento|Contacto|Telefono|Email|Direccion|IdMunicipio|TipoCliente",
        ["centros-costo"]           = "Codigo|Nombre|NombreFinca",
        ["gastos"]                  = "Fecha|Concepto|Valor|CodigoCentroCosto|Proveedor|Comprobante",
        ["ingresos"]                = "Fecha|CodigoCentroCosto|TipoIngreso|Valor|Concepto|Comprobante",
        ["insumos"]                 = "Codigo|Nombre|Tipo|UnidadMedida|PrecioUnitario|StockMinimo|StockInicial",
        ["maquinaria"]              = "CodigoCentroCosto|Codigo|Nombre|Marca|Modelo|Anio|NumeroSerie|FechaCompra|ValorCompra",
        ["trabajadores"]            = "Cedula|Nombres|Apellidos|FechaIngreso|Cargo|SalarioBase|TipoContrato",
        ["pagos-jornal"]            = "CedulaTrabajador|Fecha|ValorJornal|HorasTrabajadas|Concepto|CodigoCentroCosto",
        ["registros-ica"]           = "CodigoAnimal|TipoDocumento|NumeroDocumento|FechaExpedicion|FechaVencimiento|EntidadEmisora|Observaciones",
        ["captura-carbono"]         = "NombreFinca|Anio|Mes|EmisionesGanadoTCO2|CapturaForestal|Certificacion|Observaciones",
        ["consumo-agua"]            = "NombreFinca|Fecha|VolumenM3|FuenteAgua|NumAnimales|Observaciones",
    };

    public static IReadOnlyCollection<string> Modulos => _headers.Keys;

    public static string ObtenerEncabezados(string modulo) =>
        _headers.TryGetValue(modulo, out var h)
            ? h
            : throw new ArgumentException($"Módulo '{modulo}' no tiene plantilla definida.");
}
