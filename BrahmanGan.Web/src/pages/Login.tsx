import { useState, type FormEvent } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { Eye, EyeOff, Eraser } from 'lucide-react';
import { useAuth } from '../auth/useAuth';

export default function LoginPage() {
  const { login, loginOAuth } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const from = (location.state as { from?: Location })?.from?.pathname ?? '/';

  const [email, setEmail]     = useState('');
  const [password, setPass]   = useState('');
  const [error, setError]     = useState('');
  const [cargando, setCarg]   = useState(false);
  const [verPass, setVerPass] = useState(false);

  /** Vacía el formulario y el mensaje de error, sin cambiar de pestaña. */
  function limpiarCampos() {
    setEmail('');
    setPass('');
    setError('');
    setVerPass(false);
  }

  const hayAlgoEscrito = Boolean(email || password);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setError('');
    setCarg(true);
    try {
      await login(email, password);
      navigate(from, { replace: true });
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'Error de autenticación');
    } finally {
      setCarg(false);
    }
  }

  async function handleGoogle() {
    setError('');
    setCarg(true);
    try {
      await loginOAuth('google');
      navigate(from, { replace: true });
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'No se pudo iniciar sesión con Google.');
    } finally {
      setCarg(false);
    }
  }

  return (
    <div className="min-h-screen relative flex items-center justify-center p-4 overflow-hidden bg-gradient-to-br from-brand-800 via-brand-700 to-brand-500">

      {/* Decorative blobs */}
      <div className="absolute top-[-20%] right-[-10%] w-[600px] h-[600px] rounded-full bg-white/5 blur-3xl pointer-events-none" />
      <div className="absolute bottom-[-15%] left-[-10%] w-[500px] h-[500px] rounded-full bg-brand-900/40 blur-3xl pointer-events-none" />

      {/* Subtle grid overlay */}
      <div
        className="absolute inset-0 opacity-[0.03] pointer-events-none"
        style={{
          backgroundImage: `url("data:image/svg+xml,%3Csvg width='60' height='60' viewBox='0 0 60 60' xmlns='http://www.w3.org/2000/svg'%3E%3Cg fill='none' fill-rule='evenodd'%3E%3Cg fill='%23ffffff' fill-opacity='1'%3E%3Cpath d='M36 34v-4h-2v4h-4v2h4v4h2v-4h4v-2h-4zm0-30V0h-2v4h-4v2h4v4h2V6h4V4h-4zM6 34v-4H4v4H0v2h4v4h2v-4h4v-2H6zM6 4V0H4v4H0v2h4v4h2V6h4V4H6z'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E")`,
        }}
      />

      <div className="w-full max-w-md relative z-10 animate-fade-in-up">
        {/* Logo / título */}
        <div className="text-center mb-8">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-white/15 backdrop-blur-sm rounded-2xl shadow-lg mb-4 border border-white/20">
            <span className="text-3xl">🐄</span>
          </div>
          <h1 className="text-3xl font-bold text-white tracking-tight">BrahmanGan</h1>
          <p className="text-brand-200 text-sm mt-1 font-medium">ERP Ganadero · Sistema de Gestión</p>
        </div>

        {/* Card */}
        <div className="bg-white rounded-3xl shadow-modal overflow-hidden border border-white/10">
          {/* El alta de usuarios la hace un administrador desde Seguridad, así que
              aquí solo se inicia sesión. */}
          <div className="bg-slate-50 border-b border-slate-100 py-3.5">
            <p className="text-center text-sm font-semibold text-brand-700">Iniciar sesión</p>
          </div>

          <form onSubmit={handleSubmit} className="p-8 space-y-4">
            {error && (
              <div className="bg-red-50 border border-red-200 text-red-700 rounded-xl p-3 text-sm">
                {error}
              </div>
            )}

            <div>
              <label className="label">Correo electrónico</label>
              <input
                className="input"
                type="email"
                placeholder="usuario@ejemplo.com"
                value={email}
                onChange={e => setEmail(e.target.value)}
                required
                autoComplete="email"
              />
            </div>

            <div>
              <label className="label">Contraseña</label>
              <div className="relative">
                <input
                  className="input pr-11"
                  type={verPass ? 'text' : 'password'}
                  placeholder="••••••••"
                  value={password}
                  onChange={e => setPass(e.target.value)}
                  required
                  minLength={8}
                  autoComplete="current-password"
                />
                <button
                  type="button"
                  onClick={() => setVerPass(v => !v)}
                  // aria-label y aria-pressed para que un lector de pantalla anuncie
                  // el estado; el icono por sí solo no comunica nada.
                  aria-label={verPass ? 'Ocultar contraseña' : 'Mostrar contraseña'}
                  aria-pressed={verPass}
                  title={verPass ? 'Ocultar contraseña' : 'Mostrar contraseña'}
                  className="absolute inset-y-0 right-0 flex items-center px-3 text-slate-400 hover:text-slate-600 transition-colors rounded-r-xl focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-500"
                >
                  {verPass ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                </button>
              </div>
            </div>

            <button
              type="submit"
              disabled={cargando}
              className="w-full py-3 bg-gradient-to-b from-brand-500 to-brand-700 text-white font-semibold rounded-xl shadow-md shadow-brand-600/30 hover:from-brand-600 hover:to-brand-800 hover:shadow-lg hover:shadow-brand-600/40 active:scale-[0.99] transition-all duration-150 disabled:opacity-60 disabled:cursor-not-allowed mt-2"
            >
              {cargando ? (
                <span className="flex items-center justify-center gap-2">
                  <svg className="animate-spin w-4 h-4" viewBox="0 0 24 24" fill="none">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
                  </svg>
                  Procesando…
                </span>
              ) : 'Ingresar'}
            </button>

            <button
              type="button"
              onClick={limpiarCampos}
              // Deshabilitado si no hay nada que limpiar, para no ofrecer una acción
              // que no haría nada.
              disabled={cargando || !hayAlgoEscrito}
              className="w-full flex items-center justify-center gap-2 py-2 text-sm text-slate-500 font-medium rounded-xl hover:bg-slate-50 hover:text-slate-700 transition-all duration-150 disabled:opacity-40 disabled:cursor-not-allowed disabled:hover:bg-transparent"
            >
              <Eraser className="w-4 h-4" />
              Limpiar campos
            </button>

            <div className="flex items-center gap-3">
              <hr className="flex-1 border-slate-200" />
              <span className="text-slate-400 text-xs">o continúa con</span>
              <hr className="flex-1 border-slate-200" />
            </div>

            <button
              type="button"
              onClick={handleGoogle}
              className="w-full flex items-center justify-center gap-3 py-2.5 border border-slate-200 rounded-xl text-slate-700 font-medium hover:bg-slate-50 hover:border-slate-300 transition-all duration-150 shadow-sm"
            >
              <svg className="w-5 h-5" viewBox="0 0 24 24">
                <path fill="#4285F4" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"/>
                <path fill="#34A853" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"/>
                <path fill="#FBBC05" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l3.66-2.84z"/>
                <path fill="#EA4335" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"/>
              </svg>
              Continuar con Google
            </button>
          </form>
        </div>

        <p className="text-center text-white/50 text-xs mt-6 font-medium">
          © {new Date().getFullYear()} BrahmanGan · Todos los derechos reservados
        </p>
      </div>
    </div>
  );
}
