import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SolicitudService } from '../../../../core/services/solicitud.service';
import { Solicitud, SolicitudDto } from '../../../../core/models/solicitud.models';
import { CrearSolicitudModal } from "../../components/crear-solicitud-modal/crear-solicitud-modal";
import { FormsModule } from '@angular/forms';
import { DetalleSolicitudModal } from '../../components/detalle-solicitud-modal/detalle-solicitud-modal';
import { AuthService } from '../../../../core/services/auth.service';
import { UsuarioService } from '../../../../core/services/usuario.service';
import { UsuarioListaDto } from '../../../../core/models/usuario.models';
import { AsignarUsuarioModal } from "../../components/asignar-usuario-modal/asignar-usuario-modal";
import Swal from 'sweetalert2';

@Component({
  selector: 'app-mis-solicitudes',
  imports: [CommonModule, FormsModule, CrearSolicitudModal, DetalleSolicitudModal, AsignarUsuarioModal],
  templateUrl: './mis-solicitudes.html',
  styleUrl: './mis-solicitudes.scss',
})
export class MisSolicitudes implements OnInit {

  private solicitudService = inject(SolicitudService);
  private usuarioService = inject(UsuarioService);
  authService = inject(AuthService);

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

  mostrarModalAsignar = signal<boolean>(false);
  solicitudParaAsignar = signal<SolicitudDto | null>(null);
  listaAgentes = signal<UsuarioListaDto[]>([]);

  ngOnInit(): void {
    this.cargarSolicitudes();
    if(this.authService.isAdmin()) {
      this.cargarAgentes();
    }
  }

  cargarSolicitudes(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    const estado = this.filtroEstado() || undefined;
    const prioridad = this.filtroPrioridad() || undefined;
    if(!this.authService.isAdmin()) {
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
    else{
      this.solicitudService.getTodasSolicitudes(estado, prioridad).subscribe({
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
  }

cargarAgentes(): void {
  this.usuarioService.getTodosUsuarios('','Agente').subscribe({
    next: (agentes) => this.listaAgentes.set(agentes),
    error: (err) => console.error('Error al cargar agentes', err)
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

  abrirModalAsignar(solicitud: SolicitudDto): void {
    this.solicitudParaAsignar.set(solicitud);
    this.mostrarModalAsignar.set(true);
  }

  cerrarModalAsignar(): void {
    this.mostrarModalAsignar.set(false);
    this.solicitudParaAsignar.set(null);
}

  limpiarFiltros(): void {
    this.filtroEstado.set('');
    this.filtroPrioridad.set('');
    Swal.fire({
            icon: 'success',              
            text: `Filtros limpiados correctamente.`,
            timer: 3000
          });
    this.cargarSolicitudes();
  }

  actualizarEstado(id: number, nuevoEstado: string): void {
    this.solicitudService.cambiarEstado(id, nuevoEstado).subscribe({
      next: () => {
        Swal.fire({
            icon: 'success',              
            text: `Estado de la solicitud actualizado correctamente.`,
            timer: 3000
        });
        this.cargarSolicitudes();
      },
      error: () => {
        Swal.fire({
            title: 'Error',
            icon: 'error',
            timer: 3000,
            text: 'Ocurrió un error al actualizar el estado de la solicitud.'
        });
      }
    });
  }

  asignarSolicitud(id: number): void {
    this.actualizarEstado(id, 'Asignada');
  }

  borrarSolicitud(id: number) {
    Swal.fire({
      title: "¿Está seguro de eliminar la solicitud?",
      text: "Esta acción no se puede revertir",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Sí",
      cancelButtonText: "No"
    }).then((result) =>{
      if(result.isConfirmed){
        this.isLoading.set(true);
        this.solicitudService.borrarSolicitud(id).subscribe({
          next: () => {
            Swal.fire({
                icon: 'success',              
                text: `Solicitud eliminada correctamente.`,
                timer: 3000
            });
            this.cargarSolicitudes();
            this.isLoading.set(false);
          },
          error: () => {
            Swal.fire({
                title: 'Error',
                icon: 'error',
                timer: 3000,
                text: 'Ocurrió un error al borrar la solicitud.'
            });
          }
        });
      }
    });
    
  }
}