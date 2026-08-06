export interface SolicitudTabla {
  id: number;
  codigo: string;
  titulo: string;
  prioridad: 'Baja' | 'Media' | 'Alta';
  estado: 'Nueva'| 'Asignada' | 'En Proceso' | 'Resuelta' | 'Cerrada';
  fechaCreacion: Date;
}

export interface UsuarioOpcion {
  id: number;
  nombreCompleto: string;
}