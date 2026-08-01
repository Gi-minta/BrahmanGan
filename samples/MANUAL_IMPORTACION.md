# Manual de Importación Masiva — BrahmanGan ERP

Guía de referencia para cargar datos históricos mediante archivos CSV con separador pipe `|`.

---

## 1. Reglas generales

| Regla | Valor |
|---|---|
| Separador de columnas | `\|` (pipe) |
| Codificación | UTF-8 sin BOM |
| Formato de fecha | `YYYY-MM-DD` |
| Separador decimal | `.` (punto) |
| Booleano verdadero | `S` |
| Booleano falso | `N` |
| Primera fila | Encabezados (requerida, se ignora en la importación) |
| Columnas opcionales vacías | Dejar la celda vacía entre pipes: `...\|campo-anterior\|\|campo-siguiente\|...` |
| Tamaño máximo por archivo | 10 MB |
| Filas en error | Se reportan sin revertir las filas exitosas |

### Flujo de importación en la UI

```
Módulos  →  Seleccionar módulo  →  Descargar plantilla (opcional)
         →  Arrastrar / seleccionar archivo CSV
         →  [Importar archivo]
         →  Ver resumen: Total · Exitosos · Fallidos + tabla de errores
```

### Flujo vía API (Scalar / Postman)

```
POST /api/importacion/{modulo}
Content-Type: multipart/form-data
Body: archivo = <el fichero .csv>

Response 200:
{
  "totalFilas": 6,
  "exitosos": 5,
  "fallidos": 1,
  "errores": [
    { "fila": 4, "campo": "CodigoAnimal", "mensaje": "Animal 'BOV-999' no encontrado" }
  ]
}
```

---

## 2. Orden de carga recomendado

Los módulos tienen dependencias entre sí. Cargar en el orden incorrecto genera errores de FK no encontrado.

```
FASE 1 — Maestros base (sin dependencias)
  ├── razas
  ├── fincas
  ├── medicamentos
  ├── clientes
  ├── insumos
  └── trabajadores

FASE 2 — Maestros dependientes
  ├── animales          (→ razas, fincas)
  ├── potreros          (→ fincas)
  ├── centros-costo     (→ fincas)
  └── semen             (→ razas, opcional)

FASE 3 — Transaccional inventario
  ├── pesajes           (→ animales)
  └── maquinaria        (→ centros-costo)

FASE 4 — Reproducción
  ├── servicios-reproductivos  (→ animales)
  └── gestaciones              (→ animales)

FASE 5 — Sanidad
  ├── vacunaciones       (→ animales, medicamentos)
  ├── desparasitaciones  (→ animales, medicamentos)
  ├── aplicar-controles  (→ animales, medicamentos opt.)
  └── complementos       (→ IdTratamiento numérico)

FASE 6 — Leche
  ├── lactancias         (→ animales)
  ├── control-leche      (→ animales)
  ├── produccion-leche   (→ fincas)
  ├── ventas-leche       (→ clientes)
  └── calidad-leche      (→ animales opt.)

FASE 7 — Alimentación y Pastoreo
  ├── planes-alimentacion   (→ fincas)
  ├── detalles-alimentacion (→ planes-alimentacion)
  └── planes-pastoreo       (→ fincas, potreros)

FASE 8 — Costos
  ├── gastos    (→ centros-costo)
  └── ingresos  (→ centros-costo)

FASE 9 — Nómina
  └── pagos-jornal  (→ trabajadores, centros-costo)

FASE 10 — Trazabilidad y Sostenibilidad
  ├── registros-ica    (→ animales)
  ├── captura-carbono  (→ fincas)
  └── consumo-agua     (→ fincas)
```

---

## 3. Módulos — referencia detallada

### 3.1 `razas`

Catálogo de razas bovinas. No tiene dependencias; debe cargarse primero.

**Endpoint:** `POST /api/importacion/razas`  
**Archivo de muestra:** `samples/csv/razas.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Codigo` | Sí | Texto | Código único de la raza (ej. `BRH`) |
| `Nombre` | Sí | Texto | Nombre completo (ej. `Brahman`) |
| `PropositoRaza` | Sí | Texto | `CARNE`, `LECHE` o `DOBLE_PROPOSITO` |

