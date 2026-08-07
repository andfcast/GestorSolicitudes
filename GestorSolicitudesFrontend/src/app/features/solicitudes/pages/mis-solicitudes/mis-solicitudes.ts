import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SolicitudService } from '../../../../core/services/solicitud.service';
import { Solicitud, SolicitudDto } from '../../../../core/models/solicitud.models';
import { CrearSolicitudModal } from "../../components/crear-solicitud-modal/crear-solicitud-modal";
import { FormsModule } from '@angular/forms';
import { DetalleSolicitudModal } from '../../components/detalle-solicitud-modal/detalle-solicitud-modal';

@Component({
  selector: 'app-mis-solicitudes',
  imports: [CommonModule, FormsModule, CrearSolicitudModal, DetalleSolicitudModal],
  templateUrl: './mis-solicitudes.html',
  styleUrl: './mis-solicitudes.scss',
})
export class MisSolicitudes implements OnInit {
  private solicitudService = inject(SolicitudService);

  // Estados Reactivos con Signals
  solicitudes = signal<SolicitudDto[]>([]);
  isLoading = signal<boolean>(true);
  errorMessage = signal<string | null>(null);
  solicitudSeleccionadaId = signal<number | null>(null);
  solicitudAEditar = signal<SolicitudDto | null>(null);

  filtroEstado = signal<string>('');
  filtroPrioridad = signal<string>('');

  mostrarModalCrear = signal<boolean>(false);
  mostrarModalDetalle = signal<boolean>(false);

  ngOnInit(): void {
    this.cargarSolicitudes();
  }

  cargarSolicitudes(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    const estado = this.filtroEstado() || undefined;
    const prioridad = this.filtroPrioridad() || undefined;

    this.solicitudService.getMisSolicitudes(estado, prioridad).subscribe({
      next: (data) => {
        this.solicitudes.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Error al cargar solicitudes:', err);
        this.errorMessage.set('No se pudo conectar con el servidor para obtener el listado de solicitudes.');
        this.isLoading.set(false);
      }
    });
  }

  onFiltroChange(): void {
    this.cargarSolicitudes();
  }

  verDetalle(id: number): void {
    this.solicitudSeleccionadaId.set(id);
    this.mostrarModalDetalle.set(true);
  }

  abrirCrear(): void {
    this.solicitudAEditar.set(null); // Null indica que es creación
    this.mostrarModalCrear.set(true);
  }

  abrirEditar(solicitud: SolicitudDto): void {
    this.solicitudAEditar.set(solicitud); // Pasa el objeto para edición
    this.mostrarModalCrear.set(true);
  }

  limpiarFiltros(): void {
    this.filtroEstado.set('');
    this.filtroPrioridad.set('');
    this.cargarSolicitudes();
  }

  actualizarEstado(id: number, nuevoEstado: string): void {
    this.solicitudService.cambiarEstado(id, nuevoEstado).subscribe({
      next: () => this.cargarSolicitudes(),
      error: () => alert('Ocurrió un error al actualizar el estado de la solicitud.')
    });
  }
}