import { useState, type ComponentType } from 'react';
import { NavLink, Outlet, useNavigate, useLocation } from 'react-router-dom';
import {
  LayoutDashboard, Beef, FlaskConical, Syringe, Milk,
  Landmark, Wheat, RefreshCw, ShoppingCart, DollarSign,
  Users, Package, Tractor, ClipboardList, Leaf,
  BarChart3, ShieldCheck, Upload,
  ChevronDown, ChevronLeft, ChevronRight,
  LogOut,
} from 'lucide-react';
import { useAuth } from '../auth/useAuth';

interface IconProps { className?: string }

interface NavLeaf  { icon: ComponentType<IconProps>; label: string; to: string; rol?: string }
interface NavGroup { icon: ComponentType<IconProps>; label: string; children: NavLeaf[]; rol?: string }
type NavEntry = NavLeaf | NavGroup;

function isGroup(e: NavEntry): e is NavGroup { return 'children' in e; }

interface NavSection {
  label: string;
  entries: NavEntry[];
}

const NAV_SECTIONS: NavSection[] = [
  {
    label: '',
    entries: [
      { icon: LayoutDashboard, label: 'Dashboard', to: '/' },
    ],
  },
  {
    label: 'Ganadería',
    entries: [
      {
        icon: Beef, label: 'Ganado', children: [
          { icon: Beef,         label: 'Inventario',   to: '/inventario' },
          { icon: FlaskConical, label: 'Reproducción', to: '/reproduccion' },
          { icon: Syringe,      label: 'Sanidad',      to: '/sanidad' },
          { icon: Milk,         label: 'Leche',        to: '/leche' },
        ],
      },
      {
        icon: Landmark, label: 'Finca', children: [
          { icon: Landmark,  label: 'Finca & Potreros', to: '/finca' },
          { icon: Wheat,     label: 'Alimentación',     to: '/alimentacion' },
          { icon: RefreshCw, label: 'Pastoreo',         to: '/pastoreo' },
        ],
      },
    ],
  },
  {
    label: 'Finanzas',
    entries: [
      {
        icon: ShoppingCart, label: 'Operaciones', children: [
          { icon: ShoppingCart, label: 'Comercial', to: '/comercial' },
          { icon: DollarSign,   label: 'Costos',    to: '/costos' },
          { icon: Users,        label: 'Nómina',    to: '/nomina' },
        ],
      },
      {
        icon: Package, label: 'Recursos', children: [
          { icon: Package, label: 'Almacén', to: '/almacen' },
          { icon: Tractor, label: 'Equipos', to: '/equipos' },
        ],
      },
    ],
  },
  {
    label: 'Gestión',
    entries: [
      {
        icon: Leaf, label: 'Sostenibilidad', children: [
          { icon: ClipboardList, label: 'Trazabilidad',   to: '/trazabilidad' },
          { icon: Leaf,          label: 'Sostenibilidad', to: '/sostenibilidad' },
        ],
      },
      { icon: Upload,      label: 'Importación', to: '/importacion' },
      { icon: BarChart3,   label: 'Reportes',    to: '/reportes' },
      { icon: ShieldCheck, label: 'Seguridad',   to: '/seguridad', rol: 'Administrador' },
    ],
  },
];

const PAGE_TITLES: Record<string, string> = {
  '/':              'Dashboard',
  '/inventario':    'Inventario Animal',
  '/reproduccion':  'Reproducción',
  '/sanidad':       'Sanidad',
  '/leche':         'Leche',
  '/finca':         'Finca & Potreros',
  '/alimentacion':  'Alimentación',
  '/pastoreo':      'Pastoreo',
  '/comercial':     'Comercial',
  '/costos':        'Costos',
  '/nomina':        'Nómina',
  '/almacen':       'Almacén',
  '/equipos':       'Equipos',
  '/trazabilidad':  'Trazabilidad ICA',
  '/sostenibilidad':'Sostenibilidad',
  '/importacion':   'Importación Masiva',
  '/reportes':      'Reportes',
  '/seguridad':     'Seguridad',
};

function groupContainsActive(group: NavGroup, pathname: string): boolean {
  return group.children.some(c => pathname === c.to || pathname.startsWith(c.to + '/'));
}

