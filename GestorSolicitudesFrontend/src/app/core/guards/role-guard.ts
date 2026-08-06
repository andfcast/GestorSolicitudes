import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
  return () => {
    const authService = inject(AuthService);
    const router = inject(Router);
    const user = authService.currentUser();

    if (user && allowedRoles.includes(user.rol)) {
      return true;
    }

    // Si no tiene el rol adecuado, redirigir al login o a acceso no autorizado
    router.navigate(['/login']);
    return false;
  };
};