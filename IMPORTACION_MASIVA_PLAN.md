# Plan de Implementación: Módulo de Importación Masiva vía CSV (pipe `|`)

**Proyecto:** BrahmanGan  
**Fecha:** 2026-05-18  
**Autor:** Plan generado con Claude Code  
**Estimado total:** ~8.5 días laborales (2 semanas)

---

## 1. Objetivo

Implementar un módulo transversal de importación masiva que permita cargar datos a todos los módulos del sistema mediante archivos CSV con separador pipe `|`. El diseño prioriza **velocidad de implementación**, reutilizando los servicios y repositorios existentes sin alterar la arquitectura hexagonal vigente.

---

## 2. Principios de Diseño para Implementación Rápida

| Decisión | Justificación |
|---|---|
| Reutilizar servicios de aplicación existentes | Evita duplicar lógica de negocio y validaciones |
| Un único `CsvPipeParser<T>` genérico | Centraliza el parseo; un solo punto de mantenimiento |
| `ImportResult<T>` unificado | Respuesta homogénea: éxitos, errores con número de línea |
| Archivo CSV plano (sin relaciones anidadas) | Simplifica la UI y la carga; claves foráneas por código/nombre |
| Validación por fila con FluentValidation existente | Reutiliza los `AbstractValidator<T>` ya definidos |
| Un controller por módulo (ruta `/api/importacion/{modulo}`) | Sigue la convención de controllers del proyecto |

---

## 3. Arquitectura del Módulo

```
BrahmanGan.Application/
└── ImportacionMasiva/
    ├── CsvPipeParser.cs              ← parser genérico
    ├── ImportResult.cs               ← DTO resultado unificado
    ├── IImportService.cs             ← interfaz base
    └── Servicios/
        ├── ImportAnimalesService.cs
        ├── ImportFincasService.cs
        ├── ImportReproduccionService.cs
        ├── ImportSanidadService.cs
        ├── ImportLecheService.cs
        ├── ImportComercialService.cs
        ├── ImportCostosService.cs
        ├── ImportAlmacenService.cs
        ├── ImportEquiposService.cs
        ├── ImportNominaService.cs
        ├── ImportTrazabilidadService.cs
        └── ImportSostenibilidadService.cs

BrahmanGan.API/Controllers/Importacion/
└── ImportacionMasivaController.cs    ← un solo controller, endpoints por módulo
```

### 3.1 Contrato base (pseudocódigo)

```csharp
// CsvPipeParser.cs
public static class CsvPipeParser
{
    public static IEnumerable<ParseRow<T>> Parse<T>(Stream file) where T : class, new()
    // Devuelve índice de fila, objeto T y errores de parseo por fila
}

// ImportResult.cs
public record ImportResult(
    int TotalFilas,
    int Exitosos,
    int Fallidos,
    IReadOnlyList<ImportError> Errores
);
public record ImportError(int Fila, string Campo, string Mensaje);

// IImportService.cs
public interface IImportService<TRow>
{
    Task<ImportResult> ImportarAsync(Stream csvStream, Guid fincaId, CancellationToken ct);
}
```

### 3.2 Endpoint único de importación

```
POST /api/importacion/animales        multipart/form-data  →  file + fincaId
POST /api/importacion/potreros        multipart/form-data
POST /api/importacion/razas           multipart/form-data
POST /api/importacion/medicamentos    multipart/form-data
POST /api/importacion/vacunaciones    multipart/form-data
POST /api/importacion/produccion-leche
POST /api/importacion/clientes
POST /api/importacion/centros-costo
POST /api/importacion/insumos
POST /api/importacion/maquinaria
POST /api/importacion/trabajadores
POST /api/importacion/pagos-jornal
POST /api/importacion/registros-ica
POST /api/importacion/carbono
POST /api/importacion/consumo-agua
GET  /api/importacion/plantilla/{modulo}   ← descarga CSV plantilla vacía
```

---

## 4. Módulos y Definición de Columnas CSV