export default function Layout() {
  const { usuario, logout } = useAuth();
  const navigate  = useNavigate();
  const location  = useLocation();

  const [sidebarOpen, setSidebarOpen] = useState(true);
  const [menuUsuario, setMenuUsuario] = useState(false);

  const [openGroups, setOpenGroups] = useState<Record<string, boolean>>(() => {
    const init: Record<string, boolean> = {};
    NAV_SECTIONS.forEach(sec =>
      sec.entries.forEach(e => {
        if (isGroup(e)) init[e.label] = groupContainsActive(e, location.pathname);
      })
    );
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
    return !entry.rol || usuario?.roles.includes(entry.rol);
  }

  const linkBase = (isActive: boolean, collapsed: boolean) =>
    `flex items-center py-2 rounded-xl text-sm transition-all duration-150 cursor-pointer
     ${collapsed ? 'justify-center px-0 mx-1' : 'gap-3 px-3'}
     ${isActive
       ? 'bg-brand-600/90 text-white font-semibold shadow-sm shadow-brand-900/30'
       : 'text-slate-400 hover:bg-white/8 hover:text-white'}`;

  const pageTitle = PAGE_TITLES[location.pathname] ?? 'BrahmanGan';

  return (
    <div className="flex h-screen bg-slate-50 overflow-hidden">

      {/* ── Sidebar ──────────────────────────────────────────── */}
      <aside className={`
        flex flex-col bg-slate-900 text-white flex-shrink-0
        transition-[width] duration-300 ease-in-out overflow-hidden
        ${sidebarOpen ? 'w-60' : 'w-16'}
      `}>

        {/* Logo */}
        <div className={`flex items-center flex-shrink-0 h-16 bg-gradient-to-b from-brand-700 to-slate-900 border-b border-white/10
          ${sidebarOpen ? 'gap-3 px-5' : 'justify-center px-0'}`}>
          <div className="w-9 h-9 rounded-xl bg-white/15 flex items-center justify-center flex-shrink-0 backdrop-blur-sm border border-white/20">
            <span className="text-xl leading-none">🐄</span>
          </div>
          {sidebarOpen && (
            <div>
              <p className="font-bold text-white leading-tight text-sm tracking-wide">BrahmanGan</p>
              <p className="text-brand-300 text-xs font-medium">ERP Ganadero</p>
            </div>
          )}
        </div>

        {/* Navigation */}
        <nav className="flex-1 overflow-y-auto py-3 space-y-0.5 px-2">
          {NAV_SECTIONS.map((section, si) => {
            const visibleEntries = section.entries.filter(hasPermission);
            if (visibleEntries.length === 0) return null;

            return (
              <div key={si}>
                {sidebarOpen && section.label && (
                  <p className="px-3 pt-4 pb-1 text-[10px] font-bold text-slate-500 uppercase tracking-widest">
                    {section.label}
                  </p>
                )}
                {!sidebarOpen && section.label && si > 0 && (
                  <div className="mx-2 my-2 border-t border-slate-700/60" />
                )}

                {visibleEntries.map(entry => {
                  if (!isGroup(entry)) {
                    const Icon = entry.icon;
                    return (
                      <NavLink
                        key={entry.to}
                        to={entry.to}
                        end={entry.to === '/'}
                        className={({ isActive }) => linkBase(isActive, !sidebarOpen)}
                      >
                        <Icon className="w-5 h-5 flex-shrink-0" />
                        {sidebarOpen && <span className="truncate">{entry.label}</span>}
                      </NavLink>
                    );
                  }

                  const group = entry as NavGroup;
                  const groupOpen = openGroups[group.label] ?? false;
                  const hasActive = groupContainsActive(group, location.pathname);
                  const GroupIcon = group.icon;

                  return (
                    <div key={group.label}>
                      <button
                        onClick={() => { if (sidebarOpen) toggleGroup(group.label); }}
                        className={`w-full flex items-center py-2 rounded-xl text-sm transition-all duration-150
                          ${sidebarOpen ? 'gap-3 px-3' : 'justify-center px-0 mx-1'}
                          ${hasActive ? 'text-brand-300 font-semibold' : 'text-slate-400 hover:bg-white/8 hover:text-white'}`}
                      >
                        <GroupIcon className="w-5 h-5 flex-shrink-0" />
                        {sidebarOpen && (
                          <>
                            <span className="flex-1 truncate text-left">{group.label}</span>
                            <ChevronDown className={`w-4 h-4 transition-transform duration-200 ${groupOpen ? '' : '-rotate-90'}`} />
                          </>
                        )}
                      </button>

                      {/* Children — sidebar expanded */}
                      {sidebarOpen && groupOpen && (
                        <div className="ml-4 mt-0.5 space-y-0.5 border-l border-slate-700/60 pl-2">
                          {group.children.map(child => {
                            const ChildIcon = child.icon;
                            return (
                              <NavLink
                                key={child.to}
                                to={child.to}
                                className={({ isActive }) =>
                                  `flex items-center gap-2.5 py-1.5 px-2.5 rounded-xl text-xs transition-all duration-150
                                   ${isActive
                                     ? 'bg-brand-600/80 text-white font-semibold'
                                     : 'text-slate-400 hover:bg-white/8 hover:text-white'}`
                                }
                              >
                                <ChildIcon className="w-4 h-4 flex-shrink-0" />
                                <span className="truncate">{child.label}</span>
                              </NavLink>
                            );
                          })}
                        </div>
                      )}

                      {/* Children — sidebar collapsed */}
                      {!sidebarOpen && (
                        <div className="mt-0.5 space-y-0.5">
                          {group.children.map(child => {
                            const ChildIcon = child.icon;
                            return (
                              <NavLink
                                key={child.to}
                                to={child.to}
                                className={({ isActive }) =>
                                  `flex justify-center py-1.5 rounded-xl mx-1 transition-all duration-150
                                   ${isActive ? 'bg-brand-600/80 text-white' : 'text-slate-400 hover:bg-white/8 hover:text-white'}`
                                }
                              >
                                <ChildIcon className="w-4 h-4" />
                              </NavLink>
                            );
                          })}
                        </div>
                      )}
                    </div>
                  );
                })}
              </div>
            );
          })}
        </nav>

        {/* Collapse toggle */}
        <button
          onClick={() => setSidebarOpen(o => !o)}
          className={`flex items-center py-3 mx-2 mb-3 rounded-xl text-slate-400
            hover:text-white hover:bg-white/8 transition-all duration-150
            ${sidebarOpen ? 'gap-2 px-3' : 'justify-center px-0'}`}
        >
          {sidebarOpen
            ? <><ChevronLeft className="w-4 h-4 flex-shrink-0" /><span className="text-xs">Colapsar</span></>
            : <ChevronRight className="w-4 h-4" />
          }
        </button>
      </aside>

      {/* ── Main Content Area ──────────────────────────────────── */}
      <div className="flex-1 flex flex-col overflow-hidden min-w-0">

        {/* Header */}
        <header className="bg-white border-b border-slate-100 px-6 h-14 flex items-center justify-between flex-shrink-0 shadow-[0_1px_3px_0_rgb(0,0,0,0.06)]">

          <span className="font-semibold text-slate-800 text-sm">{pageTitle}</span>

          <div className="relative">
            <button
              onClick={() => setMenuUsuario(m => !m)}
              className="flex items-center gap-2.5 px-3 py-1.5 rounded-xl text-slate-700 hover:bg-slate-100 transition-all duration-150 cursor-pointer"
            >
              <div className="w-8 h-8 rounded-full bg-gradient-to-br from-brand-500 to-brand-700 text-white flex items-center justify-center text-sm font-bold flex-shrink-0 ring-2 ring-white">
                {usuario?.nombreCompleto?.charAt(0).toUpperCase() ?? '?'}
              </div>
              <span className="text-sm hidden md:block max-w-[10rem] truncate font-medium">
                {usuario?.nombreCompleto}
              </span>
              <ChevronDown className="w-3.5 h-3.5 text-slate-400" />
            </button>

            {menuUsuario && (
              <div
                className="absolute right-0 top-full mt-2 w-56 bg-white rounded-2xl shadow-modal border border-slate-100 z-50 overflow-hidden animate-fade-in"
                onMouseLeave={() => setMenuUsuario(false)}
              >
                <div className="px-4 py-3 border-b border-slate-100">
                  <p className="font-semibold text-slate-800 truncate text-sm">{usuario?.nombreCompleto}</p>
                  <p className="text-slate-400 text-xs truncate mt-0.5">{usuario?.email}</p>
                  <div className="flex flex-wrap gap-1 mt-2">
                    {usuario?.roles.map(r => (
                      <span key={r} className="badge badge-brand">{r}</span>
                    ))}
                  </div>
                </div>
                <div className="p-2">
                  <button
                    onClick={handleLogout}
                    className="w-full flex items-center gap-2 px-3 py-2 rounded-xl text-sm text-red-600 hover:bg-red-50 transition-colors duration-150"
                  >
                    <LogOut className="w-4 h-4" />
                    Cerrar sesión
                  </button>
                </div>
              </div>
            )}
          </div>
        </header>

        {/* Page content */}
        <main className="flex-1 overflow-y-auto p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
