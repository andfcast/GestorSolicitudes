import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UsuarioListaDto } from '../models/usuario.models';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}usuarios`;

  
 getTodosUsuarios(nombre: string, rol?: string): Observable<UsuarioListaDto[]> {
    let params = new HttpParams();
    if (nombre) params = params.set('nombre', nombre);
    if (rol) params = params.set('rol', rol);

    return this.http.get<UsuarioListaDto[]>(`${this.apiUrl}/filtro`, { params });
  }

}