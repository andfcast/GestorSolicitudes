import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SolicitudService } from '../../../../core/services/solicitud.service';
import { CrearSolicitudDto, SolicitudDto } from '../../../../core/models/solicitud.models';
import { AuthService } from '../../../../core/services/auth.service';
import { UsuarioOpcion } from '../../../../core/models/basico.models';

@Component({
  selector: 'app-crear-solicitud-modal',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './crear-solicitud-modal.html',
  styleUrl: './crear-solicitud-modal.scss',
})
export class CrearSolicitudModal implements OnChanges {
  private fb = inject(FormBuilder);
  private solicitudService = inject(SolicitudService);

  @Input() isOpen = false;
  @Input() solicitudAEditar: SolicitudDto | null = null; // HU-05: Si tiene objeto = Edición, si es null = Creación

  @Output() cerrarModal = new EventEmitter<void>();
  @Output() solicitudCreada = new EventEmitter<void>();

  // Definición del Formulario Reactivo con FormBuilder
  solicitudForm: FormGroup = this.fb.group({
    titulo: ['', [Validators.required, Validators.maxLength(150)]],
    descripcion: ['', [Validators.required]],
    cliente: ['',[Validators.required, Validators.maxLength(30)]],
    prioridad: ['Media', [Validators.required]],
    usuarioResponsableId: [null]
  });

  isSaving = signal<boolean>(false);
  errorMessage = signal<string | null>(null);

  get esEdicion(): boolean {
    return !!this.solicitudAEditar;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.isOpen) {
      if (changes['solicitudAEditar'] || changes['isOpen']) {
        if (this.solicitudAEditar) {
          // MODO EDICIÓN: Precargar los datos en el FormGroup existente
          this.solicitudForm.patchValue({
            titulo: this.solicitudAEditar.titulo || '',
            descripcion: this.solicitudAEditar.descripcion || '',
            cliente: this.solicitudAEditar.cliente || '',
            prioridad: this.solicitudAEditar.prioridad || 'Media',
            usuarioResponsableId: this.solicitudAEditar.usuarioResponsableId ?? null
          });
        } else {
          // MODO CREACIÓN: Resetear el formulario a sus valores por defecto
          this.resetearFormulario();
        }
      }
    }
  }

  guardar(): void {
    if (this.solicitudForm.invalid) {
      this.solicitudForm.markAllAsTouched();
      this.errorMessage.set('Por favor completa todos los campos requeridos correctamente.');
      return;
    }

    this.isSaving.set(true);
    this.errorMessage.set(null);

    const formValue = this.solicitudForm.value;
    const payload: CrearSolicitudDto = {
      titulo: formValue.titulo.trim(),
      descripcion: formValue.descripcion.trim(),
      cliente: formValue.cliente ? formValue.cliente.trim() : undefined,
      prioridad: formValue.prioridad,
      usuarioResponsableId: formValue.usuarioResponsableId ?? undefined
    };

    if (this.esEdicion && this.solicitudAEditar) {
      // HU-05: Petición de actualización (PUT)
      this.solicitudService.actualizarSolicitud(this.solicitudAEditar.id, payload).subscribe({
        next: () => {
          this.isSaving.set(false);
          this.solicitudCreada.emit();
          this.cerrar();
        },
        error: (err) => {
          console.error('Error al actualizar la solicitud:', err);
          this.isSaving.set(false);
          this.errorMessage.set(err.error?.mensaje || 'Ocurrió un error al intentar actualizar la solicitud.');
        }
      });
    } else {
      // HU-02: Petición de creación (POST)
      this.solicitudService.crearSolicitud(payload).subscribe({
        next: () => {
          this.isSaving.set(false);
          this.solicitudCreada.emit();
          this.cerrar();
        },
        error: (err) => {
          console.error('Error al crear la solicitud:', err);
          this.isSaving.set(false);
          this.errorMessage.set(err.error?.mensaje || 'Ocurrió un error al registrar la solicitud.');
        }
      });
    }
  }

  // Métodos auxiliares para manejo visual de errores en plantilla
  isFieldInvalid(fieldName: string): boolean {
    const field = this.solicitudForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  resetearFormulario(): void {
    this.solicitudForm.reset({
      titulo: '',
      descripcion: '',
      cliente: '',
      prioridad: 'Media',
      usuarioResponsableId: null
    });
    this.errorMessage.set(null);
  }

  cerrar(): void {
    this.resetearFormulario();
    this.cerrarModal.emit();
  }
}