> Formato de encabezado: `COLUMNA|tipo|requerido|descripcion`  
> Primera fila del CSV SIEMPRE es encabezado; se ignora.  
> Fechas en formato `YYYY-MM-DD`.  
> Booleanos: `S` / `N`.

---

### 4.1 Módulo: Inventario — Animales

**Archivo:** `animales.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Numero` | string(20) | ✓ | Número/código del animal |
| 2 | `NombreRaza` | string | ✓ | Nombre de la raza (debe existir) |
| 3 | `Sexo` | enum | ✓ | `MACHO` / `HEMBRA` |
| 4 | `FechaNacimiento` | date | ✓ | YYYY-MM-DD |
| 5 | `NombreOrigen` | string | ✓ | Nombre del origen (debe existir) |
| 6 | `Peso` | decimal | | Peso inicial en kg |
| 7 | `NombrePotrero` | string | | Potrero actual (debe existir en la finca) |
| 8 | `PropositoRaza` | enum | ✓ | `CARNE` / `LECHE` / `DOBLE_PROPOSITO` |
| 9 | `Estado` | enum | | `ACTIVO` (default) / `VENDIDO` / `MUERTO` |
| 10 | `Observaciones` | string(500) | | |

**Ejemplo:**
```
Numero|NombreRaza|Sexo|FechaNacimiento|NombreOrigen|Peso|NombrePotrero|PropositoRaza|Estado|Observaciones
A001|Brahman|HEMBRA|2023-03-15|Propio|280|Potrero Norte|CARNE|ACTIVO|
A002|Brahman|MACHO|2022-11-01|Comprado|350|Potrero Sur|CARNE|ACTIVO|Semental importado
```

---

### 4.2 Módulo: Inventario — Razas

**Archivo:** `razas.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Nombre` | string(100) | ✓ | Nombre de la raza |
| 2 | `PropositoRaza` | enum | ✓ | `CARNE` / `LECHE` / `DOBLE_PROPOSITO` |
| 3 | `Descripcion` | string(300) | | |

---

### 4.3 Módulo: Inventario — Pesajes

**Archivo:** `pesajes.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `NumeroAnimal` | string | ✓ | Código del animal |
| 2 | `FechaPesaje` | date | ✓ | |
| 3 | `Peso` | decimal | ✓ | kg |
| 4 | `Observaciones` | string(300) | | |

---

### 4.4 Módulo: Finca — Fincas

**Archivo:** `fincas.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Nombre` | string(150) | ✓ | |
| 2 | `NombreMunicipio` | string | ✓ | Debe existir |
| 3 | `HectareasTotales` | decimal | ✓ | |
| 4 | `HectareasPastoreo` | decimal | | |
| 5 | `Propietario` | string(150) | | |
| 6 | `Telefono` | string(20) | | |

---

### 4.5 Módulo: Finca — Potreros

**Archivo:** `potreros.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `NombreFinca` | string | ✓ | Debe existir |
| 2 | `Nombre` | string(100) | ✓ | |
| 3 | `Hectareas` | decimal | ✓ | |
| 4 | `TipoPasto` | string(100) | | |
| 5 | `CapacidadAnimales` | int | | |

---

### 4.6 Módulo: Reproducción — Servicios

**Archivo:** `servicios.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `NumeroHembra` | string | ✓ | Código animal hembra |
| 2 | `FechaServicio` | date | ✓ | |
| 3 | `TipoServicio` | enum | ✓ | `MONTA` / `IA` |
| 4 | `NumeroMacho` | string | | Para MONTA |
| 5 | `CodigoSemen` | string | | Para IA |
| 6 | `Confirmado` | bool | | `S` / `N` |
| 7 | `Observaciones` | string(300) | | |

---

### 4.7 Módulo: Reproducción — Gestaciones

**Archivo:** `gestaciones.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `NumeroHembra` | string | ✓ | |
| 2 | `FechaInicio` | date | ✓ | |
| 3 | `FechaProbableParto` | date | | |
| 4 | `Estado` | enum | | `EN_CURSO` / `PARIDA` / `ABORTADA` |
| 5 | `Observaciones` | string(300) | | |

