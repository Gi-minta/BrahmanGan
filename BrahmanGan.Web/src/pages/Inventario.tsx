import { useEffect, useState, type FormEvent } from 'react';
import { api, type Animal, type Raza, type Finca } from '../api/client';
import Modal from '../components/Modal';

const ESTADO_BADGE: Record<string, string> = {
  Activo:  'bg-green-100 text-green-700',
  Vendido: 'bg-blue-100 text-blue-700',
  Muerto:  'bg-red-100 text-red-700',
};

// ── Formulario nuevo animal ────────────────────────────────────
interface AnimalForm {
  codigo: string; nombre: string; sexo: 'M' | 'H';
  idRaza: string; idFinca: string;
  fechaNacimiento: string; pesoNacimiento: string;
  idMadre: string; idPadre: string;
  fechaIngreso: string; observaciones: string;
}

const FORM_VACIO: AnimalForm = {
  codigo: '', nombre: '', sexo: 'M',
  idRaza: '', idFinca: '',
  fechaNacimiento: '', pesoNacimiento: '',
  idMadre: '', idPadre: '',
  fechaIngreso: '', observaciones: '',
};

export default function InventarioPage() {
  const [animales, setAnimales] = useState<Animal[]>([]);
  const [razas, setRazas]       = useState<Raza[]>([]);
  const [fincas, setFincas]     = useState<Finca[]>([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError]       = useState('');

  const [modalAbierto, setModalAbierto] = useState(false);
  const [form, setForm]                 = useState<AnimalForm>(FORM_VACIO);
  const [guardando, setGuardando]       = useState(false);
  const [errForm, setErrForm]           = useState('');

  useEffect(() => {
    Promise.all([
      api.get<Animal[]>('/animales/activos'),
      api.get<Raza[]>('/razas'),
      api.get<Finca[]>('/fincas'),
    ])
      .then(([ants, rzs, fncs]) => { setAnimales(ants); setRazas(rzs); setFincas(fncs); })
      .catch(e => setError(e.message))
      .finally(() => setCargando(false));
  }, []);

  function set(field: keyof AnimalForm, value: string) {
    setForm(f => ({ ...f, [field]: value }));
  }

  async function handleGuardar(e: FormEvent) {
    e.preventDefault();
    setErrForm(''); setGuardando(true);
    try {
      const body = {
        codigo:          form.codigo,
        nombre:          form.nombre || undefined,
        sexo:            form.sexo,
        idRaza:          Number(form.idRaza),
        idFinca:         Number(form.idFinca),
        fechaNacimiento: form.fechaNacimiento || undefined,
        pesoNacimiento:  form.pesoNacimiento ? Number(form.pesoNacimiento) : undefined,
        idMadre:         form.idMadre ? Number(form.idMadre) : undefined,
        idPadre:         form.idPadre ? Number(form.idPadre) : undefined,
        fechaIngreso:    form.fechaIngreso || undefined,
        observaciones:   form.observaciones || undefined,
      };
      const nuevo = await api.post<Animal>('/animales', body);
      setAnimales(prev => [nuevo, ...prev]);
      setModalAbierto(false);
      setForm(FORM_VACIO);
    } catch (err: unknown) {
      setErrForm(err instanceof Error ? err.message : 'Error al guardar');
    } finally { setGuardando(false); }
  }

  return (
    <div className="space-y-6">
      {/* Cabecera */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Inventario Animal</h1>
          <p className="text-slate-500 text-sm mt-1">Registro y seguimiento del hato ganadero</p>
        </div>
        <button className="btn-primary" onClick={() => setModalAbierto(true)}>
          + Nuevo animal
        </button>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-4 text-sm">{error}</div>
      )}

      {/* Lista */}
      {cargando ? (
        <div className="text-slate-400 text-center py-16">Cargando animales…</div>
      ) : animales.length === 0 ? (
        <div className="card text-center py-16 text-slate-400">
          <p className="text-4xl mb-3">🐄</p>
          <p className="font-medium">No hay animales registrados</p>
          <p className="text-sm mt-1">Comienza registrando el primer animal del hato</p>
          <button className="btn-primary mt-4" onClick={() => setModalAbierto(true)}>
            + Registrar primer animal
          </button>
        </div>
      ) : (
        <div className="card overflow-x-auto p-0">
          <table className="w-full text-sm">
            <thead className="bg-slate-50 border-b border-slate-200">
              <tr>
                {['Código', 'Nombre', 'Sexo', 'Estado', 'Finca', 'Nacimiento'].map(h => (
                  <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
                ))}
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100">
              {animales.map(a => (
                <tr key={a.id} className="hover:bg-slate-50 transition-colors">
                  <td className="px-4 py-3 font-mono text-brand-700">{a.codigo}</td>
                  <td className="px-4 py-3">{a.nombre ?? '—'}</td>
                  <td className="px-4 py-3">
                    <span className={`px-2 py-0.5 rounded-full text-xs font-medium ${
                      a.sexo === 'M' ? 'bg-blue-100 text-blue-700' : 'bg-pink-100 text-pink-700'
                    }`}>
                      {a.sexo === 'M' ? '♂ Macho' : '♀ Hembra'}
                    </span>
                  </td>
                  <td className="px-4 py-3">
                    <span className={`px-2 py-0.5 rounded-full text-xs font-medium ${
                      ESTADO_BADGE[a.estado] ?? 'bg-slate-100 text-slate-600'
                    }`}>
                      {a.estado}
                    </span>
                  </td>
                  <td className="px-4 py-3 text-slate-500">{a.idFinca}</td>
                  <td className="px-4 py-3 text-slate-500">
                    {a.fechaNacimiento
                      ? new Date(a.fechaNacimiento).toLocaleDateString('es-CO')
                      : '—'}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Modal nuevo animal */}
      {modalAbierto && (
        <Modal titulo="Registrar nuevo animal" onClose={() => { setModalAbierto(false); setErrForm(''); setForm(FORM_VACIO); }}>
          <form onSubmit={handleGuardar} className="space-y-4">
            {errForm && (
              <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errForm}</div>
            )}

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Código *</label>
                <input className="input" value={form.codigo} onChange={e => set('codigo', e.target.value)} required placeholder="Ej: BOV-001" />
              </div>
              <div>
                <label className="label">Nombre</label>
                <input className="input" value={form.nombre} onChange={e => set('nombre', e.target.value)} placeholder="Opcional" />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Sexo *</label>
                <select className="input" value={form.sexo} onChange={e => set('sexo', e.target.value as 'M' | 'H')} required>
                  <option value="M">♂ Macho</option>
                  <option value="H">♀ Hembra</option>
                </select>
              </div>
              <div>
                <label className="label">Raza *</label>
                <select className="input" value={form.idRaza} onChange={e => set('idRaza', e.target.value)} required>
                  <option value="">Seleccionar…</option>
                  {razas.map(r => <option key={r.id} value={r.id}>{r.nombre}</option>)}
                </select>
              </div>
            </div>

            <div>
              <label className="label">Finca *</label>
              <select className="input" value={form.idFinca} onChange={e => set('idFinca', e.target.value)} required>
                <option value="">Seleccionar…</option>
                {fincas.map(f => <option key={f.id} value={f.id}>{f.nombre}</option>)}
              </select>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Fecha nacimiento</label>
                <input className="input" type="date" value={form.fechaNacimiento} onChange={e => set('fechaNacimiento', e.target.value)} />
              </div>
              <div>
                <label className="label">Peso nacimiento (kg)</label>
                <input className="input" type="number" step="0.1" min="0" value={form.pesoNacimiento} onChange={e => set('pesoNacimiento', e.target.value)} placeholder="Ej: 32.5" />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">ID Madre</label>
                <input className="input" type="number" min="1" value={form.idMadre} onChange={e => set('idMadre', e.target.value)} placeholder="ID del animal" />
              </div>
              <div>
                <label className="label">ID Padre</label>
                <input className="input" type="number" min="1" value={form.idPadre} onChange={e => set('idPadre', e.target.value)} placeholder="ID del animal" />
              </div>
            </div>

            <div>
              <label className="label">Fecha ingreso</label>
              <input className="input" type="date" value={form.fechaIngreso} onChange={e => set('fechaIngreso', e.target.value)} />
            </div>

            <div>
              <label className="label">Observaciones</label>
              <textarea className="input resize-none" rows={2} value={form.observaciones} onChange={e => set('observaciones', e.target.value)} placeholder="Notas adicionales…" />
            </div>

            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary" onClick={() => { setModalAbierto(false); setForm(FORM_VACIO); }}>
                Cancelar
              </button>
              <button type="submit" className="btn-primary" disabled={guardando}>
                {guardando ? 'Guardando…' : 'Registrar animal'}
              </button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
