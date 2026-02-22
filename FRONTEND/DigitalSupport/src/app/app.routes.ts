import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { LoginComponent } from './auth/login/login.component';
import { DashboardAdminComponent } from './pages/dashboard/admin/dashboard.admin.component';
import { DashboardClienteComponent } from './pages/dashboard/client/dashboard.cliente.component';
import { DashboardColaboradorComponent } from './pages/dashboard/collaborator/dashboard.colaborador.component';

export const routes: Routes = [
  { path: '', redirectTo: '/iniciar-sesion', pathMatch: 'full' },
  { path: 'iniciar-sesion', loadComponent: () => import('./auth/login/login.component').then(m => m.LoginComponent) },

  // 🔵 ADMIN
  { path: 'dashboard/admin', canActivate: [AuthGuard], data: { roles: ['ADMIN'] }, loadComponent: () => import('./pages/dashboard/admin/dashboard.admin.component').then(m => m.DashboardAdminComponent),
    children: [
      {
        path: 'registrar-usuario',
        loadComponent: () => import('./pages/dashboard/admin/user/registrar-usuario.component').then(m => m.RegistrarUsuarioComponent),
        data: { roles: ['CLIENTE'] }
      },
      {
        path: 'registrar-cliente',
        loadComponent: () => import('./pages/dashboard/admin/customer/registrar-cliente.component').then(m => m.RegistrarClienteComponent),
        data: { roles: ['CLIENTE'] }
      },
      {
        path: 'rol-colaborador',
        loadComponent: () => import('./pages/dashboard/admin/role/rol-colaborador.component').then(m => m.RolColaboradorComponent),
        data: { roles: ['CLIENTE'] }
      },
      {
        path: 'tickets',
        loadComponent: () => import('./pages/dashboard/admin/ticket/tickets.component').then(m => m.TicketsComponent),
        data: { roles: ['CLIENTE'] }
      }
    ]
  },

  // 🟢 CLIENTE
  { path: 'dashboard/cliente', canActivate: [AuthGuard], data: { roles: ['CLIENTE'] }, loadComponent: () => import('./pages/dashboard/client/dashboard.cliente.component').then(m => m.DashboardClienteComponent),
    children: [
      {
        path: 'agregar-aplicativo',
        loadComponent: () => import('./pages/dashboard/client/app/agregar-aplicativo.component').then(m => m.AgregarAplicativoComponent),
        data: { roles: ['CLIENTE'] },
      },
      {
        path: 'crear-solicitud',
        loadComponent: () => import('./pages/dashboard/client/request/crear-solicitud.component').then(m => m.CrearSolicitudComponent),
        data: { roles: ['CLIENTE'] },
      },
      {
        path: 'tickets-usuario-cliente',
        loadComponent: () => import('./pages/dashboard/client/ticket/tickets-usuario-cliente.component').then(m => m.TicketsUsuarioClienteComponent),
        data: { roles: ['CLIENTE'] },
      }
    ]
  },
  
  // 🟣 COLABORADOR
  { path: 'dashboard/colaborador', canActivate: [AuthGuard], data: { roles: ['COLABORADOR'] }, loadComponent: () => import('./pages/dashboard/collaborator/dashboard.colaborador.component').then(m => m.DashboardColaboradorComponent),
    children: [
      {
        path: 'registrar-trabajo',
        loadComponent: () => import('./pages/dashboard/collaborator/register/registrar-trabajo.component').then(m => m.RegistrarTrabajoComponent),
        data: { roles: ['COLABORADOR'] },
      },
      {
        path: 'crear-asignacion',
        loadComponent: () => import('./pages/dashboard/collaborator/assignment/crear-asignacion.component').then(m => m.CrearAsignacionComponent),
        data: { roles: ['COLABORADOR'] },
      },
      {
        path: 'ver-avance-trabajo',
        loadComponent: () => import('./pages/dashboard/collaborator/work/ver-avance-trabajo.component').then(m => m.VerAvanceTrabajoComponent),
        data: { roles: ['COLABORADOR'] },
      }
    ]
  },

  // 🔔 NOTIFICACIONES
  {
    path: 'dashboard/notificacion/visualizar',
    canActivate: [AuthGuard],
    loadComponent: () => import('./pages/dashboard/notification/uc/notificacion.visualizar.component').then(m => m.NotificacionVisualizarComponent),
    data: { roles: ['CLIENTE'] }
  },
  {
    path: 'dashboard/notificacion/gestionar-visualizar',
    canActivate: [AuthGuard],
    loadComponent: () => import('./pages/dashboard/notification/teamwork/notificacion.gestionar-visualizar.component').then(m => m.NotificacionGestionarVisualizarComponent),
    data: { roles: ['ADMIN', 'COLABORADOR'] }
  },

  // Rutas Extras
  { path: '**', redirectTo: '/iniciar-sesion' }
];