**Errores frecuentes**
- `PropositoRaza` con valor fuera de los tres permitidos.
- `Codigo` duplicado en la base de datos.

---

### 3.2 `fincas`

Propiedades ganaderas. No tiene dependencias.

**Endpoint:** `POST /api/importacion/fincas`  
**Archivo de muestra:** `samples/csv/fincas.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Nombre` | Sí | Texto | Nombre único de la finca |
| `NIT` | No | Texto | NIT del propietario |
| `Propietario` | No | Texto | Nombre del propietario |
| `Direccion` | No | Texto | Dirección rural |
| `Telefono` | No | Texto | Teléfono de contacto |
| `Email` | No | Texto | Correo electrónico |
| `AreaHectareas` | No | Decimal | Área total en hectáreas |
| `IdMunicipio` | No | Entero | ID del municipio en el catálogo DIVIPOLA |

**Errores frecuentes**
- `Nombre` duplicado: la finca ya existe.

---

### 3.3 `potreros`

> **Depende de:** `fincas`

**Endpoint:** `POST /api/importacion/potreros`  
**Archivo de muestra:** `samples/csv/potreros.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `NombreFinca` | Sí | Texto | Nombre exacto de la finca (ya cargada) |
| `Codigo` | Sí | Texto | Código único del potrero dentro de la finca |
| `Nombre` | Sí | Texto | Nombre descriptivo del potrero |
| `AreaHectareas` | No | Decimal | Área en hectáreas |
| `TipoPasto` | No | Texto | Especie de pasto principal |

**Errores frecuentes**
- `NombreFinca` no coincide exactamente con el nombre registrado (diferencia de tildes o espacios).

---

### 3.4 `animales`

> **Depende de:** `razas`, `fincas`

**Endpoint:** `POST /api/importacion/animales`  
**Archivo de muestra:** `samples/csv/animales.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Codigo` | Sí | Texto | Código único del animal (ej. `BOV-001`) |
| `NombreRaza` | Sí | Texto | Nombre de la raza ya cargada |
| `Sexo` | Sí | Texto | `M` (macho) o `H` (hembra) |
| `FechaNacimiento` | Sí | Fecha | Fecha de nacimiento |
| `NombreFinca` | Sí | Texto | Nombre exacto de la finca |
| `Nombre` | No | Texto | Nombre propio del animal |
| `PesoNacimiento` | No | Decimal | Peso en kg al nacer |
| `FechaIngreso` | No | Fecha | Fecha de ingreso a la finca |
| `Observaciones` | No | Texto | Notas adicionales |

---

### 3.5 `pesajes`

> **Depende de:** `animales`

**Endpoint:** `POST /api/importacion/pesajes`  
**Archivo de muestra:** `samples/csv/pesajes.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `CodigoAnimal` | Sí | Texto | Código del animal |
| `Fecha` | Sí | Fecha | Fecha del pesaje |
| `PesoKg` | Sí | Decimal | Peso registrado en kg |
| `CondicionCorporal` | No | Decimal | Escala 1-5 |
| `MetodoPesaje` | No | Texto | `BASCULA`, `CINTA_TORACICA`, etc. |
| `Responsable` | No | Texto | Nombre del operario |

---

### 3.6 `medicamentos`

Catálogo de medicamentos y biológicos. No tiene dependencias.

**Endpoint:** `POST /api/importacion/medicamentos`  
**Archivo de muestra:** `samples/csv/medicamentos.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Codigo` | Sí | Texto | Código único (ej. `VAC-001`) |
| `Nombre` | Sí | Texto | Nombre comercial |
| `Principio` | No | Texto | Principio activo |
| `TipoUso` | Sí | Texto | `VACUNA`, `ANTIPARASITARIO`, `ANTIBIOTICO`, `VITAMINA`, `OTRO` |
| `Unidad` | Sí | Texto | Unidad de medida (`ml`, `g`, `tableta`) |
| `PrecioUnitario` | No | Decimal | Precio por unidad en COP |
| `TiempoCarne` | No | Entero | Días retiro en carne |
| `TiempoLeche` | No | Entero | Días retiro en leche |

