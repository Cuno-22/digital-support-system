import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavComponent } from '../../../shared/nav/nav.component';
import { Router, RouterModule, ActivatedRoute, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { LoginService } from '../../../services/auth/login.service';
import { ClienteService } from '../../../services/clients/cliente.service';
import { ClienteUC } from '../../../services/clients/ICliente';
import { Colaborador, RolColaborador } from '../../../services/collab/IColaborador';
import { ColaboradorService } from '../../../services/collab/colaborador.service';
import { IRetorno_DigitalSupportV3 } from '../../../services/auth/IRetornoResponse';

@Component({
  selector: 'app-dashboard-cliente',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule, FormsModule],
  templateUrl: './dashboard.cliente.component.html',
  styleUrls: ['./dashboard.cliente.component.css']
})
export class DashboardClienteComponent implements OnInit, OnDestroy {
  userData?: IRetorno_DigitalSupportV3;

  mostrarUserInfo= false;
  mostrarNotificaciones = false;
  mostrarPanelCliente = false;

  listaClientesUC: ClienteUC[] = [];
  listaColaboradores: Colaborador[] = [];
  listaRolColaboradores: RolColaborador[] = [];
  rolSeleccionado: number = 6; // ROL ADMIN POR DEFECTO

  private subscriptions: Subscription[] = [];
  
  constructor(private loginService: LoginService, private clienteService: ClienteService, private colaboradorService: ColaboradorService, private router: Router) {}
  
  ngOnInit(): void {
    const sub1 = this.loginService.userData.subscribe(data => {
      this.userData = data;
    });

    const sub2 = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.mostrarPanelCliente = event.urlAfterRedirects === '/dashboard/cliente';

        if (this.mostrarPanelCliente) {
          this.rolSeleccionado = 6; // Reinicia al rol Admin
          this.cargarColaboradores(this.rolSeleccionado);
        }
      }
    });

    this.cargarUsuariosCliente();
    this.cargarRolesColaborador();
    this.cargarColaboradores(this.rolSeleccionado);

    this.mostrarPanelCliente = this.router.url === '/dashboard/cliente';

    this.subscriptions.push(sub1, sub2);
  }

  toggleUserMenu() {
    this.mostrarUserInfo = !this.mostrarUserInfo;
    this.mostrarNotificaciones = false;
  }

  toggleNotificaciones() {
    this.mostrarNotificaciones = !this.mostrarNotificaciones;
    this.mostrarUserInfo = false;
  }

  cargarUsuariosCliente(): void {
    const nIdUsuarioCliente = Number(localStorage.getItem('nIdUsuarioCliente')) || 0;

    const sub = this.clienteService.getListaClienteporUC(nIdUsuarioCliente).subscribe({
      next: (res) => {
        if (res.success && res.response?.data) {
          this.listaClientesUC = res.response.data[0].data;
        }
      },
      error: (err) => console.error('Error cargando clientes UC:', err)
    });
    this.subscriptions.push(sub);
  }

  cargarRolesColaborador(): void {
    const sub = this.colaboradorService.getListaRolColaborador().subscribe({
      next: (res) => {
        if (res.success && res.response?.data) {
          this.listaRolColaboradores = res.response.data;
        }
      },
      error: (err) => console.error('Error cargando roles:', err)
    });
    this.subscriptions.push(sub);
  }

  cargarColaboradores(nIdRolColaborador: number): void {
    const sub = this.colaboradorService.getListaColaborador(nIdRolColaborador).subscribe({
      next: (res) => {
        if (res.success && res.response?.data) {
          this.listaColaboradores = res.response.data;
        }
      },
      error: (err) => console.error('Error cargando colaboradores:', err)
    });
    this.subscriptions.push(sub);
  }

  onRolesColaboradorChange(): void {
    this.cargarColaboradores(this.rolSeleccionado);
  }

  cerrarSesion(): void {
    this.loginService.logout();
    this.router.navigateByUrl('/iniciar-sesion');
  }

  visualizarNotificaciones() {
    this.router.navigate(['/dashboard/notificacion/visualizar']);
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
