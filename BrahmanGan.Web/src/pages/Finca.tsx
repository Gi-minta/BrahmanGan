import { useEffect, useState, type FormEvent } from 'react';
import { api, type Finca, type Potrero } from '../api/client';
import Modal from '../components/Modal';

// ── Formularios ────────────────────────────────────────────────
interface FincaForm {
  nombre: string; nit: string; propietario: string;
  direccion: string; telefono: string; email: string; areaHectareas: string;
}
const FINCA_VACIA: FincaForm = { nombre: '', nit: '', propietario: '', direccion: '', telefono: '', email: '', areaHectareas: '' };

interface PotreroForm { codigo: string; nombre: string; areaHectareas: string; tipoPasto: string; }
const POTRERO_VACIO: PotreroForm = { codigo: '', nombre: '', areaHectareas: '', tipoPasto: '' };

export default function FincaPage() {
  const [fincas, setFincas]         = useState<Finca[]>([]);
  const [potreros, setPotreros]     = useState<Record<number, Potrero[]>>({});
  const [expandida, setExpandida]   = useState<number | null>(null);
  const [cargando, setCargando]     = useState(true);
  const [error, setError]           = useState('');

  // Modal nueva finca
  const [modalFinca, setModalFinca]     = useState(false);
  const [formFinca, setFormFinca]       = useState<FincaForm>(FINCA_VACIA);
  const [guardandoFinca, setGuardFinca] = useState(false);
  const [errFinca, setErrFinca]         = useState('');

  // Formulario inline potrero
  const [potreroFincaId, setPotreroFincaId] = useState<number | null>(null);
  const [formPotrero, setFormPotrero]       = useState<PotreroForm>(POTRERO_VACIO);
  const [guardandoPot, setGuardPot]         = useState(false);
  const [errPotrero, setErrPotrero]         = useState('');

  useEffect(() => {
    api.get<Finca[]>('/fincas')
      .then(setFincas)
      .catch(e => setError(e.message))
      .finally(() => setCargando(false));
  }, []);

  async function toggleFinca(id: number) {
    if (expandida === id) { setExpandida(null); return; }
    setExpandida(id);
    setPotreroFincaId(null);
    if (potreros[id] !== undefined) return;
    try {
      const lista = await api.get<Potrero[]>(`/potreros/finca/${id}`);
      setPotreros(prev => ({ ...prev, [id]: lista }));
    } catch {
      setPotreros(prev => ({ ...prev, [id]: [] }));
    }
  }

  // ── Guardar finca ──────────────────────────────────────────
  async function handleGuardarFinca(e: FormEvent) {
    e.preventDefault();
    setErrFinca(''); setGuardFinca(true);
    try {
      const body = {
        nombre:        formFinca.nombre,
        nit:           formFinca.nit || undefined,
        propietario:   formFinca.propietario || undefined,
        direccion:     formFinca.direccion || undefined,
        telefono:      formFinca.telefono || undefined,
        email:         formFinca.email || undefined,
        areaHectareas: formFinca.areaHectareas ? Number(formFinca.areaHectareas) : undefined,
      };
      const nueva = await api.post<Finca>('/fincas', body);
      setFincas(prev => [...prev, nueva]);
      setModalFinca(false);
      setFormFinca(FINCA_VACIA);
    } catch (err: unknown) {
      setErrFinca(err instanceof Error ? err.message : 'Error al guardar');
    } finally { setGuardFinca(false); }
  }

  // ── Guardar potrero ────────────────────────────────────────
  async function handleGuardarPotrero(e: FormEvent, idFinca: number) {
    e.preventDefault();
    setErrPotrero(''); setGuardPot(true);
    try {
      const body = {
        idFinca,
        codigo:        formPotrero.codigo,
        nombre:        formPotrero.nombre,
        areaHectareas: formPotrero.areaHectareas ? Number(formPotrero.areaHectareas) : undefined,
        tipoPasto:     formPotrero.tipoPasto || undefined,
      };
      const nuevo = await api.post<Potrero>('/potreros', body);
      setPotreros(prev => ({ ...prev, [idFinca]: [...(prev[idFinca] ?? []), nuevo] }));
      setPotreroFincaId(null);
      setFormPotrero(POTRERO_VACIO);
    } catch (err: unknown) {
      setErrPotrero(err instanceof Error ? err.message : 'Error al guardar');
    } finally { setGuardPot(false); }
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Finca &amp; Potreros</h1>
          <p className="text-slate-500 text-sm mt-1">Gestión de fincas y división en potreros</p>
        </div>
        <button className="btn-primary" onClick={() => setModalFinca(true)}>
          + Nueva finca
        </button>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-4 text-sm">{error}</div>
      )}

      {cargando ? (
        <div className="text-slate-400 text-center py-16">Cargando fincas…</div>
      ) : fincas.length === 0 ? (
        <div className="card text-center py-16 text-slate-400">
          <p className="text-4xl mb-3">🌿</p>
          <p className="font-medium">No hay fincas registradas</p>
          <button className="btn-primary mt-4" onClick={() => setModalFinca(true)}>
            + Registrar primera finca
          </button>
        </div>
      ) : (
        <div className="space-y-3">
          {fincas.map(f => (
            <div key={f.id} className="card p-0 overflow-hidden">
              {/* Cabecera finca */}
              <button
                onClick={() => toggleFinca(f.id)}
                className="w-full flex items-center justify-between px-5 py-4 hover:bg-slate-50 transition-colors text-left"
              >
                <div className="flex items-center gap-4">
                  <span className="text-2xl">🌿</span>
                  <div>
                    <p className="font-semibold text-slate-800">{f.nombre}</p>
                    <p className="text-sm text-slate-500">
                      {f.propietario && <span className="mr-3">{f.propietario}</span>}
                      {f.nit && <span className="mr-3">NIT: {f.nit}</span>}
                      {f.areaHectareas != null && <span>{f.areaHectareas} ha</span>}
                    </p>
                  </div>
                </div>
                <div className="flex items-center gap-3">
                  <span className={`px-2 py-0.5 rounded-full text-xs font-medium ${
                    f.activa ? 'bg-green-100 text-green-700' : 'bg-slate-100 text-slate-500'
                  }`}>
                    {f.activa ? 'Activa' : 'Inactiva'}
                  </span>
                  <svg className={`w-4 h-4 text-slate-400 transition-transform ${expandida === f.id ? 'rotate-180' : ''}`}
                    fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={2}>
                    <path strokeLinecap="round" strokeLinejoin="round" d="M19 9l-7 7-7-7" />
                  </svg>
                </div>
              </button>

              {/* Panel de potreros */}
              {expandida === f.id && (
                <div className="border-t border-slate-100 bg-slate-50 px-5 py-4 space-y-4">
                  <div className="flex items-center justify-between">
                    <p className="text-xs font-semibold text-slate-500 uppercase tracking-wide">Potreros</p>
                    <button
                      className="text-xs text-brand-600 hover:text-brand-700 font-medium"
                      onClick={() => { setPotreroFincaId(potreroFincaId === f.id ? null : f.id); setErrPotrero(''); setFormPotrero(POTRERO_VACIO); }}
                    >
                      {potreroFincaId === f.id ? 'Cancelar' : '+ Nuevo potrero'}
                    </button>
                  </div>

                  {/* Formulario nuevo potrero */}
                  {potreroFincaId === f.id && (
                    <form onSubmit={e => handleGuardarPotrero(e, f.id)} className="bg-white rounded-lg border border-slate-200 p-4 space-y-3">
                      {errPotrero && <p className="text-red-600 text-xs">{errPotrero}</p>}
                      <div className="grid grid-cols-2 gap-3">
                        <div>
                          <label className="label text-xs">Código *</label>
                          <input className="input text-sm" value={formPotrero.codigo} onChange={e => setFormPotrero(p => ({ ...p, codigo: e.target.value }))} required placeholder="Ej: P-01" />
                        </div>
                        <div>
                          <label className="label text-xs">Nombre *</label>
                          <input className="input text-sm" value={formPotrero.nombre} onChange={e => setFormPotrero(p => ({ ...p, nombre: e.target.value }))} required placeholder="Ej: El Palmar" />
                        </div>
                        <div>
                          <label className="label text-xs">Área (ha)</label>
                          <input className="input text-sm" type="number" step="0.1" min="0" value={formPotrero.areaHectareas} onChange={e => setFormPotrero(p => ({ ...p, areaHectareas: e.target.value }))} placeholder="Ej: 12.5" />
                        </div>
                        <div>
                          <label className="label text-xs">Tipo de pasto</label>
                          <input className="input text-sm" value={formPotrero.tipoPasto} onChange={e => setFormPotrero(p => ({ ...p, tipoPasto: e.target.value }))} placeholder="Ej: Brachiaria" />
                        </div>
                      </div>
                      <div className="flex justify-end gap-2">
                        <button type="button" className="btn-secondary text-sm py-1.5" onClick={() => setPotreroFincaId(null)}>Cancelar</button>
                        <button type="submit" className="btn-primary text-sm py-1.5" disabled={guardandoPot}>
                          {guardandoPot ? 'Guardando…' : 'Agregar potrero'}
                        </button>
                      </div>
                    </form>
                  )}

                  {/* Lista de potreros */}
                  {!potreros[f.id] ? (
                    <p className="text-slate-400 text-sm">Cargando…</p>
                  ) : potreros[f.id].length === 0 ? (
                    <p className="text-slate-400 text-sm">Sin potreros registrados</p>
                  ) : (
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-2">
                      {potreros[f.id].map(p => (
                        <div key={p.id} className="bg-white rounded-lg border border-slate-200 px-4 py-3">
                          <p className="font-medium text-slate-800 text-sm">{p.nombre}</p>
                          <p className="text-xs text-slate-500 mt-0.5">
                            <span className="font-mono mr-2">{p.codigo}</span>
                            {p.areaHectareas != null && <span>{p.areaHectareas} ha</span>}
                            {p.tipoPasto && <span className="ml-2 text-green-600">{p.tipoPasto}</span>}
                          </p>
                        </div>
                      ))}
                    </div>
                  )}
                </div>
              )}
            </div>
          ))}
        </div>
      )}

      {/* Modal nueva finca */}
      {modalFinca && (
        <Modal titulo="Registrar nueva finca" onClose={() => { setModalFinca(false); setErrFinca(''); setFormFinca(FINCA_VACIA); }}>
          <form onSubmit={handleGuardarFinca} className="space-y-4">
            {errFinca && (
              <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errFinca}</div>
            )}
            <div>
              <label className="label">Nombre de la finca *</label>
              <input className="input" value={formFinca.nombre} onChange={e => setFormFinca(f => ({ ...f, nombre: e.target.value }))} required placeholder="Ej: La Esperanza" />
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">NIT</label>
                <input className="input" value={formFinca.nit} onChange={e => setFormFinca(f => ({ ...f, nit: e.target.value }))} placeholder="Ej: 900123456-1" />
              </div>
              <div>
                <label className="label">Propietario</label>
                <input className="input" value={formFinca.propietario} onChange={e => setFormFinca(f => ({ ...f, propietario: e.target.value }))} placeholder="Nombre completo" />
              </div>
            </div>
            <div>
              <label className="label">Dirección</label>
              <input className="input" value={formFinca.direccion} onChange={e => setFormFinca(f => ({ ...f, direccion: e.target.value }))} placeholder="Vereda / municipio" />
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Teléfono</label>
                <input className="input" type="tel" value={formFinca.telefono} onChange={e => setFormFinca(f => ({ ...f, telefono: e.target.value }))} placeholder="Ej: 3001234567" />
              </div>
              <div>
                <label className="label">Email</label>
                <input className="input" type="email" value={formFinca.email} onChange={e => setFormFinca(f => ({ ...f, email: e.target.value }))} placeholder="correo@ejemplo.com" />
              </div>
            </div>
            <div>
              <label className="label">Área total (hectáreas)</label>
              <input className="input" type="number" step="0.1" min="0" value={formFinca.areaHectareas} onChange={e => setFormFinca(f => ({ ...f, areaHectareas: e.target.value }))} placeholder="Ej: 250.5" />
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary" onClick={() => { setModalFinca(false); setFormFinca(FINCA_VACIA); }}>
                Cancelar
              </button>
              <button type="submit" className="btn-primary" disabled={guardandoFinca}>
                {guardandoFinca ? 'Guardando…' : 'Registrar finca'}
              </button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