---

### 3.7 `vacunaciones`

> **Depende de:** `animales`, `medicamentos`

**Endpoint:** `POST /api/importacion/vacunaciones`  
**Archivo de muestra:** `samples/csv/vacunaciones.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `CodigoAnimal` | Sí | Texto | Código del animal |
| `CodigoMedicamento` | Sí | Texto | Código del medicamento |
| `Fecha` | Sí | Fecha | Fecha de aplicación |
| `Dosis` | Sí | Decimal | Dosis aplicada en la unidad del medicamento |
| `Lote` | No | Texto | Número de lote del biológico |
| `Responsable` | No | Texto | Nombre del responsable |
| `ProximaFecha` | No | Fecha | Fecha de refuerzo programado |

---

### 3.8 `desparasitaciones`

> **Depende de:** `animales`, `medicamentos`

**Endpoint:** `POST /api/importacion/desparasitaciones`  
**Archivo de muestra:** `samples/csv/desparasitaciones.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `CodigoAnimal` | Sí | Texto | Código del animal |
| `CodigoMedicamento` | Sí | Texto | Código del antiparasitario |
| `Fecha` | Sí | Fecha | Fecha de aplicación |
| `Dosis` | Sí | Decimal | Dosis aplicada |
| `TipoParasito` | No | Texto | Descripción del tipo de parásito tratado |
| `ProximaFecha` | No | Fecha | Próxima desparasitación programada |

**Errores frecuentes**
- Usar un medicamento de tipo `VACUNA` en lugar de `ANTIPARASITARIO`. El sistema permite cualquier código de medicamento, pero verifique que el tipo sea coherente.

---

### 3.9 `aplicar-controles`

Aplica controles preventivos del catálogo a animales individuales.

> **Depende de:** `animales`, `medicamentos` (medicamento es opcional)

**Endpoint:** `POST /api/importacion/aplicar-controles`  
**Archivo de muestra:** `samples/csv/aplicar-controles.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `CodigoAnimal` | Sí | Texto | Código del animal |
| `NombreControl` | Sí | Texto | Nombre exacto del control preventivo en el catálogo |
| `Fecha` | Sí | Fecha | Fecha de aplicación |
| `CodigoMedicamento` | No | Texto | Código del medicamento utilizado (dejar vacío si no aplica) |
| `Dosis` | No | Decimal | Dosis aplicada |
| `Responsable` | No | Texto | Nombre del responsable |
| `ProximaFecha` | No | Fecha | Próxima aplicación programada |

> **Nota:** Si `CodigoMedicamento` está vacío, el registro se crea sin asociar medicamento. Si `NombreControl` no existe en el catálogo, la fila falla.

---

### 3.10 `complementos`

Complementos adicionales a un tratamiento existente (vitaminas, sueros, probióticos).

> **Depende de:** El tratamiento debe existir en la base de datos. `IdTratamiento` es el ID numérico generado por el sistema.

**Endpoint:** `POST /api/importacion/complementos`  
**Archivo de muestra:** `samples/csv/complementos.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `IdTratamiento` | Sí | Entero | ID numérico del tratamiento padre |
| `Fecha` | Sí | Fecha | Fecha del complemento |
| `Descripcion` | Sí | Texto | Descripción del complemento aplicado |
| `Tipo` | Sí | Texto | `VITAMINA`, `SUPLEMENTO`, `OTRO` |
| `Costo` | No | Decimal | Costo en COP |

> **Recomendación:** Consulte primero los tratamientos en la base de datos para obtener los IDs numéricos correctos antes de preparar este archivo.

---

### 3.11 `servicios-reproductivos`

> **Depende de:** `animales` (hembra y toro)