---

### 4.8 Módulo: Sanidad — Medicamentos

**Archivo:** `medicamentos.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Nombre` | string(150) | ✓ | |
| 2 | `Principio` | string(150) | | Principio activo |
| 3 | `TipoMedicamento` | string(50) | ✓ | `VACUNA` / `ANTIPARASITARIO` / `ANTIBIOTICO` / etc. |
| 4 | `UnidadMedida` | string(20) | ✓ | `ml` / `g` / `dosis` |
| 5 | `Stock` | decimal | | Stock inicial |
| 6 | `Observaciones` | string(300) | | |

---

### 4.9 Módulo: Sanidad — Vacunaciones

**Archivo:** `vacunaciones.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `NumeroAnimal` | string | ✓ | |
| 2 | `NombreMedicamento` | string | ✓ | Debe existir |
| 3 | `FechaAplicacion` | date | ✓ | |
| 4 | `Dosis` | decimal | ✓ | |
| 5 | `ProximaAplicacion` | date | | |
| 6 | `Observaciones` | string(300) | | |

---

### 4.10 Módulo: Sanidad — Desparasitaciones

**Archivo:** `desparasitaciones.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `NumeroAnimal` | string | ✓ | |
| 2 | `NombreMedicamento` | string | ✓ | |
| 3 | `FechaAplicacion` | date | ✓ | |
| 4 | `Dosis` | decimal | ✓ | |
| 5 | `Observaciones` | string(300) | | |

---

### 4.11 Módulo: Sanidad — Tratamientos Curativos

**Archivo:** `curativos.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `NumeroAnimal` | string | ✓ | |
| 2 | `FechaInicio` | date | ✓ | |
| 3 | `Diagnostico` | string(300) | ✓ | |
| 4 | `NombreMedicamento` | string | ✓ | |
| 5 | `Dosis` | decimal | ✓ | |
| 6 | `Costo` | decimal | | |
| 7 | `Observaciones` | string(300) | | |

---

### 4.12 Módulo: Leche — Producción

**Archivo:** `produccion-leche.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Fecha` | date | ✓ | |
| 2 | `NumeroAnimal` | string | ✓ | |
| 3 | `LitrosMañana` | decimal | | |
| 4 | `LitrosTarde` | decimal | | |
| 5 | `LitrosTotal` | decimal | ✓ | |
| 6 | `Observaciones` | string(300) | | |

---

### 4.13 Módulo: Leche — Ventas

**Archivo:** `ventas-leche.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Fecha` | date | ✓ | |
| 2 | `Litros` | decimal | ✓ | |
| 3 | `PrecioLitro` | decimal | ✓ | |
| 4 | `NombreCliente` | string | | |
| 5 | `Observaciones` | string(300) | | |

---

### 4.14 Módulo: Comercial — Clientes

**Archivo:** `clientes.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `TipoDocumento` | string(10) | ✓ | `NIT` / `CC` / `CE` |
| 2 | `NumeroDocumento` | string(20) | ✓ | |
| 3 | `NombreCompleto` | string(200) | ✓ | |
| 4 | `Email` | string(150) | | |
| 5 | `Telefono` | string(20) | | |
| 6 | `Direccion` | string(300) | | |
| 7 | `NombreMunicipio` | string | | |
| 8 | `Observaciones` | string(300) | | |

---

### 4.15 Módulo: Costos — Centros de Costo

**Archivo:** `centros-costo.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Codigo` | string(20) | ✓ | |
| 2 | `Nombre` | string(150) | ✓ | |
| 3 | `Tipo` | string(50) | ✓ | `PRODUCCION` / `ADMINISTRATIVO` / `VENTA` |
| 4 | `Descripcion` | string(300) | | |

---

### 4.16 Módulo: Costos — Gastos Generales

