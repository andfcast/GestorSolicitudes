export interface LoginDto {
  usuarioOrEmail: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  nombreUsuario: string;
  email: string;
  rol: string;
  expiracion: string;
}

export interface UserSession {
  nombreUsuario: string;
  email: string;
  rol: string;
}