**Endpoint:** `POST /api/importacion/servicios-reproductivos`  
**Archivo de muestra:** `samples/csv/servicios-reproductivos.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `CodigoHembra` | Sí | Texto | Código del animal hembra |
| `CodigoToro` | Sí | Texto | Código del toro (monta natural) o código de pajilla (IA) |
| `Fecha` | Sí | Fecha | Fecha del servicio |
| `Responsable` | No | Texto | Nombre del inseminador / responsable |

---

### 3.12 `gestaciones`

> **Depende de:** `animales`

**Endpoint:** `POST /api/importacion/gestaciones`  
**Archivo de muestra:** `samples/csv/gestaciones.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `CodigoAnimal` | Sí | Texto | Código de la hembra gestante |
| `FechaInicio` | Sí | Fecha | Fecha de confirmación de gestación |
| `Observaciones` | No | Texto | Notas (diagnóstico, método de confirmación, etc.) |

---

### 3.13 `semen`

Inventario inicial de pajillas de semen.

> **Depende de:** `razas` (lookup por NombreRaza, opcional — si no existe se deja sin asignar)

**Endpoint:** `POST /api/importacion/semen`  
**Archivo de muestra:** `samples/csv/semen.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Codigo` | Sí | Texto | Código único de la pajilla o lote (ej. `SEM-BRH-001`) |
| `NombreToro` | Sí | Texto | Nombre del toro donante |
| `NombreRaza` | No | Texto | Nombre de la raza (debe existir en el catálogo) |
| `Casa` | No | Texto | Casa comercial o laboratorio proveedor |
| `StockInicial` | Sí | Entero | Número de pajillas disponibles al inicio |

---

### 3.14 `lactancias`

Períodos de lactancia por animal (inicio de cada parto / ciclo productivo).

> **Depende de:** `animales`

**Endpoint:** `POST /api/importacion/lactancias`  
**Archivo de muestra:** `samples/csv/lactancias.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `CodigoAnimal` | Sí | Texto | Código de la hembra |
| `NumeroParto` | Sí | Entero | Número de parto (1 = primípara, 2, 3…) |
| `FechaInicio` | Sí | Fecha | Fecha de inicio de la lactancia (fecha del parto) |

> **Nota:** Solo deben incluirse hembras. Un animal no puede tener dos lactancias con el mismo `NumeroParto`.

---

### 3.15 `control-leche`

Registros diarios de producción por animal y ordeño.

> **Depende de:** `animales`

**Endpoint:** `POST /api/importacion/control-leche`  
**Archivo de muestra:** `samples/csv/control-leche.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `CodigoAnimal` | Sí | Texto | Código del animal |
| `Fecha` | Sí | Fecha | Fecha del registro |
| `Maniana` | Sí | Decimal | Litros ordeño mañana |
| `Tarde` | Sí | Decimal | Litros ordeño tarde |
| `Noche` | No | Decimal | Litros ordeño noche (0 si no aplica) |
| `Ordeno` | No | Texto | Nombre del ordeñador |

---

### 3.16 `produccion-leche`

Producción total diaria por finca (consolidado, no por animal).

> **Depende de:** `fincas`

**Endpoint:** `POST /api/importacion/produccion-leche`  
**Archivo de muestra:** `samples/csv/produccion-leche.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `NombreFinca` | Sí | Texto | Nombre exacto de la finca |
| `Fecha` | Sí | Fecha | Fecha de producción |
| `TotalLitros` | Sí | Decimal | Total producido en el día |
| `Vendidos` | No | Decimal | Litros despachados a clientes |
| `Autoconsumo` | No | Decimal | Litros consumidos en finca |
| `Merma` | No | Decimal | Litros perdidos |

---

### 3.17 `calidad-leche`

Resultados de análisis de laboratorio. `CodigoAnimal` es opcional; si se omite el resultado se asocia a nivel de finca/tanque.

> **Depende de:** `animales` (opcional)

**Endpoint:** `POST /api/importacion/calidad-leche`  
**Archivo de muestra:** `samples/csv/calidad-leche.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Fecha` | Sí | Fecha | Fecha del análisis |
| `CodigoAnimal` | No | Texto | Código del animal (vacío = muestra de tanque) |
| `CelSomaticas` | No | Entero | Células somáticas (cél/mL) |
| `GrasaPct` | No | Decimal | Porcentaje de grasa |
| `ProteinaPct` | No | Decimal | Porcentaje de proteína |
| `LactozaPct` | No | Decimal | Porcentaje de lactoza |
| `UreaMgDL` | No | Decimal | Urea en mg/dL |
| `Laboratorio` | No | Texto | Nombre del laboratorio |
| `Resultado` | No | Texto | `APTO`, `OBSERVACIÓN`, `RECHAZADO` |
| `Observaciones` | No | Texto | Notas del analista |

---

### 3.18 `ventas-leche`

> **Depende de:** `clientes`

**Endpoint:** `POST /api/importacion/ventas-leche`  
**Archivo de muestra:** `samples/csv/ventas-leche.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `DocumentoCliente` | Sí | Texto | Documento del cliente ya cargado |
| `Fecha` | Sí | Fecha | Fecha de la venta |
| `Litros` | Sí | Decimal | Litros vendidos |
| `PrecioLitro` | Sí | Decimal | Precio por litro en COP |
| `Factura` | No | Texto | Número de factura o remisión |