**Archivo:** `gastos.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Fecha` | date | ✓ | |
| 2 | `CodigoCentroCosto` | string | ✓ | Debe existir |
| 3 | `Concepto` | string(200) | ✓ | |
| 4 | `Valor` | decimal | ✓ | |
| 5 | `Proveedor` | string(150) | | |
| 6 | `Observaciones` | string(300) | | |

---

### 4.17 Módulo: Almacén — Insumos

**Archivo:** `insumos.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Codigo` | string(20) | ✓ | |
| 2 | `Nombre` | string(150) | ✓ | |
| 3 | `TipoInsumo` | string(50) | ✓ | `ALIMENTO` / `FERTILIZANTE` / `HERBICIDA` / etc. |
| 4 | `UnidadMedida` | string(20) | ✓ | |
| 5 | `StockInicial` | decimal | | |
| 6 | `PrecioUnitario` | decimal | | |
| 7 | `Observaciones` | string(300) | | |

---

### 4.18 Módulo: Equipos — Maquinaria

**Archivo:** `maquinaria.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Codigo` | string(20) | ✓ | |
| 2 | `Nombre` | string(150) | ✓ | |
| 3 | `Marca` | string(100) | | |
| 4 | `Modelo` | string(100) | | |
| 5 | `Año` | int | | |
| 6 | `Estado` | enum | ✓ | `OPERATIVO` / `EN_MANTENIMIENTO` / `FUERA_SERVICIO` |
| 7 | `ValorAdquisicion` | decimal | | |
| 8 | `FechaAdquisicion` | date | | |
| 9 | `Observaciones` | string(300) | | |

---

### 4.19 Módulo: Nómina — Trabajadores

**Archivo:** `trabajadores.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `TipoDocumento` | string(10) | ✓ | `CC` / `CE` |
| 2 | `NumeroDocumento` | string(20) | ✓ | |
| 3 | `Nombres` | string(100) | ✓ | |
| 4 | `Apellidos` | string(100) | ✓ | |
| 5 | `Cargo` | string(100) | ✓ | |
| 6 | `SalarioDiario` | decimal | ✓ | |
| 7 | `FechaIngreso` | date | ✓ | |
| 8 | `Telefono` | string(20) | | |
| 9 | `Activo` | bool | | `S` / `N` (default S) |

---

### 4.20 Módulo: Nómina — Pagos de Jornal

**Archivo:** `pagos-jornal.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `NumeroDocumentoTrabajador` | string | ✓ | Debe existir |
| 2 | `FechaPago` | date | ✓ | |
| 3 | `DiasTrabajados` | decimal | ✓ | |
| 4 | `ValorDia` | decimal | ✓ | |
| 5 | `TotalPagado` | decimal | ✓ | |
| 6 | `Concepto` | string(200) | | |

---

### 4.21 Módulo: Trazabilidad — Registros ICA

