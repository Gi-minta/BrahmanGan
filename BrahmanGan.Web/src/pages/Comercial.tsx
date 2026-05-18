import { useEffect, useState, type FormEvent } from 'react';
import { api, type Cliente } from '../api/client';
import Modal from '../components/Modal';

interface ClienteForm {
  documento: string; tipoDocumento: string; razonSocial: string;
  contacto: string; telefono: string; email: string; direccion: string; tipoCliente: string;
}
const FORM_VACIO: ClienteForm = {
  documento: '', tipoDocumento: 'NIT', razonSocial: '',
  contacto: '', telefono: '', email: '', direccion: '', tipoCliente: '',
};

export default function ComercialPage() {
  const [clientes, setClientes]   = useState<Cliente[]>([]);
  const [cargando, setCargando]   = useState(true);
  const [error, setError]         = useState('');
  const [filtro, setFiltro]       = useState('');

  const [modal, setModal]       = useState(false);
  const [form, setForm]         = useState<ClienteForm>(FORM_VACIO);
  const [guardando, setGuard]   = useState(false);
  const [errForm, setErrForm]   = useState('');

  useEffect(() => {
    api.get<Cliente[]>('/clientes')
      .then(setClientes)
      .catch(e => setError(e.message))
      .finally(() => setCargando(false));
  }, []);

  function set(f: keyof ClienteForm, v: string) { setForm(p => ({ ...p, [f]: v })); }

  async function handleGuardar(e: FormEvent) {
    e.preventDefault();
    setErrForm(''); setGuard(true);
    try {
      const nuevo = await api.post<Cliente>('/clientes', {
        documento:     form.documento,
        tipoDocumento: form.tipoDocumento,
        razonSocial:   form.razonSocial,
        contacto:      form.contacto   || undefined,
        telefono:      form.telefono   || undefined,
        email:         form.email      || undefined,
        direccion:     form.direccion  || undefined,
        tipoCliente:   form.tipoCliente || undefined,
      });
      setClientes(prev => [...prev, nuevo]);
      setModal(false);
      setForm(FORM_VACIO);
    } catch (err: unknown) {
      setErrForm(err instanceof Error ? err.message : 'Error al guardar');
    } finally { setGuard(false); }
  }

  const clientesFiltrados = clientes.filter(c =>
    filtro === '' ||
    c.razonSocial.toLowerCase().includes(filtro.toLowerCase()) ||
    c.documento.includes(filtro)
  );

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Comercial</h1>
          <p className="text-slate-500 text-sm mt-1">Gestión de clientes y relaciones comerciales</p>
        </div>
        <button className="btn-primary" onClick={() => setModal(true)}>
          + Nuevo cliente
        </button>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-4 text-sm">{error}</div>
      )}

      {!cargando && clientes.length > 0 && (
        <input className="input max-w-xs" placeholder="Buscar por nombre o documento…"
          value={filtro} onChange={e => setFiltro(e.target.value)} />
      )}

      {cargando ? (
        <div className="text-slate-400 text-center py-16">Cargando clientes…</div>
      ) : clientes.length === 0 ? (
        <div className="card text-center py-16 text-slate-400">
          <p className="text-4xl mb-3">💼</p>
          <p className="font-medium">No hay clientes registrados</p>
          <button className="btn-primary mt-4" onClick={() => setModal(true)}>
            + Registrar primer cliente
          </button>
        </div>
      ) : (
        <div className="card overflow-x-auto p-0">
          <table className="w-full text-sm">
            <thead className="bg-slate-50 border-b border-slate-200">
              <tr>
                {['Documento', 'Tipo', 'Razón social / Nombre', 'Contacto', 'Teléfono', 'Email', 'Estado'].map(h => (
                  <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold text-xs">{h}</th>
                ))}
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100">
              {clientesFiltrados.map(c => (
                <tr key={c.id} className="hover:bg-slate-50 transition-colors">
                  <td className="px-4 py-3 font-mono text-slate-700 text-xs">{c.documento}</td>
                  <td className="px-4 py-3">
                    <span className="px-2 py-0.5 rounded text-xs bg-slate-100 text-slate-600">
                      {c.tipoDocumento}
                    </span>
                  </td>
                  <td className="px-4 py-3 font-medium text-slate-800">{c.razonSocial}</td>
                  <td className="px-4 py-3 text-slate-500">{c.contacto ?? '—'}</td>
                  <td className="px-4 py-3 text-slate-500">{c.telefono ?? '—'}</td>
                  <td className="px-4 py-3 text-slate-500 text-xs">{c.email ?? '—'}</td>
                  <td className="px-4 py-3">
                    <span className={`px-2 py-0.5 rounded-full text-xs font-medium ${
                      c.activo ? 'bg-green-100 text-green-700' : 'bg-slate-100 text-slate-500'
                    }`}>
                      {c.activo ? 'Activo' : 'Inactivo'}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {clientesFiltrados.length === 0 && filtro && (
            <p className="text-center text-slate-400 text-sm py-8">Sin resultados para "{filtro}"</p>
          )}
        </div>
      )}

      {modal && (
        <Modal titulo="Registrar nuevo cliente" onClose={() => { setModal(false); setErrForm(''); setForm(FORM_VACIO); }}>
          <form onSubmit={handleGuardar} className="space-y-4">
            {errForm && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errForm}</div>}

            <div>
              <label className="label">Razón social / Nombre *</label>
              <input className="input" required value={form.razonSocial}
                onChange={e => set('razonSocial', e.target.value)} placeholder="Ej: Lácteos del Valle S.A.S" />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Tipo documento *</label>
                <select className="input" required value={form.tipoDocumento}
                  onChange={e => set('tipoDocumento', e.target.value)}>
                  <option value="NIT">NIT</option>
                  <option value="CC">Cédula de ciudadanía</option>
                  <option value="CE">Cédula de extranjería</option>
                  <option value="PASAPORTE">Pasaporte</option>
                </select>
              </div>
              <div>
                <label className="label">Número documento *</label>
                <input className="input" required value={form.documento}
                  onChange={e => set('documento', e.target.value)} placeholder="Ej: 900123456-1" />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Contacto</label>
                <input className="input" value={form.contacto}
                  onChange={e => set('contacto', e.target.value)} placeholder="Nombre del contacto" />
              </div>
              <div>
                <label className="label">Tipo cliente</label>
                <select className="input" value={form.tipoCliente}
                  onChange={e => set('tipoCliente', e.target.value)}>
                  <option value="">Sin especificar</option>
                  <option value="Planta">Planta procesadora</option>
                  <option value="Distribuidor">Distribuidor</option>
                  <option value="Particular">Particular</option>
                </select>
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Teléfono</label>
                <input className="input" type="tel" value={form.telefono}
                  onChange={e => set('telefono', e.target.value)} placeholder="Ej: 3001234567" />
              </div>
              <div>
                <label className="label">Email</label>
                <input className="input" type="email" value={form.email}
                  onChange={e => set('email', e.target.value)} placeholder="correo@ejemplo.com" />
              </div>
            </div>

            <div>
              <label className="label">Dirección</label>
              <input className="input" value={form.direccion}
                onChange={e => set('direccion', e.target.value)} placeholder="Ciudad / dirección" />
            </div>

            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary"
                onClick={() => { setModal(false); setForm(FORM_VACIO); }}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardando}>
                {guardando ? 'Guardando…' : 'Registrar cliente'}
              </button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