---

### 3.19 `clientes`

Clientes y compradores. No tiene dependencias.

**Endpoint:** `POST /api/importacion/clientes`  
**Archivo de muestra:** `samples/csv/clientes.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Documento` | Sí | Texto | NIT o cédula del cliente |
| `RazonSocial` | Sí | Texto | Nombre o razón social |
| `TipoDocumento` | Sí | Texto | `NIT`, `CC`, `CE` |
| `Contacto` | No | Texto | Nombre del contacto |
| `Telefono` | No | Texto | Teléfono |
| `Email` | No | Texto | Correo electrónico |
| `Direccion` | No | Texto | Dirección |
| `IdMunicipio` | No | Entero | ID del municipio DIVIPOLA |
| `TipoCliente` | No | Texto | `ACOPIO`, `INDUSTRIA`, `DIRECTO`, `OTRO` |

---

### 3.20 `centros-costo`

> **Depende de:** `fincas`

**Endpoint:** `POST /api/importacion/centros-costo`  
**Archivo de muestra:** `samples/csv/centros-costo.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Codigo` | Sí | Texto | Código único del centro (ej. `CC-ESP-01`) |
| `Nombre` | Sí | Texto | Nombre descriptivo |
| `NombreFinca` | Sí | Texto | Nombre exacto de la finca |

---

### 3.21 `gastos`

> **Depende de:** `centros-costo`

**Endpoint:** `POST /api/importacion/gastos`  
**Archivo de muestra:** `samples/csv/gastos.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Fecha` | Sí | Fecha | Fecha del gasto |
| `Concepto` | Sí | Texto | Descripción del gasto |
| `Valor` | Sí | Decimal | Valor en COP |
| `CodigoCentroCosto` | Sí | Texto | Código del centro de costo |
| `Proveedor` | No | Texto | Nombre del proveedor |
| `Comprobante` | No | Texto | Número de factura o recibo |

---

### 3.22 `ingresos`

> **Depende de:** `centros-costo`

**Endpoint:** `POST /api/importacion/ingresos`  
**Archivo de muestra:** `samples/csv/ingresos.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Fecha` | Sí | Fecha | Fecha del ingreso |
| `CodigoCentroCosto` | Sí | Texto | Código del centro de costo |
| `TipoIngreso` | Sí | Texto | `VENTA_LECHE`, `VENTA_GANADO`, `SUBVENCIÓN`, `OTRO` |
| `Valor` | Sí | Decimal | Valor en COP |
| `Concepto` | No | Texto | Descripción adicional |
| `Comprobante` | No | Texto | Número de documento soporte |

---

### 3.23 `insumos`

Catálogo de insumos del almacén. No tiene dependencias.

**Endpoint:** `POST /api/importacion/insumos`  
**Archivo de muestra:** `samples/csv/insumos.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Codigo` | Sí | Texto | Código único del insumo |
| `Nombre` | Sí | Texto | Nombre del insumo |
| `Tipo` | Sí | Texto | `ALIMENTO`, `SUPLEMENTO`, `HERBICIDA`, `FERTILIZANTE`, `INSUMO_FINCA`, `OTRO` |
| `UnidadMedida` | Sí | Texto | `kg`, `litro`, `rollo`, `unidad`, etc. |
| `PrecioUnitario` | No | Decimal | Precio por unidad en COP |
| `StockMinimo` | No | Decimal | Stock mínimo para alerta |
| `StockInicial` | No | Decimal | Stock disponible al momento de la carga |

