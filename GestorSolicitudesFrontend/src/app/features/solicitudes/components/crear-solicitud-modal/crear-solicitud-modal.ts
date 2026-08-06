import { Component, EventEmitter, Input, Output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { SolicitudService } from '../../../../core/services/solicitud.service';
import { CrearSolicitudDto } from '../../../../core/models/solicitud.models';
import { AuthService } from '../../../../core/services/auth.service';
import { UsuarioOpcion } from '../../../../core/models/basico.models';

@Component({
  selector: 'app-crear-solicitud-modal',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './crear-solicitud-modal.html',
  styleUrl: './crear-solicitud-modal.scss',
})
export class CrearSolicitudModal {
private fb = inject(FormBuilder);
  private solicitudService = inject(SolicitudService);
  public authService = inject(AuthService);

  @Input() isOpen = false;
  @Output() cerrarModal = new EventEmitter<void>();
  @Output() solicitudCreada = new EventEmitter<void>();

  isLoading = signal<boolean>(false);
  errorMessage = signal<string | null>(null);
  
  // Lista de asesores disponibles para asignación (solo admin)
  asesores = signal<UsuarioOpcion[]>([]);

  solicitudForm = this.fb.group({
    titulo: ['', [Validators.required, Validators.maxLength(100)]],
    descripcion: ['', [Validators.required, Validators.minLength(10)]],
    prioridad: ['Media', [Validators.required]],
    usuarioResponsableId: [null as number | null] // Campo opcional/dinámico
  });

  get esAdmin(): boolean {
    return this.authService.currentUser()?.rol === 'Administrador';
  }

  ngOnInit(): void {
    if (this.esAdmin) {
      this.cargarAsesores();
    }
  }

  cargarAsesores(): void {
    // Si tienes un servicio de usuarios, reemplaza por this.usuarioService.getAsesores()
    // Ejemplo mock/servicio:
    this.asesores.set([
      { id: 1, nombreCompleto: 'Juan Pérez' },
      { id: 2, nombreCompleto: 'María Gómez' }
    ]);
  }

  onSubmit(): void {
    if (this.solicitudForm.invalid) {
      this.solicitudForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const formValues = this.solicitudForm.getRawValue();
    const dto: CrearSolicitudDto = {
      titulo: formValues.titulo!,
      descripcion: formValues.descripcion!,
      prioridad: formValues.prioridad!,
      usuarioResponsableId: formValues.usuarioResponsableId ? Number(formValues.usuarioResponsableId) : undefined
    };

    this.solicitudService.crearSolicitud(dto).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.solicitudForm.reset({ prioridad: 'Media', usuarioResponsableId: null });
        this.solicitudCreada.emit();
        this.cerrar();
      },
      error: () => {
        this.isLoading.set(false);
        this.errorMessage.set('No se pudo registrar la solicitud. Intenta nuevamente.');
      }
    });
  }

  cerrar(): void {
    this.errorMessage.set(null);
    this.solicitudForm.reset({ prioridad: 'Media', usuarioResponsableId: null });
    this.cerrarModal.emit();
  }
}
