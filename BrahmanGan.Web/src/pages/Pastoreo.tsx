import { useEffect, useState, type FormEvent } from 'react';
import { api, type Potrero, type PlanPastoreo } from '../api/client';
import Modal from '../components/Modal';

const hoy = new Date().toISOString().slice(0, 10);

interface PlanForm {
  idPotrero: string; fechaInicio: string; fechaFin: string;
  numAnimales: string; capacidadCarga: string; observaciones: string;
}
const PLAN_VACIO: PlanForm = {
  idPotrero: '', fechaInicio: hoy, fechaFin: '',
  numAnimales: '', capacidadCarga: '', observaciones: '',
};

export default function PastoreoPage() {
  const [potreros, setPotreros] = useState<Potrero[]>([]);
  const [planes, setPlanes]     = useState<PlanPastoreo[]>([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError]       = useState('');

  const [modal, setModal]       = useState(false);
  const [form, setForm]         = useState<PlanForm>(PLAN_VACIO);
  const [guardando, setGuard]   = useState(false);
  const [errForm, setErrForm]   = useState('');

  const [filtroPotrero, setFiltroPotrero] = useState('');

  useEffect(() => {
    Promise.all([
      api.get<Potrero[]>('/potreros/finca/0').catch(() => [] as Potrero[]),
      api.get<PlanPastoreo[]>('/pastoreo/planes'),
    ])
      .then(([_pot, pl]) => { setPlanes(pl); })
      .catch(e => setError(e.message))
      .finally(() => setCargando(false));

    // load potreros de todas las fincas vía fincas
    api.get<{id:number}[]>('/fincas').then(async fincas => {
      const todas = await Promise.all(fincas.map(f => api.get<Potrero[]>(`/potreros/finca/${f.id}`).catch(() => [])));
      setPotreros(todas.flat() as Potrero[]);
    }).catch(() => {});
  }, []);

  function set(f: keyof PlanForm, v: string) { setForm(p => ({ ...p, [f]: v })); }

  async function guardar(e: FormEvent) {
    e.preventDefault(); setErrForm(''); setGuard(true);
    try {
      const nuevo = await api.post<PlanPastoreo>('/pastoreo/planes', {
        idPotrero:      Number(form.idPotrero),
        fechaInicio:    form.fechaInicio,
        fechaFin:       form.fechaFin || undefined,
        numAnimales:    form.numAnimales ? Number(form.numAnimales) : undefined,
        capacidadCarga: form.capacidadCarga ? Number(form.capacidadCarga) : undefined,
        observaciones:  form.observaciones || undefined,
      });
      setPlanes(prev => [nuevo, ...prev]);
      setModal(false); setForm(PLAN_VACIO);
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuard(false); }
  }

  function potreroNombre(id: number) { return potreros.find(p => p.id === id)?.nombre ?? `Potrero #${id}`; }

  const planesFiltrados = filtroPotrero
    ? planes.filter(p => String(p.idPotrero) === filtroPotrero)
    : planes;

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Pastoreo</h1>
          <p className="text-slate-500 text-sm mt-1">Rotación de potreros y carga animal</p>
        </div>
        <button className="btn-primary" onClick={() => setModal(true)}>+ Nuevo plan</button>
      </div>

      {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-4 text-sm">{error}</div>}

      {/* Filtro potrero */}
      <div className="flex items-center gap-3">
        <select className="input max-w-xs" value={filtroPotrero} onChange={e => setFiltroPotrero(e.target.value)}>
          <option value="">Todos los potreros</option>
          {potreros.map(p => <option key={p.id} value={p.id}>{p.nombre}</option>)}
        </select>
        <span className="text-sm text-slate-500">{planesFiltrados.length} planes</span>
      </div>

      {cargando ? (
        <div className="text-slate-400 text-center py-16">Cargando planes…</div>
      ) : planesFiltrados.length === 0 ? (
        <div className="card text-center py-16 text-slate-400">Sin planes de pastoreo registrados.</div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
          {planesFiltrados.map(plan => (
            <div key={plan.id} className="card space-y-3">
              <div className="flex items-start justify-between">
                <div>
                  <p className="font-semibold text-slate-800">{potreroNombre(plan.idPotrero)}</p>
                  <p className="text-xs text-slate-500 mt-0.5">
                    {plan.fechaInicio} → {plan.fechaFin ?? 'En curso'}
                  </p>
                </div>
                <span className={`text-xs px-2 py-0.5 rounded-full font-medium ${plan.activo ? 'bg-green-100 text-green-700' : 'bg-slate-100 text-slate-500'}`}>
                  {plan.activo ? 'Activo' : 'Finalizado'}
                </span>
              </div>

              <div className="grid grid-cols-2 gap-2 text-sm">
                <div className="bg-slate-50 rounded-lg p-2 text-center">
                  <p className="text-xs text-slate-500">Animales</p>
                  <p className="font-bold text-slate-800">{plan.numAnimales ?? '—'}</p>
                </div>
                <div className="bg-slate-50 rounded-lg p-2 text-center">
                  <p className="text-xs text-slate-500">Cap. carga</p>
                  <p className="font-bold text-slate-800">{plan.capacidadCarga != null ? `${plan.capacidadCarga} UA/ha` : '—'}</p>
                </div>
              </div>

              {plan.observaciones && (
                <p className="text-xs text-slate-500 italic border-t border-slate-100 pt-2">{plan.observaciones}</p>
              )}
            </div>
          ))}
        </div>
      )}

      {modal && (
        <Modal titulo="Nuevo plan de pastoreo" onClose={() => { setModal(false); setForm(PLAN_VACIO); setErrForm(''); }}>
          <form onSubmit={guardar} className="space-y-4">
            {errForm && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errForm}</div>}
            <div>
              <label className="label">Potrero *</label>
              <select className="input" required value={form.idPotrero} onChange={e => set('idPotrero', e.target.value)}>
                <option value="">Seleccionar potrero…</option>
                {potreros.map(p => <option key={p.id} value={p.id}>{p.nombre}</option>)}
              </select>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Fecha inicio *</label>
                <input className="input" type="date" required value={form.fechaInicio} onChange={e => set('fechaInicio', e.target.value)} />
              </div>
              <div>
                <label className="label">Fecha fin</label>
                <input className="input" type="date" value={form.fechaFin} onChange={e => set('fechaFin', e.target.value)} />
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">N° animales</label>
                <input className="input" type="number" min="0" value={form.numAnimales} onChange={e => set('numAnimales', e.target.value)} />
              </div>
              <div>
                <label className="label">Cap. carga (UA/ha)</label>
                <input className="input" type="number" step="0.1" min="0" value={form.capacidadCarga} onChange={e => set('capacidadCarga', e.target.value)} />
              </div>
            </div>
            <div>
              <label className="label">Observaciones</label>
              <textarea className="input" rows={3} value={form.observaciones} onChange={e => set('observaciones', e.target.value)} />
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary" onClick={() => setModal(false)}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardando}>
                {guardando ? 'Guardando…' : 'Crear plan'}
              </button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