---

### 3.24 `maquinaria`

> **Depende de:** `centros-costo`

**Endpoint:** `POST /api/importacion/maquinaria`  
**Archivo de muestra:** `samples/csv/maquinaria.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `CodigoCentroCosto` | Sí | Texto | Código del centro de costo |
| `Codigo` | Sí | Texto | Código único del equipo |
| `Nombre` | Sí | Texto | Nombre del equipo |
| `Marca` | No | Texto | Marca |
| `Modelo` | No | Texto | Modelo |
| `Anio` | No | Entero | Año de fabricación |
| `NumeroSerie` | No | Texto | Número de serie |
| `FechaCompra` | No | Fecha | Fecha de adquisición |
| `ValorCompra` | No | Decimal | Valor de compra en COP |

---

### 3.25 `trabajadores`

Empleados y jornaleros. No tiene dependencias.

**Endpoint:** `POST /api/importacion/trabajadores`  
**Archivo de muestra:** `samples/csv/trabajadores.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `Cedula` | Sí | Texto | Número de cédula |
| `Nombres` | Sí | Texto | Nombres |
| `Apellidos` | Sí | Texto | Apellidos |
| `FechaIngreso` | Sí | Fecha | Fecha de vinculación |
| `Cargo` | No | Texto | Cargo o rol en la finca |
| `SalarioBase` | No | Decimal | Salario mensual en COP |
| `TipoContrato` | No | Texto | `INDEFINIDO`, `FIJO`, `JORNAL`, `PRESTACION` |

---

### 3.26 `pagos-jornal`

> **Depende de:** `trabajadores`, `centros-costo`

**Endpoint:** `POST /api/importacion/pagos-jornal`  
**Archivo de muestra:** `samples/csv/pagos-jornal.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `CedulaTrabajador` | Sí | Texto | Cédula del trabajador |
| `Fecha` | Sí | Fecha | Fecha del jornal |
| `ValorJornal` | Sí | Decimal | Valor pagado en COP |
| `HorasTrabajadas` | No | Decimal | Horas laboradas |
| `Concepto` | No | Texto | Descripción de la labor |
| `CodigoCentroCosto` | No | Texto | Centro de costo que absorbe el gasto |

---

### 3.27 `registros-ica`

> **Depende de:** `animales`

**Endpoint:** `POST /api/importacion/registros-ica`  
**Archivo de muestra:** `samples/csv/registros-ica.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `CodigoAnimal` | Sí | Texto | Código del animal |
| `TipoDocumento` | Sí | Texto | `GuiaMovilizacion`, `CertificadoSanidad`, `DIJ`, `Otro` |
| `NumeroDocumento` | Sí | Texto | Número del documento |
| `FechaExpedicion` | Sí | Fecha | Fecha de expedición |
| `FechaVencimiento` | No | Fecha | Fecha de vencimiento |
| `EntidadEmisora` | No | Texto | Ente que emitió el documento |
| `Observaciones` | No | Texto | Notas adicionales |

---

### 3.28 `captura-carbono`

> **Depende de:** `fincas`

**Endpoint:** `POST /api/importacion/captura-carbono`  
**Archivo de muestra:** `samples/csv/captura-carbono.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `NombreFinca` | Sí | Texto | Nombre exacto de la finca |
| `Anio` | Sí | Entero | Año del registro |
| `Mes` | Sí | Entero | Mes (1–12) |
| `EmisionesGanadoTCO2` | No | Decimal | Emisiones ganaderas en tCO₂eq |
| `CapturaForestal` | No | Decimal | Captura forestal en tCO₂eq |
| `Certificacion` | No | Booleano | `S` si tiene certificado vigente |
| `Observaciones` | No | Texto | Notas del período |

---

### 3.29 `consumo-agua`

> **Depende de:** `fincas`

**Endpoint:** `POST /api/importacion/consumo-agua`  
**Archivo de muestra:** `samples/csv/consumo-agua.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `NombreFinca` | Sí | Texto | Nombre exacto de la finca |
| `Fecha` | Sí | Fecha | Fecha de la medición |
| `VolumenM3` | Sí | Decimal | Volumen consumido en m³ |
| `FuenteAgua` | No | Texto | `ACUEDUCTO`, `POZO`, `RIO`, `LLUVIA` |
| `NumAnimales` | No | Entero | Número de animales en el período |
| `Observaciones` | No | Texto | Notas |

