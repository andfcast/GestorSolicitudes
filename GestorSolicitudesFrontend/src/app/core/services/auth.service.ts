import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginDto, LoginResponse, UserSession } from '../models/auth.models';
import { environment } from '../../../environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = `${environment.apiUrl}auth`; 
  

  currentUser = signal<UserSession | null>(this.getUserFromStorage());

  login(credentials: LoginDto): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap((response) => {
        // Guardar token y datos de usuario
        localStorage.setItem('jwt_token', response.token);
        const session: UserSession = {
          nombreUsuario: response.nombreUsuario,
          email: response.email,
          rol: response.rol
        };
        localStorage.setItem('user_session', JSON.stringify(session));
        this.currentUser.set(session);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_session');
    this.currentUser.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  private getUserFromStorage(): UserSession | null {
    const data = localStorage.getItem('user_session');
    return data ? JSON.parse(data) : null;
  }

  
}