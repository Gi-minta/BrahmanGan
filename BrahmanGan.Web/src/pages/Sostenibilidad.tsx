import { useEffect, useState, type FormEvent } from 'react';
import { api, type Finca } from '../api/client';
import Modal from '../components/Modal';

// ── Tipos ──────────────────────────────────────────────────────
type CapturaCarbono = { id: number; idFinca: number; anio: number; mes: number; emisionesGanadoTCO2?: number; capturaForestal?: number; huellaNeta: number; certificacion?: string; observaciones?: string };
type ConsumoAgua = { id: number; idFinca: number; idPotrero?: number; fecha: string; fuenteAgua?: string; volumenM3: number; numAnimales?: number; litrosAnimalDia?: number; observaciones?: string };
type EventoMedioambiental = { id: number; idFinca: number; fecha: string; tipoEvento: string; descripcion?: string; intensidad?: string; precipitacionMM?: number; tempMaxC?: number; tempMinC?: number; impactoEstimado?: string };

type Tab = 'carbono' | 'agua' | 'eventos';

const MESES = ['', 'Enero','Febrero','Marzo','Abril','Mayo','Junio','Julio','Agosto','Septiembre','Octubre','Noviembre','Diciembre'];

export default function SostenibilidadPage() {
  const [tab, setTab]             = useState<Tab>('carbono');
  const [fincas, setFincas]       = useState<Finca[]>([]);
  const [fincaSel, setFincaSel]   = useState('');
  const [capturas, setCapturas]   = useState<CapturaCarbono[]>([]);
  const [consumos, setConsumos]   = useState<ConsumoAgua[]>([]);
  const [eventos, setEventos]     = useState<EventoMedioambiental[]>([]);
  const [cargando, setCargando]   = useState(false);
  const [error, setError]         = useState('');
  const [modal, setModal]         = useState(false);
  const [guardando, setGuardando] = useState(false);
  const [errForm, setErrForm]     = useState('');

  const hoy = new Date();
  const hoyStr = hoy.toISOString().slice(0, 10);

  const [fCarb, setFCarb] = useState({ idFinca: '', anio: String(hoy.getFullYear()), mes: String(hoy.getMonth() + 1), emisiones: '', captura: '', observaciones: '' });
  const [fAgua, setFAgua] = useState({ idFinca: '', fecha: hoyStr, volumenM3: '', fuenteAgua: '', numAnimales: '', observaciones: '' });
  const [fEvento, setFEvento] = useState({ idFinca: '', fecha: hoyStr, tipoEvento: '', descripcion: '', intensidad: '', precipitacionMM: '', tempMaxC: '', tempMinC: '' });

  useEffect(() => {
    api.get<Finca[]>('/fincas').then(f => { setFincas(f); if (f.length > 0) setFincaSel(String(f[0].id)); }).catch(e => setError(e.message));
  }, []);

  useEffect(() => {
    if (!fincaSel) return;
    setCargando(true);
    const id = parseInt(fincaSel);
    Promise.all([
      api.get<CapturaCarbono[]>(`/sostenibilidad/carbono/finca/${id}`).then(setCapturas),
      api.get<ConsumoAgua[]>(`/sostenibilidad/agua/finca/${id}`).then(setConsumos),
      api.get<EventoMedioambiental[]>(`/sostenibilidad/eventos/finca/${id}`).then(setEventos),
    ]).catch(e => setError(e.message)).finally(() => setCargando(false));
  }, [fincaSel]);

  async function guardarCaptura(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<CapturaCarbono>('/sostenibilidad/carbono', {
        idFinca: parseInt(fCarb.idFinca || fincaSel), anio: parseInt(fCarb.anio), mes: parseInt(fCarb.mes),
        emisionesGanadoTCO2: fCarb.emisiones ? parseFloat(fCarb.emisiones) : null,
        capturaForestal: fCarb.captura ? parseFloat(fCarb.captura) : null,
        observaciones: fCarb.observaciones || null,
      });
      setCapturas(prev => [nuevo, ...prev]);
      setModal(false);
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  async function guardarAgua(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<ConsumoAgua>('/sostenibilidad/agua', {
        idFinca: parseInt(fAgua.idFinca || fincaSel), fecha: fAgua.fecha,
        volumenM3: parseFloat(fAgua.volumenM3),
        fuenteAgua: fAgua.fuenteAgua || null,
        numAnimales: fAgua.numAnimales ? parseInt(fAgua.numAnimales) : null,
        observaciones: fAgua.observaciones || null,
      });
      setConsumos(prev => [nuevo, ...prev]);
      setModal(false);
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  async function guardarEvento(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<EventoMedioambiental>('/sostenibilidad/eventos', {
        idFinca: parseInt(fEvento.idFinca || fincaSel), fecha: fEvento.fecha,
        tipoEvento: fEvento.tipoEvento,
        descripcion: fEvento.descripcion || null,
        intensidad: fEvento.intensidad || null,
        precipitacionMM: fEvento.precipitacionMM ? parseFloat(fEvento.precipitacionMM) : null,
        tempMaxC: fEvento.tempMaxC ? parseFloat(fEvento.tempMaxC) : null,
        tempMinC: fEvento.tempMinC ? parseFloat(fEvento.tempMinC) : null,
      });
      setEventos(prev => [nuevo, ...prev]);
      setModal(false);
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  const huellaTotalKg = capturas.reduce((s, c) => s + c.huellaNeta, 0);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Sostenibilidad</h1>
          <p className="text-slate-500 text-sm mt-1">Métricas ambientales: carbono, agua y eventos</p>
        </div>
        <button onClick={() => setModal(true)} className="btn-primary">+ Registrar</button>
      </div>

      {/* Selector finca */}
      <div className="flex items-center gap-3">
        <label className="text-sm text-slate-600">Finca</label>
        <select className="input-field w-56" value={fincaSel} onChange={e => setFincaSel(e.target.value)}>
          {fincas.map(f => <option key={f.id} value={f.id}>{f.nombre}</option>)}
        </select>
      </div>

      {/* KPIs */}
      {capturas.length > 0 && (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div className="card text-center">
            <p className="text-3xl font-bold text-slate-700">{huellaTotalKg.toFixed(1)}</p>
            <p className="text-slate-500 text-sm mt-1">t CO₂ eq. huella neta</p>
          </div>
          <div className="card text-center">
            <p className="text-3xl font-bold text-green-600">{capturas.reduce((s, c) => s + (c.capturaForestal ?? 0), 0).toFixed(1)}</p>
            <p className="text-slate-500 text-sm mt-1">t CO₂ capturadas (forestal)</p>
          </div>
          <div className="card text-center">
            <p className="text-3xl font-bold text-orange-500">{capturas.reduce((s, c) => s + (c.emisionesGanadoTCO2 ?? 0), 0).toFixed(1)}</p>
            <p className="text-slate-500 text-sm mt-1">t CO₂ emisiones ganaderas</p>
          </div>
        </div>
      )}

      {/* Tabs */}
      <div className="flex gap-1 bg-slate-100 p-1 rounded-xl w-fit">
        {(['carbono', 'agua', 'eventos'] as Tab[]).map(t => (
          <button key={t} onClick={() => setTab(t)}
            className={`px-5 py-2 rounded-lg text-sm font-medium transition-colors ${tab === t ? 'bg-white text-brand-700 shadow-sm' : 'text-slate-500 hover:text-slate-700'}`}>
            {t === 'carbono' ? '🌿 Carbono' : t === 'agua' ? '💧 Agua' : '🌦️ Eventos'}
          </button>
        ))}
      </div>

      {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{error}</div>}

      {cargando ? <div className="text-slate-400 text-center py-12">Cargando…</div> : (
        <>
          {tab === 'carbono' && (
            <div className="card overflow-x-auto p-0">
              <table className="w-full text-sm">
                <thead className="bg-slate-50 border-b border-slate-200">
                  <tr>{['Período','Emisiones (t CO₂)','Captura forestal','Huella neta','Obs.'].map(h => (
                    <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
                  ))}</tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {capturas.map(c => (
                    <tr key={c.id} className="hover:bg-slate-50">
                      <td className="px-4 py-3 font-medium text-slate-700">{MESES[c.mes]} {c.anio}</td>
                      <td className="px-4 py-3 font-mono text-orange-600">{c.emisionesGanadoTCO2?.toFixed(2) ?? '—'}</td>
                      <td className="px-4 py-3 font-mono text-green-600">{c.capturaForestal?.toFixed(2) ?? '—'}</td>
                      <td className={`px-4 py-3 font-mono font-semibold ${c.huellaNeta <= 0 ? 'text-green-600' : 'text-orange-600'}`}>{c.huellaNeta.toFixed(2)}</td>
                      <td className="px-4 py-3 text-slate-400 truncate max-w-[150px]">{c.observaciones ?? '—'}</td>
                    </tr>
                  ))}
                  {capturas.length === 0 && <tr><td colSpan={5} className="px-4 py-8 text-slate-400 text-center">Sin registros de carbono</td></tr>}
                </tbody>
              </table>
            </div>
          )}

          {tab === 'agua' && (
            <div className="card overflow-x-auto p-0">
              <table className="w-full text-sm">
                <thead className="bg-slate-50 border-b border-slate-200">
                  <tr>{['Fecha','Fuente','Volumen (m³)','Animales','L/animal/día','Obs.'].map(h => (
                    <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
                  ))}</tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {consumos.map(c => (
                    <tr key={c.id} className="hover:bg-slate-50">
                      <td className="px-4 py-3 text-slate-500">{c.fecha}</td>
                      <td className="px-4 py-3 text-slate-600">{c.fuenteAgua ?? '—'}</td>
                      <td className="px-4 py-3 font-mono text-blue-600">{c.volumenM3.toFixed(2)}</td>
                      <td className="px-4 py-3 font-mono">{c.numAnimales ?? '—'}</td>
                      <td className="px-4 py-3 font-mono">{c.litrosAnimalDia?.toFixed(1) ?? '—'}</td>
                      <td className="px-4 py-3 text-slate-400">{c.observaciones ?? '—'}</td>
                    </tr>
                  ))}
                  {consumos.length === 0 && <tr><td colSpan={6} className="px-4 py-8 text-slate-400 text-center">Sin registros de consumo de agua</td></tr>}
                </tbody>
              </table>
            </div>
          )}

          {tab === 'eventos' && (
            <div className="space-y-3">
              {eventos.map(e => (
                <div key={e.id} className="card">
                  <div className="flex items-start justify-between">
                    <div>
                      <div className="flex items-center gap-2">
                        <span className="font-semibold text-slate-800">{e.tipoEvento}</span>
                        {e.intensidad && <span className="text-xs bg-yellow-100 text-yellow-700 px-2 py-0.5 rounded-full">{e.intensidad}</span>}
                      </div>
                      <p className="text-sm text-slate-500 mt-0.5">{e.fecha}</p>
                      {e.descripcion && <p className="text-sm text-slate-600 mt-2">{e.descripcion}</p>}
                    </div>
                    <div className="text-right text-sm text-slate-400 space-y-0.5">
                      {e.precipitacionMM !== undefined && <p>🌧️ {e.precipitacionMM} mm</p>}
                      {e.tempMaxC !== undefined && <p>🌡️ {e.tempMinC}°/{e.tempMaxC}°C</p>}
                    </div>
                  </div>
                  {e.impactoEstimado && <p className="mt-2 text-xs text-slate-500 border-t border-slate-100 pt-2">Impacto: {e.impactoEstimado}</p>}
                </div>
              ))}
              {eventos.length === 0 && <div className="card text-slate-400 text-center py-8">Sin eventos registrados</div>}
            </div>
          )}
        </>
      )}

      {/* Modal dinámico por tab */}
      {modal && (
        <Modal titulo={tab === 'carbono' ? 'Registrar Captura de Carbono' : tab === 'agua' ? 'Registrar Consumo de Agua' : 'Registrar Evento Medioambiental'} onClose={() => setModal(false)}>
          {tab === 'carbono' && (
            <form onSubmit={guardarCaptura} className="space-y-4">
              {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
              <div className="grid grid-cols-2 gap-4">
                <div><label className="label">Año *</label>
                  <input type="number" className="input-field" value={fCarb.anio} onChange={e => setFCarb(p => ({...p, anio: e.target.value}))} required /></div>
                <div><label className="label">Mes *</label>
                  <select className="input-field" value={fCarb.mes} onChange={e => setFCarb(p => ({...p, mes: e.target.value}))}>
                    {MESES.slice(1).map((m, i) => <option key={i+1} value={i+1}>{m}</option>)}
                  </select></div>
                <div><label className="label">Emisiones ganado (t CO₂)</label>
                  <input type="number" min="0" step="0.01" className="input-field" value={fCarb.emisiones} onChange={e => setFCarb(p => ({...p, emisiones: e.target.value}))} /></div>
                <div><label className="label">Captura forestal (t CO₂)</label>
                  <input type="number" min="0" step="0.01" className="input-field" value={fCarb.captura} onChange={e => setFCarb(p => ({...p, captura: e.target.value}))} /></div>
              </div>
              <div><label className="label">Observaciones</label>
                <input className="input-field" value={fCarb.observaciones} onChange={e => setFCarb(p => ({...p, observaciones: e.target.value}))} /></div>
              <div className="flex gap-3 justify-end pt-2">
                <button type="button" onClick={() => setModal(false)} className="btn-secondary">Cancelar</button>
                <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Guardar'}</button>
              </div>
            </form>
          )}
          {tab === 'agua' && (
            <form onSubmit={guardarAgua} className="space-y-4">
              {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
              <div className="grid grid-cols-2 gap-4">
                <div><label className="label">Fecha *</label>
                  <input type="date" className="input-field" value={fAgua.fecha} onChange={e => setFAgua(p => ({...p, fecha: e.target.value}))} required /></div>
                <div><label className="label">Volumen (m³) *</label>
                  <input type="number" min="0" step="0.001" className="input-field" value={fAgua.volumenM3} onChange={e => setFAgua(p => ({...p, volumenM3: e.target.value}))} required /></div>
                <div><label className="label">Fuente de agua</label>
                  <input className="input-field" placeholder="Río, Pozo, Nacimiento…" value={fAgua.fuenteAgua} onChange={e => setFAgua(p => ({...p, fuenteAgua: e.target.value}))} /></div>
                <div><label className="label">Número de animales</label>
                  <input type="number" min="0" className="input-field" value={fAgua.numAnimales} onChange={e => setFAgua(p => ({...p, numAnimales: e.target.value}))} /></div>
              </div>
              <div><label className="label">Observaciones</label>
                <input className="input-field" value={fAgua.observaciones} onChange={e => setFAgua(p => ({...p, observaciones: e.target.value}))} /></div>
              <div className="flex gap-3 justify-end pt-2">
                <button type="button" onClick={() => setModal(false)} className="btn-secondary">Cancelar</button>
                <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Guardar'}</button>
              </div>
            </form>
          )}
          {tab === 'eventos' && (
            <form onSubmit={guardarEvento} className="space-y-4">
              {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
              <div className="grid grid-cols-2 gap-4">
                <div><label className="label">Fecha *</label>
                  <input type="date" className="input-field" value={fEvento.fecha} onChange={e => setFEvento(p => ({...p, fecha: e.target.value}))} required /></div>
                <div><label className="label">Tipo de evento *</label>
                  <select className="input-field" value={fEvento.tipoEvento} onChange={e => setFEvento(p => ({...p, tipoEvento: e.target.value}))} required>
                    <option value="">Seleccionar…</option>
                    <option value="Sequía">Sequía</option>
                    <option value="Inundación">Inundación</option>
                    <option value="Helada">Helada</option>
                    <option value="Ola de calor">Ola de calor</option>
                    <option value="Tormenta">Tormenta</option>
                    <option value="Plaga">Plaga</option>
                    <option value="Otro">Otro</option>
                  </select></div>
                <div><label className="label">Intensidad</label>
                  <select className="input-field" value={fEvento.intensidad} onChange={e => setFEvento(p => ({...p, intensidad: e.target.value}))}>
                    <option value="">Sin especificar</option>
                    <option value="Leve">Leve</option>
                    <option value="Moderada">Moderada</option>
                    <option value="Severa">Severa</option>
                    <option value="Extrema">Extrema</option>
                  </select></div>
                <div><label className="label">Precipitación (mm)</label>
                  <input type="number" min="0" step="0.1" className="input-field" value={fEvento.precipitacionMM} onChange={e => setFEvento(p => ({...p, precipitacionMM: e.target.value}))} /></div>
                <div><label className="label">Temp. min (°C)</label>
                  <input type="number" step="0.1" className="input-field" value={fEvento.tempMinC} onChange={e => setFEvento(p => ({...p, tempMinC: e.target.value}))} /></div>
                <div><label className="label">Temp. max (°C)</label>
                  <input type="number" step="0.1" className="input-field" value={fEvento.tempMaxC} onChange={e => setFEvento(p => ({...p, tempMaxC: e.target.value}))} /></div>
              </div>
              <div><label className="label">Descripción</label>
                <textarea className="input-field" rows={2} value={fEvento.descripcion} onChange={e => setFEvento(p => ({...p, descripcion: e.target.value}))} /></div>
              <div className="flex gap-3 justify-end pt-2">
                <button type="button" onClick={() => setModal(false)} className="btn-secondary">Cancelar</button>
                <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Registrar'}</button>
              </div>
            </form>
          )}
        </Modal>
      )}
    </div>
  );
}
