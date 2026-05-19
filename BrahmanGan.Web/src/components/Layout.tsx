import { useState } from 'react';
import { NavLink, Outlet, useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../auth/useAuth';

interface NavLeaf  { icon: string; label: string; to: string; }
interface NavGroup { icon: string; label: string; children: NavLeaf[]; rol?: string; }
type NavEntry = NavLeaf | NavGroup;

function isGroup(e: NavEntry): e is NavGroup { return 'children' in e; }

const NAV: NavEntry[] = [
  { icon: '🏠', label: 'Dashboard', to: '/' },

  {
    icon: '🐄', label: 'Ganado', children: [
      { icon: '🐄', label: 'Inventario',   to: '/inventario' },
      { icon: '🔬', label: 'Reproducción', to: '/reproduccion' },
      { icon: '💊', label: 'Sanidad',      to: '/sanidad' },
      { icon: '🥛', label: 'Leche',        to: '/leche' },
    ],
  },

  {
    icon: '🌿', label: 'Finca', children: [
      { icon: '🌿', label: 'Finca & Potreros', to: '/finca' },
      { icon: '🌾', label: 'Alimentación',     to: '/alimentacion' },
      { icon: '🔄', label: 'Pastoreo',         to: '/pastoreo' },
    ],
  },

  {
    icon: '💼', label: 'Operaciones', children: [
      { icon: '💼', label: 'Comercial', to: '/comercial' },
      { icon: '💰', label: 'Costos',    to: '/costos' },
      { icon: '👷', label: 'Nómina',    to: '/nomina' },
    ],
  },

  {
    icon: '📦', label: 'Recursos', children: [
      { icon: '📦', label: 'Almacén', to: '/almacen' },
      { icon: '🚜', label: 'Equipos', to: '/equipos' },
    ],
  },

  {
    icon: '🌱', label: 'Sostenibilidad', children: [
      { icon: '📋', label: 'Trazabilidad',   to: '/trazabilidad' },
      { icon: '🌱', label: 'Sostenibilidad', to: '/sostenibilidad' },
    ],
  },

  { icon: '📥', label: 'Importación', to: '/importacion' },
  { icon: '📊', label: 'Reportes',   to: '/reportes' },
  { icon: '🔐', label: 'Seguridad',  to: '/seguridad', rol: 'Administrador' } as NavLeaf & { rol: string },
];

function groupContainsActive(group: NavGroup, pathname: string) {
  return group.children.some(c => {
    if (c.to === '/') return pathname === '/';
    return pathname.startsWith(c.to);
  });
}

export default function Layout() {
  const { usuario, logout } = useAuth();
  const navigate   = useNavigate();
  const location   = useLocation();

  const [sidebarOpen, setSidebarOpen]   = useState(true);
  const [menuUsuario, setMenuUsuario]   = useState(false);

  // Inicializa grupos abiertos según la ruta activa
  const [openGroups, setOpenGroups] = useState<Record<string, boolean>>(() => {
    const init: Record<string, boolean> = {};
    NAV.forEach(e => {
      if (isGroup(e)) {
        init[e.label] = groupContainsActive(e, location.pathname);
      }
    });
    return init;
  });

  function toggleGroup(label: string) {
    setOpenGroups(prev => ({ ...prev, [label]: !prev[label] }));
  }

  async function handleLogout() {
    await logout();
    navigate('/login');
  }

  function hasPermission(entry: NavEntry) {
    const rol = (entry as { rol?: string }).rol;
    return !rol || usuario?.roles.includes(rol);
  }

  const linkBase = (isActive: boolean, collapsed: boolean) =>
    `flex items-center py-2 rounded-lg text-sm transition-colors
     ${collapsed ? 'justify-center px-0' : 'gap-3 px-3'}
     ${isActive
       ? 'bg-brand-600 text-white font-semibold'
       : 'text-slate-300 hover:bg-slate-800 hover:text-white'}`;

  return (
    <div className="flex h-screen bg-slate-50 overflow-hidden">

      {/* ── Sidebar ──────────────────────────────────────────── */}
      <aside className={`
        flex flex-col bg-slate-900 text-white flex-shrink-0
        transition-[width] duration-300 ease-in-out overflow-hidden
        ${sidebarOpen ? 'w-60' : 'w-16'}
      `}>

        {/* Logo */}
        <div className={`flex items-center border-b border-slate-700 h-14 flex-shrink-0
          ${sidebarOpen ? 'gap-3 px-4' : 'justify-center px-0'}`}>
          <span className="text-2xl flex-shrink-0">🐄</span>
          {sidebarOpen && (
            <div>
              <p className="font-bold text-brand-400 leading-tight text-sm">BrahmanGan</p>
              <p className="text-slate-400 text-xs">ERP Ganadero</p>
            </div>
          )}
        </div>

        {/* Navegación */}
        <nav className="flex-1 overflow-y-auto py-3 space-y-0.5 px-2">
          {NAV.filter(hasPermission).map(entry => {
            if (!isGroup(entry)) {
              /* ─ Leaf link ─ */
              const leaf = entry as NavLeaf;
              return (
                <NavLink
                  key={leaf.to}
                  to={leaf.to}
                  end={leaf.to === '/'}
                  title={!sidebarOpen ? leaf.label : undefined}
                  className={({ isActive }) => linkBase(isActive, !sidebarOpen)}
                >
                  <span className="text-xl flex-shrink-0 leading-none">{leaf.icon}</span>
                  {sidebarOpen && <span className="truncate">{leaf.label}</span>}
                </NavLink>
              );
            }

            /* ─ Group ─ */
            const group     = entry as NavGroup;
            const groupOpen = openGroups[group.label] ?? false;
            const hasActive = groupContainsActive(group, location.pathname);

            return (
              <div key={group.label}>
                {/* Group header button */}
                <button
                  onClick={() => { if (sidebarOpen) toggleGroup(group.label); }}
                  title={!sidebarOpen ? group.label : undefined}
                  className={`w-full flex items-center py-2 rounded-lg text-sm transition-colors
                    ${sidebarOpen ? 'gap-3 px-3' : 'justify-center px-0'}
                    ${hasActive
                      ? 'text-brand-300 font-semibold'
                      : 'text-slate-300 hover:bg-slate-800 hover:text-white'}
                  `}
                >
                  <span className="text-xl flex-shrink-0 leading-none">{group.icon}</span>
                  {sidebarOpen && (
                    <>
                      <span className="flex-1 truncate text-left">{group.label}</span>
                      <span className={`text-xs transition-transform duration-200 ${groupOpen ? 'rotate-90' : ''}`}>
                        ▶
                      </span>
                    </>
                  )}
                </button>

                {/* Children — shown when sidebar expanded & group open */}
                {sidebarOpen && groupOpen && (
                  <div className="ml-4 mt-0.5 space-y-0.5 border-l border-slate-700 pl-2">
                    {group.children.map(child => (
                      <NavLink
                        key={child.to}
                        to={child.to}
                        title={child.label}
                        className={({ isActive }) =>
                          `flex items-center gap-2.5 py-1.5 px-2 rounded-lg text-xs transition-colors
                           ${isActive
                             ? 'bg-brand-600 text-white font-semibold'
                             : 'text-slate-400 hover:bg-slate-800 hover:text-white'}`
                        }
                      >
                        <span className="text-base flex-shrink-0 leading-none">{child.icon}</span>
                        <span className="truncate">{child.label}</span>
                      </NavLink>
                    ))}
                  </div>
                )}

                {/* Collapsed: show individual child icons stacked */}
                {!sidebarOpen && (
                  <div className="mt-0.5 space-y-0.5">
                    {group.children.map(child => (
                      <NavLink
                        key={child.to}
                        to={child.to}
                        title={child.label}
                        className={({ isActive }) =>
                          `flex justify-center py-1.5 rounded-lg transition-colors
                           ${isActive
                             ? 'bg-brand-600 text-white'
                             : 'text-slate-400 hover:bg-slate-800 hover:text-white'}`
                        }
                      >
                        <span className="text-base leading-none">{child.icon}</span>
                      </NavLink>
                    ))}
                  </div>
                )}
              </div>
            );
          })}
        </nav>

        {/* Botón colapsar */}
        <button
          onClick={() => setSidebarOpen(o => !o)}
          title={sidebarOpen ? 'Colapsar menú' : 'Expandir menú'}
          className={`flex items-center py-3 mx-2 mb-3 rounded-lg text-slate-400
            hover:text-white hover:bg-slate-800 transition-colors
            ${sidebarOpen ? 'gap-2 px-3' : 'justify-center px-0'}`}
        >
          <span className="text-base flex-shrink-0">{sidebarOpen ? '◀' : '▶'}</span>
          {sidebarOpen && <span className="text-xs">Colapsar</span>}
        </button>
      </aside>

      {/* ── Contenido principal ──────────────────────────────── */}
      <div className="flex-1 flex flex-col overflow-hidden min-w-0">

        {/* Header */}
        <header className="bg-white border-b border-slate-200 px-6 h-14 flex items-center justify-end flex-shrink-0">
          <div className="relative">
            <button
              onClick={() => setMenuUsuario(m => !m)}
              className="flex items-center gap-2 text-slate-700 hover:text-slate-900 transition-colors"
            >
              <div className="w-8 h-8 rounded-full bg-brand-600 text-white flex items-center justify-center text-sm font-bold flex-shrink-0">
                {usuario?.nombreCompleto?.charAt(0).toUpperCase() ?? '?'}
              </div>
              <span className="text-sm hidden md:block max-w-[10rem] truncate">
                {usuario?.nombreCompleto}
              </span>
              <span className="text-xs text-slate-400">▾</span>
            </button>

            {menuUsuario && (
              <div
                className="absolute right-0 top-full mt-2 w-52 bg-white rounded-xl shadow-lg border border-slate-200 z-50"
                onMouseLeave={() => setMenuUsuario(false)}
              >
                <div className="px-4 py-3 border-b border-slate-100">
                  <p className="font-medium text-slate-800 truncate text-sm">{usuario?.nombreCompleto}</p>
                  <p className="text-slate-400 text-xs truncate">{usuario?.email}</p>
                  <div className="flex flex-wrap gap-1 mt-1.5">
                    {usuario?.roles.map(r => (
                      <span key={r} className="bg-brand-100 text-brand-700 text-xs px-2 py-0.5 rounded-full">{r}</span>
                    ))}
                  </div>
                </div>
                <div className="p-2">
                  <button
                    onClick={handleLogout}
                    className="w-full text-left px-3 py-2 rounded-lg text-sm text-red-600 hover:bg-red-50 transition-colors"
                  >
                    🚪 Cerrar sesión
                  </button>
                </div>
              </div>
            )}
          </div>
        </header>

        {/* Página */}
        <main className="flex-1 overflow-y-auto p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
