import { useState, type FormEvent } from 'react';
import { api, type ControlLeche } from '../api/client';
import Modal from '../components/Modal';

const hoy = new Date().toISOString().slice(0, 10);
const hace30 = new Date(Date.now() - 30 * 86_400_000).toISOString().slice(0, 10);

interface RegistroForm {
  idAnimal: string; fecha: string;
  maniana: string; tarde: string; noche: string; ordeno: string;
}
const FORM_VACIO: RegistroForm = {
  idAnimal: '', fecha: hoy,
  maniana: '', tarde: '', noche: '', ordeno: '',
};

export default function LechePage() {
  // Consulta
  const [idAnimal, setIdAnimal]   = useState('');
  const [desde, setDesde]         = useState(hace30);
  const [hasta, setHasta]         = useState(hoy);
  const [registros, setRegistros] = useState<ControlLeche[] | null>(null);
  const [buscando, setBuscando]   = useState(false);
  const [errBusq, setErrBusq]     = useState('');

  // Modal registro
  const [modal, setModal]       = useState(false);
  const [form, setForm]         = useState<RegistroForm>(FORM_VACIO);
  const [guardando, setGuard]   = useState(false);
  const [errForm, setErrForm]   = useState('');

  function set(f: keyof RegistroForm, v: string) { setForm(p => ({ ...p, [f]: v })); }

  async function buscar(e: FormEvent) {
    e.preventDefault();
    if (!idAnimal) return;
    setErrBusq(''); setBuscando(true); setRegistros(null);
    try {
      const lista = await api.get<ControlLeche[]>(
        `/control-leche/animal/${idAnimal}?desde=${desde}&hasta=${hasta}`
      );
      setRegistros(lista);
    } catch (err: unknown) {
      setErrBusq(err instanceof Error ? err.message : 'Error al buscar');
    } finally { setBuscando(false); }
  }

  async function handleGuardar(e: FormEvent) {
    e.preventDefault();
    setErrForm(''); setGuard(true);
    try {
      const nuevo = await api.post<ControlLeche>('/control-leche', {
        idAnimal: Number(form.idAnimal),
        fecha:    form.fecha,
        maniana:  form.maniana  ? Number(form.maniana)  : undefined,
        tarde:    form.tarde    ? Number(form.tarde)    : undefined,
        noche:    form.noche    ? Number(form.noche)    : undefined,
        ordeno:   form.ordeno   || undefined,
      });
      // Si el animal buscado coincide, agregarlo a la lista
      if (String(nuevo.idAnimal) === idAnimal) {
        setRegistros(prev => prev ? [nuevo, ...prev] : [nuevo]);
      }
      setModal(false);
      setForm(FORM_VACIO);
    } catch (err: unknown) {
      setErrForm(err instanceof Error ? err.message : 'Error al guardar');
    } finally { setGuard(false); }
  }

  const totalPeriodo  = registros?.reduce((a, r) => a + r.total, 0) ?? 0;
  const promediodia   = registros?.length ? (totalPeriodo / registros.length).toFixed(1) : '—';

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Producción de Leche</h1>
          <p className="text-slate-500 text-sm mt-1">Registro y consulta de controles de ordeño</p>
        </div>
        <button className="btn-primary" onClick={() => setModal(true)}>
          + Registrar control
        </button>
      </div>

      {/* Filtros consulta */}
      <div className="card">
        <form onSubmit={buscar} className="flex flex-wrap gap-3 items-end">
          <div className="flex-1 min-w-[130px]">
            <label className="label">ID del animal</label>
            <input className="input" type="number" placeholder="Ej: 1" value={idAnimal}
              onChange={e => setIdAnimal(e.target.value)} min={1} required />
          </div>
          <div className="flex-1 min-w-[130px]">
            <label className="label">Desde</label>
            <input className="input" type="date" value={desde} onChange={e => setDesde(e.target.value)} />
          </div>
          <div className="flex-1 min-w-[130px]">
            <label className="label">Hasta</label>
            <input className="input" type="date" value={hasta} onChange={e => setHasta(e.target.value)} />
          </div>
          <button type="submit" disabled={buscando} className="btn-primary h-[42px] px-6">
            {buscando ? 'Buscando…' : 'Consultar'}
          </button>
        </form>
      </div>

      {errBusq && (
        <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-4 text-sm">{errBusq}</div>
      )}

      {registros !== null && (
        <>
          {/* KPIs */}
          <div className="grid grid-cols-3 gap-4">
            <div className="card text-center py-4">
              <p className="text-2xl font-bold text-brand-700">{registros.length}</p>
              <p className="text-xs text-slate-500 mt-1">Registros</p>
            </div>
            <div className="card text-center py-4">
              <p className="text-2xl font-bold text-blue-700">{totalPeriodo.toFixed(1)} L</p>
              <p className="text-xs text-slate-500 mt-1">Total período</p>
            </div>
            <div className="card text-center py-4">
              <p className="text-2xl font-bold text-emerald-700">{promediodia} L</p>
              <p className="text-xs text-slate-500 mt-1">Promedio / día</p>
            </div>
          </div>

          {registros.length === 0 ? (
            <div className="card text-center py-12 text-slate-400">
              <p className="text-3xl mb-2">🥛</p>
              <p>Sin registros para este período</p>
            </div>
          ) : (
            <div className="card overflow-x-auto p-0">
              <table className="w-full text-sm">
                <thead className="bg-slate-50 border-b border-slate-200">
                  <tr>
                    {['Fecha', 'Mañana (L)', 'Tarde (L)', 'Noche (L)', 'Total (L)'].map(h => (
                      <th key={h} className="px-4 py-3 text-left text-slate-600 font-semibold text-xs">{h}</th>
                    ))}
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {registros.map(r => (
                    <tr key={r.id} className="hover:bg-slate-50">
                      <td className="px-4 py-2.5 font-medium text-slate-700">
                        {new Date(r.fecha).toLocaleDateString('es-CO')}
                      </td>
                      <td className="px-4 py-2.5 text-slate-500">{r.maniana?.toFixed(1) ?? '—'}</td>
                      <td className="px-4 py-2.5 text-slate-500">{r.tarde?.toFixed(1)   ?? '—'}</td>
                      <td className="px-4 py-2.5 text-slate-500">{r.noche?.toFixed(1)   ?? '—'}</td>
                      <td className="px-4 py-2.5 font-semibold text-brand-700">{r.total.toFixed(1)}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </>
      )}

      {/* Modal registrar control */}
      {modal && (
        <Modal titulo="Registrar control de leche" onClose={() => { setModal(false); setErrForm(''); setForm(FORM_VACIO); }}>
          <form onSubmit={handleGuardar} className="space-y-4">
            {errForm && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errForm}</div>}

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">ID Animal *</label>
                <input className="input" type="number" min="1" required value={form.idAnimal}
                  onChange={e => set('idAnimal', e.target.value)} placeholder="ID del animal" />
              </div>
              <div>
                <label className="label">Fecha *</label>
                <input className="input" type="date" required value={form.fecha}
                  onChange={e => set('fecha', e.target.value)} />
              </div>
            </div>

            <div>
              <p className="text-sm font-medium text-slate-700 mb-2">Litros por ordeño</p>
              <div className="grid grid-cols-3 gap-3">
                <div>
                  <label className="label text-xs">Mañana</label>
                  <input className="input" type="number" step="0.1" min="0" value={form.maniana}
                    onChange={e => set('maniana', e.target.value)} placeholder="0.0" />
                </div>
                <div>
                  <label className="label text-xs">Tarde</label>
                  <input className="input" type="number" step="0.1" min="0" value={form.tarde}
                    onChange={e => set('tarde', e.target.value)} placeholder="0.0" />
                </div>
                <div>
                  <label className="label text-xs">Noche</label>
                  <input className="input" type="number" step="0.1" min="0" value={form.noche}
                    onChange={e => set('noche', e.target.value)} placeholder="0.0" />
                </div>
              </div>
            </div>

            <div>
              <label className="label">Ordeñador</label>
              <input className="input" value={form.ordeno}
                onChange={e => set('ordeno', e.target.value)} placeholder="Nombre del ordeñador" />
            </div>

            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary"
                onClick={() => { setModal(false); setForm(FORM_VACIO); }}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardando}>
                {guardando ? 'Guardando…' : 'Registrar control'}
              </button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
