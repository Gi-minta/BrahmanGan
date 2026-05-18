import { useEffect, useState, type FormEvent } from 'react';
import { api } from '../api/client';
import Modal from '../components/Modal';

// ── Tipos ──────────────────────────────────────────────────────
type CentroCosto = { id: number; codigo: string; nombre: string; idFinca?: number; activo: boolean };
type GastoGeneral = { id: number; fecha: string; idCentro?: number; concepto: string; valor: number; proveedor?: string; comprobante?: string };
type Ingreso = { id: number; fecha: string; idCentro: number; tipoIngreso: string; valor: number; concepto?: string; comprobante?: string };

type Tab = 'centros' | 'gastos' | 'ingresos';

export default function CostosPage() {
  const [tab, setTab] = useState<Tab>('centros');
  const [centros, setCentros]   = useState<CentroCosto[]>([]);
  const [gastos, setGastos]     = useState<GastoGeneral[]>([]);
  const [ingresos, setIngresos] = useState<Ingreso[]>([]);
  const [cargando, setCargando] = useState(false);
  const [error, setError]       = useState('');
  const [modal, setModal]       = useState<Tab | null>(null);
  const [guardando, setGuardando] = useState(false);
  const [errForm, setErrForm]   = useState('');

  // Filtro período
  const hoy = new Date().toISOString().slice(0, 10);
  const primerMes = hoy.slice(0, 8) + '01';
  const [desde, setDesde] = useState(primerMes);
  const [hasta, setHasta] = useState(hoy);

  useEffect(() => {
    setCargando(true); setError('');
    const promesas: Promise<unknown>[] = [api.get<CentroCosto[]>('/centros-costo').then(setCentros)];
    if (tab === 'gastos')
      promesas.push(api.get<GastoGeneral[]>(`/gastos-generales?desde=${desde}&hasta=${hasta}`).then(setGastos));
    if (tab === 'ingresos' && centros.length > 0)
      promesas.push(api.get<Ingreso[]>(`/ingresos/centro/${centros[0].id}`).then(setIngresos));
    Promise.all(promesas).catch(e => setError(e.message)).finally(() => setCargando(false));
  }, [tab, desde, hasta]);

  // ── Formularios ────────────────────────────────────────────
  const [fCentro, setFCentro] = useState({ codigo: '', nombre: '' });
  const [fGasto, setFGasto] = useState({ fecha: hoy, concepto: '', valor: '', proveedor: '', idCentro: '' });
  const [fIngreso, setFIngreso] = useState({ fecha: hoy, idCentro: '', tipoIngreso: '', valor: '', concepto: '' });

  async function guardarCentro(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<CentroCosto>('/centros-costo', { codigo: fCentro.codigo, nombre: fCentro.nombre });
      setCentros(prev => [nuevo, ...prev]);
      setModal(null); setFCentro({ codigo: '', nombre: '' });
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  async function guardarGasto(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<GastoGeneral>('/gastos-generales', {
        fecha: fGasto.fecha, concepto: fGasto.concepto,
        valor: parseFloat(fGasto.valor), proveedor: fGasto.proveedor || null,
        idCentro: fGasto.idCentro ? parseInt(fGasto.idCentro) : null,
      });
      setGastos(prev => [nuevo, ...prev]);
      setModal(null); setFGasto({ fecha: hoy, concepto: '', valor: '', proveedor: '', idCentro: '' });
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  async function guardarIngreso(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<Ingreso>('/ingresos', {
        fecha: fIngreso.fecha, idCentro: parseInt(fIngreso.idCentro),
        tipoIngreso: fIngreso.tipoIngreso, valor: parseFloat(fIngreso.valor),
        concepto: fIngreso.concepto || null,
      });
      setIngresos(prev => [nuevo, ...prev]);
      setModal(null); setFIngreso({ fecha: hoy, idCentro: '', tipoIngreso: '', valor: '', concepto: '' });
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  const fmt = (v: number) => v.toLocaleString('es-CO', { style: 'currency', currency: 'COP', maximumFractionDigits: 0 });

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Costos</h1>
          <p className="text-slate-500 text-sm mt-1">Centros de costo, gastos e ingresos</p>
        </div>
        <button onClick={() => setModal(tab)} className="btn-primary">+ Nuevo</button>
      </div>

      {/* Tabs */}
      <div className="flex gap-1 bg-slate-100 p-1 rounded-xl w-fit">
        {(['centros', 'gastos', 'ingresos'] as Tab[]).map(t => (
          <button key={t} onClick={() => setTab(t)}
            className={`px-5 py-2 rounded-lg text-sm font-medium transition-colors capitalize ${tab === t ? 'bg-white text-brand-700 shadow-sm' : 'text-slate-500 hover:text-slate-700'}`}>
            {t === 'centros' ? '🏢 Centros' : t === 'gastos' ? '📉 Gastos' : '📈 Ingresos'}
          </button>
        ))}
      </div>

      {tab !== 'centros' && (
        <div className="flex gap-3 items-center">
          <label className="text-sm text-slate-600">Desde</label>
          <input type="date" value={desde} onChange={e => setDesde(e.target.value)} className="input-field w-40" />
          <label className="text-sm text-slate-600">Hasta</label>
          <input type="date" value={hasta} onChange={e => setHasta(e.target.value)} className="input-field w-40" />
        </div>
      )}

      {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{error}</div>}

      {cargando ? <div className="text-slate-400 text-center py-12">Cargando…</div> : (
        <>
          {tab === 'centros' && (
            <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
              {centros.map(c => (
                <div key={c.id} className="card flex items-start justify-between">
                  <div>
                    <p className="font-semibold text-slate-800">{c.nombre}</p>
                    <p className="text-xs text-slate-400 font-mono mt-0.5">{c.codigo}</p>
                  </div>
                  <span className={`text-xs px-2 py-0.5 rounded-full ${c.activo ? 'bg-green-100 text-green-700' : 'bg-slate-100 text-slate-500'}`}>
                    {c.activo ? 'Activo' : 'Inactivo'}
                  </span>
                </div>
              ))}
              {centros.length === 0 && <p className="text-slate-400 col-span-3 text-center py-8">Sin centros registrados</p>}
            </div>
          )}

          {tab === 'gastos' && (
            <div className="card overflow-x-auto p-0">
              <table className="w-full text-sm">
                <thead className="bg-slate-50 border-b border-slate-200">
                  <tr>{['Fecha','Concepto','Valor','Proveedor','Centro'].map(h => (
                    <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
                  ))}</tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {gastos.map(g => (
                    <tr key={g.id} className="hover:bg-slate-50">
                      <td className="px-4 py-3 text-slate-500">{g.fecha}</td>
                      <td className="px-4 py-3 font-medium text-slate-800">{g.concepto}</td>
                      <td className="px-4 py-3 font-mono text-red-600">{fmt(g.valor)}</td>
                      <td className="px-4 py-3 text-slate-500">{g.proveedor ?? '—'}</td>
                      <td className="px-4 py-3 text-slate-400">{g.idCentro ? centros.find(c => c.id === g.idCentro)?.nombre ?? g.idCentro : '—'}</td>
                    </tr>
                  ))}
                  {gastos.length === 0 && <tr><td colSpan={5} className="px-4 py-8 text-slate-400 text-center">Sin registros en el período</td></tr>}
                </tbody>
              </table>
            </div>
          )}

          {tab === 'ingresos' && (
            <div className="card overflow-x-auto p-0">
              <table className="w-full text-sm">
                <thead className="bg-slate-50 border-b border-slate-200">
                  <tr>{['Fecha','Tipo','Valor','Concepto','Centro'].map(h => (
                    <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
                  ))}</tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {ingresos.map(i => (
                    <tr key={i.id} className="hover:bg-slate-50">
                      <td className="px-4 py-3 text-slate-500">{i.fecha}</td>
                      <td className="px-4 py-3 font-medium text-slate-800">{i.tipoIngreso}</td>
                      <td className="px-4 py-3 font-mono text-green-600">{fmt(i.valor)}</td>
                      <td className="px-4 py-3 text-slate-500">{i.concepto ?? '—'}</td>
                      <td className="px-4 py-3 text-slate-400">{centros.find(c => c.id === i.idCentro)?.nombre ?? i.idCentro}</td>
                    </tr>
                  ))}
                  {ingresos.length === 0 && <tr><td colSpan={5} className="px-4 py-8 text-slate-400 text-center">Sin ingresos registrados</td></tr>}
                </tbody>
              </table>
            </div>
          )}
        </>
      )}

      {/* Modal Centro */}
      {modal === 'centros' && (
        <Modal titulo="Nuevo Centro de Costo" onClose={() => setModal(null)}>
          <form onSubmit={guardarCentro} className="space-y-4">
            {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
            <div><label className="label">Código *</label>
              <input className="input-field" value={fCentro.codigo} onChange={e => setFCentro(p => ({...p, codigo: e.target.value}))} required /></div>
            <div><label className="label">Nombre *</label>
              <input className="input-field" value={fCentro.nombre} onChange={e => setFCentro(p => ({...p, nombre: e.target.value}))} required /></div>
            <div className="flex gap-3 justify-end pt-2">
              <button type="button" onClick={() => setModal(null)} className="btn-secondary">Cancelar</button>
              <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Guardar'}</button>
            </div>
          </form>
        </Modal>
      )}

      {/* Modal Gasto */}
      {modal === 'gastos' && (
        <Modal titulo="Registrar Gasto General" onClose={() => setModal(null)}>
          <form onSubmit={guardarGasto} className="space-y-4">
            {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
            <div><label className="label">Fecha *</label>
              <input type="date" className="input-field" value={fGasto.fecha} onChange={e => setFGasto(p => ({...p, fecha: e.target.value}))} required /></div>
            <div><label className="label">Concepto *</label>
              <input className="input-field" value={fGasto.concepto} onChange={e => setFGasto(p => ({...p, concepto: e.target.value}))} required /></div>
            <div><label className="label">Valor (COP) *</label>
              <input type="number" min="0" step="100" className="input-field" value={fGasto.valor} onChange={e => setFGasto(p => ({...p, valor: e.target.value}))} required /></div>
            <div><label className="label">Proveedor</label>
              <input className="input-field" value={fGasto.proveedor} onChange={e => setFGasto(p => ({...p, proveedor: e.target.value}))} /></div>
            <div><label className="label">Centro de costo</label>
              <select className="input-field" value={fGasto.idCentro} onChange={e => setFGasto(p => ({...p, idCentro: e.target.value}))}>
                <option value="">Sin centro</option>
                {centros.map(c => <option key={c.id} value={c.id}>{c.nombre}</option>)}
              </select></div>
            <div className="flex gap-3 justify-end pt-2">
              <button type="button" onClick={() => setModal(null)} className="btn-secondary">Cancelar</button>
              <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Guardar'}</button>
            </div>
          </form>
        </Modal>
      )}

      {/* Modal Ingreso */}
      {modal === 'ingresos' && (
        <Modal titulo="Registrar Ingreso" onClose={() => setModal(null)}>
          <form onSubmit={guardarIngreso} className="space-y-4">
            {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
            <div><label className="label">Fecha *</label>
              <input type="date" className="input-field" value={fIngreso.fecha} onChange={e => setFIngreso(p => ({...p, fecha: e.target.value}))} required /></div>
            <div><label className="label">Centro de costo *</label>
              <select className="input-field" value={fIngreso.idCentro} onChange={e => setFIngreso(p => ({...p, idCentro: e.target.value}))} required>
                <option value="">Seleccionar…</option>
                {centros.map(c => <option key={c.id} value={c.id}>{c.nombre}</option>)}
              </select></div>
            <div><label className="label">Tipo de ingreso *</label>
              <input className="input-field" placeholder="Venta leche, animales…" value={fIngreso.tipoIngreso} onChange={e => setFIngreso(p => ({...p, tipoIngreso: e.target.value}))} required /></div>
            <div><label className="label">Valor (COP) *</label>
              <input type="number" min="1" step="100" className="input-field" value={fIngreso.valor} onChange={e => setFIngreso(p => ({...p, valor: e.target.value}))} required /></div>
            <div><label className="label">Concepto</label>
              <input className="input-field" value={fIngreso.concepto} onChange={e => setFIngreso(p => ({...p, concepto: e.target.value}))} /></div>
            <div className="flex gap-3 justify-end pt-2">
              <button type="button" onClick={() => setModal(null)} className="btn-secondary">Cancelar</button>
              <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Guardar'}</button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
