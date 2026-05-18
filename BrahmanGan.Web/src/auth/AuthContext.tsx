import {
  createContext,
  useCallback,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from 'react';
import { api } from '../api/client';

// ─────────────────────────────────────────────────────────────
//  Tipos
// ─────────────────────────────────────────────────────────────
export interface UsuarioInfo {
  id: number;
  email: string;
  nombreCompleto: string;
  roles: string[];
  permisos: string[];
}

export interface TokenResponse {
  accessToken: string;
  refreshToken: string;
  expira: string;
  usuario: UsuarioInfo;
}

export interface AuthState {
  usuario: UsuarioInfo | null;
  token: string | null;
  cargando: boolean;
}

export interface AuthContextValue extends AuthState {
  login: (email: string, password: string) => Promise<void>;
  registrar: (email: string, nombre: string, password: string) => Promise<void>;
  loginOAuth: (provider: 'google') => Promise<void>;
  logout: () => Promise<void>;
  tieneRol: (rol: string) => boolean;
  tienePermiso: (permiso: string) => boolean;
}

// ─────────────────────────────────────────────────────────────
//  Context
// ─────────────────────────────────────────────────────────────
// eslint-disable-next-line react-refresh/only-export-components
export const AuthContext = createContext<AuthContextValue | null>(null);

const TOKEN_KEY   = 'bg_access_token';
const REFRESH_KEY = 'bg_refresh_token';

// ─────────────────────────────────────────────────────────────
//  Provider
// ─────────────────────────────────────────────────────────────
export function AuthProvider({ children }: { children: ReactNode }) {
  const [usuario, setUsuario] = useState<UsuarioInfo | null>(null);
  const [token, setToken]     = useState<string | null>(() => localStorage.getItem(TOKEN_KEY));
  const [cargando, setCargando] = useState(true);

  // ── Guardar tokens ──────────────────────────────────────────
  const guardarTokens = useCallback((resp: TokenResponse) => {
    localStorage.setItem(TOKEN_KEY,   resp.accessToken);
    localStorage.setItem(REFRESH_KEY, resp.refreshToken);
    setToken(resp.accessToken);
    setUsuario(resp.usuario);
  }, []);

  // ── Limpiar sesión ──────────────────────────────────────────
  const limpiarSesion = useCallback(() => {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_KEY);
    setToken(null);
    setUsuario(null);
  }, []);

  // ── Al montar: cargar perfil si hay token guardado ──────────
  useEffect(() => {
    const savedToken = localStorage.getItem(TOKEN_KEY);
    if (!savedToken) { setCargando(false); return; }

    api.get<UsuarioInfo>('/auth/me')
      .then(u => setUsuario(u))
      .catch(() => {
        // token expirado → intentar refresh
        const rt = localStorage.getItem(REFRESH_KEY);
        if (!rt) { limpiarSesion(); return; }
        api.post<TokenResponse>('/auth/refresh', { accessToken: savedToken, refreshToken: rt })
          .then(guardarTokens)
          .catch(limpiarSesion);
      })
      .finally(() => setCargando(false));
  }, []); // eslint-disable-line react-hooks/exhaustive-deps

  // ── Login ───────────────────────────────────────────────────
  const login = useCallback(async (email: string, password: string) => {
    const resp = await api.post<TokenResponse>('/auth/login', { email, password });
    guardarTokens(resp);
  }, [guardarTokens]);

  // ── Registro ────────────────────────────────────────────────
  const registrar = useCallback(async (email: string, nombre: string, password: string) => {
    const resp = await api.post<TokenResponse>('/auth/registrar', {
      email,
      nombreCompleto: nombre,
      password,
      confirmarPassword: password,
    });
    guardarTokens(resp);
  }, [guardarTokens]);

  // ── OAuth2 Google ───────────────────────────────────────────
  const loginOAuth = useCallback(async (_provider: 'google') => {
    // En un flujo real se abriría una ventana popup con google.accounts.oauth2.initTokenClient
    // Aquí mostramos la ventana y procesamos el callback.
    // Para el MVP, redirigimos al endpoint de Google usando la librería GIS.
    // El backend /api/auth/oauth/google recibe { email, nombreCompleto, idExterno }.
    throw new Error('Integra Google Identity Services en Login.tsx para obtener el ID token.');
  }, []);

  // ── Logout ──────────────────────────────────────────────────
  const logout = useCallback(async () => {
    try { await api.post('/auth/logout', {}); } catch { /* ignorar errores de red */ }
    limpiarSesion();
  }, [limpiarSesion]);

  // ── Helpers de autorización ─────────────────────────────────
  const tieneRol     = useCallback((rol: string) => usuario?.roles.includes(rol) ?? false, [usuario]);
  const tienePermiso = useCallback((p: string)   => usuario?.permisos.includes(p) ?? false, [usuario]);

  const value = useMemo<AuthContextValue>(() => ({
    usuario, token, cargando,
    login, registrar, loginOAuth, logout,
    tieneRol, tienePermiso,
  }), [usuario, token, cargando, login, registrar, loginOAuth, logout, tieneRol, tienePermiso]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
