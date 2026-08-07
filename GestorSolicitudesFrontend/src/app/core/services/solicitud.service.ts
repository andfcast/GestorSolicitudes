import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Solicitud, SolicitudDto, CrearSolicitudDto } from '../models/solicitud.models';

@Injectable({
  providedIn: 'root'
})
export class SolicitudService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}solicitudes`;

  getMisSolicitudes(estado?: string, prioridad?: string): Observable<SolicitudDto[]> {
    let params = new HttpParams();
    if (estado) params = params.set('estado', estado);
    if (prioridad) params = params.set('prioridad', prioridad);

    return this.http.get<SolicitudDto[]>(`${this.apiUrl}/filtro`, { params });
  }

    getTodasSolicitudes(estado?: string, prioridad?: string): Observable<SolicitudDto[]> {
    let params = new HttpParams();
    if (estado) params = params.set('estado', estado);
    if (prioridad) params = params.set('prioridad', prioridad);

    return this.http.get<SolicitudDto[]>(`${this.apiUrl}/todas`, { params });
  }

  getDetalleSolicitud(id: number): Observable<SolicitudDto> {
    return this.http.get<SolicitudDto>(`${this.apiUrl}/${id}`);
}

  // HU-02: Cambiar estado
  cambiarEstado(id: number, nuevoEstado: string): Observable<{ mensaje: string }> {
    return this.http.patch<{ mensaje: string }>(`${this.apiUrl}/${id}/estado`, { nuevoEstado });
  }

  // HU-02: Crear solicitud
  crearSolicitud(dto: CrearSolicitudDto): Observable<SolicitudDto> {
    return this.http.post<SolicitudDto>(this.apiUrl, dto);
  }

  actualizarSolicitud(id: number, dto: Partial<CrearSolicitudDto>): Observable<{ mensaje: string }> {
    return this.http.put<{ mensaje: string }>(`${this.apiUrl}/${id}`, dto);
 }

 asignarResponsable(id: number, usuarioResponsableId: number): Observable<{ mensaje: string }> {
    return this.http.patch<{ mensaje: string }>(`${this.apiUrl}/${id}/asignar/${usuarioResponsableId}`, {});
  }

 borrarSolicitud(id: number): Observable<{ mensaje: string }> {
    return this.http.delete<{ mensaje: string }>(`${this.apiUrl}/${id}`);
  }
}