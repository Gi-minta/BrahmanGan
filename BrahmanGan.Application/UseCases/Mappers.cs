using BrahmanGan.Application.DTOs;
using BrahmanGan.Domain.Modulos.Inventario;
using BrahmanGan.Domain.Modulos.Reproduccion;
using BrahmanGan.Domain.Modulos.Sanidad;
using BrahmanGan.Domain.Modulos.Leche;
using BrahmanGan.Domain.Modulos.Comercial;
using FincaEntity = BrahmanGan.Domain.Modulos.Finca.Finca;
using BrahmanGan.Domain.Modulos.Finca;

namespace BrahmanGan.Application.UseCases;

internal static class Mappers
{
    public static AnimalResponse ToDto(this Animal a) => new(
        a.Id.Value, a.Codigo, a.Nombre, a.IdRaza.Value, a.Sexo.ToString(),
        a.FechaNacimiento, a.IdMadre?.Value, a.IdPadre?.Value, a.IdOrigen?.Value,
        a.Estado.ToString(), a.PesoNacimiento, a.IdFinca.Value, a.FechaIngreso,
        a.Observaciones, a.FechaRegistro);

    public static RazaResponse ToDto(this Raza r) => new(r.Id.Value, r.Codigo, r.Nombre, r.Proposito?.ToString());

    public static PesajeResponse ToDto(this Pesaje p) => new(p.Id.Value, p.IdAnimal.Value, p.Fecha, p.PesoKg, p.CondicionCorporal);

    public static FincaResponse ToDto(this FincaEntity f) =>
        new(f.Id.Value, f.Nombre, f.NIT, f.Propietario, f.IdMunicipio?.Value, f.AreaHectareas, f.Activa);

    public static PotreroResponse ToDto(this Potrero p) =>
        new(p.Id.Value, p.IdFinca.Value, p.Codigo, p.Nombre, p.AreaHectareas, p.TipoPasto, p.Activo);

    public static ServicioResponse ToDto(this Servicio s) => new(
        s.Id.Value, s.IdHembra.Value, s.TipoServicio.ToString(), s.Fecha,
        s.IdToro?.Value, s.IdSemen?.Value, s.ResultadoPreniez, s.FechaConfirmacion);

    public static GestacionResponse ToDto(this Gestacion g) => new(
        g.Id.Value, g.IdAnimal.Value, g.IdServicio?.Value, g.FechaInicio,
        g.FechaPartoEstimado, g.FechaPartoReal, g.Estado.ToString());

    public static MedicamentoResponse ToDto(this Medicamento m) =>
        new(m.Id.Value, m.Codigo, m.Nombre, m.TiempoCarne, m.TiempoLeche, m.Activo);

    public static VacunacionResponse ToDto(this HistorialVacunacion v) =>
        new(v.Id.Value, v.IdAnimal.Value, v.IdMedicamento.Value, v.Fecha, v.Dosis, v.Lote, v.ProximaFecha);

    public static ControlLecheResponse ToDto(this ControlLecheAnimal c) =>
        new(c.Id.Value, c.IdAnimal.Value, c.Fecha, c.LitrosManiana, c.LitrosTarde, c.LitrosNoche, c.TotalLitros);

    public static ProduccionLecheResponse ToDto(this ProduccionLeche p) =>
        new(p.Id.Value, p.IdFinca.Value, p.Fecha, p.TotalLitros, p.LitrosVendidos, p.LitrosAutoconsumo, p.LitrosMerma);

    public static VentaLecheResponse ToDto(this VentaLeche v) =>
        new(v.Id.Value, v.Fecha, v.IdCliente.Value, v.LitrosVendidos, v.PrecioLitro, v.TotalVenta, v.IdContrato?.Value);

    public static ClienteResponse ToDto(this Cliente c) =>
        new(c.Id.Value, c.Documento, c.TipoDocumento, c.RazonSocial, c.Contacto, c.Telefono, c.Email?.Address, c.Activo);

    public static ContratoResponse ToDto(this Contrato c) =>
        new(c.Id.Value, c.IdCliente.Value, c.Tipo, c.FechaInicio, c.FechaFin, c.PrecioAcordado, c.Estado);

    public static CotizacionResponse ToDto(this CotizacionVenta c) =>
        new(c.Id.Value, c.IdCliente.Value, c.Fecha, c.PrecioOfertado, c.Estado);
}
