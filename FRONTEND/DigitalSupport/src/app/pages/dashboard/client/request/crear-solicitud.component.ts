import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavComponent } from '../../../../shared/nav/nav.component';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { SolicitudService } from '../../../../services/request/solicitud.service';
import { AplicativoService } from '../../../../services/application/aplicativo.service';
import { Aplicativo } from '../../../../services/application/IAplicativo';
import { SolicitudDATA, TipoSolicitud } from '../../../../services/request/ISolicitud';

@Component({
  selector: 'crear-solicitud',
  standalone: true,
  imports: [CommonModule, MatIconModule, FormsModule],
  templateUrl: './crear-solicitud.component.html',
  styleUrls: ['./crear-solicitud.component.css']
})
export class CrearSolicitudComponent implements OnInit, OnDestroy {

    tiposSolicitud: TipoSolicitud[] = [];
    listaAplicativos: Aplicativo[] = [];

    nombreUsuarioCliente: string = '';
    fechaActual: Date = new Date();

    mensajeError: string = '';
    mensajeExito: string = '';
    mostrarMensaje: boolean = false;

    nuevaSolicitud: SolicitudDATA = {
        nIdUsuarioCliente: 0,
        nIdAplicativo: 0,
        nIdTipoSolicitud: 0,
        sMotivo: '',
        dFechaCreacion: null,
        dFechaFinalizacion: null,
        sEstado: 'Pendiente',
        bEstado: true
    };

    private subscriptions: Subscription[] = [];
    
    constructor(private solicitudService: SolicitudService, private aplicativoService: AplicativoService, private router: Router) {}

    ngOnInit(): void {
        
        const idUsuarioCliente = Number(localStorage.getItem('nIdUsuarioCliente'));
        const nombreUsuarioCliente = localStorage.getItem('sNombreUsuarioCliente') || '';
        
        this.nuevaSolicitud.nIdUsuarioCliente = idUsuarioCliente;
        this.nombreUsuarioCliente = nombreUsuarioCliente;

        this.solicitudService.getListaTipoSolicitud().subscribe(res => {
            if (res.success && res.response?.data) {
                this.tiposSolicitud = res.response.data;
            }
        });
        
        this.aplicativoService.getListaAplicativo(idUsuarioCliente).subscribe(res => {
            if (res.success && res.response?.data) {
                this.listaAplicativos = res.response.data;
            }
        });
    }

    crearSolicitud(): void {
        const texto = this.nuevaSolicitud.sMotivo.trim().split(/\s+/);

        if (texto.length >= 200) {
            this.mostrarError(`El motivo no puede exceder las 200 palabras (actual: ${texto.length}).`);
            return;
        }

        if (!this.nuevaSolicitud.nIdAplicativo || !this.nuevaSolicitud.nIdTipoSolicitud || !this.nuevaSolicitud.sMotivo.trim()) {
            this.mostrarError('Todos los campos son obligatorios.');
            return;
        }

        this.fechaActual = new Date();
        this.nuevaSolicitud.dFechaCreacion = new Date();

        this.solicitudService.postInsertarSolicitud(this.nuevaSolicitud).subscribe(res => {
        
            if (res?.nRetorno === 0 && res?.sRetorno) {
                this.mostrarError(res.sRetorno);
                return;
            }

            this.mostrarMensajeExito(res?.sRetorno ?? 'Solicitud registrada correctamente');

            // resetear form
            this.nuevaSolicitud = {
                nIdUsuarioCliente: Number(localStorage.getItem('nIdUsuarioCliente')) || 0,
                nIdAplicativo: 0,
                nIdTipoSolicitud: 0,
                sMotivo: '',
                dFechaCreacion: null,
                dFechaFinalizacion: null,
                sEstado: 'Pendiente',
                bEstado: true
            };
            this.fechaActual = new Date();
        });
    }

    mostrarError(msg: string): void {
        this.mensajeError = msg;
        setTimeout(() => this.mensajeError = '', 3500);
    }

    mostrarMensajeExito(msg: string): void {
        this.mensajeExito = msg;
        this.mostrarMensaje = true;
        setTimeout(() => {
        this.mostrarMensaje = false;
        this.mensajeExito = '';
        }, 3500);
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(sub => sub.unsubscribe());
    }
}