---

### 3.30 `planes-alimentacion`

Plan de suplementación o alimentación por finca y período.

> **Depende de:** `fincas`

**Endpoint:** `POST /api/importacion/planes-alimentacion`  
**Archivo de muestra:** `samples/csv/planes-alimentacion.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `NombreFinca` | Sí | Texto | Nombre exacto de la finca |
| `NombrePlan` | Sí | Texto | Nombre descriptivo del plan |
| `FechaInicio` | Sí | Fecha | Inicio de vigencia |
| `FechaFin` | No | Fecha | Fin de vigencia (vacío = plan abierto) |
| `Observaciones` | No | Texto | Notas del plan |

> **Importante:** Cargue este archivo **antes** que `detalles-alimentacion`. Los planes se crean con IDs numéricos secuenciales que luego se referencian en los detalles.

---

### 3.31 `detalles-alimentacion`

Alimentos e insumos que componen cada plan de alimentación.

> **Depende de:** `planes-alimentacion` (por `IdPlan` numérico), `insumos` (opcional)

**Endpoint:** `POST /api/importacion/detalles-alimentacion`  
**Archivo de muestra:** `samples/csv/detalles-alimentacion.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `IdPlan` | Sí | Entero | ID numérico del plan ya creado |
| `Alimento` | Sí | Texto | Nombre del alimento o suplemento |
| `CantidadDiaria` | Sí | Decimal | Cantidad diaria por animal |
| `UnidadMedida` | Sí | Texto | `kg`, `litro`, `g`, etc. |
| `IdInsumo` | No | Entero | ID del insumo del almacén (si existe) |
| `Observaciones` | No | Texto | Notas del componente |

> **Cómo obtener el `IdPlan`:** Consulte la tabla `PlanesAlimentacion` en la base de datos o en el módulo de Alimentación de la UI después de importar los planes.

---

### 3.32 `planes-pastoreo`

Rotación de animales por potrero en un período determinado.

> **Depende de:** `fincas`, `potreros`

**Endpoint:** `POST /api/importacion/planes-pastoreo`  
**Archivo de muestra:** `samples/csv/planes-pastoreo.csv`

| Columna | Req. | Tipo | Descripción |
|---|---|---|---|
| `NombreFinca` | Sí | Texto | Nombre exacto de la finca |
| `CodigoPotrero` | Sí | Texto | Código exacto del potrero (ej. `POT-01`) |
| `FechaInicio` | Sí | Fecha | Inicio del período de pastoreo |
| `FechaFin` | No | Fecha | Fin del período (vacío = en curso) |
| `NumAnimales` | No | Entero | Número de animales en el potrero |
| `CapacidadCarga` | No | Decimal | Carga animal (UA/ha) |
| `Observaciones` | No | Texto | Notas del período |

> **Nota:** El sistema resuelve el potrero buscando la combinación exacta (`NombreFinca`, `CodigoPotrero`). Si el potrero pertenece a otra finca, la fila falla aunque el código exista.

---

## 4. Manejo de errores

### Estructura del reporte de errores

Cada fila con error devuelve:

```json
{ "fila": 4, "campo": "CodigoAnimal", "mensaje": "Animal 'BOV-999' no encontrado" }
```

- **`fila`:** Número de fila del archivo (incluyendo encabezado como fila 1; el error en fila `4` = 3ª fila de datos).
- **`campo`:** Columna que causó el error (puede ser `null` si el error es general de la fila).
- **`mensaje`:** Descripción legible del problema.

### Errores más comunes

