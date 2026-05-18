import { useEffect, useState } from 'react';
import { api, type Animal, type Finca } from '../api/client';

// ── Tipos locales ──────────────────────────────────────────────
type Vacunacion = { id: number; idAnimal: number; idMedicamento: number; fecha: string; proximaFecha?: string };
type Gestacion  = { id: number; idAnimal: number; estado: string; fechaPartoEstimado?: string };
type ControlLeche = { id: number; idAnimal: number; fecha: string; totalLitros: number };

interface KpiCard { titulo: string; valor: string | number; sub?: string; color?: string; icono: string }

function Kpi({ titulo, valor, sub, color = 'text-slate-800', icono }: KpiCard) {
  return (
    <div className="card text-center">
      <div className="text-3xl mb-1">{icono}</div>
      <p className={`text-3xl font-bold ${color}`}>{valor}</p>
      <p className="text-slate-600 text-sm font-medium mt-1">{titulo}</p>
      {sub && <p className="text-slate-400 text-xs mt-0.5">{sub}</p>}
    </div>
  );
}

export default function ReportesPage() {
  const [animales, setAnimales]   = useState<Animal[]>([]);
  const [fincas, setFincas]       = useState<Finca[]>([]);
  const [vacunas, setVacunas]     = useState<Vacunacion[]>([]);
  const [gestaciones, setGest]    = useState<Gestacion[]>([]);
  const [leche, setLeche]         = useState<ControlLeche[]>([]);
  const [cargando, setCargando]   = useState(true);
  const [error, setError]         = useState('');

  useEffect(() => {
    const resultados = Promise.allSettled([
      api.get<Animal[]>('/animales/activos').then(setAnimales),
      api.get<Finca[]>('/fincas').then(setFincas),
      api.get<Vacunacion[]>('/vacunaciones/alertas?dias=90').then(setVacunas),
      api.get<Gestacion[]>('/gestaciones/activas').then(setGest),
      api.get<ControlLeche[]>('/control-leche/hoy').then(setLeche),
    ]);
    resultados.finally(() => setCargando(false));
  }, []);

  // ── Derivados ──────────────────────────────────────────────
  const totalAnimales  = animales.length;
  const machos         = animales.filter(a => a.sexo === 'M').length;
  const hembras        = animales.filter(a => a.sexo === 'H').length;
  const totalFincas    = fincas.length;
  const gestActivas    = gestaciones.filter(g => g.estado === 'GESTANDO').length;
  const litrosHoy      = leche.reduce((s, c) => s + c.totalLitros, 0);
  const vacPendientes  = vacunas.length;

  // Distribución por sexo
  const pctHembras = totalAnimales > 0 ? Math.round((hembras / totalAnimales) * 100) : 0;

  // Próximas vacunaciones vencidas pronto
  const hoy = new Date();
  const proximosPartos = gestaciones
    .filter(g => g.estado === 'GESTANDO' && g.fechaPartoEstimado)
    .map(g => ({ ...g, dias: Math.round((new Date(g.fechaPartoEstimado!).getTime() - hoy.getTime()) / 86400000) }))
    .filter(g => g.dias >= 0 && g.dias <= 30)
    .sort((a, b) => a.dias - b.dias);

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-2xl font-bold text-slate-800">Reportes & KPIs</h1>
        <p className="text-slate-500 text-sm mt-1">Indicadores de gestión ganadera en tiempo real</p>
      </div>

      {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{error}</div>}

      {cargando ? (
        <div className="text-slate-400 text-center py-20">Cargando indicadores…</div>
      ) : (
        <>
          {/* ── KPIs principales ── */}
          <section>
            <h2 className="text-sm font-semibold text-slate-500 uppercase tracking-wider mb-4">Inventario Animal</h2>
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
              <Kpi icono="🐄" titulo="Total animales activos" valor={totalAnimales} color="text-brand-700" />
              <Kpi icono="♂️"  titulo="Machos" valor={machos} sub={`${100 - pctHembras}% del hato`} />
              <Kpi icono="♀️"  titulo="Hembras" valor={hembras} sub={`${pctHembras}% del hato`} color="text-pink-600" />
              <Kpi icono="🌿" titulo="Fincas activas" valor={totalFincas} />
            </div>
          </section>

          <section>
            <h2 className="text-sm font-semibold text-slate-500 uppercase tracking-wider mb-4">Producción & Salud</h2>
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
              <Kpi icono="🥛" titulo="Litros leche hoy" valor={litrosHoy > 0 ? litrosHoy.toFixed(1) : '—'} sub="Suma de controles del día" color="text-blue-600" />
              <Kpi icono="🤰" titulo="Gestaciones activas" valor={gestActivas > 0 ? gestActivas : '—'} color="text-purple-600" />
              <Kpi icono="💉" titulo="Vacunaciones próx." valor={vacPendientes > 0 ? vacPendientes : '✓'} sub="90 días" color={vacPendientes > 0 ? 'text-amber-600' : 'text-green-600'} />
              <Kpi icono="📋" titulo="Partos próximos" valor={proximosPartos.length > 0 ? proximosPartos.length : '—'} sub="30 días" color="text-orange-600" />
            </div>
          </section>

          {/* ── Partos próximos ── */}
          {proximosPartos.length > 0 && (
            <section>
              <h2 className="text-sm font-semibold text-slate-500 uppercase tracking-wider mb-4">Partos en los próximos 30 días</h2>
              <div className="card overflow-x-auto p-0">
                <table className="w-full text-sm">
                  <thead className="bg-slate-50 border-b border-slate-200">
                    <tr>
                      {['Animal', 'Fecha estimada', 'Días restantes'].map(h => (
                        <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
                      ))}
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {proximosPartos.map(g => {
                      const animal = animales.find(a => a.id === g.idAnimal);
                      return (
                        <tr key={g.id} className="hover:bg-slate-50">
                          <td className="px-4 py-3 font-medium text-slate-800">{animal?.nombre ?? animal?.codigo ?? g.idAnimal}</td>
                          <td className="px-4 py-3 text-slate-500">{g.fechaPartoEstimado}</td>
                          <td className="px-4 py-3">
                            <span className={`font-semibold ${g.dias <= 7 ? 'text-red-600' : g.dias <= 15 ? 'text-amber-600' : 'text-green-600'}`}>
                              {g.dias === 0 ? 'Hoy' : `${g.dias} días`}
                            </span>
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              </div>
            </section>
          )}

          {/* ── Distribución por finca ── */}
          {fincas.length > 0 && animales.length > 0 && (
            <section>
              <h2 className="text-sm font-semibold text-slate-500 uppercase tracking-wider mb-4">Distribución de animales por finca</h2>
              <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
                {fincas.map(f => {
                  const count = animales.filter(a => a.idFinca === f.id).length;
                  const pct   = totalAnimales > 0 ? Math.round((count / totalAnimales) * 100) : 0;
                  return (
                    <div key={f.id} className="card">
                      <div className="flex items-center justify-between mb-3">
                        <p className="font-semibold text-slate-800">{f.nombre}</p>
                        <span className="text-2xl font-bold text-brand-600">{count}</span>
                      </div>
                      <div className="w-full bg-slate-100 rounded-full h-2">
                        <div className="bg-brand-500 h-2 rounded-full transition-all" style={{ width: `${pct}%` }} />
                      </div>
                      <p className="text-xs text-slate-400 mt-1">{pct}% del inventario total</p>
                    </div>
                  );
                })}
              </div>
            </section>
          )}

          {/* ── Vacunaciones pendientes ── */}
          {vacunas.length > 0 && (
            <section>
              <h2 className="text-sm font-semibold text-slate-500 uppercase tracking-wider mb-4">Vacunaciones con alerta (90 días)</h2>
              <div className="card overflow-x-auto p-0">
                <table className="w-full text-sm">
                  <thead className="bg-slate-50 border-b border-slate-200">
                    <tr>
                      {['Animal', 'Última vacuna', 'Próxima fecha'].map(h => (
                        <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
                      ))}
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {vacunas.slice(0, 10).map(v => {
                      const animal = animales.find(a => a.id === v.idAnimal);
                      return (
                        <tr key={v.id} className="hover:bg-slate-50">
                          <td className="px-4 py-3 font-medium text-slate-800">{animal?.nombre ?? animal?.codigo ?? v.idAnimal}</td>
                          <td className="px-4 py-3 text-slate-500">{v.fecha}</td>
                          <td className="px-4 py-3">
                            {v.proximaFecha
                              ? <span className="text-amber-600 font-medium">{v.proximaFecha}</span>
                              : <span className="text-slate-400">No programada</span>}
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
                {vacunas.length > 10 && (
                  <p className="px-4 py-2 text-xs text-slate-400 border-t border-slate-100">
                    Mostrando 10 de {vacunas.length} alertas
                  </p>
                )}
              </div>
            </section>
          )}

          {/* Estado general */}
          {totalAnimales === 0 && totalFincas === 0 && (
            <div className="text-center py-16 text-slate-400">
              <p className="text-5xl mb-4">📊</p>
              <p className="font-medium text-slate-500">Sin datos suficientes para mostrar reportes</p>
              <p className="text-sm mt-1">Registra fincas, animales y datos productivos para ver los indicadores</p>
            </div>
          )}
        </>
      )}
    </div>
  );
}
