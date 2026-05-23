import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import {
  Beef, Landmark, Syringe, Milk, FlaskConical, ShoppingCart,
  DollarSign, Users, Package, Tractor, ClipboardList, Leaf,
  BarChart3, ShieldCheck,
} from 'lucide-react';
import { api, type Animal, type Finca, type Vacunacion } from '../api/client';
import { useAuth } from '../auth/useAuth';

interface Modulo {
  icon: string;
  label: string;
  desc: string;
  to: string;
  soloAdmin?: boolean;
}

const MODULOS: Modulo[] = [
  { icon: '🐄', label: 'Inventario Animal',  desc: 'Razas, pesajes, movimientos', to: '/inventario' },
  { icon: '🌿', label: 'Finca & Potreros',   desc: 'Geografía, grupos, áreas',    to: '/finca' },
  { icon: '🔬', label: 'Reproducción',        desc: 'Servicios, gestaciones',      to: '/reproduccion' },
  { icon: '💊', label: 'Sanidad',             desc: 'Vacunas, tratamientos',       to: '/sanidad' },
  { icon: '🥛', label: 'Producción Leche',    desc: 'Controles, calidad, ventas',  to: '/leche' },
  { icon: '💼', label: 'Comercial',           desc: 'Clientes, contratos',         to: '/comercial' },
  { icon: '💰', label: 'Costos',              desc: 'Centros, gastos, ingresos',   to: '/costos' },
  { icon: '👷', label: 'Nómina',              desc: 'Trabajadores, jornales',      to: '/nomina' },
  { icon: '📦', label: 'Almacén',             desc: 'Insumos, kardex',             to: '/almacen' },
  { icon: '🚜', label: 'Equipos',             desc: 'Maquinaria, mantenimiento',   to: '/equipos' },
  { icon: '📋', label: 'Trazabilidad ICA',    desc: 'Registros ICA oficiales',     to: '/trazabilidad' },
  { icon: '🌱', label: 'Sostenibilidad',      desc: 'Carbono, agua, ambiente',     to: '/sostenibilidad' },
  { icon: '📊', label: 'Reportes',            desc: 'KPIs, exportación, análisis', to: '/reportes' },
  { icon: '🔐', label: 'Seguridad',           desc: 'Roles, permisos, usuarios',   to: '/seguridad', soloAdmin: true },
];

const MODULO_COLORS: Record<string, string> = {
  '/inventario':     'from-brand-500 to-brand-700',
  '/finca':          'from-emerald-500 to-emerald-700',
  '/reproduccion':   'from-purple-500 to-purple-700',
  '/sanidad':        'from-red-400 to-red-600',
  '/leche':          'from-blue-400 to-blue-600',
  '/comercial':      'from-indigo-500 to-indigo-700',
  '/costos':         'from-amber-500 to-amber-700',
  '/nomina':         'from-orange-400 to-orange-600',
  '/almacen':        'from-teal-500 to-teal-700',
  '/equipos':        'from-slate-500 to-slate-700',
  '/trazabilidad':   'from-cyan-500 to-cyan-700',
  '/sostenibilidad': 'from-green-500 to-green-700',
  '/reportes':       'from-violet-500 to-violet-700',
  '/seguridad':      'from-rose-500 to-rose-700',
};

interface KPIs {
  animalesActivos: number | null;
  fincas: number | null;
  alertasVacunas: number | null;
}

