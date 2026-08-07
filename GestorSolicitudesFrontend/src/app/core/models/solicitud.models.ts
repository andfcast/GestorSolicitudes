export interface Solicitud {
  id: number;
  codigo: string;
  titulo: string;
  descripcion: string;
  prioridad: string;
  estado: string;
  fechaCreacion: string;
  fechaCierre?: string;
}

export interface SolicitudDto {
  id: number;
  codigo: string;
  titulo: string;
  descripcion: string;
  cliente?: string;
  prioridad: string;
  estado: string;
  fechaCreacion: string;
  fechaCierre?: string;
  usuarioResponsableId?: number;
  nombreUsuarioResponsable?: string;
}

export interface CrearSolicitudDto {
  titulo: string;
  descripcion: string;
  cliente: string;
  prioridad: string;
  usuarioResponsableId?: number;
}