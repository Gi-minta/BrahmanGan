import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
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

  const STATS = [
    { icon: '🐄', label: 'Animales activos',    valor: fmt(kpis.animalesActivos), color: 'bg-brand-50 text-brand-700' },
    { icon: '🌿', label: 'Fincas registradas',  valor: fmt(kpis.fincas),          color: 'bg-green-50 text-green-700' },
    { icon: '💊', label: 'Alertas vacunas (7d)', valor: fmt(kpis.alertasVacunas), color: 'bg-amber-50 text-amber-700' },
  ];

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-2xl font-bold text-slate-800">
          Bienvenido, {usuario?.nombreCompleto?.split(' ')[0]}
        </h1>
        <p className="text-slate-500 mt-1">Panel principal del Sistema ERP Ganadero BrahmanGan</p>
      </div>

      {/* KPIs */}
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
        {STATS.map(s => (
          <div key={s.label} className={`card flex items-center gap-4 ${s.color}`}>
            <span className="text-3xl">{s.icon}</span>
            <div>
              <p className="text-2xl font-bold">{s.valor}</p>
              <p className="text-xs font-medium opacity-80">{s.label}</p>
            </div>
          </div>
        ))}
      </div>

      {/* Módulos */}
      <div>
        <h2 className="text-lg font-semibold text-slate-700 mb-4">Módulos del sistema</h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
          {modulosVisibles.map(m => (
            <Link
              key={m.to}
              to={m.to}
              className="card hover:shadow-md hover:border-brand-300 transition-all group no-underline"
            >
              <div className="flex items-start gap-4">
                <span className="text-3xl group-hover:scale-110 transition-transform">{m.icon}</span>
                <div>
                  <p className="font-semibold text-slate-800 group-hover:text-brand-700 transition-colors">
                    {m.label}
                  </p>
                  <p className="text-slate-500 text-sm mt-0.5">{m.desc}</p>
                </div>
              </div>
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
}
