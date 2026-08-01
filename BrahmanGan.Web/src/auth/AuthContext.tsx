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
  /** La contraseña la fijó un administrador: hay que obligar a cambiarla. */
  debeCambiarPassword?: boolean;
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

/** Respuesta de /auth/oauth/config: disponibilidad y ClientId público de Google. */
export interface OAuthConfig {
  googleHabilitado: boolean;
  googleClientId: string | null;
}

// Superficie mínima de Google Identity Services que usa la app (window.google.accounts.id).
interface GisNotification {
  isNotDisplayed: () => boolean;
  isSkippedMoment: () => boolean;
}

declare global {
  interface Window {
    google?: {
      accounts?: {
        id?: {
          initialize: (config: {
            client_id: string;
            callback: (response: { credential?: string }) => void;
          }) => void;
          prompt: (listener?: (notification: GisNotification) => void) => void;
        };
      };
    };
  }
}

export interface AuthContextValue extends AuthState {
  login: (email: string, password: string) => Promise<void>;
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
  // ── OAuth2 Google ───────────────────────────────────────────
  // El ClientId lo publica el backend en /auth/oauth/config; si no está configurado allí,
  // la opción no se ofrece. Solo se envía el ID token: el backend valida su firma contra
  // Google y saca de él el correo y el identificador de cuenta.
  const loginOAuth = useCallback(async (_provider: 'google') => {
    const config = await api.get<OAuthConfig>('/auth/oauth/config');

    if (!config.googleHabilitado || !config.googleClientId)
      throw new Error(
        'El inicio de sesión con Google no está habilitado. Configura OAuth__Google__ClientId en el servidor.',
      );

    const gis = window.google?.accounts?.id;
    if (!gis)
      throw new Error(
        'No se pudo cargar Google Identity Services. Revisa la conexión o si un bloqueador lo está impidiendo.',
      );

    const credential = await new Promise<string>((resolve, reject) => {
      gis.initialize({
        client_id: config.googleClientId!,
        callback: (resp) =>
          resp.credential
            ? resolve(resp.credential)
            : reject(new Error('Google no devolvió ningún ID token.')),
      });

      // El diálogo puede no llegar a mostrarse (sin sesión de Google, cookies de terceros
      // bloqueadas, cierre del usuario). Sin este aviso la promesa quedaría pendiente para
      // siempre y el botón se quedaría girando.
      gis.prompt((notification) => {
        if (notification.isNotDisplayed() || notification.isSkippedMoment())
          reject(
            new Error(
              'No se pudo abrir el diálogo de Google. Inicia sesión en Google o permite las cookies de terceros.',
            ),
          );
      });
    });

    const resp = await api.post<TokenResponse>('/auth/oauth/google', { idToken: credential });
    guardarTokens(resp);
  }, [guardarTokens]);

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
    login, loginOAuth, logout,
    tieneRol, tienePermiso,
  }), [usuario, token, cargando, login, loginOAuth, logout, tieneRol, tienePermiso]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
