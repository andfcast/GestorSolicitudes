import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role-guard';

export const routes: Routes = [
    {
    path: 'login',
    loadComponent: () => import('./features/auth/pages/login/login').then(m => m.Login)
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () => import('./features/dashboard/pages/dashboard-layout/dashboard-layout').then(m => m.DashboardLayout),
    children: [
      {
        path: 'mis-solicitudes',
        canActivate: [roleGuard(['Agente'])],
        loadComponent: () => import('./features/solicitudes/pages/mis-solicitudes/mis-solicitudes').then(m => m.MisSolicitudes)
      },
      {
        path: 'solicitudes',
        canActivate: [roleGuard(['Administrador'])],
        loadComponent: () => import('./features/solicitudes/pages/mis-solicitudes/mis-solicitudes').then(m => m.MisSolicitudes)
      },
      { path: '', redirectTo: 'mis-solicitudes', pathMatch: 'full' }
    ]
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];