**Archivo:** `registros-ica.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `NumeroAnimal` | string | ✓ | |
| 2 | `NumeroRegistroICA` | string(50) | ✓ | |
| 3 | `FechaRegistro` | date | ✓ | |
| 4 | `FechaVencimiento` | date | | |
| 5 | `TipoRegistro` | string(50) | ✓ | |
| 6 | `Estado` | enum | | `VIGENTE` / `VENCIDO` / `CANCELADO` |
| 7 | `Observaciones` | string(500) | | |

---

### 4.22 Módulo: Sostenibilidad — Captura de Carbono

**Archivo:** `captura-carbono.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Fecha` | date | ✓ | |
| 2 | `NombrePotrero` | string | ✓ | |
| 3 | `ToneladasCO2` | decimal | ✓ | |
| 4 | `MetodologiaMedicion` | string(150) | | |
| 5 | `Observaciones` | string(300) | | |

---

### 4.23 Módulo: Sostenibilidad — Consumo de Agua

**Archivo:** `consumo-agua.csv`

| # | Columna | Tipo | Req | Descripción |
|---|---------|------|-----|-------------|
| 1 | `Fecha` | date | ✓ | |
| 2 | `FuenteAgua` | string(100) | ✓ | |
| 3 | `MetrosCubicos` | decimal | ✓ | |
| 4 | `NombrePotrero` | string | | |
| 5 | `Observaciones` | string(300) | | |

---

## 5. Plan de Implementación por Fases

### Fase 1 — Infraestructura Común (1 día)

| Tarea | Archivo | Tiempo |
|---|---|---|
| `CsvPipeParser<T>` genérico con mapeo por nombre de columna | `Application/ImportacionMasiva/CsvPipeParser.cs` | 2h |
| `ImportResult` + `ImportError` DTOs | `Application/ImportacionMasiva/ImportResult.cs` | 0.5h |
| `IImportService<TRow>` interfaz base | `Application/ImportacionMasiva/IImportService.cs` | 0.5h |
| `ImportacionMasivaController` base con upload + validación de archivo | `API/Controllers/Importacion/ImportacionMasivaController.cs` | 2h |
| Endpoint `GET /api/importacion/plantilla/{modulo}` para descarga de plantillas | mismo controller | 1h |
| Registro DI en `DependencyInjection.cs` de Application | | 0.5h |
| **Total Fase 1** | | **~6.5h** |

---

### Fase 2 — Módulos Catálogos/Maestros (2 días)

Entidades simples, pocas referencias externas:

| Módulo | Servicio | Tiempo |
|---|---|---|
| Razas | `ImportRazasService` | 1h |
| Orígenes | `ImportOrigenesService` | 1h |
| Fincas | `ImportFincasService` | 1.5h |
| Potreros | `ImportPotrerosService` | 1.5h |
| Medicamentos | `ImportMedicamentosService` | 1.5h |
| Clientes | `ImportClientesService` | 1.5h |
| Centros de Costo | `ImportCentrosCostoService` | 1h |
| Insumos | `ImportInsumosService` | 1.5h |
| Maquinaria | `ImportMaquinariaService` | 1.5h |
| Trabajadores | `ImportTrabajadoresService` | 1.5h |
| **Total Fase 2** | | **~13h ≈ 2 días** |

---

### Fase 3 — Módulos Transaccionales (4 días)

Entidades con referencias cruzadas y validaciones de negocio más complejas:

| Módulo | Servicio | Tiempo |
|---|---|---|
| Animales (ref Raza, Origen, Finca, Potrero) | `ImportAnimalesService` | 4h |
| Pesajes | `ImportPesajesService` | 2h |
| Servicios Reproductivos | `ImportServiciosReprodService` | 3h |
| Gestaciones | `ImportGestacionesService` | 2h |
| Vacunaciones | `ImportVacunacionesService` | 2h |
| Desparasitaciones | `ImportDesparasitacionesService` | 1.5h |
| Tratamientos Curativos | `ImportCurativosService` | 2h |
| Producción Leche | `ImportProduccionLecheService` | 2h |
| Ventas Leche | `ImportVentasLecheService` | 1.5h |
| Pagos de Jornal | `ImportPagosJornalService` | 2h |
| Gastos Generales | `ImportGastosService` | 2h |
| Registros ICA | `ImportRegistrosICAService` | 2.5h |
| Captura Carbono | `ImportCarbonoCapturaService` | 1.5h |
| Consumo Agua | `ImportConsumoAguaService` | 1.5h |
| **Total Fase 3** | | **~31h ≈ 4 días** |

---

### Fase 4 — Testing, Plantillas y Documentación (1.5 días)

| Tarea | Tiempo |
|---|---|
| Unit tests: `CsvPipeParser<T>` (casos límite: comillas, líneas vacías, encoding) | 2h |
| Integration tests básicos por módulo (happy path + error path) | 3h |
| Generación de archivos CSV de plantilla por módulo (hardcoded headers) | 2h |
| Anotaciones Swagger en controller (`[ProducesResponseType]`, examples) | 1h |
| Validación de tamaño máximo de archivo (default 10 MB) | 0.5h |
| **Total Fase 4** | | **~8.5h ≈ 1.5 días** |

---

## 6. Resumen de Tiempos

| Fase | Descripción | Días Laborales |
|---|---|---|
| Fase 1 | Infraestructura común (parser, DTOs, controller base) | **1** |
| Fase 2 | Módulos catálogos/maestros (10 módulos simples) | **2** |
| Fase 3 | Módulos transaccionales (14 módulos complejos) | **4** |
| Fase 4 | Testing, plantillas, Swagger | **1.5** |
| **Total** | | **8.5 días ≈ 2 semanas** |

> Estimado con un solo desarrollador full-time. Con dos desarrolladores en paralelo (Fase 2 y Fase 3 simultáneas): **~5 días ≈ 1 semana**.

---

## 7. Comportamiento de Errores por Fila

El sistema procesará **todas las filas** aunque algunas fallen (no se detiene al primer error). La respuesta incluirá:

```json
{
  "totalFilas": 100,
  "exitosos": 97,
  "fallidos": 3,
  "errores": [
    { "fila": 12, "campo": "NumeroAnimal", "mensaje": "El animal A099 no existe en la finca." },
    { "fila": 34, "campo": "FechaNacimiento", "mensaje": "Formato de fecha inválido: '15/03/2023'. Use YYYY-MM-DD." },
    { "fila": 67, "campo": "NombreRaza", "mensaje": "La raza 'Angus' no está registrada." }
  ]
}
```

Las **filas exitosas se persisten inmediatamente**; las fallidas se reportan sin revertir las exitosas (comportamiento append, no transaccional por lote).

> Si se requiere **todo-o-nada**: wrappear el loop de inserción en un `IUnitOfWork` transaction y hacer rollback si `fallidos > 0`.

---

## 8. Consideraciones de Seguridad

- Endpoint protegido con `[Authorize]` — requiere JWT válido.
- Autorización por rol: solo `Administrador` y `EncargadoFinca` pueden importar.
- Tamaño máximo de archivo: 10 MB (configurable en `appsettings.json`).
- Tipo MIME validado: solo `text/csv` y `text/plain`.
- Sanitización de strings antes de persistir (evitar inyección SQL a través de campos de texto).
- Rate limiting: máximo 5 importaciones por minuto por usuario.

---

## 9. Deuda Técnica Aceptada (por velocidad)

| Item | Impacto | Solución futura |
|---|---|---|
| Importación síncrona (bloquea request) | Archivos >1000 filas pueden tardar | Mover a background job (Hangfire/Channels) |
| Sin progreso en tiempo real | Usuario no sabe el avance | SignalR o polling endpoint |
| Sin historial de importaciones | No auditoría de quién importó qué | Tabla `HistorialImportacion` |
| Sin validación de duplicados antes de insertar | Pueden generarse duplicados | Pre-check con HashSet de códigos existentes |
| Sin importación de entidades compuestas (DetalleCotizacion, etc.) | Cotizaciones deben crearse manualmente | Formato multi-tabla futuro |

---

## 10. Orden de Implementación Recomendado

```
1. CsvPipeParser + ImportResult + IImportService       (día 1 mañana)
2. ImportacionMasivaController + plantillas            (día 1 tarde)
3. Razas → Orígenes → Fincas → Potreros               (día 2)
4. Medicamentos → Insumos → Maquinaria                 (día 3 mañana)
5. Clientes → Trabajadores → CentrosCosto              (día 3 tarde)
6. Animales (más complejo, empieza cuando catálogos están listos)  (día 4)
7. Pesajes → Vacunaciones → Desparasitaciones → Curativos          (día 4-5)
8. Servicios reproductivos → Gestaciones               (día 5)
9. Producción leche → Ventas leche → Gastos → Pagos jornal         (día 6)
10. RegistroICA → Carbono → ConsumoAgua                (día 7)
11. Tests + Plantillas + Swagger                        (día 8-9)
```
