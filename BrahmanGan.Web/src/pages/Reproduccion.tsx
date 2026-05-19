import { useState, type FormEvent } from 'react';
import { api, type Servicio, type Gestacion, type Semen, type Nacimiento } from '../api/client';
import Modal from '../components/Modal';

type Tab = 'servicios' | 'semen' | 'nacimientos';
type TabRegistro = 'monta' | 'ia';
type TabConsulta = 'servicios' | 'gestacion';

const ESTADO_COLOR: Record<string, string> = {
  EnCurso:    'bg-blue-100 text-blue-700',
  Confirmada: 'bg-green-100 text-green-700',
  Parto:      'bg-emerald-100 text-emerald-700',
  Aborto:     'bg-red-100 text-red-700',
};

const hoy = new Date().toISOString().slice(0, 10);

interface SemenForm { codigo: string; nombreToro: string; idRaza: string; casa: string; stockDosis: string; }
const SEMEN_VACIO: SemenForm = { codigo: '', nombreToro: '', idRaza: '', casa: '', stockDosis: '' };

const TABS: { id: Tab; label: string }[] = [
  { id: 'servicios',   label: '🔬 Servicios' },
  { id: 'semen',       label: '🧊 Semen' },
  { id: 'nacimientos', label: '🐄 Nacimientos' },
];

