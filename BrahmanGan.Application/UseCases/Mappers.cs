using BrahmanGan.Application.DTOs;
using BrahmanGan.Domain.Modulos.Inventario;
using BrahmanGan.Domain.Modulos.Reproduccion;
using BrahmanGan.Domain.Modulos.Sanidad;
using BrahmanGan.Domain.Modulos.Leche;
using BrahmanGan.Domain.Modulos.Comercial;
using BrahmanGan.Domain.Modulos.Costos;
using BrahmanGan.Domain.Modulos.Almacen;
using BrahmanGan.Domain.Modulos.Equipos;
using BrahmanGan.Domain.Modulos.Trazabilidad;
using BrahmanGan.Domain.Modulos.Nomina;
using BrahmanGan.Domain.Modulos.Sostenibilidad;
using BrahmanGan.Domain.Modulos.Alimentacion;
using BrahmanGan.Domain.Modulos.Pastoreo;
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

    // ===== Fase 7: Costos =====
    public static CentroCostoResponse ToDto(this CentroCosto c) =>
        new(c.Id.Value, c.Codigo, c.Nombre, c.IdFinca?.Value, c.Activo);

    public static GastoGeneralResponse ToDto(this GastoGeneral g) =>
        new(g.Id.Value, g.Fecha, g.IdCentro?.Value, g.Concepto, g.Valor, g.Proveedor, g.Comprobante);

    public static IngresoResponse ToDto(this Ingreso i) =>
        new(i.Id.Value, i.Fecha, i.IdCentro.Value, i.TipoIngreso, i.Valor, i.Concepto, i.Comprobante);

    // ===== Fase 7: Almacén =====
    public static InsumoResponse ToDto(this Insumo i) =>
        new(i.Id.Value, i.Codigo, i.Nombre, i.Tipo, i.UnidadMedida, i.PrecioUnitario,
            i.StockMinimo, i.StockActual, i.Activo);

    public static KardexInsumoResponse ToDto(this KardexInsumo k) =>
        new(k.Id.Value, k.IdInsumo.Value, k.Fecha, k.TipoMovimiento.ToString(),
            k.Cantidad, k.CostoUnitario, k.Concepto, k.Referencia, k.SaldoAnterior, k.SaldoNuevo);

    // ===== Fase 7: Equipos =====
    public static MaquinariaResponse ToDto(this Maquinaria m) =>
        new(m.Id.Value, m.IdCentro.Value, m.Codigo, m.Nombre, m.Marca, m.Modelo, m.Anio,
            m.NumeroSerie, m.FechaCompra, m.ValorCompra, m.HorasUso, m.Estado.ToString());

    public static MantenimientoEquipoResponse ToDto(this MantenimientoEquipo m) =>
        new(m.Id.Value, m.IdMaquinaria.Value, m.Fecha, m.TipoMantenimiento.ToString(),
            m.Descripcion, m.Tecnico, m.Proveedor, m.CostoManoObra, m.CostoRepuestos,
            m.CostoTotal, m.ProximoMantenimiento, m.HorasAlMomento);

    // ===== Fase 7: Trazabilidad =====
    public static RegistroICAResponse ToDto(this RegistroICA r) =>
        new(r.Id.Value, r.IdAnimal.Value, r.TipoDocumento, r.NumeroDocumento,
            r.FechaExpedicion, r.FechaVencimiento, r.EntidadEmisora,
            r.IdMunicipio?.Value, r.Estado.ToString(), r.UrlDocumento, r.Observaciones);

    // ===== Fase 8: Nómina =====
    public static TrabajadorResponse ToDto(this Trabajador t) =>
        new(t.Id.Value, t.Cedula, t.Nombres, t.Apellidos, t.Cargo,
            t.FechaIngreso, t.FechaRetiro, t.SalarioBase, t.TipoContrato, t.Activo);

    public static PagoJornalResponse ToDto(this PagoJornal p) =>
        new(p.Id.Value, p.IdTrabajador.Value, p.Fecha, p.HorasTrabajadas,
            p.ValorJornal, p.Concepto, p.IdCentro?.Value);

    // ===== Fase 8: Sostenibilidad =====
    public static CapturaCarbonoResponse ToDto(this CapturaCarbono c) =>
        new(c.Id.Value, c.IdFinca.Value, c.Anio, c.Mes,
            c.EmisionesGanadoTCO2, c.CapturaForestal, c.HuellaNeta,
            c.Certificacion, c.Observaciones);

    public static ConsumoAguaResponse ToDto(this ConsumoAgua c) =>
        new(c.Id.Value, c.IdFinca.Value, c.IdPotrero?.Value, c.Fecha, c.FuenteAgua,
            c.VolumenM3, c.NumAnimales, c.LitrosAnimalDia, c.Observaciones);

    public static EventoMedioambientalResponse ToDto(this EventoMedioambiental e) =>
        new(e.Id.Value, e.IdFinca.Value, e.Fecha, e.TipoEvento,
            e.Descripcion, e.Intensidad, e.PrecipitacionMM, e.TempMaxC, e.TempMinC, e.ImpactoEstimado);

    // ===== Alimentación =====
    public static PlanAlimentacionResponse ToDto(this PlanAlimentacion p) =>
        new(p.Id.Value, p.IdFinca.Value, p.Nombre, p.FechaInicio, p.FechaFin, p.Observaciones, p.Activo);

    public static DetallePlanAlimentacionResponse ToDto(this DetallePlanAlimentacion d) =>
        new(d.Id.Value, d.IdPlan.Value, d.Alimento, d.CantidadDiaria,
            d.UnidadMedida, d.IdInsumo?.Value, d.Observaciones);

    // ===== Pastoreo =====
    public static PlanPastoreoResponse ToDto(this PlanPastoreo p) =>
        new(p.Id.Value, p.IdPotrero.Value, p.FechaInicio, p.FechaFin,
            p.NumAnimales, p.CapacidadCarga, p.Observaciones, p.Activo);

    // ===== Sanidad - Extendida =====
    public static DesparasitacionResponse ToDto(this HistorialDesparasitacion d) =>
        new(d.Id.Value, d.IdAnimal.Value, d.IdMedicamento.Value, d.Fecha, d.Dosis, d.TipoParasito, d.ProximaFecha);

    public static ControlPreventivoResponse ToDto(this ControlPreventivo c) =>
        new(c.Id.Value, c.Nombre, c.Periodicidad, c.Descripcion);

    public static HistorialPreventivoResponse ToDto(this HistorialPreventivo h) =>
        new(h.Id.Value, h.IdAnimal.Value, h.IdControl.Value, h.Fecha, h.IdMedicamento?.Value, h.Dosis, h.Responsable, h.ProximaFecha);

    public static ComplementoResponse ToDto(this Complemento c) =>
        new(c.Id.Value, c.IdTratamiento.Value, c.Fecha, c.Descripcion, c.Tipo, c.Costo);

    // ===== Reproducción - Extendida =====
    public static SemenResponse ToDto(this Semen s) =>
        new(s.Id.Value, s.Codigo, s.NombreToro, s.IdRaza?.Value, s.Casa, s.StockDosis, s.Activo);

    public static NacimientoResponse ToDto(this Nacimiento n) =>
        new(n.Id.Value, n.IdGestacion.Value, n.IdAnimalCria?.Value, n.Fecha, n.Sexo?.ToString(), n.PesoNacimiento, n.Condicion, n.Observaciones);

    // ===== Leche - Extendida =====
    public static ParametroLactanciaResponse ToDto(this ParametroLactancia p) =>
        new(p.Id.Value, p.IdAnimal.Value, p.NumeroParto, p.FechaInicio, p.FechaFin, p.LitrosTotales);

    public static CalidadLecheResponse ToDto(this CalidadLeche c) =>
        new(c.Id.Value, c.IdAnimal?.Value, c.Fecha, c.CelSomaticas, c.GrasaPct, c.ProteinaPct, c.LactozaPct, c.UreaMgDL, c.Laboratorio, c.Resultado, c.Observaciones);

    // ===== Finca - Extendida =====
    public static GrupoManejoResponse ToDto(this GrupoManejo g) =>
        new(g.Id.Value, g.Codigo, g.Nombre, g.Descripcion, g.TipoAnimal, g.Activo);

    public static AnimalPotreroResponse ToDto(this AnimalPotrero ap) =>
        new(ap.Id.Value, ap.IdAnimal.Value, ap.IdPotrero.Value, ap.FechaIngreso, ap.FechaSalida, ap.IdGrupo?.Value, ap.EstaVigente());

    public static AcumulacionInsumoPotreroResponse ToDto(this AcumulacionInsumoPotrero a) =>
        new(a.Id.Value, a.IdPotrero.Value, a.IdInsumo.Value, a.Fecha, a.Cantidad, a.CostoUnitario);

    // ===== Inventario - Extendida =====
    public static HistorialAnimalResponse ToDto(this HistorialAnimal h) =>
        new(h.Id.Value, h.IdAnimal.Value, h.TipoEvento.ToString(), h.Fecha, h.Descripcion, h.Valor, h.UsuarioRegistro, h.FechaRegistro);

    public static MovimientoAnimalResponse ToDto(this MovimientoAnimal m) =>
        new(m.Id.Value, m.IdAnimal.Value, m.TipoMovimiento.ToString(), m.Fecha, m.Valor, m.IdCentro, m.PesoKg, m.Observaciones);

    public static PedigriResponse ToDto(this Pedigri p) =>
        new(p.Id.Value, p.IdAnimal.Value, p.IdAbuelo1?.Value, p.IdAbuela1?.Value, p.IdAbuelo2?.Value, p.IdAbuela2?.Value, p.PuntajeMorfologia, p.Observaciones);
}
