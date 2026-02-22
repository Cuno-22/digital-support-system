import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { SolicitudService } from '../../../../services/request/solicitud.service';
import { SolicitudUC } from '../../../../services/request/ISolicitud';

@Component({
  selector: 'tickets-usuario-cliente',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './tickets-usuario-cliente.component.html',
  styleUrls: ['./tickets-usuario-cliente.component.css']
})
export class TicketsUsuarioClienteComponent implements OnInit, OnDestroy {

  listaSolicitudesUC: SolicitudUC[] = [];

  private subscriptions: Subscription[] = [];
  
  constructor(private solicitudService: SolicitudService, private router: Router) {}
  
  ngOnInit(): void {
    const idUsuarioCliente = Number(localStorage.getItem('nIdUsuarioCliente'));
    
    const sub = this.solicitudService.getSolicitudEspecificoUC(idUsuarioCliente)
      .subscribe(res => {
        if (res.success && res.response?.data) {
          this.listaSolicitudesUC = res.response.data.sort(
            (a, b) => new Date(b.dFechaCreacion).getTime() - new Date(a.dFechaCreacion).getTime()
          );
        } else {
          this.listaSolicitudesUC = [];
        }
      });
    this.subscriptions.push(sub);
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}