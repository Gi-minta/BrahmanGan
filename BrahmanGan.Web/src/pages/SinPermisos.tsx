import { useNavigate } from 'react-router-dom';

export default function SinPermisos() {
  const navigate = useNavigate();
  return (
    <div className="min-h-screen bg-slate-50 flex items-center justify-center p-8">
      <div className="text-center max-w-md">
        <div className="text-7xl mb-6">🔒</div>
        <h1 className="text-2xl font-bold text-slate-800 mb-2">Acceso denegado</h1>
        <p className="text-slate-500 mb-8">
          No tienes permisos para acceder a esta sección.
          Contacta al administrador del sistema si necesitas acceso.
        </p>
        <button onClick={() => navigate(-1)} className="btn-primary">
          ← Volver
        </button>
      </div>
    </div>
  );
}
