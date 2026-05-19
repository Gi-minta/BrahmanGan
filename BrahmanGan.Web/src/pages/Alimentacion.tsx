import { useEffect, useState, type FormEvent } from 'react';
import { api, type Finca, type PlanAlimentacion, type DetallePlanAlimentacion } from '../api/client';
import Modal from '../components/Modal';

const hoy = new Date().toISOString().slice(0, 10);

interface PlanForm { idFinca: string; nombre: string; fechaInicio: string; fechaFin: string; observaciones: string; }
const PLAN_VACIO: PlanForm = { idFinca: '', nombre: '', fechaInicio: hoy, fechaFin: '', observaciones: '' };

interface DetalleForm { alimento: string; cantidadDiaria: string; unidadMedida: string; idInsumo: string; observaciones: string; }
const DET_VACIO: DetalleForm = { alimento: '', cantidadDiaria: '', unidadMedida: 'kg', idInsumo: '', observaciones: '' };

export default function AlimentacionPage() {
  const [fincas, setFincas]     = useState<Finca[]>([]);
  const [planes, setPlanes]     = useState<PlanAlimentacion[]>([]);
  const [detalles, setDetalles] = useState<Record<number, DetallePlanAlimentacion[]>>({});
  const [expandido, setExpandido] = useState<number | null>(null);
  const [cargando, setCargando] = useState(true);
  const [error, setError]       = useState('');

  const [modalPlan, setModalPlan]   = useState(false);
  const [formPlan, setFormPlan]     = useState<PlanForm>(PLAN_VACIO);
  const [guardPlan, setGuardPlan]   = useState(false);
  const [errPlan, setErrPlan]       = useState('');

  const [planDetalle, setPlanDetalle] = useState<number | null>(null);
  const [formDet, setFormDet]         = useState<DetalleForm>(DET_VACIO);
  const [guardDet, setGuardDet]       = useState(false);
  const [errDet, setErrDet]           = useState('');

  useEffect(() => {
    Promise.all([
      api.get<Finca[]>('/fincas'),
      api.get<PlanAlimentacion[]>('/alimentacion/planes'),
    ])
      .then(([f, p]) => { setFincas(f); setPlanes(p); })
      .catch(e => setError(e.message))
      .finally(() => setCargando(false));
  }, []);

  async function togglePlan(id: number) {
    if (expandido === id) { setExpandido(null); return; }
    setExpandido(id);
    setPlanDetalle(null);
    if (detalles[id] !== undefined) return;
    try {
      const d = await api.get<DetallePlanAlimentacion[]>(`/alimentacion/planes/${id}/detalles`);
      setDetalles(prev => ({ ...prev, [id]: d }));
    } catch { setDetalles(prev => ({ ...prev, [id]: [] })); }
  }

  async function guardarPlan(e: FormEvent) {
    e.preventDefault(); setErrPlan(''); setGuardPlan(true);
    try {
      const nuevo = await api.post<PlanAlimentacion>('/alimentacion/planes', {
        idFinca:     Number(formPlan.idFinca),
        nombre:      formPlan.nombre,
        fechaInicio: formPlan.fechaInicio,
        fechaFin:    formPlan.fechaFin || undefined,
        observaciones: formPlan.observaciones || undefined,
      });
      setPlanes(prev => [nuevo, ...prev]);
      setModalPlan(false); setFormPlan(PLAN_VACIO);
    } catch (err: unknown) { setErrPlan(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardPlan(false); }
  }

  async function guardarDetalle(e: FormEvent, idPlan: number) {
    e.preventDefault(); setErrDet(''); setGuardDet(true);
    try {
      const d = await api.post<DetallePlanAlimentacion>('/alimentacion/detalles', {
        idPlan,
        alimento:      formDet.alimento,
        cantidadDiaria: Number(formDet.cantidadDiaria),
        unidadMedida:  formDet.unidadMedida,
        idInsumo:      formDet.idInsumo ? Number(formDet.idInsumo) : undefined,
        observaciones: formDet.observaciones || undefined,
      });
      setDetalles(prev => ({ ...prev, [idPlan]: [...(prev[idPlan] ?? []), d] }));
      setPlanDetalle(null); setFormDet(DET_VACIO);
    } catch (err: unknown) { setErrDet(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardDet(false); }
  }

  function fincaNombre(id: number) { return fincas.find(f => f.id === id)?.nombre ?? `Finca #${id}`; }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Alimentación</h1>
          <p className="text-slate-500 text-sm mt-1">Planes de alimentación y suministro de forraje</p>
        </div>
        <button className="btn-primary" onClick={() => setModalPlan(true)}>+ Nuevo plan</button>
      </div>

      {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-4 text-sm">{error}</div>}

      {cargando ? (
        <div className="text-slate-400 text-center py-16">Cargando planes…</div>
      ) : planes.length === 0 ? (
        <div className="card text-center py-16 text-slate-400">No hay planes de alimentación registrados.</div>
      ) : (
        <div className="space-y-3">
          {planes.map(plan => (
            <div key={plan.id} className="card p-0 overflow-hidden">
              <button
                className="w-full flex items-center gap-4 px-5 py-4 hover:bg-slate-50 transition-colors text-left"
                onClick={() => togglePlan(plan.id)}
              >
                <span className="text-2xl">🌾</span>
                <div className="flex-1 min-w-0">
                  <p className="font-semibold text-slate-800 truncate">{plan.nombre}</p>
                  <p className="text-xs text-slate-500">
                    {fincaNombre(plan.idFinca)} · Inicio: {plan.fechaInicio}
                    {plan.fechaFin && ` → ${plan.fechaFin}`}
                  </p>
                </div>
                <span className={`text-xs px-2 py-0.5 rounded-full font-medium ${plan.activo ? 'bg-green-100 text-green-700' : 'bg-slate-100 text-slate-500'}`}>
                  {plan.activo ? 'Activo' : 'Inactivo'}
                </span>
                <span className="text-slate-400 text-sm ml-2">{expandido === plan.id ? '▲' : '▼'}</span>
              </button>

              {expandido === plan.id && (
                <div className="border-t border-slate-100 px-5 py-4 space-y-4">
                  {plan.observaciones && (
                    <p className="text-sm text-slate-600 italic">{plan.observaciones}</p>
                  )}

                  <div className="flex items-center justify-between">
                    <p className="text-sm font-semibold text-slate-700">Componentes del plan</p>
                    <button className="text-xs text-brand-600 hover:underline"
                      onClick={() => setPlanDetalle(planDetalle === plan.id ? null : plan.id)}>
                      + Agregar componente
                    </button>
                  </div>

                  {planDetalle === plan.id && (
                    <form onSubmit={e => guardarDetalle(e, plan.id)} className="bg-slate-50 rounded-xl p-4 space-y-3">
                      {errDet && <p className="text-red-600 text-sm">{errDet}</p>}
                      <div className="grid grid-cols-2 gap-3">
                        <div>
                          <label className="label">Alimento *</label>
                          <input className="input" required value={formDet.alimento}
                            onChange={e => setFormDet(p => ({ ...p, alimento: e.target.value }))}
                            placeholder="Ej: Heno de kikuyo" />
                        </div>
                        <div>
                          <label className="label">Unidad</label>
                          <input className="input" value={formDet.unidadMedida}
                            onChange={e => setFormDet(p => ({ ...p, unidadMedida: e.target.value }))}
                            placeholder="kg, lt, unidades…" />
                        </div>
                      </div>
                      <div className="grid grid-cols-2 gap-3">
                        <div>
                          <label className="label">Cantidad diaria *</label>
                          <input className="input" type="number" step="0.1" min="0" required
                            value={formDet.cantidadDiaria}
                            onChange={e => setFormDet(p => ({ ...p, cantidadDiaria: e.target.value }))} />
                        </div>
                        <div>
                          <label className="label">ID Insumo (opc.)</label>
                          <input className="input" type="number" min="1" value={formDet.idInsumo}
                            onChange={e => setFormDet(p => ({ ...p, idInsumo: e.target.value }))} />
                        </div>
                      </div>
                      <div>
                        <label className="label">Observaciones</label>
                        <input className="input" value={formDet.observaciones}
                          onChange={e => setFormDet(p => ({ ...p, observaciones: e.target.value }))} />
                      </div>
                      <div className="flex gap-2 justify-end">
                        <button type="button" className="btn-secondary" onClick={() => setPlanDetalle(null)}>Cancelar</button>
                        <button type="submit" className="btn-primary" disabled={guardDet}>
                          {guardDet ? 'Guardando…' : 'Agregar'}
                        </button>
                      </div>
                    </form>
                  )}

                  {detalles[plan.id] === undefined ? (
                    <p className="text-slate-400 text-sm">Cargando componentes…</p>
                  ) : detalles[plan.id].length === 0 ? (
                    <p className="text-slate-400 text-sm text-center py-4">Sin componentes registrados</p>
                  ) : (
                    <table className="w-full text-sm">
                      <thead>
                        <tr className="border-b border-slate-100">
                          {['Alimento', 'Cantidad diaria', 'Unidad', 'Insumo'].map(h => (
                            <th key={h} className="text-left py-2 px-3 text-xs font-semibold text-slate-500">{h}</th>
                          ))}
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-slate-50">
                        {detalles[plan.id].map(d => (
                          <tr key={d.id} className="hover:bg-slate-50">
                            <td className="py-2 px-3 text-slate-700">{d.alimento}</td>
                            <td className="py-2 px-3 font-medium text-slate-800">{d.cantidadDiaria}</td>
                            <td className="py-2 px-3 text-slate-500">{d.unidadMedida}</td>
                            <td className="py-2 px-3 text-slate-400 text-xs">{d.idInsumo ? `#${d.idInsumo}` : '—'}</td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  )}
                </div>
              )}
            </div>
          ))}
        </div>
      )}

      {modalPlan && (
        <Modal titulo="Nuevo plan de alimentación" onClose={() => { setModalPlan(false); setFormPlan(PLAN_VACIO); setErrPlan(''); }}>
          <form onSubmit={guardarPlan} className="space-y-4">
            {errPlan && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errPlan}</div>}
            <div>
              <label className="label">Finca *</label>
              <select className="input" required value={formPlan.idFinca}
                onChange={e => setFormPlan(p => ({ ...p, idFinca: e.target.value }))}>
                <option value="">Seleccionar finca…</option>
                {fincas.map(f => <option key={f.id} value={f.id}>{f.nombre}</option>)}
              </select>
            </div>
            <div>
              <label className="label">Nombre del plan *</label>
              <input className="input" required value={formPlan.nombre}
                onChange={e => setFormPlan(p => ({ ...p, nombre: e.target.value }))}
                placeholder="Ej: Plan verano 2026" />
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Fecha inicio *</label>
                <input className="input" type="date" required value={formPlan.fechaInicio}
                  onChange={e => setFormPlan(p => ({ ...p, fechaInicio: e.target.value }))} />
              </div>
              <div>
                <label className="label">Fecha fin</label>
                <input className="input" type="date" value={formPlan.fechaFin}
                  onChange={e => setFormPlan(p => ({ ...p, fechaFin: e.target.value }))} />
              </div>
            </div>
            <div>
              <label className="label">Observaciones</label>
              <textarea className="input" rows={3} value={formPlan.observaciones}
                onChange={e => setFormPlan(p => ({ ...p, observaciones: e.target.value }))} />
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary" onClick={() => setModalPlan(false)}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardPlan}>
                {guardPlan ? 'Guardando…' : 'Crear plan'}
              </button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
