import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavComponent } from '../../../../shared/nav/nav.component';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { SolicitudService } from '../../../../services/request/solicitud.service';
import { Solicitud, TipoSolicitud } from '../../../../services/request/ISolicitud';

@Component({
  selector: 'tickets',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './tickets.component.html',
  styleUrls: ['./tickets.component.css']
})
export class TicketsComponent implements OnInit, OnDestroy {
  
  tiposSolicitud: TipoSolicitud[] = [];
  solicitudes: Solicitud[] = [];

  solicitudesFiltradas: Solicitud[] = [];
  filtroActual: string | null = null

  private subscriptions: Subscription[] = [];
  
  constructor(private solicitudService: SolicitudService, private router: Router) {}

  ngOnInit(): void {
    const subTipo = this.solicitudService.getListaTipoSolicitud().subscribe((res) => {
      if (res.success && res.response?.data) {
        this.tiposSolicitud = res.response.data;
      }
    });
    this.subscriptions.push(subTipo);


    const sub = this.solicitudService.getListaTotalSolicitudes().subscribe((res) => {
      if (res.success && res.response?.data) {
        this.solicitudesFiltradas = res.response.data;
      }
    });
    this.subscriptions.push(sub);
  }

  aplicarFiltro(tipo: string | null): void {
    this.filtroActual = tipo;

    if (!tipo) {
      // Opción "Todas"
      const sub = this.solicitudService.getListaTotalSolicitudes().subscribe(res => {
        if (res.success && res.response?.data) {
          this.solicitudesFiltradas = res.response.data;
        } else {
          this.solicitudesFiltradas = [];
        }
      });
      this.subscriptions.push(sub);
    } else {
      // Opción filtrada por tipo
      const tipoSeleccionado = this.tiposSolicitud.find(t => t.sTipoSolicitud === tipo);
      if (!tipoSeleccionado) return;

      const sub = this.solicitudService.getListaSolicitud(tipoSeleccionado.nIdTipoSolicitud)
        .subscribe(res => {
          const wrapper = res.response?.data?.[0]; 
          if (res.success && wrapper?.data) {
            this.solicitudesFiltradas = wrapper.data;
          } else {
            this.solicitudesFiltradas = [];
        }
        });
      this.subscriptions.push(sub);
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}