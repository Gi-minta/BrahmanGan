import { useState, type FormEvent } from 'react';
import { api, type ControlLeche, type ParametroLactancia, type CalidadLeche } from '../api/client';
import Modal from '../components/Modal';

type Tab = 'diario' | 'lactancias' | 'calidad';

const hoy    = new Date().toISOString().slice(0, 10);
const hace30 = new Date(Date.now() - 30 * 86_400_000).toISOString().slice(0, 10);

interface RegistroForm {
  idAnimal: string; fecha: string;
  maniana: string; tarde: string; noche: string; ordeno: string;
}
const REGISTRO_VACIO: RegistroForm = {
  idAnimal: '', fecha: hoy,
  maniana: '', tarde: '', noche: '', ordeno: '',
};

interface LactanciaForm { idAnimal: string; numeroParto: string; fechaInicio: string; }
const LACTANCIA_VACIA: LactanciaForm = { idAnimal: '', numeroParto: '', fechaInicio: hoy };

interface CalidadForm {
  idAnimal: string; fecha: string; celSomaticas: string;
  grasaPct: string; proteinaPct: string; laboratorio: string; resultado: string; observaciones: string;
}
const CALIDAD_VACIA: CalidadForm = {
  idAnimal: '', fecha: hoy, celSomaticas: '',
  grasaPct: '', proteinaPct: '', laboratorio: '', resultado: '', observaciones: '',
};

const TABS: { id: Tab; label: string }[] = [
  { id: 'diario',     label: '🥛 Control Diario' },
  { id: 'lactancias', label: '📈 Lactancias' },
  { id: 'calidad',    label: '🔬 Calidad' },
];