export default function ReproduccionPage() {
  const [tab, setTab] = useState<Tab>('servicios');

  // ── Tabs internos ──────────────────────────────────────────────
  const [tabReg, setTabReg] = useState<TabRegistro>('monta');
  const [tabCon, setTabCon] = useState<TabConsulta>('servicios');

  // ── Monta Natural ─────────────────────────────────────────────
  const [monta, setMonta]       = useState({ idHembra: '', idToro: '', fecha: hoy, responsable: '' });
  const [guardMonta, setGuardM] = useState(false);
  const [okMonta, setOkMonta]   = useState('');
  const [errMonta, setErrMonta] = useState('');

  // ── IA ────────────────────────────────────────────────────────
  const [ia, setIa]           = useState({ idHembra: '', idSemen: '', fecha: hoy, responsable: '' });
  const [guardIa, setGuardI]  = useState(false);
  const [okIa, setOkIa]       = useState('');
  const [errIa, setErrIa]     = useState('');

  // ── Consulta servicios ─────────────────────────────────────────
  const [idHembra, setIdHembra]     = useState('');
  const [servicios, setServicios]   = useState<Servicio[] | null>(null);
  const [buscandoS, setBuscandoS]   = useState(false);
  const [errS, setErrS]             = useState('');

  // ── Consulta gestación ─────────────────────────────────────────
  const [idGest, setIdGest]         = useState('');
  const [gestacion, setGestacion]   = useState<Gestacion | null>(null);
  const [buscandoG, setBuscandoG]   = useState(false);
  const [errG, setErrG]             = useState('');

  // ── Semen ──────────────────────────────────────────────────────
  const [semenList, setSemenList]       = useState<Semen[] | null>(null);
  const [cargandoSemen, setCargSemen]   = useState(false);
  const [modalSemen, setModalSemen]     = useState(false);
  const [formSemen, setFormSemen]       = useState<SemenForm>(SEMEN_VACIO);
  const [guardSemen, setGuardSemen]     = useState(false);
  const [errSemen, setErrSemen]         = useState('');

  // ── Nacimientos ────────────────────────────────────────────────
  const [idGestNac, setIdGestNac]       = useState('');
  const [nacimientos, setNacimientos]   = useState<Nacimiento[] | null>(null);
  const [buscandoNac, setBuscandoNac]   = useState(false);
  const [errNac, setErrNac]             = useState('');

  // ── Handlers servicios ─────────────────────────────────────────
  async function registrarMonta(e: FormEvent) {
    e.preventDefault();
    setErrMonta(''); setOkMonta(''); setGuardM(true);
    try {
      await api.post('/servicios-reproductivos/monta', {
        idHembra:    Number(monta.idHembra),
        idToro:      Number(monta.idToro),
        fecha:       monta.fecha,
        responsable: monta.responsable || undefined,
      });
      setOkMonta('Monta registrada correctamente');
      setMonta({ idHembra: '', idToro: '', fecha: hoy, responsable: '' });
    } catch (err: unknown) {
      setErrMonta(err instanceof Error ? err.message : 'Error al registrar');
    } finally { setGuardM(false); }
  }

  async function registrarIa(e: FormEvent) {
    e.preventDefault();
    setErrIa(''); setOkIa(''); setGuardI(true);
    try {
      await api.post('/servicios-reproductivos/ia', {
        idHembra:    Number(ia.idHembra),
        idSemen:     Number(ia.idSemen),
        fecha:       ia.fecha,
        responsable: ia.responsable || undefined,
      });
      setOkIa('Inseminación registrada correctamente');
      setIa({ idHembra: '', idSemen: '', fecha: hoy, responsable: '' });
    } catch (err: unknown) {
      setErrIa(err instanceof Error ? err.message : 'Error al registrar');
    } finally { setGuardI(false); }
  }

  async function buscarServicios(e: FormEvent) {
    e.preventDefault();
    setErrS(''); setBuscandoS(true); setServicios(null);
    try {
      const lista = await api.get<Servicio[]>(`/servicios-reproductivos/hembra/${idHembra}`);
      setServicios(lista);
    } catch (err: unknown) {
      setErrS(err instanceof Error ? err.message : 'Error al buscar');
    } finally { setBuscandoS(false); }
  }

  async function buscarGestacion(e: FormEvent) {
    e.preventDefault();
    setErrG(''); setBuscandoG(true); setGestacion(null);
    try {
      const g = await api.get<Gestacion>(`/gestaciones/${idGest}`);
      setGestacion(g);
    } catch (err: unknown) {
      setErrG(err instanceof Error ? err.message : 'Gestación no encontrada');
    } finally { setBuscandoG(false); }
  }

  // ── Handlers semen ─────────────────────────────────────────────
  async function cargarSemen() {
    if (semenList !== null) return;
    setCargSemen(true);
    try {
      const lista = await api.get<Semen[]>('/servicios-reproductivos/semen');
      setSemenList(lista);
    } catch { setSemenList([]); }
    finally { setCargSemen(false); }
  }

  async function guardarSemen(e: FormEvent) {
    e.preventDefault(); setErrSemen(''); setGuardSemen(true);
    try {
      const nuevo = await api.post<Semen>('/servicios-reproductivos/semen', {
        codigo:     formSemen.codigo,
        nombreToro: formSemen.nombreToro,
        idRaza:     formSemen.idRaza ? Number(formSemen.idRaza) : undefined,
        casa:       formSemen.casa || undefined,
        stockDosis: Number(formSemen.stockDosis),
      });
      setSemenList(prev => prev ? [nuevo, ...prev] : [nuevo]);
      setModalSemen(false); setFormSemen(SEMEN_VACIO);
    } catch (err: unknown) { setErrSemen(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardSemen(false); }
  }

  // ── Handlers nacimientos ───────────────────────────────────────
  async function buscarNacimientos(e: FormEvent) {
    e.preventDefault(); setBuscandoNac(true); setNacimientos(null);
    try {
      const lista = await api.get<Nacimiento[]>(
        `/servicios-reproductivos/nacimientos/gestacion/${idGestNac}`,
      );
      setNacimientos(lista);
    } catch (err: unknown) {
      setErrNac(err instanceof Error ? err.message : 'Error al buscar');
      setNacimientos([]);
    } finally { setBuscandoNac(false); }
  }

  function switchTab(t: Tab) {
    setTab(t);
    if (t === 'semen') cargarSemen();
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Reproducción</h1>
          <p className="text-slate-500 text-sm mt-1">Servicios reproductivos, inventario de semen y nacimientos</p>
        </div>
        {tab === 'semen' && (
          <button className="btn-primary" onClick={() => setModalSemen(true)}>+ Registrar semen</button>
        )}
      </div>

      {/* Tabs */}
      <div className="flex border-b border-slate-200 gap-1">
        {TABS.map(t => (
          <button key={t.id} onClick={() => switchTab(t.id)}
            className={`px-4 py-2.5 text-sm font-medium border-b-2 transition-colors -mb-px ${
              tab === t.id
                ? 'border-brand-600 text-brand-700'
                : 'border-transparent text-slate-500 hover:text-slate-700'
            }`}>
            {t.label}
          </button>
        ))}
      </div>

      {/* ── Tab Servicios ──────────────────────────────────────── */}
      {tab === 'servicios' && (
        <div className="grid grid-cols-1 xl:grid-cols-2 gap-6">
          {/* Panel registro */}
          <div className="card space-y-4">
            <p className="font-semibold text-slate-800">Registrar servicio</p>
            <div className="flex rounded-lg overflow-hidden border border-slate-200">
              {(['monta', 'ia'] as TabRegistro[]).map(t => (
                <button key={t} onClick={() => setTabReg(t)}
                  className={`flex-1 py-2 text-sm font-medium transition-colors ${
                    tabReg === t ? 'bg-brand-600 text-white' : 'text-slate-600 hover:bg-slate-50'
                  }`}>
                  {t === 'monta' ? '🐂 Monta Natural' : '🧪 Inseminación Artificial'}
                </button>
              ))}
            </div>

            {tabReg === 'monta' && (
              <form onSubmit={registrarMonta} className="space-y-3">
                {errMonta && <p className="text-red-600 text-sm bg-red-50 rounded-lg p-2">{errMonta}</p>}
                {okMonta  && <p className="text-green-700 text-sm bg-green-50 rounded-lg p-2">✓ {okMonta}</p>}
                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="label">ID Hembra *</label>
                    <input className="input" type="number" min="1" required value={monta.idHembra}
                      onChange={e => setMonta(m => ({ ...m, idHembra: e.target.value }))} placeholder="ID del animal" />
                  </div>
                  <div>
                    <label className="label">ID Toro *</label>
                    <input className="input" type="number" min="1" required value={monta.idToro}
                      onChange={e => setMonta(m => ({ ...m, idToro: e.target.value }))} placeholder="ID del animal" />
                  </div>
                </div>
                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="label">Fecha *</label>
                    <input className="input" type="date" required value={monta.fecha}
                      onChange={e => setMonta(m => ({ ...m, fecha: e.target.value }))} />
                  </div>
                  <div>
                    <label className="label">Responsable</label>
                    <input className="input" value={monta.responsable}
                      onChange={e => setMonta(m => ({ ...m, responsable: e.target.value }))} placeholder="Nombre" />
                  </div>
                </div>
                <button type="submit" className="btn-primary w-full" disabled={guardMonta}>
                  {guardMonta ? 'Registrando…' : 'Registrar monta'}
                </button>
              </form>
            )}

            {tabReg === 'ia' && (
              <form onSubmit={registrarIa} className="space-y-3">
                {errIa && <p className="text-red-600 text-sm bg-red-50 rounded-lg p-2">{errIa}</p>}
                {okIa  && <p className="text-green-700 text-sm bg-green-50 rounded-lg p-2">✓ {okIa}</p>}
                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="label">ID Hembra *</label>
                    <input className="input" type="number" min="1" required value={ia.idHembra}
                      onChange={e => setIa(i => ({ ...i, idHembra: e.target.value }))} placeholder="ID del animal" />
                  </div>
                  <div>
                    <label className="label">ID Semen *</label>
                    <input className="input" type="number" min="1" required value={ia.idSemen}
                      onChange={e => setIa(i => ({ ...i, idSemen: e.target.value }))} placeholder="ID del semen" />
                  </div>
                </div>
                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="label">Fecha *</label>
                    <input className="input" type="date" required value={ia.fecha}
                      onChange={e => setIa(i => ({ ...i, fecha: e.target.value }))} />
                  </div>
                  <div>
                    <label className="label">Responsable</label>
                    <input className="input" value={ia.responsable}
                      onChange={e => setIa(i => ({ ...i, responsable: e.target.value }))} placeholder="Nombre" />
                  </div>
                </div>
                <button type="submit" className="btn-primary w-full" disabled={guardIa}>
                  {guardIa ? 'Registrando…' : 'Registrar inseminación'}
                </button>
              </form>
            )}
          </div>

          {/* Panel consulta */}
          <div className="card space-y-4">
            <p className="font-semibold text-slate-800">Consultar historial</p>
            <div className="flex rounded-lg overflow-hidden border border-slate-200">
              {(['servicios', 'gestacion'] as TabConsulta[]).map(t => (
                <button key={t} onClick={() => setTabCon(t)}
                  className={`flex-1 py-2 text-sm font-medium transition-colors ${
                    tabCon === t ? 'bg-brand-600 text-white' : 'text-slate-600 hover:bg-slate-50'
                  }`}>
                  {t === 'servicios' ? '🔬 Servicios' : '🤰 Gestación'}
                </button>
              ))}
            </div>

            {tabCon === 'servicios' && (
              <div className="space-y-3">
                <form onSubmit={buscarServicios} className="flex gap-2">
                  <input className="input flex-1" type="number" min="1" placeholder="ID de la hembra"
                    value={idHembra} onChange={e => setIdHembra(e.target.value)} required />
                  <button type="submit" disabled={buscandoS} className="btn-primary px-4">
                    {buscandoS ? '…' : 'Buscar'}
                  </button>
                </form>
                {errS && <p className="text-red-600 text-sm">{errS}</p>}
                {servicios !== null && (
                  servicios.length === 0 ? (
                    <p className="text-slate-400 text-sm text-center py-4">Sin servicios para esta hembra</p>
                  ) : (
                    <div className="overflow-x-auto">
                      <table className="w-full text-sm">
                        <thead>
                          <tr className="border-b border-slate-200">
                            {['Fecha', 'Tipo', 'Resultado', 'Confirmación'].map(h => (
                              <th key={h} className="px-2 py-2 text-left text-xs font-semibold text-slate-500">{h}</th>
                            ))}
                          </tr>
                        </thead>
                        <tbody className="divide-y divide-slate-100">
                          {servicios.map(s => (
                            <tr key={s.id}>
                              <td className="px-2 py-2 text-slate-700">
                                {new Date(s.fecha).toLocaleDateString('es-CO')}
                              </td>
                              <td className="px-2 py-2">
                                <span className="px-2 py-0.5 rounded-full text-xs font-medium bg-purple-100 text-purple-700">
                                  {s.tipoServicio}
                                </span>
                              </td>
                              <td className="px-2 py-2 text-xs">
                                {s.resultadoPreniez == null
                                  ? <span className="text-slate-400">Pendiente</span>
                                  : s.resultadoPreniez
                                    ? <span className="text-green-600 font-medium">Preñada ✓</span>
                                    : <span className="text-red-500 font-medium">Negativo ✗</span>}
                              </td>
                              <td className="px-2 py-2 text-slate-500 text-xs">
                                {s.fechaConfirmacion
                                  ? new Date(s.fechaConfirmacion).toLocaleDateString('es-CO') : '—'}
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

            {tabCon === 'gestacion' && (
              <div className="space-y-3">
                <form onSubmit={buscarGestacion} className="flex gap-2">
                  <input className="input flex-1" type="number" min="1" placeholder="ID de gestación"
                    value={idGest} onChange={e => setIdGest(e.target.value)} required />
                  <button type="submit" disabled={buscandoG} className="btn-primary px-4">
                    {buscandoG ? '…' : 'Buscar'}
                  </button>
                </form>
                {errG && <p className="text-red-600 text-sm">{errG}</p>}
                {gestacion && (
                  <div className="bg-slate-50 rounded-lg p-4 space-y-2 text-sm">
                    {[
                      ['Animal', `#${gestacion.idAnimal}`],
                      ['Inicio gestación', new Date(gestacion.fechaInicio).toLocaleDateString('es-CO')],
                      ['Parto estimado', gestacion.fechaPartoEstimado
                        ? new Date(gestacion.fechaPartoEstimado).toLocaleDateString('es-CO') : '—'],
                      ['Parto real', gestacion.fechaPartoReal
                        ? new Date(gestacion.fechaPartoReal).toLocaleDateString('es-CO') : '—'],
                    ].map(([k, v]) => (
                      <div key={k} className="flex justify-between">
                        <span className="text-slate-500">{k}</span>
                        <span className="font-medium">{v}</span>
                      </div>
                    ))}
                    <div className="flex justify-between">
                      <span className="text-slate-500">Estado</span>
                      <span className={`px-2 py-0.5 rounded-full text-xs font-medium ${
                        ESTADO_COLOR[gestacion.estado] ?? 'bg-slate-100 text-slate-600'
                      }`}>{gestacion.estado}</span>
                    </div>
                  </div>
                )}
              </div>
            )}
          </div>
        </div>
      )}

      {/* ── Tab Semen ──────────────────────────────────────────── */}
      {tab === 'semen' && (
        <div className="space-y-4">
          {cargandoSemen ? (
            <div className="text-slate-400 text-center py-12">Cargando inventario…</div>
          ) : !semenList || semenList.length === 0 ? (
            <div className="card text-center py-12 text-slate-400">
              <p className="text-3xl mb-2">🧊</p>
              <p>Sin registros de semen</p>
            </div>
          ) : (
            <div className="card overflow-x-auto p-0">
              <table className="w-full text-sm">
                <thead className="bg-slate-50 border-b border-slate-200">
                  <tr>
                    {['Código', 'Toro', 'Casa', 'Stock dosis', 'Activo'].map(h => (
                      <th key={h} className="px-4 py-3 text-left text-xs font-semibold text-slate-600">{h}</th>
                    ))}
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {semenList.map(s => (
                    <tr key={s.id} className={`hover:bg-slate-50 ${!s.activo ? 'opacity-50' : ''}`}>
                      <td className="px-4 py-2.5 font-mono text-brand-700 text-xs">{s.codigo}</td>
                      <td className="px-4 py-2.5 font-medium text-slate-800">{s.nombreToro}</td>
                      <td className="px-4 py-2.5 text-slate-500">{s.casa ?? '—'}</td>
                      <td className="px-4 py-2.5">
                        <span className={`font-bold ${s.stockDosis <= 5 ? 'text-red-600' : 'text-slate-800'}`}>
                          {s.stockDosis}
                        </span>
                      </td>
                      <td className="px-4 py-2.5">
                        <span className={`text-xs px-2 py-0.5 rounded-full font-medium ${
                          s.activo ? 'bg-green-100 text-green-700' : 'bg-slate-100 text-slate-500'
                        }`}>
                          {s.activo ? 'Activo' : 'Inactivo'}
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      )}

      {/* ── Tab Nacimientos ────────────────────────────────────── */}
      {tab === 'nacimientos' && (
        <div className="space-y-4">
          <div className="card">
            <p className="text-sm font-semibold text-slate-700 mb-3">Nacimientos por gestación</p>
            <form onSubmit={buscarNacimientos} className="flex gap-2">
              <input className="input flex-1" type="number" min="1" placeholder="ID de gestación"
                value={idGestNac} onChange={e => setIdGestNac(e.target.value)} required />
              <button type="submit" disabled={buscandoNac} className="btn-primary px-4">
                {buscandoNac ? '…' : 'Consultar'}
              </button>
            </form>
            {errNac && <p className="text-red-600 text-sm mt-2">{errNac}</p>}
          </div>
          {nacimientos !== null && (
            nacimientos.length === 0 ? (
              <div className="card text-center py-10 text-slate-400">Sin nacimientos para esta gestación</div>
            ) : (
              <div className="card overflow-x-auto p-0">
                <table className="w-full text-sm">
                  <thead className="bg-slate-50 border-b border-slate-200">
                    <tr>
                      {['Fecha', 'Cría #', 'Sexo', 'Peso nac.', 'Condición', 'Observaciones'].map(h => (
                        <th key={h} className="px-4 py-3 text-left text-xs font-semibold text-slate-600">{h}</th>
                      ))}
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {nacimientos.map(n => (
                      <tr key={n.id} className="hover:bg-slate-50">
                        <td className="px-4 py-2.5 text-slate-700">
                          {new Date(n.fecha).toLocaleDateString('es-CO')}
                        </td>
                        <td className="px-4 py-2.5 text-slate-500">{n.idAnimalCria ? `#${n.idAnimalCria}` : '—'}</td>
                        <td className="px-4 py-2.5 text-slate-500">{n.sexo ?? '—'}</td>
                        <td className="px-4 py-2.5 text-slate-500">
                          {n.pesoNacimiento ? `${n.pesoNacimiento} kg` : '—'}
                        </td>
                        <td className="px-4 py-2.5 text-slate-500">{n.condicion ?? '—'}</td>
                        <td className="px-4 py-2.5 text-slate-400 text-xs">{n.observaciones ?? '—'}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )
          )}
        </div>
      )}

      {/* ── Modal Nuevo Semen ─────────────────────────────────── */}
      {modalSemen && (
        <Modal titulo="Registrar semen"
          onClose={() => { setModalSemen(false); setErrSemen(''); setFormSemen(SEMEN_VACIO); }}>
          <form onSubmit={guardarSemen} className="space-y-4">
            {errSemen && (
              <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errSemen}</div>
            )}
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Código *</label>
                <input className="input" required value={formSemen.codigo}
                  onChange={e => setFormSemen(f => ({ ...f, codigo: e.target.value }))} placeholder="Ej: SEM-001" />
              </div>
              <div>
                <label className="label">Nombre del toro *</label>
                <input className="input" required value={formSemen.nombreToro}
                  onChange={e => setFormSemen(f => ({ ...f, nombreToro: e.target.value }))} />
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Casa comercial</label>
                <input className="input" value={formSemen.casa}
                  onChange={e => setFormSemen(f => ({ ...f, casa: e.target.value }))} />
              </div>
              <div>
                <label className="label">Stock inicial (dosis) *</label>
                <input className="input" type="number" min="0" required value={formSemen.stockDosis}
                  onChange={e => setFormSemen(f => ({ ...f, stockDosis: e.target.value }))} />
              </div>
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary" onClick={() => setModalSemen(false)}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardSemen}>
                {guardSemen ? 'Guardando…' : 'Registrar'}
              </button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
