export interface LoginDto {
  email: string;
  password: string;
}

export interface SesionUsuario {
  token: string;
  mensaje: string;
  nombre: string;
  usuarioId: string;
  email: string;
  rol: 'Agente' | 'Admin'; 
}