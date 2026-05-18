import { useEffect, useState, type FormEvent } from 'react';
import { api } from '../api/client';
import Modal from '../components/Modal';

// ── Tipos ──────────────────────────────────────────────────────
type Insumo = { id: number; codigo: string; nombre: string; tipo?: string; unidadMedida?: string; precioUnitario?: number; stockMinimo: number; stockActual: number; activo: boolean };
type Kardex = { id: number; idInsumo: number; fecha: string; tipoMovimiento: string; cantidad: number; costoUnitario?: number; concepto?: string; referencia?: string; saldoAnterior: number; saldoNuevo: number };

type Vista = 'lista' | 'bajoMinimo' | 'kardex';

export default function AlmacenPage() {
  const [vista, setVista]         = useState<Vista>('lista');
  const [insumos, setInsumos]     = useState<Insumo[]>([]);
  const [bajoMin, setBajoMin]     = useState<Insumo[]>([]);
  const [kardex, setKardex]       = useState<Kardex[]>([]);
  const [insumoSel, setInsumoSel] = useState<Insumo | null>(null);
  const [cargando, setCargando]   = useState(false);
  const [error, setError]         = useState('');
  const [modalInsumo, setModalInsumo]   = useState(false);
  const [modalMov, setModalMov]         = useState(false);
  const [guardando, setGuardando]       = useState(false);
  const [errForm, setErrForm]           = useState('');

  const hoy = new Date().toISOString().slice(0, 10);

  const [fInsumo, setFInsumo] = useState({ codigo: '', nombre: '', tipo: '', unidadMedida: '', precioUnitario: '', stockMinimo: '0', stockInicial: '0' });
  const [fMov, setFMov] = useState({ idInsumo: '', fecha: hoy, tipoMovimiento: 'ENTRADA', cantidad: '', concepto: '' });

  useEffect(() => {
    setCargando(true); setError('');
    Promise.all([
      api.get<Insumo[]>('/insumos').then(setInsumos),
      api.get<Insumo[]>('/insumos/bajo-minimo').then(setBajoMin),
    ]).catch(e => setError(e.message)).finally(() => setCargando(false));
  }, []);

  async function verKardex(ins: Insumo) {
    setInsumoSel(ins); setVista('kardex');
    const k = await api.get<Kardex[]>(`/insumos/${ins.id}/kardex`);
    setKardex(k);
  }

  async function guardarInsumo(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<Insumo>('/insumos', {
        codigo: fInsumo.codigo, nombre: fInsumo.nombre,
        tipo: fInsumo.tipo || null, unidadMedida: fInsumo.unidadMedida || null,
        precioUnitario: fInsumo.precioUnitario ? parseFloat(fInsumo.precioUnitario) : null,
        stockMinimo: parseFloat(fInsumo.stockMinimo), stockInicial: parseFloat(fInsumo.stockInicial),
      });
      setInsumos(prev => [nuevo, ...prev]);
      setModalInsumo(false); setFInsumo({ codigo: '', nombre: '', tipo: '', unidadMedida: '', precioUnitario: '', stockMinimo: '0', stockInicial: '0' });
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  async function guardarMovimiento(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      await api.post<Kardex>('/insumos/movimiento', {
        idInsumo: parseInt(fMov.idInsumo), fecha: fMov.fecha,
        tipoMovimiento: fMov.tipoMovimiento, cantidad: parseFloat(fMov.cantidad),
        concepto: fMov.concepto || null,
      });
      // Reload insumos to get updated stock
      const ins = await api.get<Insumo[]>('/insumos');
      setInsumos(ins);
      setModalMov(false); setFMov({ idInsumo: '', fecha: hoy, tipoMovimiento: 'ENTRADA', cantidad: '', concepto: '' });
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  const stockColor = (ins: Insumo) => ins.stockActual < ins.stockMinimo
    ? 'text-red-600 font-semibold'
    : ins.stockActual < ins.stockMinimo * 1.2
      ? 'text-yellow-600'
      : 'text-green-700';

  const movColor = (tipo: string) => tipo === 'ENTRADA' ? 'bg-green-100 text-green-700' : tipo === 'SALIDA' ? 'bg-red-100 text-red-700' : 'bg-yellow-100 text-yellow-700';

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Almacén</h1>
          <p className="text-slate-500 text-sm mt-1">Insumos agropecuarios e inventario Kardex</p>
        </div>
        <div className="flex gap-2">
          <button onClick={() => setModalMov(true)} className="btn-secondary">📋 Movimiento</button>
          <button onClick={() => setModalInsumo(true)} className="btn-primary">+ Insumo</button>
        </div>
      </div>

      {/* KPI bajo mínimo */}
      {bajoMin.length > 0 && (
        <div className="bg-amber-50 border border-amber-200 rounded-xl p-4 flex items-center gap-3">
          <span className="text-2xl">⚠️</span>
          <div>
            <p className="font-semibold text-amber-800">{bajoMin.length} insumo{bajoMin.length > 1 ? 's' : ''} bajo stock mínimo</p>
            <p className="text-amber-700 text-sm">{bajoMin.map(i => i.nombre).join(', ')}</p>
          </div>
          <button onClick={() => setVista('bajoMinimo')} className="ml-auto text-sm text-amber-700 underline">Ver todos</button>
        </div>
      )}

      {/* Vista tabs */}
      {vista === 'kardex' && insumoSel ? (
        <div className="space-y-4">
          <div className="flex items-center gap-3">
            <button onClick={() => setVista('lista')} className="text-brand-600 hover:text-brand-700 text-sm flex items-center gap-1">← Volver</button>
            <h2 className="font-semibold text-slate-800">Kardex: {insumoSel.nombre}</h2>
            <span className="ml-auto text-sm text-slate-500">Stock actual: <span className={stockColor(insumoSel)}>{insumoSel.stockActual} {insumoSel.unidadMedida ?? ''}</span></span>
          </div>
          <div className="card overflow-x-auto p-0">
            <table className="w-full text-sm">
              <thead className="bg-slate-50 border-b border-slate-200">
                <tr>{['Fecha','Tipo','Cantidad','Saldo ant.','Saldo nuevo','Concepto'].map(h => (
                  <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
                ))}</tr>
              </thead>
              <tbody className="divide-y divide-slate-100">
                {kardex.map(k => (
                  <tr key={k.id} className="hover:bg-slate-50">
                    <td className="px-4 py-3 text-slate-500">{k.fecha}</td>
                    <td className="px-4 py-3"><span className={`text-xs px-2 py-0.5 rounded-full ${movColor(k.tipoMovimiento)}`}>{k.tipoMovimiento}</span></td>
                    <td className="px-4 py-3 font-mono">{k.cantidad}</td>
                    <td className="px-4 py-3 font-mono text-slate-400">{k.saldoAnterior}</td>
                    <td className="px-4 py-3 font-mono font-semibold">{k.saldoNuevo}</td>
                    <td className="px-4 py-3 text-slate-500">{k.concepto ?? '—'}</td>
                  </tr>
                ))}
                {kardex.length === 0 && <tr><td colSpan={6} className="px-4 py-8 text-slate-400 text-center">Sin movimientos</td></tr>}
              </tbody>
            </table>
          </div>
        </div>
      ) : (
        <>
          {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{error}</div>}
          {cargando ? <div className="text-slate-400 text-center py-12">Cargando…</div> : (
            <div className="card overflow-x-auto p-0">
              <table className="w-full text-sm">
                <thead className="bg-slate-50 border-b border-slate-200">
                  <tr>{['Código','Nombre','Tipo','Unidad','Precio','Stock mín.','Stock actual',''].map(h => (
                    <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
                  ))}</tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {(vista === 'bajoMinimo' ? bajoMin : insumos).map(ins => (
                    <tr key={ins.id} className="hover:bg-slate-50">
                      <td className="px-4 py-3 font-mono text-slate-500">{ins.codigo}</td>
                      <td className="px-4 py-3 font-medium text-slate-800">{ins.nombre}</td>
                      <td className="px-4 py-3 text-slate-500">{ins.tipo ?? '—'}</td>
                      <td className="px-4 py-3 text-slate-400">{ins.unidadMedida ?? '—'}</td>
                      <td className="px-4 py-3 font-mono">{ins.precioUnitario?.toLocaleString('es-CO') ?? '—'}</td>
                      <td className="px-4 py-3 font-mono">{ins.stockMinimo}</td>
                      <td className="px-4 py-3"><span className={stockColor(ins)}>{ins.stockActual}</span></td>
                      <td className="px-4 py-3">
                        <button onClick={() => verKardex(ins)} className="text-brand-600 hover:text-brand-700 text-xs underline">Kardex</button>
                      </td>
                    </tr>
                  ))}
                  {(vista === 'bajoMinimo' ? bajoMin : insumos).length === 0 && (
                    <tr><td colSpan={8} className="px-4 py-8 text-slate-400 text-center">
                      {vista === 'bajoMinimo' ? 'Todos los insumos sobre el mínimo ✓' : 'Sin insumos registrados'}
                    </td></tr>
                  )}
                </tbody>
              </table>
            </div>
          )}
          {vista === 'bajoMinimo' && <button onClick={() => setVista('lista')} className="text-sm text-brand-600 underline">← Ver todos los insumos</button>}
        </>
      )}

      {/* Modal nuevo insumo */}
      {modalInsumo && (
        <Modal titulo="Nuevo Insumo" onClose={() => setModalInsumo(false)}>
          <form onSubmit={guardarInsumo} className="space-y-4">
            {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
            <div className="grid grid-cols-2 gap-4">
              <div><label className="label">Código *</label>
                <input className="input-field" value={fInsumo.codigo} onChange={e => setFInsumo(p => ({...p, codigo: e.target.value}))} required /></div>
              <div><label className="label">Nombre *</label>
                <input className="input-field" value={fInsumo.nombre} onChange={e => setFInsumo(p => ({...p, nombre: e.target.value}))} required /></div>
              <div><label className="label">Tipo</label>
                <input className="input-field" placeholder="Concentrado, Sal, …" value={fInsumo.tipo} onChange={e => setFInsumo(p => ({...p, tipo: e.target.value}))} /></div>
              <div><label className="label">Unidad de medida</label>
                <input className="input-field" placeholder="kg, L, saco…" value={fInsumo.unidadMedida} onChange={e => setFInsumo(p => ({...p, unidadMedida: e.target.value}))} /></div>
              <div><label className="label">Precio unitario</label>
                <input type="number" min="0" step="0.01" className="input-field" value={fInsumo.precioUnitario} onChange={e => setFInsumo(p => ({...p, precioUnitario: e.target.value}))} /></div>
              <div><label className="label">Stock mínimo</label>
                <input type="number" min="0" step="0.01" className="input-field" value={fInsumo.stockMinimo} onChange={e => setFInsumo(p => ({...p, stockMinimo: e.target.value}))} /></div>
              <div className="col-span-2"><label className="label">Stock inicial</label>
                <input type="number" min="0" step="0.01" className="input-field" value={fInsumo.stockInicial} onChange={e => setFInsumo(p => ({...p, stockInicial: e.target.value}))} /></div>
            </div>
            <div className="flex gap-3 justify-end pt-2">
              <button type="button" onClick={() => setModalInsumo(false)} className="btn-secondary">Cancelar</button>
              <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Guardar'}</button>
            </div>
          </form>
        </Modal>
      )}

      {/* Modal movimiento */}
      {modalMov && (
        <Modal titulo="Registrar Movimiento Kardex" onClose={() => setModalMov(false)}>
          <form onSubmit={guardarMovimiento} className="space-y-4">
            {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
            <div><label className="label">Insumo *</label>
              <select className="input-field" value={fMov.idInsumo} onChange={e => setFMov(p => ({...p, idInsumo: e.target.value}))} required>
                <option value="">Seleccionar…</option>
                {insumos.map(i => <option key={i.id} value={i.id}>{i.nombre} (stock: {i.stockActual})</option>)}
              </select></div>
            <div className="grid grid-cols-2 gap-4">
              <div><label className="label">Fecha *</label>
                <input type="date" className="input-field" value={fMov.fecha} onChange={e => setFMov(p => ({...p, fecha: e.target.value}))} required /></div>
              <div><label className="label">Tipo *</label>
                <select className="input-field" value={fMov.tipoMovimiento} onChange={e => setFMov(p => ({...p, tipoMovimiento: e.target.value}))}>
                  <option value="ENTRADA">ENTRADA</option>
                  <option value="SALIDA">SALIDA</option>
                  <option value="AJUSTE">AJUSTE</option>
                </select></div>
              <div className="col-span-2"><label className="label">Cantidad *</label>
                <input type="number" min="0.01" step="0.01" className="input-field" value={fMov.cantidad} onChange={e => setFMov(p => ({...p, cantidad: e.target.value}))} required /></div>
            </div>
            <div><label className="label">Concepto</label>
              <input className="input-field" value={fMov.concepto} onChange={e => setFMov(p => ({...p, concepto: e.target.value}))} /></div>
            <div className="flex gap-3 justify-end pt-2">
              <button type="button" onClick={() => setModalMov(false)} className="btn-secondary">Cancelar</button>
              <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Registrar'}</button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