export default function LechePage() {
  const [tab, setTab] = useState<Tab>('diario');

  // ── Control Diario ─────────────────────────────────────────────
  const [idAnimal, setIdAnimal]   = useState('');
  const [desde, setDesde]         = useState(hace30);
  const [hasta, setHasta]         = useState(hoy);
  const [registros, setRegistros] = useState<ControlLeche[] | null>(null);
  const [buscando, setBuscando]   = useState(false);
  const [errBusq, setErrBusq]     = useState('');

  const [modal, setModal]       = useState(false);
  const [form, setForm]         = useState<RegistroForm>(REGISTRO_VACIO);
  const [guardando, setGuard]   = useState(false);
  const [errForm, setErrForm]   = useState('');

  // ── Lactancias ─────────────────────────────────────────────────
  const [lactanciaAnimal, setLactanciaAnimal]       = useState('');
  const [lactancias, setLactancias]                 = useState<ParametroLactancia[] | null>(null);
  const [buscandoLact, setBuscandoLact]             = useState(false);
  const [modalLact, setModalLact]                   = useState(false);
  const [formLact, setFormLact]                     = useState<LactanciaForm>(LACTANCIA_VACIA);
  const [guardLact, setGuardLact]                   = useState(false);
  const [errLact, setErrLact]                       = useState('');

  // ── Calidad ────────────────────────────────────────────────────
  const [calDesde, setCalDesde]                     = useState(hace30);
  const [calHasta, setCalHasta]                     = useState(hoy);
  const [calidades, setCalidades]                   = useState<CalidadLeche[] | null>(null);
  const [buscandoCal, setBuscandoCal]               = useState(false);
  const [errCal, setErrCal]                         = useState('');
  const [modalCal, setModalCal]                     = useState(false);
  const [formCal, setFormCal]                       = useState<CalidadForm>(CALIDAD_VACIA);
  const [guardCal, setGuardCal]                     = useState(false);
  const [errFormCal, setErrFormCal]                 = useState('');

  function set(f: keyof RegistroForm, v: string) { setForm(p => ({ ...p, [f]: v })); }

  // ── Handlers Control Diario ────────────────────────────────────
  async function buscar(e: FormEvent) {
    e.preventDefault();
    if (!idAnimal) return;
    setErrBusq(''); setBuscando(true); setRegistros(null);
    try {
      const lista = await api.get<ControlLeche[]>(
        `/control-leche/animal/${idAnimal}?desde=${desde}&hasta=${hasta}`,
      );
      setRegistros(lista);
    } catch (err: unknown) {
      setErrBusq(err instanceof Error ? err.message : 'Error al buscar');
    } finally { setBuscando(false); }
  }

  async function handleGuardar(e: FormEvent) {
    e.preventDefault();
    setErrForm(''); setGuard(true);
    try {
      const nuevo = await api.post<ControlLeche>('/control-leche', {
        idAnimal: Number(form.idAnimal),
        fecha:    form.fecha,
        maniana:  form.maniana ? Number(form.maniana) : undefined,
        tarde:    form.tarde   ? Number(form.tarde)   : undefined,
        noche:    form.noche   ? Number(form.noche)   : undefined,
        ordeno:   form.ordeno  || undefined,
      });
      if (String(nuevo.idAnimal) === idAnimal) {
        setRegistros(prev => prev ? [nuevo, ...prev] : [nuevo]);
      }
      setModal(false); setForm(REGISTRO_VACIO);
    } catch (err: unknown) {
      setErrForm(err instanceof Error ? err.message : 'Error al guardar');
    } finally { setGuard(false); }
  }

  // ── Handlers Lactancias ────────────────────────────────────────
  async function buscarLactancias(e: FormEvent) {
    e.preventDefault(); setBuscandoLact(true); setLactancias(null);
    try {
      const lista = await api.get<ParametroLactancia[]>(
        `/control-leche/lactancias/${lactanciaAnimal}`,
      );
      setLactancias(lista);
    } catch { setLactancias([]); }
    finally { setBuscandoLact(false); }
  }

  async function guardarLactancia(e: FormEvent) {
    e.preventDefault(); setErrLact(''); setGuardLact(true);
    try {
      const nueva = await api.post<ParametroLactancia>('/control-leche/lactancias', {
        idAnimal:    Number(formLact.idAnimal),
        numeroParto: Number(formLact.numeroParto),
        fechaInicio: formLact.fechaInicio,
      });
      if (formLact.idAnimal === lactanciaAnimal) {
        setLactancias(prev => prev ? [nueva, ...prev] : [nueva]);
      }
      setModalLact(false); setFormLact(LACTANCIA_VACIA);
    } catch (err: unknown) { setErrLact(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardLact(false); }
  }

  // ── Handlers Calidad ───────────────────────────────────────────
  async function buscarCalidad(e: FormEvent) {
    e.preventDefault(); setBuscandoCal(true); setCalidades(null); setErrCal('');
    try {
      const lista = await api.get<CalidadLeche[]>(
        `/control-leche/calidad?desde=${calDesde}&hasta=${calHasta}`,
      );
      setCalidades(lista);
    } catch (err: unknown) {
      setErrCal(err instanceof Error ? err.message : 'Error al buscar');
    } finally { setBuscandoCal(false); }
  }

  async function guardarCalidad(e: FormEvent) {
    e.preventDefault(); setErrFormCal(''); setGuardCal(true);
    try {
      const nuevo = await api.post<CalidadLeche>('/control-leche/calidad', {
        idAnimal:    formCal.idAnimal ? Number(formCal.idAnimal) : undefined,
        fecha:       formCal.fecha,
        celSomaticas: formCal.celSomaticas ? Number(formCal.celSomaticas) : undefined,
        grasaPct:    formCal.grasaPct    ? Number(formCal.grasaPct)    : undefined,
        proteinaPct: formCal.proteinaPct ? Number(formCal.proteinaPct) : undefined,
        laboratorio: formCal.laboratorio || undefined,
        resultado:   formCal.resultado   || undefined,
        observaciones: formCal.observaciones || undefined,
      });
      if (calidades) setCalidades(prev => prev ? [nuevo, ...prev] : [nuevo]);
      setModalCal(false); setFormCal(CALIDAD_VACIA);
    } catch (err: unknown) { setErrFormCal(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardCal(false); }
  }

  const totalPeriodo = registros?.reduce((a, r) => a + r.total, 0) ?? 0;
  const promediodia  = registros?.length ? (totalPeriodo / registros.length).toFixed(1) : '—';

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Producción de Leche</h1>
          <p className="text-slate-500 text-sm mt-1">Control diario, lactancias y calidad</p>
        </div>
        {tab === 'diario' && (
          <button className="btn-primary" onClick={() => setModal(true)}>+ Registrar control</button>
        )}
        {tab === 'lactancias' && (
          <button className="btn-primary" onClick={() => setModalLact(true)}>+ Iniciar lactancia</button>
        )}
        {tab === 'calidad' && (
          <button className="btn-primary" onClick={() => setModalCal(true)}>+ Registrar calidad</button>
        )}
      </div>

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

      {/* ── Tab Control Diario ────────────────────────────────── */}
      {tab === 'diario' && (
        <>
          <div className="card">
            <form onSubmit={buscar} className="flex flex-wrap gap-3 items-end">
              <div className="flex-1 min-w-[130px]">
                <label className="label">ID del animal</label>
                <input className="input" type="number" placeholder="Ej: 1" value={idAnimal}
                  onChange={e => setIdAnimal(e.target.value)} min={1} required />
              </div>
              <div className="flex-1 min-w-[130px]">
                <label className="label">Desde</label>
                <input className="input" type="date" value={desde} onChange={e => setDesde(e.target.value)} />
              </div>
              <div className="flex-1 min-w-[130px]">
                <label className="label">Hasta</label>
                <input className="input" type="date" value={hasta} onChange={e => setHasta(e.target.value)} />
              </div>
              <button type="submit" disabled={buscando} className="btn-primary h-[42px] px-6">
                {buscando ? 'Buscando…' : 'Consultar'}
              </button>
            </form>
          </div>

          {errBusq && (
            <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-4 text-sm">{errBusq}</div>
          )}

          {registros !== null && (
            <>
              <div className="grid grid-cols-3 gap-4">
                <div className="card text-center py-4">
                  <p className="text-2xl font-bold text-brand-700">{registros.length}</p>
                  <p className="text-xs text-slate-500 mt-1">Registros</p>
                </div>
                <div className="card text-center py-4">
                  <p className="text-2xl font-bold text-blue-700">{totalPeriodo.toFixed(1)} L</p>
                  <p className="text-xs text-slate-500 mt-1">Total período</p>
                </div>
                <div className="card text-center py-4">
                  <p className="text-2xl font-bold text-emerald-700">{promediodia} L</p>
                  <p className="text-xs text-slate-500 mt-1">Promedio / día</p>
                </div>
              </div>

              {registros.length === 0 ? (
                <div className="card text-center py-12 text-slate-400">
                  <p className="text-3xl mb-2">🥛</p>
                  <p>Sin registros para este período</p>
                </div>
              ) : (
                <div className="card overflow-x-auto p-0">
                  <table className="w-full text-sm">
                    <thead className="bg-slate-50 border-b border-slate-200">
                      <tr>
                        {['Fecha', 'Mañana (L)', 'Tarde (L)', 'Noche (L)', 'Total (L)'].map(h => (
                          <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold text-xs">{h}</th>
                        ))}
                      </tr>
                    </thead>
                    <tbody className="divide-y divide-slate-100">
                      {registros.map(r => (
                        <tr key={r.id} className="hover:bg-slate-50">
                          <td className="px-4 py-2.5 font-medium text-slate-700">
                            {new Date(r.fecha).toLocaleDateString('es-CO')}
                          </td>
                          <td className="px-4 py-2.5 text-slate-500">{r.maniana?.toFixed(1) ?? '—'}</td>
                          <td className="px-4 py-2.5 text-slate-500">{r.tarde?.toFixed(1)   ?? '—'}</td>
                          <td className="px-4 py-2.5 text-slate-500">{r.noche?.toFixed(1)   ?? '—'}</td>
                          <td className="px-4 py-2.5 font-semibold text-brand-700">{r.total.toFixed(1)}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
            </>
          )}
        </>
      )}

      {/* ── Tab Lactancias ────────────────────────────────────── */}
      {tab === 'lactancias' && (
        <div className="space-y-4">
          <div className="card">
            <p className="text-sm font-semibold text-slate-700 mb-3">Historial de lactancias por animal</p>
            <form onSubmit={buscarLactancias} className="flex gap-2">
              <input className="input flex-1" type="number" min="1" placeholder="ID del animal"
                value={lactanciaAnimal} onChange={e => setLactanciaAnimal(e.target.value)} required />
              <button type="submit" className="btn-primary px-4" disabled={buscandoLact}>
                {buscandoLact ? '…' : 'Consultar'}
              </button>
            </form>
          </div>
          {lactancias !== null && (
            lactancias.length === 0 ? (
              <div className="card text-center py-10 text-slate-400">Sin lactancias registradas para este animal</div>
            ) : (
              <div className="card overflow-x-auto p-0">
                <table className="w-full text-sm">
                  <thead className="bg-slate-50 border-b border-slate-200">
                    <tr>
                      {['Parto #', 'Fecha inicio', 'Fecha fin', 'Litros totales'].map(h => (
                        <th key={h} className="px-4 py-3 text-left text-xs font-semibold text-slate-600">{h}</th>
                      ))}
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {lactancias.map(l => (
                      <tr key={l.id} className="hover:bg-slate-50">
                        <td className="px-4 py-2.5 font-medium text-slate-800">{l.numeroParto}</td>
                        <td className="px-4 py-2.5 text-slate-700">
                          {new Date(l.fechaInicio).toLocaleDateString('es-CO')}
                        </td>
                        <td className="px-4 py-2.5 text-slate-500">
                          {l.fechaFin ? new Date(l.fechaFin).toLocaleDateString('es-CO') : (
                            <span className="bg-green-100 text-green-700 text-xs px-2 py-0.5 rounded-full">En curso</span>
                          )}
                        </td>
                        <td className="px-4 py-2.5 font-semibold text-brand-700">
                          {l.litrosTotales != null ? `${l.litrosTotales.toFixed(1)} L` : '—'}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )
          )}
        </div>
      )}

      {/* ── Tab Calidad ───────────────────────────────────────── */}
      {tab === 'calidad' && (
        <div className="space-y-4">
          <div className="card">
            <p className="text-sm font-semibold text-slate-700 mb-3">Consultar análisis por período</p>
            <form onSubmit={buscarCalidad} className="flex flex-wrap gap-3 items-end">
              <div className="flex-1 min-w-[130px]">
                <label className="label">Desde</label>
                <input className="input" type="date" value={calDesde} onChange={e => setCalDesde(e.target.value)} />
              </div>
              <div className="flex-1 min-w-[130px]">
                <label className="label">Hasta</label>
                <input className="input" type="date" value={calHasta} onChange={e => setCalHasta(e.target.value)} />
              </div>
              <button type="submit" className="btn-primary h-[42px] px-6" disabled={buscandoCal}>
                {buscandoCal ? 'Buscando…' : 'Consultar'}
              </button>
            </form>
            {errCal && <p className="text-red-600 text-sm mt-2">{errCal}</p>}
          </div>

          {calidades !== null && (
            calidades.length === 0 ? (
              <div className="card text-center py-10 text-slate-400">Sin análisis de calidad para este período</div>
            ) : (
              <div className="card overflow-x-auto p-0">
                <table className="w-full text-sm">
                  <thead className="bg-slate-50 border-b border-slate-200">
                    <tr>
                      {['Fecha', 'Animal', 'Cel. somáticas', 'Grasa %', 'Proteína %', 'Laboratorio', 'Resultado'].map(h => (
                        <th key={h} className="px-4 py-3 text-left text-xs font-semibold text-slate-600">{h}</th>
                      ))}
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {calidades.map(c => (
                      <tr key={c.id} className="hover:bg-slate-50">
                        <td className="px-4 py-2.5 text-slate-700">
                          {new Date(c.fecha).toLocaleDateString('es-CO')}
                        </td>
                        <td className="px-4 py-2.5 text-slate-500">{c.idAnimal ? `#${c.idAnimal}` : '—'}</td>
                        <td className="px-4 py-2.5 text-slate-500">{c.celSomaticas ?? '—'}</td>
                        <td className="px-4 py-2.5 text-slate-500">
                          {c.grasaPct != null ? `${c.grasaPct}%` : '—'}
                        </td>
                        <td className="px-4 py-2.5 text-slate-500">
                          {c.proteinaPct != null ? `${c.proteinaPct}%` : '—'}
                        </td>
                        <td className="px-4 py-2.5 text-slate-500">{c.laboratorio ?? '—'}</td>
                        <td className="px-4 py-2.5">
                          {c.resultado
                            ? <span className="text-xs px-2 py-0.5 rounded-full bg-blue-100 text-blue-700">
                                {c.resultado}
                              </span>
                            : <span className="text-slate-400">—</span>}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )
          )}
        </div>
      )}

      {/* ── Modal Registro Control Diario ─────────────────────── */}
      {modal && (
        <Modal titulo="Registrar control de leche"
          onClose={() => { setModal(false); setErrForm(''); setForm(REGISTRO_VACIO); }}>
          <form onSubmit={handleGuardar} className="space-y-4">
            {errForm && (
              <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errForm}</div>
            )}
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">ID Animal *</label>
                <input className="input" type="number" min="1" required value={form.idAnimal}
                  onChange={e => set('idAnimal', e.target.value)} placeholder="ID del animal" />
              </div>
              <div>
                <label className="label">Fecha *</label>
                <input className="input" type="date" required value={form.fecha}
                  onChange={e => set('fecha', e.target.value)} />
              </div>
            </div>
            <div>
              <p className="text-sm font-medium text-slate-700 mb-2">Litros por ordeño</p>
              <div className="grid grid-cols-3 gap-3">
                <div>
                  <label className="label text-xs">Mañana</label>
                  <input className="input" type="number" step="0.1" min="0" value={form.maniana}
                    onChange={e => set('maniana', e.target.value)} placeholder="0.0" />
                </div>
                <div>
                  <label className="label text-xs">Tarde</label>
                  <input className="input" type="number" step="0.1" min="0" value={form.tarde}
                    onChange={e => set('tarde', e.target.value)} placeholder="0.0" />
                </div>
                <div>
                  <label className="label text-xs">Noche</label>
                  <input className="input" type="number" step="0.1" min="0" value={form.noche}
                    onChange={e => set('noche', e.target.value)} placeholder="0.0" />
                </div>
              </div>
            </div>
            <div>
              <label className="label">Ordeñador</label>
              <input className="input" value={form.ordeno}
                onChange={e => set('ordeno', e.target.value)} placeholder="Nombre del ordeñador" />
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary"
                onClick={() => { setModal(false); setForm(REGISTRO_VACIO); }}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardando}>
                {guardando ? 'Guardando…' : 'Registrar control'}
              </button>
            </div>
          </form>
        </Modal>
      )}

      {/* ── Modal Iniciar Lactancia ───────────────────────────── */}
      {modalLact && (
        <Modal titulo="Iniciar período de lactancia"
          onClose={() => { setModalLact(false); setErrLact(''); setFormLact(LACTANCIA_VACIA); }}>
          <form onSubmit={guardarLactancia} className="space-y-4">
            {errLact && (
              <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errLact}</div>
            )}
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">ID Animal *</label>
                <input className="input" type="number" min="1" required value={formLact.idAnimal}
                  onChange={e => setFormLact(f => ({ ...f, idAnimal: e.target.value }))} />
              </div>
              <div>
                <label className="label">Número de parto *</label>
                <input className="input" type="number" min="1" required value={formLact.numeroParto}
                  onChange={e => setFormLact(f => ({ ...f, numeroParto: e.target.value }))} placeholder="Ej: 1" />
              </div>
            </div>
            <div>
              <label className="label">Fecha de inicio *</label>
              <input className="input" type="date" required value={formLact.fechaInicio}
                onChange={e => setFormLact(f => ({ ...f, fechaInicio: e.target.value }))} />
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary" onClick={() => setModalLact(false)}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardLact}>
                {guardLact ? 'Guardando…' : 'Iniciar lactancia'}
              </button>
            </div>
          </form>
        </Modal>
      )}

      {/* ── Modal Registrar Calidad ───────────────────────────── */}
      {modalCal && (
        <Modal titulo="Registrar análisis de calidad"
          onClose={() => { setModalCal(false); setErrFormCal(''); setFormCal(CALIDAD_VACIA); }}>
          <form onSubmit={guardarCalidad} className="space-y-4">
            {errFormCal && (
              <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errFormCal}</div>
            )}
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">ID Animal (opc.)</label>
                <input className="input" type="number" min="1" value={formCal.idAnimal}
                  onChange={e => setFormCal(f => ({ ...f, idAnimal: e.target.value }))} />
              </div>
              <div>
                <label className="label">Fecha *</label>
                <input className="input" type="date" required value={formCal.fecha}
                  onChange={e => setFormCal(f => ({ ...f, fecha: e.target.value }))} />
              </div>
            </div>
            <div className="grid grid-cols-3 gap-3">
              <div>
                <label className="label">Cel. somáticas</label>
                <input className="input" type="number" min="0" value={formCal.celSomaticas}
                  onChange={e => setFormCal(f => ({ ...f, celSomaticas: e.target.value }))} placeholder="×10³/ml" />
              </div>
              <div>
                <label className="label">Grasa %</label>
                <input className="input" type="number" step="0.01" min="0" value={formCal.grasaPct}
                  onChange={e => setFormCal(f => ({ ...f, grasaPct: e.target.value }))} />
              </div>
              <div>
                <label className="label">Proteína %</label>
                <input className="input" type="number" step="0.01" min="0" value={formCal.proteinaPct}
                  onChange={e => setFormCal(f => ({ ...f, proteinaPct: e.target.value }))} />
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Laboratorio</label>
                <input className="input" value={formCal.laboratorio}
                  onChange={e => setFormCal(f => ({ ...f, laboratorio: e.target.value }))} />
              </div>
              <div>
                <label className="label">Resultado</label>
                <input className="input" value={formCal.resultado}
                  onChange={e => setFormCal(f => ({ ...f, resultado: e.target.value }))}
                  placeholder="Ej: Apto, Rechazado" />
              </div>
            </div>
            <div>
              <label className="label">Observaciones</label>
              <textarea className="input" rows={2} value={formCal.observaciones}
                onChange={e => setFormCal(f => ({ ...f, observaciones: e.target.value }))} />
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary" onClick={() => setModalCal(false)}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardCal}>
                {guardCal ? 'Guardando…' : 'Registrar'}
              </button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
