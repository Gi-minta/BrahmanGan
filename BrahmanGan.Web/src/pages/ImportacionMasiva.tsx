import { useRef, useState, type DragEvent } from 'react';
import { api, type ImportResult } from '../api/client';

// ── Definición de módulos ─────────────────────────────────────────
interface Modulo {
  id: string;
  label: string;
  icon: string;
  grupo: string;
  descripcion: string;
}

const MODULOS: Modulo[] = [
  // Inventario
  { id: 'razas',                   label: 'Razas',                   icon: '🐂', grupo: 'Inventario',     descripcion: 'Catálogo de razas bovinas' },
  { id: 'animales',                label: 'Animales',                icon: '🐄', grupo: 'Inventario',     descripcion: 'Registro del hato ganadero' },
  { id: 'pesajes',                 label: 'Pesajes',                 icon: '⚖️', grupo: 'Inventario',     descripcion: 'Historial de pesajes por animal' },
  // Finca
  { id: 'fincas',                  label: 'Fincas',                  icon: '🌿', grupo: 'Finca',          descripcion: 'Fincas y propiedades' },
  { id: 'potreros',                label: 'Potreros',                icon: '🌾', grupo: 'Finca',          descripcion: 'Potreros por finca' },
  // Reproducción
  { id: 'servicios-reproductivos', label: 'Servicios Reproductivos', icon: '🔬', grupo: 'Reproducción',   descripcion: 'Montas registradas' },
  { id: 'gestaciones',             label: 'Gestaciones',             icon: '🤰', grupo: 'Reproducción',   descripcion: 'Gestaciones iniciadas' },
  // Sanidad
  { id: 'medicamentos',            label: 'Medicamentos',            icon: '💊', grupo: 'Sanidad',        descripcion: 'Catálogo de medicamentos' },
  { id: 'vacunaciones',            label: 'Vacunaciones',            icon: '💉', grupo: 'Sanidad',        descripcion: 'Historial de vacunaciones' },
  // Leche
  { id: 'control-leche',           label: 'Control de Leche',        icon: '🥛', grupo: 'Leche',          descripcion: 'Control diario por animal' },
  { id: 'produccion-leche',        label: 'Producción Leche',        icon: '🏭', grupo: 'Leche',          descripcion: 'Producción total por finca' },
  { id: 'ventas-leche',            label: 'Ventas de Leche',         icon: '💰', grupo: 'Leche',          descripcion: 'Ventas a clientes' },
  // Comercial
  { id: 'clientes',                label: 'Clientes',                icon: '🤝', grupo: 'Comercial',      descripcion: 'Clientes y compradores' },
  // Costos
  { id: 'centros-costo',           label: 'Centros de Costo',        icon: '🗂️', grupo: 'Costos',         descripcion: 'Centros de costo por finca' },
  { id: 'gastos',                  label: 'Gastos',                  icon: '📉', grupo: 'Costos',         descripcion: 'Gastos generales' },
  { id: 'ingresos',                label: 'Ingresos',                icon: '📈', grupo: 'Costos',         descripcion: 'Ingresos por centro' },
  // Almacén
  { id: 'insumos',                 label: 'Insumos',                 icon: '📦', grupo: 'Almacén',        descripcion: 'Catálogo de insumos' },
  // Equipos
  { id: 'maquinaria',              label: 'Maquinaria',              icon: '🚜', grupo: 'Equipos',        descripcion: 'Equipos y maquinaria' },
  // Nómina
  { id: 'trabajadores',            label: 'Trabajadores',            icon: '👷', grupo: 'Nómina',         descripcion: 'Empleados y jornaleros' },
  { id: 'pagos-jornal',            label: 'Pagos de Jornal',         icon: '💵', grupo: 'Nómina',         descripcion: 'Pagos a trabajadores' },
  // Trazabilidad
  { id: 'registros-ica',           label: 'Registros ICA',           icon: '📋', grupo: 'Trazabilidad',   descripcion: 'Documentos ICA por animal' },
  // Sostenibilidad
  { id: 'captura-carbono',         label: 'Captura Carbono',         icon: '🌱', grupo: 'Sostenibilidad', descripcion: 'Capturas de CO₂ por finca' },
  { id: 'consumo-agua',            label: 'Consumo Agua',            icon: '💧', grupo: 'Sostenibilidad', descripcion: 'Consumo hídrico' },
  // Alimentación
  { id: 'planes-alimentacion',     label: 'Planes Alimentación',     icon: '🌾', grupo: 'Alimentación',   descripcion: 'Planes de alimentación por finca' },
  { id: 'detalles-alimentacion',   label: 'Detalles Alimentación',   icon: '🥣', grupo: 'Alimentación',   descripcion: 'Detalles e insumos por plan' },
  // Pastoreo
  { id: 'planes-pastoreo',         label: 'Planes de Pastoreo',      icon: '🔄', grupo: 'Pastoreo',       descripcion: 'Rotación de potreros' },
  // Sanidad (nuevos)
  { id: 'complementos',            label: 'Complementos',            icon: '🩺', grupo: 'Sanidad',        descripcion: 'Complementos a tratamientos' },
  { id: 'desparasitaciones',       label: 'Desparasitaciones',       icon: '🪱', grupo: 'Sanidad',        descripcion: 'Historial de desparasitación' },
  { id: 'aplicar-controles',       label: 'Controles Preventivos',   icon: '🛡️', grupo: 'Sanidad',        descripcion: 'Aplicación de controles por animal' },
  // Leche (nuevos)
  { id: 'lactancias',              label: 'Lactancias',              icon: '🐄', grupo: 'Leche',          descripcion: 'Períodos de lactancia por animal' },
  { id: 'calidad-leche',           label: 'Calidad de Leche',        icon: '🧪', grupo: 'Leche',          descripcion: 'Análisis de calidad (laboratorio)' },
  // Reproducción (nuevos)
  { id: 'semen',                   label: 'Inventario Semen',        icon: '🧬', grupo: 'Reproducción',   descripcion: 'Stock de pajillas por toro' },
];

