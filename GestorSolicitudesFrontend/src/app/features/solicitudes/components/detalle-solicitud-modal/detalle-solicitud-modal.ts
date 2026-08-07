import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SolicitudService } from '../../../../core/services/solicitud.service';
import { SolicitudDto } from '../../../../core/models/solicitud.models';

@Component({
  selector: 'app-detalle-solicitud-modal',
  imports: [CommonModule],
  templateUrl: './detalle-solicitud-modal.html',
  styleUrl: './detalle-solicitud-modal.scss',
})
export class DetalleSolicitudModal implements OnChanges {
  private solicitudService = inject(SolicitudService);

  @Input() isOpen = false;
  @Input() solicitudId: number | null = null;
  @Output() cerrarModal = new EventEmitter<void>();

  detalle = signal<SolicitudDto | null>(null);
  isLoading = signal<boolean>(false);
  errorMessage = signal<string | null>(null);

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['solicitudId'] && this.solicitudId && this.isOpen) {
      this.cargarDetalle(this.solicitudId);
    }
  }

  cargarDetalle(id: number): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.detalle.set(null);

    this.solicitudService.getDetalleSolicitud(id).subscribe({
      next: (data) => {
        this.detalle.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.isLoading.set(false);
        if (err.status === 404) {
          this.errorMessage.set(err.error?.mensaje || 'La solicitud consultada no existe.');
        } else {
          this.errorMessage.set('No se pudo cargar la información de la solicitud.');
        }
      }
    });
  }

  cerrar(): void {
    this.detalle.set(null);
    this.errorMessage.set(null);
    this.cerrarModal.emit();
  }
}