export default function Dashboard() {
  const { usuario, tieneRol } = useAuth();
  const esAdmin = tieneRol('Administrador');
  const modulosVisibles = MODULOS.filter(m => !m.soloAdmin || esAdmin);

  const [kpis, setKpis] = useState<KPIs>({ animalesActivos: null, fincas: null, alertasVacunas: null });

  useEffect(() => {
    Promise.allSettled([
      api.get<Animal[]>('/animales/activos'),
      api.get<Finca[]>('/fincas'),
      api.get<Vacunacion[]>('/vacunaciones/alertas?dias=7'),
    ]).then(([animalesRes, fincasRes, alertasRes]) => {
      setKpis({
        animalesActivos: animalesRes.status === 'fulfilled' ? animalesRes.value.length : null,
        fincas:          fincasRes.status  === 'fulfilled' ? fincasRes.value.length  : null,
        alertasVacunas:  alertasRes.status === 'fulfilled' ? alertasRes.value.length : null,
      });
    });
  }, []);

  function fmt(val: number | null) { return val == null ? '…' : String(val); }

  const hour = new Date().getHours();
  const greeting = hour < 12 ? 'Buenos días' : hour < 18 ? 'Buenas tardes' : 'Buenas noches';

  const STATS = [
    {
      Icon: Beef,
      label: 'Animales activos',
      valor: fmt(kpis.animalesActivos),
      gradient: 'from-brand-500 to-brand-700',
      alert: null as string | null,
    },
    {
      Icon: Landmark,
      label: 'Fincas registradas',
      valor: fmt(kpis.fincas),
      gradient: 'from-emerald-400 to-emerald-600',
      alert: null,
    },
    {
      Icon: Syringe,
      label: 'Alertas vacunas (7d)',
      valor: fmt(kpis.alertasVacunas),
      gradient: 'from-amber-400 to-orange-500',
      alert: kpis.alertasVacunas !== null && kpis.alertasVacunas > 0 ? 'Requiere atención' : null,
    },
  ];

  return (
    <div className="space-y-8 animate-fade-in-up">
      {/* Header */}
      <div className="flex items-end justify-between">
        <div>
          <p className="text-sm font-medium text-slate-500 mb-1">{greeting}</p>
          <h1 className="text-3xl font-bold text-slate-900 tracking-tight">
            {usuario?.nombreCompleto?.split(' ')[0]}
          </h1>
          <p className="text-slate-500 mt-1 text-sm">Panel de control · BrahmanGan ERP</p>
        </div>
        <p className="text-sm text-slate-400 hidden md:block font-medium">
          {new Date().toLocaleDateString('es-CO', { weekday: 'long', day: 'numeric', month: 'long' })}
        </p>
      </div>

      {/* KPIs */}
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
        {STATS.map(s => {
          const Icon = s.Icon;
          return (
            <div
              key={s.label}
              className={`relative overflow-hidden rounded-2xl bg-gradient-to-br ${s.gradient} p-6 text-white shadow-lg`}
            >
              <div className="absolute -top-4 -right-4 w-24 h-24 rounded-full bg-white/10" />
              <div className="absolute -bottom-6 -right-2 w-20 h-20 rounded-full bg-white/5" />
              <div className="relative">
                <div className="inline-flex items-center justify-center w-10 h-10 rounded-xl bg-white/20 mb-3">
                  <Icon className="w-5 h-5 text-white" />
                </div>
                <p className="text-3xl font-bold tabular-nums">{s.valor}</p>
                <p className="text-sm font-medium text-white/80 mt-0.5">{s.label}</p>
                {s.alert && (
                  <p className="text-xs text-white/60 mt-2 font-medium">{s.alert}</p>
                )}
              </div>
            </div>
          );
        })}
      </div>

      {/* Módulos */}
      <div>
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-base font-bold text-slate-800">Módulos del sistema</h2>
          <span className="text-xs text-slate-400 font-medium">{modulosVisibles.length} módulos</span>
        </div>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
          {modulosVisibles.map(m => {
            const color = MODULO_COLORS[m.to] ?? 'from-brand-500 to-brand-700';
            return (
              <Link
                key={m.to}
                to={m.to}
                className="group relative overflow-hidden bg-white rounded-2xl border border-slate-100 shadow-card p-5 hover:shadow-card-hover hover:-translate-y-0.5 transition-all duration-200 no-underline"
              >
                <div className={`absolute top-0 left-0 right-0 h-0.5 bg-gradient-to-r ${color}`} />
                <div className="flex items-start gap-3 mt-1">
                  <div className={`w-10 h-10 rounded-xl bg-gradient-to-br ${color} flex items-center justify-center flex-shrink-0 group-hover:scale-105 transition-transform duration-200 shadow-sm`}>
                    <span className="text-lg text-white">{m.icon}</span>
                  </div>
                  <div className="min-w-0">
                    <p className="font-semibold text-slate-800 group-hover:text-brand-700 transition-colors text-sm">
                      {m.label}
                    </p>
                    <p className="text-slate-500 text-xs mt-0.5 leading-relaxed">{m.desc}</p>
                  </div>
                </div>
              </Link>
            );
          })}
        </div>
      </div>
    </div>
  );
}
