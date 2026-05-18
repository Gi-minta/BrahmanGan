import { useEffect, useState, type FormEvent } from 'react';
import { api, type Medicamento, type Vacunacion } from '../api/client';
import Modal from '../components/Modal';

const hoy = new Date().toISOString().slice(0, 10);

interface VacunaForm {
  idAnimal: string; idMedicamento: string; fecha: string;
  dosis: string; lote: string; responsable: string; proximaFecha: string;
}
const FORM_VACIO: VacunaForm = {
  idAnimal: '', idMedicamento: '', fecha: hoy,
  dosis: '', lote: '', responsable: '', proximaFecha: '',
};

export default function SanidadPage() {
  const [medicamentos, setMedicamentos] = useState<Medicamento[]>([]);
  const [alertas, setAlertas]           = useState<Vacunacion[]>([]);
  const [cargando, setCargando]         = useState(true);
  const [error, setError]               = useState('');

  const [modal, setModal]       = useState(false);
  const [form, setForm]         = useState<VacunaForm>(FORM_VACIO);
  const [guardando, setGuard]   = useState(false);
  const [errForm, setErrForm]   = useState('');
  const [okForm, setOkForm]     = useState('');

  useEffect(() => {
    Promise.all([
      api.get<Medicamento[]>('/medicamentos'),
      api.get<Vacunacion[]>('/vacunaciones/alertas?dias=30'),
    ])
      .then(([meds, alts]) => { setMedicamentos(meds); setAlertas(alts); })
      .catch(e => setError(e.message))
      .finally(() => setCargando(false));
  }, []);

  function set(f: keyof VacunaForm, v: string) { setForm(p => ({ ...p, [f]: v })); }

  async function handleGuardar(e: FormEvent) {
    e.preventDefault();
    setErrForm(''); setOkForm(''); setGuard(true);
    try {
      const nueva = await api.post<Vacunacion>('/vacunaciones', {
        idAnimal:      Number(form.idAnimal),
        idMedicamento: Number(form.idMedicamento),
        fecha:         form.fecha,
        dosis:         form.dosis ? Number(form.dosis) : undefined,
        lote:          form.lote || undefined,
        responsable:   form.responsable || undefined,
        proximaFecha:  form.proximaFecha || undefined,
      });
      // Si tiene próxima fecha, puede aparecer en alertas
      if (nueva.proximaFecha) setAlertas(prev => [nueva, ...prev]);
      setModal(false);
      setForm(FORM_VACIO);
    } catch (err: unknown) {
      setErrForm(err instanceof Error ? err.message : 'Error al guardar');
    } finally { setGuard(false); }
  }

  const diasRestantes = (fecha?: string) => {
    if (!fecha) return null;
    return Math.ceil((new Date(fecha).getTime() - Date.now()) / 86_400_000);
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">Sanidad Animal</h1>
          <p className="text-slate-500 text-sm mt-1">Vacunaciones y catálogo de medicamentos</p>
        </div>
        <button className="btn-primary" onClick={() => setModal(true)}>
          + Registrar vacunación
        </button>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-4 text-sm">{error}</div>
      )}

      {cargando ? (
        <div className="text-slate-400 text-center py-16">Cargando datos de sanidad…</div>
      ) : (
        <div className="grid grid-cols-1 xl:grid-cols-2 gap-6">

          {/* Alertas de vacunación */}
          <div className="card p-0 overflow-hidden">
            <div className="px-5 py-4 border-b border-slate-100 flex items-center gap-2">
              <span className="text-xl">⚠️</span>
              <div>
                <p className="font-semibold text-slate-800">Alertas de vacunación</p>
                <p className="text-xs text-slate-500">Próximas 30 días</p>
              </div>
              <span className="ml-auto bg-amber-100 text-amber-700 text-xs font-semibold px-2 py-0.5 rounded-full">
                {alertas.length}
              </span>
            </div>
            {alertas.length === 0 ? (
              <div className="text-center py-10 text-slate-400 text-sm">Sin alertas pendientes</div>
            ) : (
              <div className="divide-y divide-slate-100 max-h-96 overflow-y-auto">
                {alertas.map(v => {
                  const dias = diasRestantes(v.proximaFecha);
                  return (
                    <div key={v.id} className="px-5 py-3 flex items-center justify-between">
                      <div>
                        <p className="text-sm font-medium text-slate-700">Animal #{v.idAnimal}</p>
                        <p className="text-xs text-slate-500">
                          Med. #{v.idMedicamento}
                          {v.lote && <span className="ml-2">Lote: {v.lote}</span>}
                          {v.dosis && <span className="ml-2">{v.dosis} ml</span>}
                        </p>
                      </div>
                      <div className="text-right">
                        <p className="text-xs text-slate-500">Próxima</p>
                        <p className={`text-sm font-semibold ${
                          dias != null && dias <= 0  ? 'text-red-700'
                          : dias != null && dias <= 7  ? 'text-red-600'
                          : dias != null && dias <= 15 ? 'text-amber-600'
                          : 'text-slate-700'
                        }`}>
                          {v.proximaFecha ? new Date(v.proximaFecha).toLocaleDateString('es-CO') : '—'}
                          {dias != null && (
                            <span className="ml-1 text-xs font-normal">
                              ({dias <= 0 ? 'vencida' : `${dias}d`})
                            </span>
                          )}
                        </p>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </div>

          {/* Catálogo de medicamentos */}
          <div className="card p-0 overflow-hidden">
            <div className="px-5 py-4 border-b border-slate-100 flex items-center gap-2">
              <span className="text-xl">💊</span>
              <div>
                <p className="font-semibold text-slate-800">Medicamentos</p>
                <p className="text-xs text-slate-500">Catálogo activo</p>
              </div>
              <span className="ml-auto bg-blue-100 text-blue-700 text-xs font-semibold px-2 py-0.5 rounded-full">
                {medicamentos.filter(m => m.activo).length}
              </span>
            </div>
            {medicamentos.length === 0 ? (
              <div className="text-center py-10 text-slate-400 text-sm">Sin medicamentos registrados</div>
            ) : (
              <div className="overflow-x-auto max-h-96 overflow-y-auto">
                <table className="w-full text-sm">
                  <thead className="bg-slate-50 sticky top-0">
                    <tr>
                      {['Código', 'Nombre', 'Carne', 'Leche'].map(h => (
                        <th key={h} className="px-4 py-2.5 text-left text-xs font-semibold text-slate-600">{h}</th>
                      ))}
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-slate-100">
                    {medicamentos.map(m => (
                      <tr key={m.id} className={`hover:bg-slate-50 ${!m.activo ? 'opacity-40' : ''}`}>
                        <td className="px-4 py-2.5 font-mono text-brand-700 text-xs">{m.codigo}</td>
                        <td className="px-4 py-2.5 text-slate-700">{m.nombre}</td>
                        <td className="px-4 py-2.5 text-slate-500 text-xs">
                          {m.tiempoCarne != null ? `${m.tiempoCarne}d` : '—'}
                        </td>
                        <td className="px-4 py-2.5 text-slate-500 text-xs">
                          {m.tiempoLeche != null ? `${m.tiempoLeche}d` : '—'}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        </div>
      )}

      {/* Modal registrar vacunación */}
      {modal && (
        <Modal titulo="Registrar vacunación" onClose={() => { setModal(false); setErrForm(''); setForm(FORM_VACIO); }}>
          <form onSubmit={handleGuardar} className="space-y-4">
            {errForm && <div className="bg-red-50 border border-red-200 text-red-700 rounded-lg p-3 text-sm">{errForm}</div>}
            {okForm  && <div className="bg-green-50 border border-green-200 text-green-700 rounded-lg p-3 text-sm">✓ {okForm}</div>}

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">ID Animal *</label>
                <input className="input" type="number" min="1" required value={form.idAnimal}
                  onChange={e => set('idAnimal', e.target.value)} placeholder="ID del animal" />
              </div>
              <div>
                <label className="label">Medicamento *</label>
                <select className="input" required value={form.idMedicamento}
                  onChange={e => set('idMedicamento', e.target.value)}>
                  <option value="">Seleccionar…</option>
                  {medicamentos.filter(m => m.activo).map(m => (
                    <option key={m.id} value={m.id}>{m.nombre}</option>
                  ))}
                </select>
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Fecha aplicación *</label>
                <input className="input" type="date" required value={form.fecha}
                  onChange={e => set('fecha', e.target.value)} />
              </div>
              <div>
                <label className="label">Próxima aplicación</label>
                <input className="input" type="date" value={form.proximaFecha}
                  onChange={e => set('proximaFecha', e.target.value)} />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="label">Dosis (ml)</label>
                <input className="input" type="number" step="0.1" min="0" value={form.dosis}
                  onChange={e => set('dosis', e.target.value)} placeholder="Ej: 5.0" />
              </div>
              <div>
                <label className="label">Lote</label>
                <input className="input" value={form.lote}
                  onChange={e => set('lote', e.target.value)} placeholder="Número de lote" />
              </div>
            </div>

            <div>
              <label className="label">Responsable</label>
              <input className="input" value={form.responsable}
                onChange={e => set('responsable', e.target.value)} placeholder="Nombre del veterinario" />
            </div>

            <div className="flex justify-end gap-3 pt-2">
              <button type="button" className="btn-secondary"
                onClick={() => { setModal(false); setForm(FORM_VACIO); }}>Cancelar</button>
              <button type="submit" className="btn-primary" disabled={guardando}>
                {guardando ? 'Guardando…' : 'Registrar vacunación'}
              </button>
            </div>
          </form>
        </Modal>
      )}
    </div>
  );
}
