import { useEffect, useState, type FormEvent } from 'react';
import { api } from '../api/client';
import Modal from '../components/Modal';

// ── Tipos ──────────────────────────────────────────────────────
type Trabajador = { id: number; cedula: string; nombres: string; apellidos: string; cargo?: string; fechaIngreso: string; fechaRetiro?: string; salarioBase?: number; tipoContrato?: string; activo: boolean };
type PagoJornal = { id: number; idTrabajador: number; fecha: string; horasTrabajadas?: number; valorJornal: number; concepto?: string; idCentro?: number };

export default function NominaPage() {
  const [trabajadores, setTrabajadores] = useState<Trabajador[]>([]);
  const [pagos, setPagos]               = useState<PagoJornal[]>([]);
  const [trabSel, setTrabSel]           = useState<Trabajador | null>(null);
  const [cargando, setCargando]         = useState(false);
  const [error, setError]               = useState('');
  const [modalTrab, setModalTrab]       = useState(false);
  const [modalPago, setModalPago]       = useState(false);
  const [guardando, setGuardando]       = useState(false);
  const [errForm, setErrForm]           = useState('');
  const [busqueda, setBusqueda]         = useState('');

  const hoy = new Date().toISOString().slice(0, 10);

  const [fTrab, setFTrab] = useState({ cedula: '', nombres: '', apellidos: '', cargo: '', fechaIngreso: hoy, salarioBase: '', tipoContrato: '' });
  const [fPago, setFPago] = useState({ idTrabajador: '', fecha: hoy, valorJornal: '', horasTrabajadas: '', concepto: '' });

  useEffect(() => {
    setCargando(true);
    api.get<Trabajador[]>('/trabajadores').then(setTrabajadores).catch(e => setError(e.message)).finally(() => setCargando(false));
  }, []);

  async function verPagos(t: Trabajador) {
    setTrabSel(t);
    const p = await api.get<PagoJornal[]>(`/trabajadores/${t.id}/pagos`);
    setPagos(p);
  }

  async function guardarTrabajador(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<Trabajador>('/trabajadores', {
        cedula: fTrab.cedula, nombres: fTrab.nombres, apellidos: fTrab.apellidos,
        cargo: fTrab.cargo || null, fechaIngreso: fTrab.fechaIngreso,
        salarioBase: fTrab.salarioBase ? parseFloat(fTrab.salarioBase) : null,
        tipoContrato: fTrab.tipoContrato || null,
      });
      setTrabajadores(prev => [nuevo, ...prev]);
      setModalTrab(false); setFTrab({ cedula: '', nombres: '', apellidos: '', cargo: '', fechaIngreso: hoy, salarioBase: '', tipoContrato: '' });
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  async function guardarPago(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<PagoJornal>('/trabajadores/pagos', {
        idTrabajador: parseInt(fPago.idTrabajador), fecha: fPago.fecha,
        valorJornal: parseFloat(fPago.valorJornal),
        horasTrabajadas: fPago.horasTrabajadas ? parseFloat(fPago.horasTrabajadas) : null,
        concepto: fPago.concepto || null,
      });
      if (trabSel && nuevo.idTrabajador === trabSel.id) setPagos(prev => [nuevo, ...prev]);
      setModalPago(false); setFPago({ idTrabajador: '', fecha: hoy, valorJornal: '', horasTrabajadas: '', concepto: '' });
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  const fmt = (v: number) => v.toLocaleString('es-CO', { style: 'currency', currency: 'COP', maximumFractionDigits: 0 });
  const totalPagos = pagos.reduce((s, p) => s + p.valorJornal, 0);

  const filtrados = trabajadores.filter(t =>
    `${t.nombres} ${t.apellidos} ${t.cedula}`.toLowerCase().includes(busqueda.toLowerCase())
  );

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Nómina</h1>
          <p className="text-slate-500 text-sm mt-1">Trabajadores y pagos de jornal</p>
        </div>
        <div className="flex gap-2">
          <button onClick={() => setModalPago(true)} className="btn-secondary">💵 Pago Jornal</button>
          <button onClick={() => setModalTrab(true)} className="btn-primary">+ Trabajador</button>
        </div>
      </div>

      {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{error}</div>}

      {trabSel ? (
        <div className="space-y-4">
          <div className="flex items-center gap-3">
            <button onClick={() => setTrabSel(null)} className="text-brand-600 hover:text-brand-700 text-sm">← Volver</button>
            <h2 className="font-semibold text-slate-800">Pagos: {trabSel.nombres} {trabSel.apellidos}</h2>
            <span className="ml-auto font-semibold text-green-700">{fmt(totalPagos)} total</span>
          </div>
          <div className="card overflow-x-auto p-0">
            <table className="w-full text-sm">
              <thead className="bg-slate-50 border-b border-slate-200">
                <tr>{['Fecha','Horas','Valor Jornal','Concepto'].map(h => (
                  <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
                ))}</tr>
              </thead>
              <tbody className="divide-y divide-slate-100">
                {pagos.map(p => (
                  <tr key={p.id} className="hover:bg-slate-50">
                    <td className="px-4 py-3 text-slate-500">{p.fecha}</td>
                    <td className="px-4 py-3 font-mono">{p.horasTrabajadas ?? '—'}</td>
                    <td className="px-4 py-3 font-mono font-semibold text-green-700">{fmt(p.valorJornal)}</td>
                    <td className="px-4 py-3 text-slate-500">{p.concepto ?? '—'}</td>
                  </tr>
                ))}
                {pagos.length === 0 && <tr><td colSpan={4} className="px-4 py-8 text-slate-400 text-center">Sin pagos registrados</td></tr>}
              </tbody>
            </table>
          </div>
        </div>
      ) : (
        <>
          <input className="input-field max-w-sm" placeholder="Buscar trabajador…" value={busqueda} onChange={e => setBusqueda(e.target.value)} />
          {cargando ? <div className="text-slate-400 text-center py-12">Cargando…</div> : (
            <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
              {filtrados.map(t => (
                <div key={t.id} className="card hover:shadow-md transition-shadow cursor-pointer" onClick={() => verPagos(t)}>
                  <div className="flex items-start justify-between mb-2">
                    <div>
                      <p className="font-semibold text-slate-800">{t.nombres} {t.apellidos}</p>
                      <p className="text-xs text-slate-400">CC {t.cedula}</p>
                    </div>
                    <span className={`text-xs px-2 py-0.5 rounded-full ${t.activo ? 'bg-green-100 text-green-700' : 'bg-slate-100 text-slate-500'}`}>
                      {t.activo ? 'Activo' : 'Retirado'}
                    </span>
                  </div>
                  <div className="space-y-1 text-sm text-slate-500">
                    {t.cargo && <p>🔧 {t.cargo}</p>}
                    {t.tipoContrato && <p>📋 {t.tipoContrato}</p>}
                    {t.salarioBase && <p>💰 {fmt(t.salarioBase)}</p>}
                    <p className="text-brand-600 text-xs mt-2">Ver pagos →</p>
                  </div>
                </div>
              ))}
              {filtrados.length === 0 && <p className="text-slate-400 col-span-3 text-center py-8">Sin trabajadores</p>}
            </div>
          )}
        </>
      )}

      {/* Modal Trabajador */}
      {modalTrab && (
        <Modal titulo="Contratar Trabajador" onClose={() => setModalTrab(false)}>
          <form onSubmit={guardarTrabajador} className="space-y-4">
            {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
            <div className="grid grid-cols-2 gap-4">
              <div className="col-span-2"><label className="label">Cédula *</label>
                <input className="input-field" value={fTrab.cedula} onChange={e => setFTrab(p => ({...p, cedula: e.target.value}))} required /></div>
              <div><label className="label">Nombres *</label>
                <input className="input-field" value={fTrab.nombres} onChange={e => setFTrab(p => ({...p, nombres: e.target.value}))} required /></div>
              <div><label className="label">Apellidos *</label>
                <input className="input-field" value={fTrab.apellidos} onChange={e => setFTrab(p => ({...p, apellidos: e.target.value}))} required /></div>
              <div><label className="label">Cargo</label>
                <input className="input-field" placeholder="Ordeñador, Mayordomía…" value={fTrab.cargo} onChange={e => setFTrab(p => ({...p, cargo: e.target.value}))} /></div>
              <div><label className="label">Fecha ingreso *</label>
                <input type="date" className="input-field" value={fTrab.fechaIngreso} onChange={e => setFTrab(p => ({...p, fechaIngreso: e.target.value}))} required /></div>
              <div><label className="label">Salario base (COP)</label>
                <input type="number" min="0" className="input-field" value={fTrab.salarioBase} onChange={e => setFTrab(p => ({...p, salarioBase: e.target.value}))} /></div>
              <div><label className="label">Tipo contrato</label>
                <select className="input-field" value={fTrab.tipoContrato} onChange={e => setFTrab(p => ({...p, tipoContrato: e.target.value}))}>
                  <option value="">Seleccionar…</option>
                  <option value="Término fijo">Término fijo</option>
                  <option value="Término indefinido">Término indefinido</option>
                  <option value="Obra o labor">Obra o labor</option>
                  <option value="Jornal">Jornal</option>
                </select></div>
            </div>
            <div className="flex gap-3 justify-end pt-2">
              <button type="button" onClick={() => setModalTrab(false)} className="btn-secondary">Cancelar</button>
              <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Contratar'}</button>
            </div>
          </form>
        </Modal>
      )}

      {/* Modal Pago */}
      {modalPago && (
        <Modal titulo="Registrar Pago de Jornal" onClose={() => setModalPago(false)}>
          <form onSubmit={guardarPago} className="space-y-4">
            {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
            <div><label className="label">Trabajador *</label>
              <select className="input-field" value={fPago.idTrabajador} onChange={e => setFPago(p => ({...p, idTrabajador: e.target.value}))} required>
                <option value="">Seleccionar…</option>
                {trabajadores.map(t => <option key={t.id} value={t.id}>{t.nombres} {t.apellidos}</option>)}
              </select></div>
            <div className="grid grid-cols-2 gap-4">
              <div><label className="label">Fecha *</label>
                <input type="date" className="input-field" value={fPago.fecha} onChange={e => setFPago(p => ({...p, fecha: e.target.value}))} required /></div>
              <div><label className="label">Valor jornal (COP) *</label>
                <input type="number" min="1" className="input-field" value={fPago.valorJornal} onChange={e => setFPago(p => ({...p, valorJornal: e.target.value}))} required /></div>
              <div><label className="label">Horas trabajadas</label>
                <input type="number" min="0" step="0.5" className="input-field" value={fPago.horasTrabajadas} onChange={e => setFPago(p => ({...p, horasTrabajadas: e.target.value}))} /></div>
              <div><label className="label">Concepto</label>
                <input className="input-field" value={fPago.concepto} onChange={e => setFPago(p => ({...p, concepto: e.target.value}))} /></div>
            </div>
            <div className="flex gap-3 justify-end pt-2">
              <button type="button" onClick={() => setModalPago(false)} className="btn-secondary">Cancelar</button>
              <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Registrar'}</button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
