import { useEffect, useState, type FormEvent } from 'react';
import { api, type Animal } from '../api/client';
import Modal from '../components/Modal';

// ── Tipos ──────────────────────────────────────────────────────
type RegistroICA = { id: number; idAnimal: number; tipoDocumento: string; numeroDocumento: string; fechaExpedicion: string; fechaVencimiento?: string; entidadEmisora?: string; idMunicipio?: number; estado: string; urlDocumento?: string; observaciones?: string };

const ESTADO_COLOR: Record<string, string> = {
  VIGENTE: 'bg-green-100 text-green-700',
  VENCIDO: 'bg-red-100 text-red-700',
  CANCELADO: 'bg-slate-100 text-slate-500',
};

export default function TrazabilidadPage() {
  const [registros, setRegistros] = useState<RegistroICA[]>([]);
  const [proximos, setProximos]   = useState<RegistroICA[]>([]);
  const [animales, setAnimales]   = useState<Animal[]>([]);
  const [cargando, setCargando]   = useState(false);
  const [error, setError]         = useState('');
  const [modal, setModal]         = useState(false);
  const [guardando, setGuardando] = useState(false);
  const [errForm, setErrForm]     = useState('');
  const [filtroAnimal, setFiltroAnimal] = useState('');
  const [tab, setTab]             = useState<'todos' | 'proximos'>('todos');

  const hoy = new Date().toISOString().slice(0, 10);
  const [fReg, setFReg] = useState({ idAnimal: '', tipoDocumento: '', numeroDocumento: '', fechaExpedicion: hoy, fechaVencimiento: '', entidadEmisora: '', observaciones: '' });

  useEffect(() => {
    setCargando(true);
    Promise.all([
      api.get<RegistroICA[]>('/registros-ica/proximos-vencer?dias=30').then(setProximos),
      api.get<Animal[]>('/animales/activos').then(setAnimales),
    ]).catch(e => setError(e.message)).finally(() => setCargando(false));
  }, []);

  useEffect(() => {
    if (!filtroAnimal) return;
    api.get<RegistroICA[]>(`/registros-ica/animal/${filtroAnimal}`).then(setRegistros).catch(e => setError(e.message));
  }, [filtroAnimal]);

  async function guardarRegistro(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<RegistroICA>('/registros-ica', {
        idAnimal: parseInt(fReg.idAnimal),
        tipoDocumento: fReg.tipoDocumento,
        numeroDocumento: fReg.numeroDocumento,
        fechaExpedicion: fReg.fechaExpedicion,
        fechaVencimiento: fReg.fechaVencimiento || null,
        entidadEmisora: fReg.entidadEmisora || null,
        observaciones: fReg.observaciones || null,
      });
      setRegistros(prev => [nuevo, ...prev]);
      setModal(false); setFReg({ idAnimal: '', tipoDocumento: '', numeroDocumento: '', fechaExpedicion: hoy, fechaVencimiento: '', entidadEmisora: '', observaciones: '' });
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  async function cancelar(id: number) {
    if (!confirm('¿Cancelar este registro ICA?')) return;
    await api.del(`/registros-ica/${id}`);
    setRegistros(prev => prev.map(r => r.id === id ? {...r, estado: 'CANCELADO'} : r));
    setProximos(prev => prev.filter(r => r.id !== id));
  }

  const listaActual = tab === 'proximos' ? proximos : registros;
  const nombreAnimal = (id: number) => animales.find(a => a.id === id)?.nombre ?? animales.find(a => a.id === id)?.codigo ?? String(id);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Trazabilidad ICA</h1>
          <p className="text-slate-500 text-sm mt-1">Registros oficiales de trazabilidad animal</p>
        </div>
        <button onClick={() => setModal(true)} className="btn-primary">+ Registro ICA</button>
      </div>

      {/* Alerta proximos */}
      {proximos.length > 0 && (
        <div className="bg-amber-50 border border-amber-200 rounded-xl p-4 flex items-center gap-3">
          <span className="text-2xl">⏰</span>
          <div>
            <p className="font-semibold text-amber-800">{proximos.length} registro{proximos.length > 1 ? 's' : ''} próximos a vencer (30 días)</p>
          </div>
        </div>
      )}

      {/* Tabs */}
      <div className="flex gap-1 bg-slate-100 p-1 rounded-xl w-fit">
        <button onClick={() => setTab('todos')} className={`px-5 py-2 rounded-lg text-sm font-medium transition-colors ${tab === 'todos' ? 'bg-white text-brand-700 shadow-sm' : 'text-slate-500 hover:text-slate-700'}`}>
          📋 Por animal
        </button>
        <button onClick={() => setTab('proximos')} className={`px-5 py-2 rounded-lg text-sm font-medium transition-colors ${tab === 'proximos' ? 'bg-white text-brand-700 shadow-sm' : 'text-slate-500 hover:text-slate-700'}`}>
          ⏰ Próximos a vencer {proximos.length > 0 && <span className="ml-1 bg-amber-500 text-white text-xs rounded-full px-1.5">{proximos.length}</span>}
        </button>
      </div>

      {tab === 'todos' && (
        <div className="flex gap-3 items-center">
          <label className="text-sm text-slate-600 whitespace-nowrap">Buscar por animal</label>
          <select className="input-field w-64" value={filtroAnimal} onChange={e => setFiltroAnimal(e.target.value)}>
            <option value="">Seleccionar animal…</option>
            {animales.map(a => <option key={a.id} value={a.id}>{a.nombre ?? a.codigo}</option>)}
          </select>
        </div>
      )}

      {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{error}</div>}

      {cargando ? <div className="text-slate-400 text-center py-12">Cargando…</div> : (
        <div className="card overflow-x-auto p-0">
          <table className="w-full text-sm">
            <thead className="bg-slate-50 border-b border-slate-200">
              <tr>{['Animal','Tipo Doc.','Número','Expedición','Vencimiento','Entidad','Estado',''].map(h => (
                <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
              ))}</tr>
            </thead>
            <tbody className="divide-y divide-slate-100">
              {listaActual.map(r => (
                <tr key={r.id} className="hover:bg-slate-50">
                  <td className="px-4 py-3 font-medium text-slate-800">{nombreAnimal(r.idAnimal)}</td>
                  <td className="px-4 py-3 text-slate-600">{r.tipoDocumento}</td>
                  <td className="px-4 py-3 font-mono text-slate-500">{r.numeroDocumento}</td>
                  <td className="px-4 py-3 text-slate-500">{r.fechaExpedicion}</td>
                  <td className="px-4 py-3 text-slate-500">{r.fechaVencimiento ?? '—'}</td>
                  <td className="px-4 py-3 text-slate-500">{r.entidadEmisora ?? '—'}</td>
                  <td className="px-4 py-3">
                    <span className={`text-xs px-2 py-0.5 rounded-full ${ESTADO_COLOR[r.estado] ?? 'bg-slate-100 text-slate-500'}`}>{r.estado}</span>
                  </td>
                  <td className="px-4 py-3">
                    {r.estado === 'VIGENTE' && (
                      <button onClick={() => cancelar(r.id)} className="text-red-500 hover:text-red-700 text-xs underline">Cancelar</button>
                    )}
                  </td>
                </tr>
              ))}
              {listaActual.length === 0 && (
                <tr><td colSpan={8} className="px-4 py-8 text-slate-400 text-center">
                  {tab === 'todos' ? (filtroAnimal ? 'Sin registros para este animal' : 'Selecciona un animal para ver sus registros') : 'Sin registros próximos a vencer'}
                </td></tr>
              )}
            </tbody>
          </table>
        </div>
      )}

      {modal && (
        <Modal titulo="Emitir Registro ICA" onClose={() => setModal(false)}>
          <form onSubmit={guardarRegistro} className="space-y-4">
            {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
            <div><label className="label">Animal *</label>
              <select className="input-field" value={fReg.idAnimal} onChange={e => setFReg(p => ({...p, idAnimal: e.target.value}))} required>
                <option value="">Seleccionar…</option>
                {animales.map(a => <option key={a.id} value={a.id}>{a.nombre ?? a.codigo}</option>)}
              </select></div>
            <div className="grid grid-cols-2 gap-4">
              <div><label className="label">Tipo documento *</label>
                <input className="input-field" placeholder="Guía ICA, SINIGAN…" value={fReg.tipoDocumento} onChange={e => setFReg(p => ({...p, tipoDocumento: e.target.value}))} required /></div>
              <div><label className="label">Número documento *</label>
                <input className="input-field" value={fReg.numeroDocumento} onChange={e => setFReg(p => ({...p, numeroDocumento: e.target.value}))} required /></div>
              <div><label className="label">Fecha expedición *</label>
                <input type="date" className="input-field" value={fReg.fechaExpedicion} onChange={e => setFReg(p => ({...p, fechaExpedicion: e.target.value}))} required /></div>
              <div><label className="label">Fecha vencimiento</label>
                <input type="date" className="input-field" value={fReg.fechaVencimiento} onChange={e => setFReg(p => ({...p, fechaVencimiento: e.target.value}))} /></div>
            </div>
            <div><label className="label">Entidad emisora</label>
              <input className="input-field" placeholder="ICA, MADR…" value={fReg.entidadEmisora} onChange={e => setFReg(p => ({...p, entidadEmisora: e.target.value}))} /></div>
            <div><label className="label">Observaciones</label>
              <textarea className="input-field" rows={2} value={fReg.observaciones} onChange={e => setFReg(p => ({...p, observaciones: e.target.value}))} /></div>
            <div className="flex gap-3 justify-end pt-2">
              <button type="button" onClick={() => setModal(false)} className="btn-secondary">Cancelar</button>
              <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Emitir'}</button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
