import { useState } from 'react';
import { api } from '../api/client';
import { useAuth } from '../auth/useAuth';

/**
 * Diálogo bloqueante que aparece cuando la contraseña actual la fijó un administrador.
 * No se puede cerrar: mientras siga siendo temporal, el usuario no debería trabajar con
 * una credencial que un tercero conoce.
 */
export default function CambioPasswordObligatorio() {
  const { logout } = useAuth();
  const [actual, setActual]   = useState('');
  const [nueva, setNueva]     = useState('');
  const [confirm, setConfirm] = useState('');
  const [error, setError]     = useState('');
  const [enviando, setEnv]    = useState(false);

  async function enviar() {
    setError('');
    if (nueva !== confirm) { setError('Las contraseñas no coinciden.'); return; }
    if (nueva.length < 8)  { setError('La nueva contraseña debe tener al menos 8 caracteres.'); return; }

    setEnv(true);
    try {
      await api.post('/auth/cambiar-password', {
        passwordActual: actual,
        nuevoPassword: nueva,
        confirmarNuevoPassword: confirm,
      });
      // El flag vive dentro del token, así que hay que pedir uno nuevo: se cierra sesión
      // para que el siguiente inicio traiga ya la marca apagada.
      await logout();
    } catch (e: unknown) {
      setError(e instanceof Error ? e.message : 'No se pudo cambiar la contraseña.');
      setEnv(false);
    }
  }

  return (
    <div className="fixed inset-0 z-50 bg-slate-900/70 backdrop-blur-sm flex items-center justify-center p-4">
      <div className="bg-white rounded-2xl shadow-modal w-full max-w-md p-6 space-y-4">
        <div>
          <h2 className="text-lg font-bold text-slate-900">Cambia tu contraseña</h2>
          <p className="text-sm text-slate-500 mt-1">
            Tu contraseña actual la definió un administrador. Elige una propia para continuar.
          </p>
        </div>

        {error && (
          <div className="bg-red-50 border border-red-200 text-red-700 rounded-xl p-3 text-sm">{error}</div>
        )}

        <div>
          <label className="label">Contraseña temporal</label>
          <input className="input" type="password" value={actual}
            onChange={e => setActual(e.target.value)} autoComplete="current-password" />
        </div>
        <div>
          <label className="label">Nueva contraseña</label>
          <input className="input" type="password" value={nueva} minLength={8}
            onChange={e => setNueva(e.target.value)} autoComplete="new-password" />
        </div>
        <div>
          <label className="label">Confirmar nueva contraseña</label>
          <input className="input" type="password" value={confirm} minLength={8}
            onChange={e => setConfirm(e.target.value)} autoComplete="new-password" />
        </div>

        <div className="flex justify-between items-center pt-2">
          {/* Única salida: no hay «cancelar», pero tampoco se deja al usuario atrapado. */}
          <button className="text-sm text-slate-500 hover:text-slate-700" onClick={logout}>
            Cerrar sesión
          </button>
          <button className="btn-primary text-sm px-5 py-2" disabled={enviando} onClick={enviar}>
            {enviando ? 'Guardando…' : 'Cambiar contraseña'}
          </button>
        </div>
      </div>
    </div>
  );
}
