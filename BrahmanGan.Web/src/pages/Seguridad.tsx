import { useEffect, useState } from 'react';
import { api } from '../api/client';
import Modal from '../components/Modal';
import { useAuth } from '../auth/useAuth';

interface RolResponse {
  id: number;
  nombre: string;
  descripcion: string;
  esSistema: boolean;
  activo: boolean;
  permisos: { id: number; modulo: string; accion: string; clave: string }[];
}

interface UsuarioResponse {
  id: number;
  email: string;
  nombreCompleto: string;
  proveedor: string;
  activo: boolean;
  fechaCreacion: string;
  ultimoAcceso: string | null;
  roles: string[];
}

type Tab = 'roles' | 'usuarios' | 'permisos';

export default function SeguridadPage() {
  const { tieneRol } = useAuth();
  const [tab, setTab] = useState<Tab>('roles');
  const [roles, setRoles]       = useState<RolResponse[]>([]);
  const [usuarios, setUsuarios] = useState<UsuarioResponse[]>([]);
  const [cargando, setCargando] = useState(false);
  const [error, setError]       = useState('');
  // Se incrementa tras crear o restablecer, para volver a pedir la lista.
  const [recarga, setRecarga]   = useState(0);

  const esAdmin = tieneRol('Administrador');

  useEffect(() => {
    if (!esAdmin) return;
    setCargando(true);
    setError('');

    // La pestaña de usuarios necesita también los roles: el alta pide elegir uno.
    const promesa = tab === 'roles'
      ? api.get<RolResponse[]>('/roles').then(r => { setRoles(r); })
      : tab === 'usuarios'
        ? Promise.all([
            api.get<UsuarioResponse[]>('/usuarios'),
            api.get<RolResponse[]>('/roles'),
          ]).then(([u, r]) => { setUsuarios(u); setRoles(r); })
        : api.get<RolResponse[]>('/roles').then(r => { setRoles(r); });

    promesa.catch(e => setError(e.message)).finally(() => setCargando(false));
  }, [tab, esAdmin, recarga]);

  if (!esAdmin) {
    return (
      <div className="card text-center py-16 text-slate-400">
        <p className="text-4xl mb-3">🔒</p>
        <p className="font-medium">Acceso restringido</p>
        <p className="text-sm mt-1">Solo los Administradores pueden gestionar seguridad</p>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-slate-800">🔐 Seguridad</h1>
        <p className="text-slate-500 text-sm mt-1">Gestión de roles, permisos y usuarios del sistema</p>
      </div>

      {/* Tabs */}
      <div className="flex gap-1 bg-slate-100 p-1 rounded-xl w-fit">
        {(['roles', 'usuarios', 'permisos'] as Tab[]).map(t => (
          <button
            key={t}
            onClick={() => setTab(t)}
            className={`px-5 py-2 rounded-lg text-sm font-medium transition-colors capitalize ${
              tab === t ? 'bg-white text-brand-700 shadow-sm' : 'text-slate-500 hover:text-slate-700'
            }`}
          >
            {t === 'roles' ? '🎭 Roles' : t === 'usuarios' ? '👤 Usuarios' : '🔑 Permisos'}
          </button>
        ))}
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-4 text-sm">{error}</div>
      )}

      {cargando ? (
        <div className="text-slate-400 text-center py-12">Cargando…</div>
      ) : tab === 'roles' ? (
        <RolesTab roles={roles} />
      ) : tab === 'usuarios' ? (
        <UsuariosTab usuarios={usuarios} roles={roles} onCambio={() => setRecarga(n => n + 1)} />
      ) : (
        <PermisosTab roles={roles} />
      )}
    </div>
  );
}

// ── Roles ──────────────────────────────────────────────────────
function RolesTab({ roles }: { roles: RolResponse[] }) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
      {roles.map(rol => (
        <div key={rol.id} className="card">
          <div className="flex items-start justify-between mb-3">
            <div>
              <p className="font-semibold text-slate-800">{rol.nombre}</p>
              <p className="text-slate-400 text-sm">{rol.descripcion}</p>
            </div>
            <div className="flex flex-col gap-1 items-end">
              {rol.esSistema && (
                <span className="bg-brand-100 text-brand-700 text-xs px-2 py-0.5 rounded-full">Sistema</span>
              )}
              <span className={`text-xs px-2 py-0.5 rounded-full ${
                rol.activo ? 'bg-green-100 text-green-700' : 'bg-slate-100 text-slate-500'
              }`}>
                {rol.activo ? 'Activo' : 'Inactivo'}
              </span>
            </div>
          </div>
          <p className="text-xs text-slate-400">
            {rol.permisos.length} permiso{rol.permisos.length !== 1 ? 's' : ''}
          </p>
          {rol.permisos.length > 0 && (
            <div className="mt-2 flex flex-wrap gap-1">
              {[...new Set(rol.permisos.map(p => p.modulo))].map(mod => (
                <span key={mod} className="bg-slate-100 text-slate-600 text-xs px-2 py-0.5 rounded">
                  {mod}
                </span>
              ))}
            </div>
          )}
        </div>
      ))}
    </div>
  );
}

