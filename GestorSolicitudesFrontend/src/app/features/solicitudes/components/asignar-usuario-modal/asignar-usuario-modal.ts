import { Component, EventEmitter, Input, Output, inject, signal, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SolicitudService } from '../../../../core/services/solicitud.service';
import { SolicitudDto } from '../../../../core/models/solicitud.models';
import { UsuarioListaDto } from '../../../../core/models/usuario.models';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-asignar-usuario-modal',
  imports: [CommonModule, FormsModule],
  templateUrl: './asignar-usuario-modal.html',
  styleUrl: './asignar-usuario-modal.scss',
})
export class AsignarUsuarioModal implements OnChanges {
  private solicitudService = inject(SolicitudService);

  @Input() isOpen = false;
  @Input() solicitud: SolicitudDto | null = null;
  @Input() listaAgentes: UsuarioListaDto[] = [];

  @Output() cerrarModal = new EventEmitter<void>();
  @Output() asignacionExitosa = new EventEmitter<void>();

  usuarioSeleccionadoId: number | null = null;
  isSaving = signal<boolean>(false);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen'] && this.isOpen) {
      // Precargar el responsable actual si ya tuviera uno
      this.usuarioSeleccionadoId = this.solicitud?.usuarioResponsableId ?? null;
    }
  }

  guardarAsignacion(): void {
    if (!this.solicitud || !this.usuarioSeleccionadoId) return;

    this.isSaving.set(true);
    this.solicitudService.asignarResponsable(this.solicitud.id, Number(this.usuarioSeleccionadoId)).subscribe({
      next: () => {
        this.isSaving.set(false);
        Swal.fire({
            icon: 'success',              
            text: `Solicitud actualizada correctamente.`,
            timer: 3000
        });
        this.asignacionExitosa.emit();
        this.cerrar();
      },
      error: (err) => {
        Swal.fire({
            title: 'Error',
            icon: 'error',
            timer: 3000,
            text: 'Ocurrió un error al intentar asignar el responsable.'
        });
        this.isSaving.set(false);
      }
    });
  }

  cerrar(): void {
    this.usuarioSeleccionadoId = null;
    this.cerrarModal.emit();
  }
}
