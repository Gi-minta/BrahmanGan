import { useEffect, useState, type FormEvent } from 'react';
import { api } from '../api/client';
import Modal from '../components/Modal';

// ── Tipos ──────────────────────────────────────────────────────
type Maquinaria = { id: number; idCentro: number; codigo: string; nombre: string; marca?: string; modelo?: string; anio?: number; numeroSerie?: string; fechaCompra?: string; valorCompra?: number; horasUso: number; estado: string };
type Mantenimiento = { id: number; idMaquinaria: number; fecha: string; tipoMantenimiento: string; descripcion: string; tecnico?: string; proveedor?: string; costoManoObra: number; costoRepuestos: number; costoTotal: number; proximoMantenimiento?: string; horasAlMomento?: number };
type CentroCosto = { id: number; nombre: string };

const ESTADO_COLOR: Record<string, string> = {
  OPERATIVO: 'bg-green-100 text-green-700',
  MANTENIMIENTO: 'bg-yellow-100 text-yellow-700',
  BAJA: 'bg-red-100 text-red-700',
};

export default function EquiposPage() {
  const [maquinaria, setMaquinaria] = useState<Maquinaria[]>([]);
  const [centros, setCentros]       = useState<CentroCosto[]>([]);
  const [mantenimientos, setMantenimientos] = useState<Mantenimiento[]>([]);
  const [maqSel, setMaqSel]         = useState<Maquinaria | null>(null);
  const [cargando, setCargando]     = useState(false);
  const [error, setError]           = useState('');
  const [modalMaq, setModalMaq]     = useState(false);
  const [modalMant, setModalMant]   = useState(false);
  const [guardando, setGuardando]   = useState(false);
  const [errForm, setErrForm]       = useState('');

  const hoy = new Date().toISOString().slice(0, 10);

  const [fMaq, setFMaq] = useState({ idCentro: '', codigo: '', nombre: '', marca: '', modelo: '', anio: '', numeroSerie: '', fechaCompra: '', valorCompra: '' });
  const [fMant, setFMant] = useState({ idMaquinaria: '', fecha: hoy, tipoMantenimiento: 'PREVENTIVO', descripcion: '', tecnico: '', costoManoObra: '0', costoRepuestos: '0' });

  useEffect(() => {
    setCargando(true);
    Promise.all([
      api.get<Maquinaria[]>('/maquinaria').then(setMaquinaria),
      api.get<CentroCosto[]>('/centros-costo').then(setCentros),
    ]).catch(e => setError(e.message)).finally(() => setCargando(false));
  }, []);

  async function verMantenimientos(maq: Maquinaria) {
    setMaqSel(maq);
    const m = await api.get<Mantenimiento[]>(`/maquinaria/${maq.id}/mantenimientos`);
    setMantenimientos(m);
  }

  async function guardarMaquinaria(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<Maquinaria>('/maquinaria', {
        idCentro: parseInt(fMaq.idCentro), codigo: fMaq.codigo, nombre: fMaq.nombre,
        marca: fMaq.marca || null, modelo: fMaq.modelo || null,
        anio: fMaq.anio ? parseInt(fMaq.anio) : null,
        numeroSerie: fMaq.numeroSerie || null,
        fechaCompra: fMaq.fechaCompra || null,
        valorCompra: fMaq.valorCompra ? parseFloat(fMaq.valorCompra) : null,
      });
      setMaquinaria(prev => [nuevo, ...prev]);
      setModalMaq(false); setFMaq({ idCentro: '', codigo: '', nombre: '', marca: '', modelo: '', anio: '', numeroSerie: '', fechaCompra: '', valorCompra: '' });
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  async function guardarMantenimiento(e: FormEvent) {
    e.preventDefault(); setGuardando(true); setErrForm('');
    try {
      const nuevo = await api.post<Mantenimiento>('/maquinaria/mantenimiento', {
        idMaquinaria: parseInt(fMant.idMaquinaria), fecha: fMant.fecha,
        tipoMantenimiento: fMant.tipoMantenimiento, descripcion: fMant.descripcion,
        tecnico: fMant.tecnico || null,
        costoManoObra: parseFloat(fMant.costoManoObra),
        costoRepuestos: parseFloat(fMant.costoRepuestos),
      });
      setMantenimientos(prev => [nuevo, ...prev]);
      setModalMant(false); setFMant({ idMaquinaria: '', fecha: hoy, tipoMantenimiento: 'PREVENTIVO', descripcion: '', tecnico: '', costoManoObra: '0', costoRepuestos: '0' });
    } catch (err: unknown) { setErrForm(err instanceof Error ? err.message : 'Error'); }
    finally { setGuardando(false); }
  }

  const fmt = (v: number) => v.toLocaleString('es-CO', { style: 'currency', currency: 'COP', maximumFractionDigits: 0 });

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Equipos & Maquinaria</h1>
          <p className="text-slate-500 text-sm mt-1">Inventario y mantenimiento de maquinaria agrícola</p>
        </div>
        <div className="flex gap-2">
          <button onClick={() => setModalMant(true)} className="btn-secondary">🔧 Mantenimiento</button>
          <button onClick={() => setModalMaq(true)} className="btn-primary">+ Equipo</button>
        </div>
      </div>

      {error && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{error}</div>}

      {maqSel ? (
        <div className="space-y-4">
          <div className="flex items-center gap-3">
            <button onClick={() => setMaqSel(null)} className="text-brand-600 hover:text-brand-700 text-sm">← Volver</button>
            <h2 className="font-semibold text-slate-800">Mantenimientos: {maqSel.nombre}</h2>
            <span className="ml-auto text-sm text-slate-500">{maqSel.horasUso.toFixed(1)} horas de uso</span>
          </div>
          <div className="card overflow-x-auto p-0">
            <table className="w-full text-sm">
              <thead className="bg-slate-50 border-b border-slate-200">
                <tr>{['Fecha','Tipo','Descripción','Técnico','Costo M.O.','Costo Rep.','Total','Próximo'].map(h => (
                  <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold">{h}</th>
                ))}</tr>
              </thead>
              <tbody className="divide-y divide-slate-100">
                {mantenimientos.map(m => (
                  <tr key={m.id} className="hover:bg-slate-50">
                    <td className="px-4 py-3 text-slate-500">{m.fecha}</td>
                    <td className="px-4 py-3"><span className="text-xs px-2 py-0.5 rounded-full bg-blue-100 text-blue-700">{m.tipoMantenimiento}</span></td>
                    <td className="px-4 py-3 text-slate-800 max-w-[180px] truncate">{m.descripcion}</td>
                    <td className="px-4 py-3 text-slate-500">{m.tecnico ?? '—'}</td>
                    <td className="px-4 py-3 font-mono">{fmt(m.costoManoObra)}</td>
                    <td className="px-4 py-3 font-mono">{fmt(m.costoRepuestos)}</td>
                    <td className="px-4 py-3 font-mono font-semibold">{fmt(m.costoTotal)}</td>
                    <td className="px-4 py-3 text-slate-400">{m.proximoMantenimiento ?? '—'}</td>
                  </tr>
                ))}
                {mantenimientos.length === 0 && <tr><td colSpan={8} className="px-4 py-8 text-slate-400 text-center">Sin mantenimientos registrados</td></tr>}
              </tbody>
            </table>
          </div>
        </div>
      ) : (
        cargando ? <div className="text-slate-400 text-center py-12">Cargando…</div> : (
          <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
            {maquinaria.map(m => (
              <div key={m.id} className="card hover:shadow-md transition-shadow cursor-pointer" onClick={() => verMantenimientos(m)}>
                <div className="flex items-start justify-between mb-3">
                  <div>
                    <p className="font-semibold text-slate-800">{m.nombre}</p>
                    <p className="text-xs text-slate-400 font-mono">{m.codigo}</p>
                    {m.marca && <p className="text-sm text-slate-500 mt-0.5">{m.marca} {m.modelo} {m.anio && `(${m.anio})`}</p>}
                  </div>
                  <span className={`text-xs px-2 py-0.5 rounded-full ${ESTADO_COLOR[m.estado] ?? 'bg-slate-100 text-slate-500'}`}>{m.estado}</span>
                </div>
                <div className="flex items-center justify-between text-sm">
                  <span className="text-slate-500">Horas uso: <span className="font-mono font-semibold text-slate-700">{m.horasUso.toFixed(1)}</span></span>
                  <span className="text-brand-600 text-xs underline">Ver mantenimientos →</span>
                </div>
              </div>
            ))}
            {maquinaria.length === 0 && <p className="text-slate-400 col-span-3 text-center py-8">Sin equipos registrados</p>}
          </div>
        )
      )}

      {/* Modal nuevo equipo */}
      {modalMaq && (
        <Modal titulo="Registrar Equipo" onClose={() => setModalMaq(false)}>
          <form onSubmit={guardarMaquinaria} className="space-y-4">
            {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
            <div><label className="label">Centro de costo *</label>
              <select className="input-field" value={fMaq.idCentro} onChange={e => setFMaq(p => ({...p, idCentro: e.target.value}))} required>
                <option value="">Seleccionar…</option>
                {centros.map(c => <option key={c.id} value={c.id}>{c.nombre}</option>)}
              </select></div>
            <div className="grid grid-cols-2 gap-4">
              <div><label className="label">Código *</label>
                <input className="input-field" value={fMaq.codigo} onChange={e => setFMaq(p => ({...p, codigo: e.target.value}))} required /></div>
              <div><label className="label">Nombre *</label>
                <input className="input-field" value={fMaq.nombre} onChange={e => setFMaq(p => ({...p, nombre: e.target.value}))} required /></div>
              <div><label className="label">Marca</label>
                <input className="input-field" value={fMaq.marca} onChange={e => setFMaq(p => ({...p, marca: e.target.value}))} /></div>
              <div><label className="label">Modelo</label>
                <input className="input-field" value={fMaq.modelo} onChange={e => setFMaq(p => ({...p, modelo: e.target.value}))} /></div>
              <div><label className="label">Año</label>
                <input type="number" min="1900" max="2030" className="input-field" value={fMaq.anio} onChange={e => setFMaq(p => ({...p, anio: e.target.value}))} /></div>
              <div><label className="label">Fecha compra</label>
                <input type="date" className="input-field" value={fMaq.fechaCompra} onChange={e => setFMaq(p => ({...p, fechaCompra: e.target.value}))} /></div>
              <div className="col-span-2"><label className="label">Valor compra (COP)</label>
                <input type="number" min="0" className="input-field" value={fMaq.valorCompra} onChange={e => setFMaq(p => ({...p, valorCompra: e.target.value}))} /></div>
            </div>
            <div className="flex gap-3 justify-end pt-2">
              <button type="button" onClick={() => setModalMaq(false)} className="btn-secondary">Cancelar</button>
              <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Guardar'}</button>
            </div>
          </form>
        </Modal>
      )}

      {/* Modal mantenimiento */}
      {modalMant && (
        <Modal titulo="Registrar Mantenimiento" onClose={() => setModalMant(false)}>
          <form onSubmit={guardarMantenimiento} className="space-y-4">
            {errForm && <p className="text-red-600 text-sm">{errForm}</p>}
            <div><label className="label">Equipo *</label>
              <select className="input-field" value={fMant.idMaquinaria} onChange={e => setFMant(p => ({...p, idMaquinaria: e.target.value}))} required>
                <option value="">Seleccionar…</option>
                {maquinaria.map(m => <option key={m.id} value={m.id}>{m.nombre} ({m.codigo})</option>)}
              </select></div>
            <div className="grid grid-cols-2 gap-4">
              <div><label className="label">Fecha *</label>
                <input type="date" className="input-field" value={fMant.fecha} onChange={e => setFMant(p => ({...p, fecha: e.target.value}))} required /></div>
              <div><label className="label">Tipo *</label>
                <select className="input-field" value={fMant.tipoMantenimiento} onChange={e => setFMant(p => ({...p, tipoMantenimiento: e.target.value}))}>
                  <option value="PREVENTIVO">Preventivo</option>
                  <option value="CORRECTIVO">Correctivo</option>
                  <option value="PREDICTIVO">Predictivo</option>
                </select></div>
            </div>
            <div><label className="label">Descripción *</label>
              <textarea className="input-field" rows={2} value={fMant.descripcion} onChange={e => setFMant(p => ({...p, descripcion: e.target.value}))} required /></div>
            <div className="grid grid-cols-2 gap-4">
              <div><label className="label">Técnico</label>
                <input className="input-field" value={fMant.tecnico} onChange={e => setFMant(p => ({...p, tecnico: e.target.value}))} /></div>
              <div><label className="label">Costo mano de obra</label>
                <input type="number" min="0" className="input-field" value={fMant.costoManoObra} onChange={e => setFMant(p => ({...p, costoManoObra: e.target.value}))} /></div>
              <div><label className="label">Costo repuestos</label>
                <input type="number" min="0" className="input-field" value={fMant.costoRepuestos} onChange={e => setFMant(p => ({...p, costoRepuestos: e.target.value}))} /></div>
            </div>
            <div className="flex gap-3 justify-end pt-2">
              <button type="button" onClick={() => setModalMant(false)} className="btn-secondary">Cancelar</button>
              <button type="submit" disabled={guardando} className="btn-primary">{guardando ? 'Guardando…' : 'Registrar'}</button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
