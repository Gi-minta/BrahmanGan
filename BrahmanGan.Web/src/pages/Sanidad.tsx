import { useEffect, useState, type FormEvent } from 'react';
import {
  api, type Medicamento, type Vacunacion,
  type Desparasitacion, type ControlPreventivo, type HistorialPreventivo,
} from '../api/client';
import Modal from '../components/Modal';

type Tab = 'vacunaciones' | 'desparasitacion' | 'controles';

const hoy = new Date().toISOString().slice(0, 10);

interface VacunaForm {
  idAnimal: string; idMedicamento: string; fecha: string;
  dosis: string; lote: string; responsable: string; proximaFecha: string;
}
const VACUNA_VACIA: VacunaForm = {
  idAnimal: '', idMedicamento: '', fecha: hoy,
  dosis: '', lote: '', responsable: '', proximaFecha: '',
};

interface DesparForm {
  idAnimal: string; idMedicamento: string; fecha: string;
  dosis: string; tipoParasito: string; proximaFecha: string;
}
const DESPAR_VACIO: DesparForm = {
  idAnimal: '', idMedicamento: '', fecha: hoy,
  dosis: '', tipoParasito: '', proximaFecha: '',
};

interface ControlForm { nombre: string; periodicidad: string; descripcion: string; }
const CONTROL_VACIO: ControlForm = { nombre: '', periodicidad: '', descripcion: '' };

interface AplicarForm {
  idAnimal: string; idControl: string; fecha: string;
  idMedicamento: string; dosis: string; responsable: string; proximaFecha: string;
}
const APLICAR_VACIO: AplicarForm = {
  idAnimal: '', idControl: '', fecha: hoy,
  idMedicamento: '', dosis: '', responsable: '', proximaFecha: '',
};

const TABS: { id: Tab; label: string }[] = [
  { id: 'vacunaciones',    label: '💊 Vacunaciones' },
  { id: 'desparasitacion', label: '🔬 Desparasitación' },
  { id: 'controles',       label: '📋 Controles Preventivos' },
];

