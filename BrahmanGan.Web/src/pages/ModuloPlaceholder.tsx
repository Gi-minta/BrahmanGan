interface Props {
  icon: string;
  titulo: string;
  descripcion: string;
  items?: string[];
}

export default function ModuloPlaceholder({ icon, titulo, descripcion, items }: Props) {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-slate-800">{titulo}</h1>
        <p className="text-slate-500 text-sm mt-1">{descripcion}</p>
      </div>

      <div className="card">
        <div className="flex flex-col items-center text-center py-10 border-b border-slate-100 mb-6">
          <div className="w-16 h-16 rounded-2xl bg-brand-50 flex items-center justify-center text-4xl mb-4">
            {icon}
          </div>
          <p className="text-slate-400 text-sm">Módulo en desarrollo</p>
        </div>

        {items && items.length > 0 && (
          <div>
            <p className="text-xs font-semibold text-slate-500 uppercase tracking-wide mb-3">
              Funcionalidades planificadas
            </p>
            <ul className="grid grid-cols-1 sm:grid-cols-2 gap-2">
              {items.map(i => (
                <li key={i} className="flex items-start gap-2 text-sm text-slate-600">
                  <span className="w-1.5 h-1.5 rounded-full bg-brand-400 flex-shrink-0 mt-1.5" />
                  {i}
                </li>
              ))}
            </ul>
          </div>
        )}
      </div>
    </div>
  );
}
