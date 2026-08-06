import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  errorMessage = signal<string | null>(null);
  isLoading = signal<boolean>(false);

  loginForm = this.fb.nonNullable.group({
    usuarioOrEmail: ['', [Validators.required]],
    password: ['', [Validators.required, Validators.minLength(4)]]
  });

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.authService.login(this.loginForm.getRawValue()).subscribe({
      next: (response) => {
        debugger;
        this.isLoading.set(false);
        if (response.rol === 'Agente' || response.rol === 'Asesor') {
          this.router.navigate(['/dashboard/mis-solicitudes']);
        } else if (response.rol === 'Administrador') {
          this.router.navigate(['/dashboard/todas-solicitudes']);
        }
      },
      error: (err) => {
        this.isLoading.set(false);
        if (err.status === 401) {
          this.errorMessage.set('Usuario o contraseña incorrectos.');
        } else {
          this.errorMessage.set('Ocurrió un error al conectar con el servidor.');
        }
      }
    });
  }
}