| Error | Causa | Solución |
|---|---|---|
| `'XYZ' no encontrado` | FK no existe en la BD | Verificar que el maestro referenciado fue cargado primero |
| `Formato de fecha inválido` | Fecha no es `YYYY-MM-DD` | Revisar separadores (usar `-` no `/`) |
| `Columna requerida vacía` | Campo obligatorio en blanco | Completar el valor o revisar que no haya columnas desplazadas |
| `No se puede convertir a número` | Valor decimal con coma | Usar punto `.` como separador decimal |
| `Columnas insuficientes` | Fila con menos `\|` que el encabezado | Verificar filas incompletas; los opcionales vacíos deben mantener el `\|` |

### Estrategia ante fallos parciales

Las filas fallidas **no revierten** las exitosas. Se recomienda:

1. Importar el archivo completo.
2. Exportar el reporte de errores (copiar la tabla de la UI).
3. Corregir solo las filas fallidas en un archivo separado.
4. Reimportar únicamente el archivo corregido.

---

## 5. Referencia rápida — archivos y endpoints

| Módulo | Archivo CSV | Endpoint |
|---|---|---|
| Razas | `razas.csv` | `POST /api/importacion/razas` |
| Fincas | `fincas.csv` | `POST /api/importacion/fincas` |
| Potreros | `potreros.csv` | `POST /api/importacion/potreros` |
| Animales | `animales.csv` | `POST /api/importacion/animales` |
| Pesajes | `pesajes.csv` | `POST /api/importacion/pesajes` |
| Medicamentos | `medicamentos.csv` | `POST /api/importacion/medicamentos` |
| Vacunaciones | `vacunaciones.csv` | `POST /api/importacion/vacunaciones` |
| Desparasitaciones | `desparasitaciones.csv` | `POST /api/importacion/desparasitaciones` |
| Controles Preventivos | `aplicar-controles.csv` | `POST /api/importacion/aplicar-controles` |
| Complementos | `complementos.csv` | `POST /api/importacion/complementos` |
| Serv. Reproductivos | `servicios-reproductivos.csv` | `POST /api/importacion/servicios-reproductivos` |
| Gestaciones | `gestaciones.csv` | `POST /api/importacion/gestaciones` |
| Semen | `semen.csv` | `POST /api/importacion/semen` |
| Lactancias | `lactancias.csv` | `POST /api/importacion/lactancias` |
| Control de Leche | `control-leche.csv` | `POST /api/importacion/control-leche` |
| Producción Leche | `produccion-leche.csv` | `POST /api/importacion/produccion-leche` |
| Calidad de Leche | `calidad-leche.csv` | `POST /api/importacion/calidad-leche` |
| Ventas de Leche | `ventas-leche.csv` | `POST /api/importacion/ventas-leche` |
| Clientes | `clientes.csv` | `POST /api/importacion/clientes` |
| Centros de Costo | `centros-costo.csv` | `POST /api/importacion/centros-costo` |
| Gastos | `gastos.csv` | `POST /api/importacion/gastos` |
| Ingresos | `ingresos.csv` | `POST /api/importacion/ingresos` |
| Insumos | `insumos.csv` | `POST /api/importacion/insumos` |
| Maquinaria | `maquinaria.csv` | `POST /api/importacion/maquinaria` |
| Trabajadores | `trabajadores.csv` | `POST /api/importacion/trabajadores` |
| Pagos de Jornal | `pagos-jornal.csv` | `POST /api/importacion/pagos-jornal` |
| Registros ICA | `registros-ica.csv` | `POST /api/importacion/registros-ica` |
| Captura Carbono | `captura-carbono.csv` | `POST /api/importacion/captura-carbono` |
| Consumo Agua | `consumo-agua.csv` | `POST /api/importacion/consumo-agua` |
| Planes Alimentación | `planes-alimentacion.csv` | `POST /api/importacion/planes-alimentacion` |
| Detalles Alimentación | `detalles-alimentacion.csv` | `POST /api/importacion/detalles-alimentacion` |
| Planes Pastoreo | `planes-pastoreo.csv` | `POST /api/importacion/planes-pastoreo` |
