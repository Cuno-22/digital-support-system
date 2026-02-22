import { Component, OnInit, OnDestroy} from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavComponent } from '../../../../shared/nav/nav.component';
import { Router } from '@angular/router';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { Notificacion } from '../../../../services/notify/INotificacion';
import { NotificacionService } from '../../../../services/notify/notificacion.service';

@Component({
  selector: 'app-dashboard-notificacion-visualizar',
  standalone: true,
  imports: [CommonModule, MatIconModule, FormsModule, MatFormFieldModule, MatInputModule, MatTableModule, MatSortModule, MatIconModule],
  templateUrl: './notificacion.visualizar.component.html',
  styleUrls: ['./notificacion.visualizar.component.css']
})
export class NotificacionVisualizarComponent implements OnInit, OnDestroy {

  notificaciones: Notificacion[] = [];
  notificacionesFiltradas: Notificacion[] = [];
  filtro: string = '';
  filtroEstado: string = 'todos'; // todos | leido | no leido

  columnas: string[] = [
    'sMotivoSolicitud', 
    'sInformacionNotificada',
    'sEstadoSolicitud', 
    'dFechaEnvio', 
    'bLeido', 
    'bEstado'
  ];

  mostrarUserInfo = false;
  mostrarNotificaciones = false;
  cargando = false;

  private subscriptions: Subscription[] = [];

  constructor(private notificacionService: NotificacionService, private sanitizer: DomSanitizer, private router: Router) {}

  ngOnInit(): void {
    const idUsuarioCliente = Number(localStorage.getItem('nIdUsuarioCliente'));

    if (!idUsuarioCliente) {
      this.router.navigate(['/iniciar-sesion']);
      return;
    }

    const sub1 = this.notificacionService.getListaNotificacionesUC(idUsuarioCliente).subscribe();

    const sub2 = this.notificacionService.notificaciones.subscribe(data => {
      this.notificaciones = data;
      this.aplicarFiltro();
    });

    this.subscriptions.push(sub1, sub2);
  }
  
  aplicarFiltro(): void {
    const texto = this.filtro.trim().toLowerCase();
    this.notificacionesFiltradas = this.notificaciones.filter(n => {
      const coincideTexto =
        n.sInformacionNotificada?.toLowerCase().includes(texto) ||
        n.sEstadoSolicitud?.toLowerCase().includes(texto);

      const coincideEstado =
        this.filtroEstado === 'todos' ||
        (this.filtroEstado === 'leido' && n.bLeido) ||
        (this.filtroEstado === 'no_leido' && !n.bLeido);

      return coincideTexto && coincideEstado;
    });
  }

  resaltarSimilitud(texto: string): string {
    if (!this.filtro.trim()) return texto;
    const regex = new RegExp(`(${this.filtro})`, 'gi');
    return texto?.replace(regex, '<strong>$1</strong>') ?? '';
  }

  regresarDashboard(): void {
    this.router.navigate(['/dashboard/cliente']);
  }

  cerrarSesion(): void {
    localStorage.clear();
    this.router.navigateByUrl('/iniciar-sesion');
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}