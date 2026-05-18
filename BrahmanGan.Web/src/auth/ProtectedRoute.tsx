import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { useAuth } from './useAuth';

interface Props {
  /** Rol requerido para acceder a esta ruta. */
  rol?: string;
  /** Permiso requerido (clave "Modulo:Accion"). */
  permiso?: string;
  /** Ruta de redirección cuando no autenticado (default: /login). */
  redirectTo?: string;
}

/**
 * Envuelve rutas que requieren autenticación (y opcionalmente un rol o permiso).
 * Uso en el router:
 * ```tsx
 * <Route element={<ProtectedRoute />}>
 *   <Route path="animales" element={<AnimalesPage />} />
 * </Route>
 * <Route element={<ProtectedRoute rol="Administrador" />}>
 *   <Route path="seguridad" element={<SeguridadPage />} />
 * </Route>
 * ```
 */
export function ProtectedRoute({ rol, permiso, redirectTo = '/login' }: Props) {
  const { usuario, cargando } = useAuth();
  const location = useLocation();

  if (cargando) {
    return (
      <div className="flex items-center justify-center h-screen text-slate-400 text-lg">
        Cargando…
      </div>
    );
  }

  if (!usuario) {
    return <Navigate to={redirectTo} state={{ from: location }} replace />;
  }

  if (rol && !usuario.roles.includes(rol)) {
    return <Navigate to="/sin-permisos" replace />;
  }

  if (permiso && !usuario.permisos.includes(permiso)) {
    return <Navigate to="/sin-permisos" replace />;
  }

  return <Outlet />;
}
