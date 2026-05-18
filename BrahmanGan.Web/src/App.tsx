import { Routes, Route } from 'react-router-dom';
import { AuthProvider }     from './auth/AuthContext';
import { ProtectedRoute }   from './auth/ProtectedRoute';
import Layout               from './components/Layout';

// Páginas
import LoginPage        from './pages/Login';
import SinPermisos      from './pages/SinPermisos';
import Dashboard        from './pages/Dashboard';
import InventarioPage   from './pages/Inventario';
import FincaPage        from './pages/Finca';
import ReproduccionPage from './pages/Reproduccion';
import SanidadPage      from './pages/Sanidad';
import LechePage        from './pages/Leche';
import ComercialPage    from './pages/Comercial';
import CostosPage       from './pages/Costos';
import NominaPage       from './pages/Nomina';
import AlmacenPage      from './pages/Almacen';
import EquiposPage      from './pages/Equipos';
import TrazabilidadPage from './pages/Trazabilidad';
import SostenibilidadPage from './pages/Sostenibilidad';
import ReportesPage         from './pages/Reportes';
import SeguridadPage         from './pages/Seguridad';
import ImportacionMasivaPage from './pages/ImportacionMasiva';

/**
 * Árbol de rutas de BrahmanGan ERP.
 *
 * Estructura:
 *  /login           → público
 *  /sin-permisos    → público
 *  / (Layout)       → protegido (usuario autenticado)
 *    /              → Dashboard
 *    /inventario    → Inventario Animal
 *    /finca         → Finca & Potreros
 *    /reproduccion  → Reproducción
 *    /sanidad       → Sanidad
 *    /leche         → Leche
 *    /comercial     → Comercial
 *    /costos        → Costos
 *    /nomina        → Nómina
 *    /almacen       → Almacén
 *    /equipos       → Equipos
 *    /trazabilidad  → Trazabilidad ICA
 *    /sostenibilidad→ Sostenibilidad
 *    /reportes      → Reportes
 *    /seguridad     → Seguridad (solo Administrador)
 */
export default function App() {
  return (
    <AuthProvider>
      <Routes>
        {/* Públicas */}
        <Route path="/login"         element={<LoginPage />} />
        <Route path="/sin-permisos"  element={<SinPermisos />} />

        {/* Protegidas — usuario autenticado */}
        <Route element={<ProtectedRoute />}>
          <Route element={<Layout />}>
            <Route index element={<Dashboard />} />
            <Route path="inventario"    element={<InventarioPage />} />
            <Route path="finca"         element={<FincaPage />} />
            <Route path="reproduccion"  element={<ReproduccionPage />} />
            <Route path="sanidad"       element={<SanidadPage />} />
            <Route path="leche"         element={<LechePage />} />
            <Route path="comercial"     element={<ComercialPage />} />
            <Route path="costos"        element={<CostosPage />} />
            <Route path="nomina"        element={<NominaPage />} />
            <Route path="almacen"       element={<AlmacenPage />} />
            <Route path="equipos"       element={<EquiposPage />} />
            <Route path="trazabilidad"  element={<TrazabilidadPage />} />
            <Route path="sostenibilidad" element={<SostenibilidadPage />} />
            <Route path="reportes"         element={<ReportesPage />} />
            <Route path="importacion"      element={<ImportacionMasivaPage />} />

            {/* Solo Administradores */}
            <Route element={<ProtectedRoute rol="Administrador" />}>
              <Route path="seguridad" element={<SeguridadPage />} />
            </Route>
          </Route>
        </Route>

        {/* 404 — redirige al dashboard */}
        <Route path="*" element={<ProtectedRoute redirectTo="/login" />} />
      </Routes>
    </AuthProvider>
  );
}
