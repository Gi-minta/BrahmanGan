/**
 * Cliente HTTP genérico para la API BrahmanGan.
 * Usa fetch nativo. El proxy de Vite redirige /api hacia https://localhost:7001.
 * Inyecta automáticamente el Bearer token desde localStorage.
 */
const BASE = '/api';
const TOKEN_KEY = 'bg_access_token';

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const token = localStorage.getItem(TOKEN_KEY);
  const res = await fetch(`${BASE}${path}`, {
    ...init,
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...(init?.headers ?? {}),
    },
  });
  if (!res.ok) {
    const text = await res.text();
    throw new Error(`HTTP ${res.status}: ${text || res.statusText}`);
  }
  if (res.status === 204) return undefined as T;
  return res.json() as Promise<T>;
}

async function upload<T>(path: string, formData: FormData): Promise<T> {
  const token = localStorage.getItem(TOKEN_KEY);
  const res = await fetch(`${BASE}${path}`, {
    method: 'POST',
    headers: token ? { Authorization: `Bearer ${token}` } : {},
    body: formData,
  });
  if (!res.ok) {
    const text = await res.text();
    throw new Error(`HTTP ${res.status}: ${text || res.statusText}`);
  }
  return res.json() as Promise<T>;
}

export const api = {
  get: <T>(path: string) => request<T>(path),
  post: <T>(path: string, body: unknown) =>
    request<T>(path, { method: 'POST', body: JSON.stringify(body) }),
  put: <T>(path: string, body: unknown) =>
    request<T>(path, { method: 'PUT', body: JSON.stringify(body) }),
  del: <T>(path: string) => request<T>(path, { method: 'DELETE' }),
  upload: <T>(path: string, formData: FormData) => upload<T>(path, formData),
};

// ===== Tipos compartidos con la API =====
export type Animal = {
  id: number; codigo: string; nombre?: string; idRaza: number; sexo: string;
  fechaNacimiento?: string; idMadre?: number; idPadre?: number; idOrigen?: number;
  estado: string; pesoNacimiento?: number; idFinca: number; fechaIngreso?: string;
  observaciones?: string; fechaRegistro: string;
};
export type Raza = { id: number; codigo: string; nombre: string; proposito?: string };
export type Finca = { id: number; nombre: string; nit?: string; propietario?: string;
  idMunicipio?: number; areaHectareas?: number; activa: boolean };
export type Potrero = { id: number; idFinca: number; codigo: string; nombre: string;
  areaHectareas?: number; tipoPasto?: string; activo: boolean };
export type Medicamento = { id: number; codigo: string; nombre: string;
  tiempoCarne?: number; tiempoLeche?: number; activo: boolean };
export type Vacunacion = { id: number; idAnimal: number; idMedicamento: number;
  fecha: string; dosis?: number; lote?: string; proximaFecha?: string };
export type Servicio = { id: number; idHembra: number; tipoServicio: string; fecha: string;
  idToro?: number; idSemen?: number; resultadoPreniez?: boolean; fechaConfirmacion?: string };
export type Gestacion = { id: number; idAnimal: number; idServicio?: number;
  fechaInicio: string; fechaPartoEstimado?: string; fechaPartoReal?: string; estado: string };
export type ControlLeche = { id: number; idAnimal: number; fecha: string;
  maniana?: number; tarde?: number; noche?: number; total: number };
export type Cliente = { id: number; documento: string; tipoDocumento: string;
  razonSocial: string; contacto?: string; telefono?: string; email?: string; activo: boolean };

// ===== Importación Masiva =====
export type ImportError = { fila: number; campo?: string; mensaje: string };
export type ImportResult = {
  totalFilas: number;
  exitosos: number;
  fallidos: number;
  errores: ImportError[];
};