export default function SanidadPage() {
  const [tab, setTab] = useState<Tab>('vacunaciones');

  const [medicamentos, setMedicamentos] = useState<Medicamento[]>([]);
  const [controles, setControles]       = useState<ControlPreventivo[]>([]);
  const [cargando, setCargando]         = useState(true);
  const [error, setError]               = useState('');

  // Vacunaciones
  const [alertas, setAlertas]         = useState<Vacunacion[]>([]);
  const [modalVacuna, setModalVacuna] = useState(false);
  const [formVacuna, setFormVacuna]   = useState<VacunaForm>(VACUNA_VACIA);
  const [guardVacuna, setGuardVacuna] = useState(false);
  const [errVacuna, setErrVacuna]     = useState('');

  // Desparasitación
  const [desparsAnimal, setDesparsAnimal]   = useState<Desparasitacion[] | null>(null);
  const [idAnimalDespar, setIdAnimalDespar] = useState('');
  const [buscandoDespar, setBuscandoDespar] = useState(false);
  const [modalDespar, setModalDespar]       = useState(false);
  const [formDespar, setFormDespar]         = useState<DesparForm>(DESPAR_VACIO);
  const [guardDespar, setGuardDespar]       = useState(false);
  const [errDespar, setErrDespar]           = useState('');

  // Controles Preventivos
  const [modalControl, setModalControl]       = useState(false);
  const [formControl, setFormControl]         = useState<ControlForm>(CONTROL_VACIO);
  const [guardControl, setGuardControl]       = useState(false);
  const [errControl, setErrControl]           = useState('');
  const [modalAplicar, setModalAplicar]       = useState(false);
  const [formAplicar, setFormAplicar]         = useState<AplicarForm>(APLICAR_VACIO);
  const [guardAplicar, setGuardAplicar]       = useState(false);
  const [errAplicar, setErrAplicar]           = useState('');
  const [historialAnimal, setHistorialAnimal] = useState<HistorialPreventivo[] | null>(null);
  const [idAnimalHist, setIdAnimalHist]       = useState('');
  const [buscandoHist, setBuscandoHist]       = useState(false);

  useEffect(() => {
    Promise.all([
      api.get<Medicamento[]>('/medicamentos'),
      api.get<Vacunacion[]>('/vacunaciones/alertas?dias=30'),
      api.get<ControlPreventivo[]>('/medicamentos/controles'),
    ])
      .then(([meds, alts, ctrls]) => {
        setMedicamentos(meds); setAlertas(alts); setControles(ctrls);
      })
      .catch(e => setError(e.message))
      .finally(() => setCargando(false));
  }, []);

  // ── Vacunaciones ──────────────────────────────────────────────
  async function guardarVacuna(e: FormEvent) {
    e.preventDefault(); setErrVacuna(''); setGuardVacuna(true);
    try {
      const nueva = await api.post<Vacunacion>('/vacunaciones', {
        idAnimal:      Number(formVacuna.idAnimal),
        idMedicamento: Number(formVacuna.idMedicamento),
        fecha:         formVacuna.fecha,
        dosis:         formVacuna.dosis ? Number(formVacuna.dosis) : undefined,
        lote:          formVacuna.lote || undefined,
        responsable:   formVacuna.responsable || undefined,
        proximaFecha:  formVacuna.proximaFecha || undefined,
      });
      if (nueva.proximaFecha) setAlertas(prev => [nueva, ...prev]);
      setModalVacuna(false); setFormVacuna(VACUNA_VACIA);
    } catch (err: unknown) { setErrVacuna(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardVacuna(false); }
  }

  // ── Desparasitación ───────────────────────────────────────────
  async function buscarDespar(e: FormEvent) {
    e.preventDefault(); setBuscandoDespar(true); setDesparsAnimal(null);
    try {
      const lista = await api.get<Desparasitacion[]>(
        `/vacunaciones/desparasitacion/animal/${idAnimalDespar}`,
      );
      setDesparsAnimal(lista);
    } catch { setDesparsAnimal([]); }
    finally { setBuscandoDespar(false); }
  }

  async function guardarDespar(e: FormEvent) {
    e.preventDefault(); setErrDespar(''); setGuardDespar(true);
    try {
      await api.post('/vacunaciones/desparasitacion', {
        idAnimal:      Number(formDespar.idAnimal),
        idMedicamento: Number(formDespar.idMedicamento),
        fecha:         formDespar.fecha,
        dosis:         formDespar.dosis ? Number(formDespar.dosis) : undefined,
        tipoParasito:  formDespar.tipoParasito || undefined,
        proximaFecha:  formDespar.proximaFecha || undefined,
      });
      setModalDespar(false); setFormDespar(DESPAR_VACIO);
      if (formDespar.idAnimal && formDespar.idAnimal === idAnimalDespar) {
        const lista = await api.get<Desparasitacion[]>(
          `/vacunaciones/desparasitacion/animal/${idAnimalDespar}`,
        );
        setDesparsAnimal(lista);
      }
    } catch (err: unknown) { setErrDespar(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardDespar(false); }
  }

  // ── Controles Preventivos ─────────────────────────────────────
  async function guardarControl(e: FormEvent) {
    e.preventDefault(); setErrControl(''); setGuardControl(true);
    try {
      const nuevo = await api.post<ControlPreventivo>('/medicamentos/controles', {
        nombre:       formControl.nombre,
        periodicidad: formControl.periodicidad || undefined,
        descripcion:  formControl.descripcion || undefined,
      });
      setControles(prev => [...prev, nuevo]);
      setModalControl(false); setFormControl(CONTROL_VACIO);
    } catch (err: unknown) { setErrControl(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardControl(false); }
  }

  async function aplicarControl(e: FormEvent) {
    e.preventDefault(); setErrAplicar(''); setGuardAplicar(true);
    try {
      await api.post('/medicamentos/controles/aplicar', {
        idAnimal:      Number(formAplicar.idAnimal),
        idControl:     Number(formAplicar.idControl),
        fecha:         formAplicar.fecha,
        idMedicamento: formAplicar.idMedicamento ? Number(formAplicar.idMedicamento) : undefined,
        dosis:         formAplicar.dosis ? Number(formAplicar.dosis) : undefined,
        responsable:   formAplicar.responsable || undefined,
        proximaFecha:  formAplicar.proximaFecha || undefined,
      });
      setModalAplicar(false); setFormAplicar(APLICAR_VACIO);
    } catch (err: unknown) { setErrAplicar(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardAplicar(false); }
  }

  async function buscarHistorial(e: FormEvent) {
    e.preventDefault(); setBuscandoHist(true); setHistorialAnimal(null);
    try {
      const lista = await api.get<HistorialPreventivo[]>(
        `/medicamentos/controles/historial/${idAnimalHist}`,
      );
      setHistorialAnimal(lista);
    } catch { setHistorialAnimal([]); }
    finally { setBuscandoHist(false); }
  }

  const diasRestantes = (fecha?: string) => {
    if (!fecha) return null;
    return Math.ceil((new Date(fecha).getTime() - Date.now()) / 86_400_000);
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Sanidad Animal</h1>
          <p className="text-slate-500 text-sm mt-1">Vacunaciones, desparasitación y controles preventivos</p>
        </div>
        {tab === 'vacunaciones' && (
          <button className="btn-primary" onClick={() => setModalVacuna(true)}>+ Registrar vacunación</button>
        )}
        {tab === 'desparasitacion' && (
          <button className="btn-primary" onClick={() => setModalDespar(true)}>+ Registrar desparasitación</button>
        )}
        {tab === 'controles' && (
          <div className="flex gap-2">
            <button className="btn-secondary" onClick={() => setModalControl(true)}>+ Nuevo control</button>
            <button className="btn-primary"   onClick={() => setModalAplicar(true)}>+ Aplicar control</button>
          </div>
        )}
      </div>

      {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-4 text-sm">{error}</div>}

      {/* Tabs */}
      <div className="flex border-b border-slate-200 gap-1">
        {TABS.map(t => (
          <button key={t.id} onClick={() => setTab(t.id)}
            className={`px-4 py-2.5 text-sm font-medium border-b-2 transition-colors -mb-px ${
              tab === t.id
                ? 'border-brand-600 text-brand-700'
                : 'border-transparent text-slate-500 hover:text-slate-700'
            }`}>
            {t.label}
          </button>
        ))}
      </div>

      {cargando ? (
        <div className="text-slate-400 text-center py-16">Cargando datos de sanidad…</div>
      ) : (
        <>
          {/* ── Vacunaciones ───────────────────────────────────── */}
          {tab === 'vacunaciones' && (
            <div className="grid grid-cols-1 xl:grid-cols-2 gap-6">
              <div className="card p-0 overflow-hidden">
                <div className="px-5 py-4 border-b border-slate-100 flex items-center gap-2">
                  <span className="text-xl">⚠️</span>
                  <div>
                    <p className="font-semibold text-slate-800">Alertas de vacunación</p>
                    <p className="text-xs text-slate-500">Próximas 30 días</p>
                  </div>
                  <span className="ml-auto bg-amber-100 text-amber-700 text-xs font-semibold px-2 py-0.5 rounded-full">
                    {alertas.length}
                  </span>
                </div>
                {alertas.length === 0 ? (
                  <div className="text-center py-10 text-slate-400 text-sm">Sin alertas pendientes</div>
                ) : (
                  <div className="divide-y divide-slate-100 max-h-96 overflow-y-auto">
                    {alertas.map(v => {
                      const dias = diasRestantes(v.proximaFecha);
                      return (
                        <div key={v.id} className="px-5 py-3 flex items-center justify-between">
                          <div>
                            <p className="text-sm font-medium text-slate-700">Animal #{v.idAnimal}</p>
                            <p className="text-xs text-slate-500">
                              Med. #{v.idMedicamento}
                              {v.lote && <span className="ml-2">Lote: {v.lote}</span>}
                              {v.dosis && <span className="ml-2">{v.dosis} ml</span>}
                            </p>
                          </div>
                          <div className="text-right">
                            <p className="text-xs text-slate-500">Próxima</p>
                            <p className={`text-sm font-semibold ${
                              dias != null && dias <= 0  ? 'text-red-700'
                              : dias != null && dias <= 7  ? 'text-red-600'
                              : dias != null && dias <= 15 ? 'text-amber-600'
                              : 'text-slate-700'
                            }`}>
                              {v.proximaFecha ? new Date(v.proximaFecha).toLocaleDateString('es-CO') : '—'}
                              {dias != null && (
                                <span className="ml-1 text-xs font-normal">
                                  ({dias <= 0 ? 'vencida' : `${dias}d`})
                                </span>
                              )}
                            </p>
                          </div>
                        </div>
                      );
                    })}
                  </div>
                )}
              </div>

              <div className="card p-0 overflow-hidden">
                <div className="px-5 py-4 border-b border-slate-100 flex items-center gap-2">
                  <span className="text-xl">💊</span>
                  <div>
                    <p className="font-semibold text-slate-800">Medicamentos</p>
                    <p className="text-xs text-slate-500">Catálogo activo</p>
                  </div>
                  <span className="ml-auto bg-blue-100 text-blue-700 text-xs font-semibold px-2 py-0.5 rounded-full">
                    {medicamentos.filter(m => m.activo).length}
                  </span>
                </div>
                {medicamentos.length === 0 ? (
                  <div className="text-center py-10 text-slate-400 text-sm">Sin medicamentos registrados</div>
                ) : (
                  <div className="overflow-x-auto max-h-96 overflow-y-auto">
                    <table className="w-full text-sm">
                      <thead className="bg-slate-50 sticky top-0">
                        <tr>
                          {['Código', 'Nombre', 'Carne', 'Leche'].map(h => (
                            <th key={h} className="px-4 py-2.5 text-left text-xs font-semibold text-slate-600">{h}</th>
                          ))}
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-slate-100">
                        {medicamentos.map(m => (
                          <tr key={m.id} className={`hover:bg-slate-50 ${!m.activo ? 'opacity-40' : ''}`}>
                            <td className="px-4 py-2.5 font-mono text-brand-700 text-xs">{m.codigo}</td>
                            <td className="px-4 py-2.5 text-slate-700">{m.nombre}</td>
                            <td className="px-4 py-2.5 text-slate-500 text-xs">
                              {m.tiempoCarne != null ? `${m.tiempoCarne}d` : '—'}
                            </td>
                            <td className="px-4 py-2.5 text-slate-500 text-xs">
                              {m.tiempoLeche != null ? `${m.tiempoLeche}d` : '—'}
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                )}
              </div>
            </div>
          )}

          {/* ── Desparasitación ────────────────────────────────── */}
          {tab === 'desparasitacion' && (
            <div className="space-y-4">
              <div className="card">
                <p className="text-sm font-semibold text-slate-700 mb-3">Historial por animal</p>
                <form onSubmit={buscarDespar} className="flex gap-2">
                  <input className="input flex-1" type="number" min="1" placeholder="ID del animal"
                    value={idAnimalDespar} onChange={e => setIdAnimalDespar(e.target.value)} required />
                  <button type="submit" className="btn-primary px-4" disabled={buscandoDespar}>
                    {buscandoDespar ? '…' : 'Consultar'}
                  </button>
                </form>
              </div>
              {desparsAnimal !== null && (
                desparsAnimal.length === 0 ? (
                  <div className="card text-center py-10 text-slate-400">Sin registros de desparasitación</div>
                ) : (
                  <div className="card overflow-x-auto p-0">
                    <table className="w-full text-sm">
                      <thead className="bg-slate-50 border-b border-slate-200">
                        <tr>
                          {['Fecha', 'Medicamento', 'Dosis', 'Tipo parásito', 'Próxima'].map(h => (
                            <th key={h} className="px-4 py-3 text-left text-xs font-semibold text-slate-600">{h}</th>
                          ))}
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-slate-100">
                        {desparsAnimal.map(d => (
                          <tr key={d.id} className="hover:bg-slate-50">
                            <td className="px-4 py-2.5 text-slate-700">
                              {new Date(d.fecha).toLocaleDateString('es-CO')}
                            </td>
                            <td className="px-4 py-2.5 text-slate-500">#{d.idMedicamento}</td>
                            <td className="px-4 py-2.5 text-slate-500">{d.dosis ?? '—'}</td>
                            <td className="px-4 py-2.5 text-slate-500">{d.tipoParasito ?? '—'}</td>
                            <td className="px-4 py-2.5 text-slate-500">{d.proximaFecha ?? '—'}</td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                )
              )}
            </div>
          )}

          {/* ── Controles Preventivos ──────────────────────────── */}
          {tab === 'controles' && (
            <div className="grid grid-cols-1 xl:grid-cols-2 gap-6">
              <div className="card p-0 overflow-hidden">
                <div className="px-5 py-4 border-b border-slate-100 flex items-center gap-2">
                  <span className="text-xl">📋</span>
                  <p className="font-semibold text-slate-800">Catálogo de controles</p>
                  <span className="ml-auto bg-blue-100 text-blue-700 text-xs font-semibold px-2 py-0.5 rounded-full">
                    {controles.length}
                  </span>
                </div>
                {controles.length === 0 ? (
                  <div className="text-center py-10 text-slate-400 text-sm">Sin controles registrados</div>
                ) : (
                  <div className="divide-y divide-slate-100 max-h-80 overflow-y-auto">
                    {controles.map(c => (
                      <div key={c.id} className="px-5 py-3">
                        <p className="text-sm font-medium text-slate-800">{c.nombre}</p>
                        {c.periodicidad && <p className="text-xs text-slate-500">{c.periodicidad}</p>}
                        {c.descripcion  && <p className="text-xs text-slate-400 mt-0.5">{c.descripcion}</p>}
                      </div>
                    ))}
                  </div>
                )}
              </div>

              <div className="card space-y-4">
                <p className="text-sm font-semibold text-slate-700">Historial por animal</p>
                <form onSubmit={buscarHistorial} className="flex gap-2">
                  <input className="input flex-1" type="number" min="1" placeholder="ID del animal"
                    value={idAnimalHist} onChange={e => setIdAnimalHist(e.target.value)} required />
                  <button type="submit" className="btn-primary px-4" disabled={buscandoHist}>
                    {buscandoHist ? '…' : 'Consultar'}
                  </button>
                </form>
                {historialAnimal !== null && (
                  historialAnimal.length === 0 ? (
                    <p className="text-slate-400 text-sm text-center py-4">Sin historial para este animal</p>
                  ) : (
                    <div className="overflow-x-auto max-h-60 overflow-y-auto">
                      <table className="w-full text-sm">
                        <thead>
                          <tr className="border-b border-slate-200">
                            {['Fecha', 'Control', 'Responsable', 'Próxima'].map(h => (
                              <th key={h} className="px-2 py-2 text-left text-xs font-semibold text-slate-500">{h}</th>
                            ))}
                          </tr>
                        </thead>
                        <tbody className="divide-y divide-slate-100">
                          {historialAnimal.map(h => (
                            <tr key={h.id}>
                              <td className="px-2 py-2 text-slate-700">
                                {new Date(h.fecha).toLocaleDateString('es-CO')}
                              </td>
                              <td className="px-2 py-2 text-slate-500">#{h.idControl}</td>
                              <td className="px-2 py-2 text-slate-500">{h.responsable ?? '—'}</td>
                              <td className="px-2 py-2 text-slate-500 text-xs">{h.proximaFecha ?? '—'}</td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </div>
                  )
                )}
              </div>
            </div>
          )}
        </>
      )}

      {/* ── Modal Vacunación ─────────────────────────────────────── */}
      {modalVacuna && (
        <Modal titulo="Registrar vacunación"
          onClose={() => { setModalVacuna(false); setErrVacuna(''); setFormVacuna(VACUNA_VACIA); }}>
          <form onSubmit={guardarVacuna} className="space-y-4">
            {errVacuna && (
              <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errVacuna}</div>
            )}
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">ID Animal *</label>
                <input className="input" type="number" min="1" required value={formVacuna.idAnimal}
                  onChange={e => setFormVacuna(f => ({ ...f, idAnimal: e.target.value }))} />
              </div>
              <div>
                <label className="label">Medicamento *</label>
                <select className="input" required value={formVacuna.idMedicamento}
                  onChange={e => setFormVacuna(f => ({ ...f, idMedicamento: e.target.value }))}>
                  <option value="">Seleccionar…</option>
                  {medicamentos.filter(m => m.activo).map(m => (
                    <option key={m.id} value={m.id}>{m.nombre}</option>
                  ))}
                </select>
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Fecha aplicación *</label>
                <input className="input" type="date" required value={formVacuna.fecha}
                  onChange={e => setFormVacuna(f => ({ ...f, fecha: e.target.value }))} />
              </div>
              <div>
                <label className="label">Próxima aplicación</label>
                <input className="input" type="date" value={formVacuna.proximaFecha}
                  onChange={e => setFormVacuna(f => ({ ...f, proximaFecha: e.target.value }))} />
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Dosis (ml)</label>
                <input className="input" type="number" step="0.1" min="0" value={formVacuna.dosis}
                  onChange={e => setFormVacuna(f => ({ ...f, dosis: e.target.value }))} placeholder="Ej: 5.0" />
              </div>
              <div>
                <label className="label">Lote</label>
                <input className="input" value={formVacuna.lote}
                  onChange={e => setFormVacuna(f => ({ ...f, lote: e.target.value }))} placeholder="Número de lote" />
              </div>
            </div>
            <div>
              <label className="label">Responsable</label>
              <input className="input" value={formVacuna.responsable}
                onChange={e => setFormVacuna(f => ({ ...f, responsable: e.target.value }))} placeholder="Veterinario" />
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary" onClick={() => setModalVacuna(false)}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardVacuna}>
                {guardVacuna ? 'Guardando…' : 'Registrar vacunación'}
              </button>
            </div>
          </form>
        </Modal>
      )}

      {/* ── Modal Desparasitación ─────────────────────────────────── */}
      {modalDespar && (
        <Modal titulo="Registrar desparasitación"
          onClose={() => { setModalDespar(false); setErrDespar(''); setFormDespar(DESPAR_VACIO); }}>
          <form onSubmit={guardarDespar} className="space-y-4">
            {errDespar && (
              <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errDespar}</div>
            )}
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">ID Animal *</label>
                <input className="input" type="number" min="1" required value={formDespar.idAnimal}
                  onChange={e => setFormDespar(f => ({ ...f, idAnimal: e.target.value }))} />
              </div>
              <div>
                <label className="label">Medicamento *</label>
                <select className="input" required value={formDespar.idMedicamento}
                  onChange={e => setFormDespar(f => ({ ...f, idMedicamento: e.target.value }))}>
                  <option value="">Seleccionar…</option>
                  {medicamentos.filter(m => m.activo).map(m => (
                    <option key={m.id} value={m.id}>{m.nombre}</option>
                  ))}
                </select>
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Fecha *</label>
                <input className="input" type="date" required value={formDespar.fecha}
                  onChange={e => setFormDespar(f => ({ ...f, fecha: e.target.value }))} />
              </div>
              <div>
                <label className="label">Próxima fecha</label>
                <input className="input" type="date" value={formDespar.proximaFecha}
                  onChange={e => setFormDespar(f => ({ ...f, proximaFecha: e.target.value }))} />
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Dosis (ml)</label>
                <input className="input" type="number" step="0.1" min="0" value={formDespar.dosis}
                  onChange={e => setFormDespar(f => ({ ...f, dosis: e.target.value }))} />
              </div>
              <div>
                <label className="label">Tipo parásito</label>
                <input className="input" value={formDespar.tipoParasito}
                  onChange={e => setFormDespar(f => ({ ...f, tipoParasito: e.target.value }))}
                  placeholder="Ej: Interno, Externo" />
              </div>
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary" onClick={() => setModalDespar(false)}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardDespar}>
                {guardDespar ? 'Guardando…' : 'Registrar'}
              </button>
            </div>
          </form>
        </Modal>
      )}

      {/* ── Modal Nuevo Control Preventivo ───────────────────────── */}
      {modalControl && (
        <Modal titulo="Nuevo control preventivo"
          onClose={() => { setModalControl(false); setErrControl(''); setFormControl(CONTROL_VACIO); }}>
          <form onSubmit={guardarControl} className="space-y-4">
            {errControl && (
              <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errControl}</div>
            )}
            <div>
              <label className="label">Nombre *</label>
              <input className="input" required value={formControl.nombre}
                onChange={e => setFormControl(f => ({ ...f, nombre: e.target.value }))}
                placeholder="Ej: Vitaminización" />
            </div>
            <div>
              <label className="label">Periodicidad</label>
              <input className="input" value={formControl.periodicidad}
                onChange={e => setFormControl(f => ({ ...f, periodicidad: e.target.value }))}
                placeholder="Ej: Trimestral" />
            </div>
            <div>
              <label className="label">Descripción</label>
              <textarea className="input" rows={2} value={formControl.descripcion}
                onChange={e => setFormControl(f => ({ ...f, descripcion: e.target.value }))} />
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary" onClick={() => setModalControl(false)}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardControl}>
                {guardControl ? 'Guardando…' : 'Crear control'}
              </button>
            </div>
          </form>
        </Modal>
      )}

      {/* ── Modal Aplicar Control ─────────────────────────────────── */}
      {modalAplicar && (
        <Modal titulo="Aplicar control preventivo"
          onClose={() => { setModalAplicar(false); setErrAplicar(''); setFormAplicar(APLICAR_VACIO); }}>
          <form onSubmit={aplicarControl} className="space-y-4">
            {errAplicar && (
              <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errAplicar}</div>
            )}
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">ID Animal *</label>
                <input className="input" type="number" min="1" required value={formAplicar.idAnimal}
                  onChange={e => setFormAplicar(f => ({ ...f, idAnimal: e.target.value }))} />
              </div>
              <div>
                <label className="label">Control *</label>
                <select className="input" required value={formAplicar.idControl}
                  onChange={e => setFormAplicar(f => ({ ...f, idControl: e.target.value }))}>
                  <option value="">Seleccionar…</option>
                  {controles.map(c => <option key={c.id} value={c.id}>{c.nombre}</option>)}
                </select>
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Fecha *</label>
                <input className="input" type="date" required value={formAplicar.fecha}
                  onChange={e => setFormAplicar(f => ({ ...f, fecha: e.target.value }))} />
              </div>
              <div>
                <label className="label">Próxima fecha</label>
                <input className="input" type="date" value={formAplicar.proximaFecha}
                  onChange={e => setFormAplicar(f => ({ ...f, proximaFecha: e.target.value }))} />
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Medicamento (opc.)</label>
                <select className="input" value={formAplicar.idMedicamento}
                  onChange={e => setFormAplicar(f => ({ ...f, idMedicamento: e.target.value }))}>
                  <option value="">Ninguno</option>
                  {medicamentos.filter(m => m.activo).map(m => (
                    <option key={m.id} value={m.id}>{m.nombre}</option>
                  ))}
                </select>
              </div>
              <div>
                <label className="label">Dosis (ml)</label>
                <input className="input" type="number" step="0.1" min="0" value={formAplicar.dosis}
                  onChange={e => setFormAplicar(f => ({ ...f, dosis: e.target.value }))} />
              </div>
            </div>
            <div>
              <label className="label">Responsable</label>
              <input className="input" value={formAplicar.responsable}
                onChange={e => setFormAplicar(f => ({ ...f, responsable: e.target.value }))} />
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary" onClick={() => setModalAplicar(false)}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardAplicar}>
                {guardAplicar ? 'Guardando…' : 'Aplicar'}
              </button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