const GRUPOS = [...new Set(MODULOS.map(m => m.grupo))];

const GRUPO_ICON: Record<string, string> = {
  Inventario: '🐄', Finca: '🌿', Reproducción: '🔬', Sanidad: '💊',
  Leche: '🥛', Comercial: '🤝', Costos: '💰', Almacén: '📦',
  Equipos: '🚜', Nómina: '👷', Trazabilidad: '📋', Sostenibilidad: '🌱',
  Alimentación: '🌾', Pastoreo: '🔄',
};

function fmtBytes(bytes: number) {
  if (bytes < 1024) return `${bytes} B`;
  if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`;
  return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
}

// ── Componente principal ──────────────────────────────────────────
export default function ImportacionMasivaPage() {
  const [modulo, setModulo]       = useState<Modulo | null>(null);
  const [archivo, setArchivo]     = useState<File | null>(null);
  const [arrastando, setArrastrando] = useState(false);
  const [importando, setImportando] = useState(false);
  const [descargando, setDescargando] = useState(false);
  const [resultado, setResultado] = useState<ImportResult | null>(null);
  const [error, setError]         = useState('');
  const inputRef = useRef<HTMLInputElement>(null);

  // ── Handlers de archivo ─────────────────────────────────────────
  function onFileSelect(files: FileList | null) {
    if (!files || files.length === 0) return;
    const f = files[0];
    if (!f.name.endsWith('.csv') && f.type !== 'text/csv' && f.type !== 'text/plain') {
      setError('Solo se aceptan archivos .csv');
      return;
    }
    setArchivo(f);
    setResultado(null);
    setError('');
  }

  function onDrop(e: DragEvent<HTMLDivElement>) {
    e.preventDefault();
    setArrastrando(false);
    onFileSelect(e.dataTransfer.files);
  }

  // ── Importar ────────────────────────────────────────────────────
  async function handleImportar() {
    if (!archivo || !modulo) return;
    setImportando(true);
    setError('');
    try {
      const fd = new FormData();
      fd.append('archivo', archivo);
      const res = await api.upload<ImportResult>(`/importacion/${modulo.id}`, fd);
      setResultado(res);
    } catch (e: unknown) {
      setError(e instanceof Error ? e.message : 'Error al importar');
    } finally {
      setImportando(false);
    }
  }

  // ── Descargar plantilla ─────────────────────────────────────────
  async function handleDescargarPlantilla() {
    if (!modulo) return;
    setDescargando(true);
    try {
      const token = localStorage.getItem('bg_access_token');
      const res = await fetch(`/api/importacion/plantilla/${modulo.id}`, {
        headers: token ? { Authorization: `Bearer ${token}` } : {},
      });
      if (!res.ok) throw new Error(`HTTP ${res.status}`);
      const blob = await res.blob();
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `plantilla_${modulo.id}.csv`;
      a.click();
      URL.revokeObjectURL(url);
    } catch (e: unknown) {
      setError(e instanceof Error ? e.message : 'Error al descargar plantilla');
    } finally {
      setDescargando(false);
    }
  }

  function resetArchivo() { setArchivo(null); setResultado(null); setError(''); }
  function resetModulo()  { setModulo(null); resetArchivo(); }

  // ── Render ──────────────────────────────────────────────────────
  return (
    <div className="space-y-6">

      {/* Cabecera */}
      <div>
        <h1 className="text-2xl font-bold text-slate-800">Importación Masiva</h1>
        <p className="text-slate-500 text-sm mt-1">
          Carga de datos históricos mediante archivos CSV con separador pipe <code className="bg-slate-100 px-1 rounded text-xs font-mono">|</code>
        </p>
      </div>

      {/* ── PASO 1: Selección de módulo ────────────────────────── */}
      {!modulo && (
        <div className="space-y-5">
          <p className="text-sm font-semibold text-slate-600 uppercase tracking-wide">
            1 · Selecciona el módulo a importar
          </p>

          {GRUPOS.map(grupo => (
            <div key={grupo}>
              <h3 className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-2 flex items-center gap-1.5">
                <span>{GRUPO_ICON[grupo]}</span> {grupo}
              </h3>
              <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-2">
                {MODULOS.filter(m => m.grupo === grupo).map(m => (
                  <button
                    key={m.id}
                    onClick={() => { setModulo(m); resetArchivo(); }}
                    className="text-left p-3 bg-white border border-slate-200 rounded-xl hover:border-brand-400 hover:bg-brand-50 hover:shadow-sm transition-all group"
                  >
                    <span className="text-2xl block mb-1">{m.icon}</span>
                    <p className="text-sm font-semibold text-slate-700 group-hover:text-brand-700 leading-tight">{m.label}</p>
                    <p className="text-xs text-slate-400 mt-0.5 leading-tight">{m.descripcion}</p>
                  </button>
                ))}
              </div>
            </div>
          ))}
        </div>
      )}

      {/* ── PASO 2 & 3: Panel de importación ────────────────────── */}
      {modulo && (
        <div className="space-y-4">

          {/* Breadcrumb del módulo seleccionado */}
          <div className="flex items-center gap-3">
            <button
              onClick={resetModulo}
              className="text-sm text-slate-500 hover:text-brand-600 transition-colors flex items-center gap-1"
            >
              ← Cambiar módulo
            </button>
            <span className="text-slate-300">|</span>
            <span className="flex items-center gap-1.5 text-sm font-semibold text-slate-700">
              <span>{modulo.icon}</span> {modulo.grupo} · {modulo.label}
            </span>
          </div>

          <div className="grid grid-cols-1 lg:grid-cols-5 gap-4">

            {/* Panel izquierdo — instrucciones y plantilla */}
            <div className="lg:col-span-2 space-y-3">
              <div className="card p-4 space-y-3">
                <h3 className="font-semibold text-slate-700 text-sm">📋 Instrucciones</h3>
                <ul className="text-xs text-slate-500 space-y-1.5">
                  <li className="flex gap-1.5"><span>1.</span><span>Descarga la plantilla CSV con los encabezados del módulo.</span></li>
                  <li className="flex gap-1.5"><span>2.</span><span>Completa los datos usando <strong className="text-slate-600">pipe</strong> <code className="bg-slate-100 px-1 rounded font-mono">|</code> como separador.</span></li>
                  <li className="flex gap-1.5"><span>3.</span><span>Las fechas deben estar en formato <code className="bg-slate-100 px-1 rounded font-mono">YYYY-MM-DD</code>.</span></li>
                  <li className="flex gap-1.5"><span>4.</span><span>Los decimales usan punto <code className="bg-slate-100 px-1 rounded font-mono">.</code> como separador.</span></li>
                  <li className="flex gap-1.5"><span>5.</span><span>Booleanos: <code className="bg-slate-100 px-1 rounded font-mono">S</code> / <code className="bg-slate-100 px-1 rounded font-mono">N</code>.</span></li>
                  <li className="flex gap-1.5"><span>6.</span><span>Las filas con error se reportan sin revertir las exitosas.</span></li>
                </ul>

                <button
                  onClick={handleDescargarPlantilla}
                  disabled={descargando}
                  className="w-full btn-secondary text-sm flex items-center justify-center gap-2"
                >
                  {descargando ? '⏳ Descargando…' : '📥 Descargar plantilla CSV'}
                </button>
              </div>
            </div>

            {/* Panel derecho — zona de carga y resultado */}
            <div className="lg:col-span-3 space-y-4">

              {/* Error general */}
              {error && (
                <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{error}</div>
              )}

              {/* Zona de carga (solo si no hay resultado exitoso total) */}
              {!resultado && (
                <div className="card p-4 space-y-3">
                  <h3 className="font-semibold text-slate-700 text-sm">⬆️ Cargar archivo</h3>

                  {/* Drop zone */}
                  <div
                    onDragOver={e => { e.preventDefault(); setArrastrando(true); }}
                    onDragLeave={() => setArrastrando(false)}
                    onDrop={onDrop}
                    onClick={() => inputRef.current?.click()}
                    className={`border-2 border-dashed rounded-xl p-8 text-center cursor-pointer transition-colors
                      ${arrastando
                        ? 'border-brand-400 bg-brand-50'
                        : archivo
                          ? 'border-green-400 bg-green-50'
                          : 'border-slate-300 hover:border-brand-400 hover:bg-slate-50'
                      }`}
                  >
                    <input
                      ref={inputRef}
                      type="file"
                      accept=".csv,text/csv,text/plain"
                      className="hidden"
                      onChange={e => onFileSelect(e.target.files)}
                    />

                    {archivo ? (
                      <div className="space-y-1">
                        <p className="text-3xl">📄</p>
                        <p className="font-semibold text-green-700 text-sm">{archivo.name}</p>
                        <p className="text-xs text-slate-400">{fmtBytes(archivo.size)}</p>
                        <button
                          type="button"
                          onClick={e => { e.stopPropagation(); resetArchivo(); }}
                          className="text-xs text-red-500 hover:text-red-700 transition-colors mt-1"
                        >
                          ✕ Quitar archivo
                        </button>
                      </div>
                    ) : (
                      <div className="space-y-1 text-slate-400">
                        <p className="text-3xl">📂</p>
                        <p className="text-sm font-medium text-slate-600">
                          Arrastra el archivo CSV aquí
                        </p>
                        <p className="text-xs">o haz clic para seleccionar</p>
                        <p className="text-xs text-slate-300 mt-2">Máximo 10 MB · Formato .csv</p>
                      </div>
                    )}
                  </div>

                  {/* Botón importar */}
                  <button
                    onClick={handleImportar}
                    disabled={!archivo || importando}
                    className="w-full btn-primary text-sm flex items-center justify-center gap-2"
                  >
                    {importando
                      ? <><span className="animate-spin">⏳</span> Importando filas…</>
                      : '🚀 Importar archivo'
                    }
                  </button>
                </div>
              )}

              {/* Resultado */}
              {resultado && <ResultadoImportacion resultado={resultado} onReintentar={resetArchivo} />}
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

// ── Sub-componente: resultado ─────────────────────────────────────
function ResultadoImportacion({
  resultado,
  onReintentar,
}: {
  resultado: ImportResult;
  onReintentar: () => void;
}) {
  const todoOk = resultado.fallidos === 0;

  return (
    <div className="space-y-4">

      {/* Stat cards */}
      <div className="grid grid-cols-3 gap-3">
        <div className="card p-4 text-center">
          <p className="text-2xl font-bold text-slate-700">{resultado.totalFilas}</p>
          <p className="text-xs text-slate-400 mt-0.5">Total filas</p>
        </div>
        <div className="card p-4 text-center border-green-200 bg-green-50">
          <p className="text-2xl font-bold text-green-700">{resultado.exitosos}</p>
          <p className="text-xs text-green-500 mt-0.5">Exitosos</p>
        </div>
        <div className={`card p-4 text-center ${resultado.fallidos > 0 ? 'border-red-200 bg-red-50' : 'border-slate-200'}`}>
          <p className={`text-2xl font-bold ${resultado.fallidos > 0 ? 'text-red-600' : 'text-slate-300'}`}>
            {resultado.fallidos}
          </p>
          <p className={`text-xs mt-0.5 ${resultado.fallidos > 0 ? 'text-red-400' : 'text-slate-300'}`}>Fallidos</p>
        </div>
      </div>

      {/* Mensaje de éxito */}
      {todoOk && (
        <div className="bg-green-50 border border-green-200 text-green-700 rounded-lg p-4 text-sm flex items-center gap-2">
          <span className="text-lg">✅</span>
          <span>Todas las filas fueron importadas exitosamente.</span>
        </div>
      )}

      {/* Tabla de errores */}
      {resultado.errores.length > 0 && (
        <div className="card p-0 overflow-hidden">
          <div className="px-4 py-3 bg-red-50 border-b border-red-100 flex items-center gap-2">
            <span className="text-red-500 text-sm">⚠️</span>
            <p className="text-sm font-semibold text-red-700">
              {resultado.errores.length} fila{resultado.errores.length > 1 ? 's' : ''} con error
            </p>
          </div>
          <div className="overflow-x-auto">
            <table className="w-full text-xs">
              <thead className="bg-slate-50 border-b border-slate-200">
                <tr>
                  <th className="px-4 py-2 text-left text-slate-500 font-semibold w-16">Fila</th>
                  <th className="px-4 py-2 text-left text-slate-500 font-semibold w-28">Campo</th>
                  <th className="px-4 py-2 text-left text-slate-500 font-semibold">Mensaje</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-100">
                {resultado.errores.map((err, i) => (
                  <tr key={i} className="hover:bg-red-50 transition-colors">
                    <td className="px-4 py-2 font-mono text-slate-600">{err.fila}</td>
                    <td className="px-4 py-2 text-slate-500">{err.campo ?? '—'}</td>
                    <td className="px-4 py-2 text-red-700">{err.mensaje}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {/* Acciones post-resultado */}
      <button onClick={onReintentar} className="btn-secondary text-sm w-full">
        ⬆️ Importar otro archivo
      </button>
    </div>
  );
}
