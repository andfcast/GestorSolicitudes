export interface LoginDto {
  usuarioOrEmail: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  id: number;
  nombreUsuario: string;
  email: string;
  rol: string;
  expiracion: string;
}

export interface UserSession {
  id: number;
  nombreUsuario: string;
  email: string;
  rol: string;
}