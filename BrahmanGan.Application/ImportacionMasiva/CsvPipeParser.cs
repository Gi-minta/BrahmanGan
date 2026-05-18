using System.Globalization;
using System.Text;

namespace BrahmanGan.Application.ImportacionMasiva;

public sealed record FilaCsv(int NumeroFila, IReadOnlyDictionary<string, string> Valores);

public static class CsvPipeParser
{
    public static async Task<IReadOnlyList<FilaCsv>> LeerAsync(Stream stream, CancellationToken ct = default)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        var lineas = new List<string>();
        string? linea;
        while ((linea = await reader.ReadLineAsync(ct)) is not null)
            lineas.Add(linea);

        if (lineas.Count == 0) return [];

        var encabezados = lineas[0].Split('|').Select(h => h.Trim()).ToArray();
        var resultado = new List<FilaCsv>(lineas.Count - 1);

        for (int i = 1; i < lineas.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(lineas[i])) continue;
            var valores = lineas[i].Split('|');
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int j = 0; j < encabezados.Length; j++)
                dict[encabezados[j]] = j < valores.Length ? valores[j].Trim() : string.Empty;
            resultado.Add(new FilaCsv(i + 1, dict));
        }

        return resultado;
    }
}

internal static class FilaExtensions
{
    internal static string Req(this IReadOnlyDictionary<string, string> v, string col)
    {
        if (!v.TryGetValue(col, out var s) || string.IsNullOrWhiteSpace(s))
            throw new InvalidOperationException($"Columna '{col}' es requerida y está vacía.");
        return s.Trim();
    }

    internal static string? Opt(this IReadOnlyDictionary<string, string> v, string col) =>
        v.TryGetValue(col, out var s) && !string.IsNullOrWhiteSpace(s) ? s.Trim() : null;

    internal static DateOnly Fecha(this IReadOnlyDictionary<string, string> v, string col) =>
        DateOnly.ParseExact(v.Req(col), "yyyy-MM-dd", CultureInfo.InvariantCulture);

    internal static DateOnly? FechaOpt(this IReadOnlyDictionary<string, string> v, string col) =>
        v.Opt(col) is { } s ? DateOnly.ParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture) : null;

    internal static decimal Dec(this IReadOnlyDictionary<string, string> v, string col) =>
        decimal.Parse(v.Req(col), CultureInfo.InvariantCulture);

    internal static decimal? DecOpt(this IReadOnlyDictionary<string, string> v, string col) =>
        v.Opt(col) is { } s ? decimal.Parse(s, CultureInfo.InvariantCulture) : null;

    internal static int Int32(this IReadOnlyDictionary<string, string> v, string col) =>
        int.Parse(v.Req(col), CultureInfo.InvariantCulture);

    internal static int? Int32Opt(this IReadOnlyDictionary<string, string> v, string col) =>
        v.Opt(col) is { } s ? int.Parse(s, CultureInfo.InvariantCulture) : null;

    internal static bool Bool(this IReadOnlyDictionary<string, string> v, string col, bool def = false) =>
        v.Opt(col) is { } s ? s.Equals("S", StringComparison.OrdinalIgnoreCase) : def;

    internal static T Enum<T>(this IReadOnlyDictionary<string, string> v, string col) where T : struct, System.Enum =>
        System.Enum.Parse<T>(v.Req(col), ignoreCase: true);

    internal static T? EnumOpt<T>(this IReadOnlyDictionary<string, string> v, string col) where T : struct, System.Enum =>
        v.Opt(col) is { } s ? System.Enum.Parse<T>(s, ignoreCase: true) : null;
}