// ── Usuarios ───────────────────────────────────────────────────
function UsuariosTab({ usuarios, roles, onCambio }: {
  usuarios: UsuarioResponse[];
  roles: RolResponse[];
  onCambio: () => void;
}) {
  const [modal, setModal]     = useState<'crear' | 'restablecer' | null>(null);
  const [destino, setDestino] = useState<UsuarioResponse | null>(null);
  const [form, setForm]       = useState({ email: '', nombreCompleto: '', password: '', rolId: 0 });
  const [msg, setMsg]         = useState('');
  const [err, setErr]         = useState('');
  const [enviando, setEnv]    = useState(false);

  function abrirCrear() {
    setForm({ email: '', nombreCompleto: '', password: '', rolId: roles[0]?.id ?? 0 });
    setErr(''); setMsg(''); setModal('crear');
  }

  function abrirRestablecer(u: UsuarioResponse) {
    setDestino(u);
    setForm(f => ({ ...f, password: '' }));
    setErr(''); setMsg(''); setModal('restablecer');
  }

  async function enviar() {
    setErr(''); setEnv(true);
    try {
      if (modal === 'crear') {
        await api.post('/usuarios', {
          email: form.email,
          nombreCompleto: form.nombreCompleto,
          passwordTemporal: form.password,
          rolId: form.rolId,
        });
        setMsg(`Usuario ${form.email} creado. Entrégale la contraseña temporal: deberá cambiarla al entrar.`);
      } else if (destino) {
        await api.post(`/usuarios/${destino.id}/restablecer-password`, {
          passwordTemporal: form.password,
        });
        setMsg(`Contraseña de ${destino.email} restablecida. Sus sesiones abiertas se cerraron.`);
      }
      setModal(null);
      onCambio();
    } catch (e: unknown) {
      setErr(e instanceof Error ? e.message : 'No se pudo completar la operación.');
    } finally {
      setEnv(false);
    }
  }

  return (
    <div className="space-y-3">
      {msg && (
        <div className="bg-green-50 border border-green-200 text-green-800 rounded-xl p-3 text-sm">
          {msg}
        </div>
      )}

      <div className="flex justify-end">
        <button onClick={abrirCrear} className="btn-primary text-sm px-4 py-2">
          Nuevo usuario
        </button>
      </div>

      {modal !== null && (
      <Modal onClose={() => setModal(null)}
        titulo={modal === 'crear' ? 'Nuevo usuario' : `Restablecer contraseña de ${destino?.email ?? ''}`}>
        <div className="space-y-3">
          {err && (
            <div className="bg-red-50 border border-red-200 text-red-700 rounded-xl p-3 text-sm">{err}</div>
          )}

          {modal === 'crear' && (
            <>
              <div>
                <label className="label">Correo electrónico</label>
                <input className="input" type="email" value={form.email}
                  onChange={e => setForm({ ...form, email: e.target.value })} />
              </div>
              <div>
                <label className="label">Nombre completo</label>
                <input className="input" type="text" value={form.nombreCompleto}
                  onChange={e => setForm({ ...form, nombreCompleto: e.target.value })} />
              </div>
              <div>
                <label className="label">Rol</label>
                <select className="input" value={form.rolId}
                  onChange={e => setForm({ ...form, rolId: Number(e.target.value) })}>
                  {roles.map(r => <option key={r.id} value={r.id}>{r.nombre}</option>)}
                </select>
              </div>
            </>
          )}

          <div>
            <label className="label">Contraseña temporal</label>
            <input className="input" type="text" value={form.password} minLength={8}
              onChange={e => setForm({ ...form, password: e.target.value })} />
            {/* Se muestra en claro a propósito: el administrador tiene que poder
                copiarla para entregársela al usuario. */}
            <p className="text-xs text-slate-500 mt-1">
              Mínimo 8 caracteres. El usuario deberá cambiarla la primera vez que entre.
            </p>
          </div>

          <div className="flex justify-end gap-2 pt-2">
            <button className="btn-secondary text-sm px-4 py-2" onClick={() => setModal(null)}>
              Cancelar
            </button>
            <button className="btn-primary text-sm px-4 py-2" disabled={enviando} onClick={enviar}>
              {enviando ? 'Guardando…' : 'Guardar'}
            </button>
          </div>
        </div>
      </Modal>
      )}

    <div className="card overflow-x-auto p-0">
      <table className="w-full text-sm">
        <thead className="bg-slate-50 border-b border-slate-200">
          <tr>
            {['Usuario','Email','Proveedor','Roles','Último acceso','Estado',''].map((h, i) => (
              <th key={i} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
            ))}
          </tr>
        </thead>
        <tbody className="divide-y divide-slate-100">
          {usuarios.map(u => (
            <tr key={u.id} className="hover:bg-slate-50 transition-colors">
              <td className="px-4 py-3 font-medium text-slate-800">{u.nombreCompleto}</td>
              <td className="px-4 py-3 text-slate-500">{u.email}</td>
              <td className="px-4 py-3">
                <span className={`px-2 py-0.5 rounded-full text-xs ${
                  u.proveedor === 'Google' ? 'bg-blue-100 text-blue-700' : 'bg-slate-100 text-slate-600'
                }`}>
                  {u.proveedor}
                </span>
              </td>
              <td className="px-4 py-3">
                <div className="flex flex-wrap gap-1">
                  {u.roles.map(r => (
                    <span key={r} className="bg-brand-100 text-brand-700 text-xs px-1.5 py-0.5 rounded">
                      {r}
                    </span>
                  ))}
                </div>
              </td>
              <td className="px-4 py-3 text-slate-400 text-xs">
                {u.ultimoAcceso
                  ? new Date(u.ultimoAcceso).toLocaleDateString('es-CO')
                  : 'Nunca'}
              </td>
              <td className="px-4 py-3">
                <span className={`px-2 py-0.5 rounded-full text-xs ${
                  u.activo ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
                }`}>
                  {u.activo ? 'Activo' : 'Inactivo'}
                </span>
              </td>
              <td className="px-4 py-3 text-right">
                {/* Los usuarios de Google no tienen contraseña local que restablecer. */}
                {u.proveedor !== 'Google' && (
                  <button onClick={() => abrirRestablecer(u)}
                    className="text-xs text-brand-700 hover:text-brand-900 font-medium">
                    Restablecer contraseña
                  </button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
    </div>
  );
}

// ── Permisos (matrix) ──────────────────────────────────────────
const MODULOS = ['Inventario','Finca','Reproduccion','Sanidad','Leche','Comercial',
                 'Costos','Nomina','Almacen','Equipos','Trazabilidad','Sostenibilidad','Seguridad','Reportes'];
const ACCIONES = ['Leer','Crear','Editar','Eliminar','Exportar','Administrar'];

function PermisosTab({ roles }: { roles: RolResponse[] }) {
  const [rolSeleccionado, setRolSeleccionado] = useState<RolResponse | null>(roles[0] ?? null);

  const permisoSet = new Set(rolSeleccionado?.permisos.map(p => p.clave) ?? []);

  return (
    <div className="space-y-4">
      {/* Selector de rol */}
      <div className="flex gap-2 flex-wrap">
        {roles.map(r => (
          <button
            key={r.id}
            onClick={() => setRolSeleccionado(r)}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
              rolSeleccionado?.id === r.id
                ? 'bg-brand-600 text-white'
                : 'bg-slate-100 text-slate-600 hover:bg-slate-200'
            }`}
          >
            {r.nombre}
          </button>
        ))}
      </div>

      {/* Matriz */}
      <div className="card overflow-x-auto p-0">
        <table className="w-full text-xs">
          <thead className="bg-slate-50 border-b border-slate-200">
            <tr>
              <th className="px-4 py-3 text-left text-slate-600 font-semibold">Módulo</th>
              {ACCIONES.map(a => (
                <th key={a} className="px-3 py-3 text-center text-slate-600 font-semibold">{a}</th>
              ))}
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100">
            {MODULOS.map(mod => (
              <tr key={mod} className="hover:bg-slate-50">
                <td className="px-4 py-2.5 font-medium text-slate-700">{mod}</td>
                {ACCIONES.map(acc => {
                  const clave = `${mod}:${acc}`;
                  const tiene = permisoSet.has(clave);
                  return (
                    <td key={acc} className="px-3 py-2.5 text-center">
                      <span className={`inline-flex items-center justify-center w-6 h-6 rounded-full text-sm ${
                        tiene ? 'bg-green-100 text-green-600' : 'bg-slate-100 text-slate-300'
                      }`}>
                        {tiene ? '✓' : '·'}
                      </span>
                    </td>
                  );
                })}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
