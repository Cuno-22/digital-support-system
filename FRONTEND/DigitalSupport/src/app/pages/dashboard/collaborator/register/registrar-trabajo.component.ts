import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavComponent } from '../../../shared/nav/nav.component';
import { Router, RouterModule, ActivatedRoute, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { RegistroService } from '../../../../services/register/registro.service';
import { SolicitudService } from '../../../../services/request/solicitud.service';
import { RegistroTrabajoDATA } from '../../../../services/register/IRegistroTrabajo';
import { Solicitud } from '../../../../services/request/ISolicitud';

@Component({
  selector: 'registrar-trabajo',
  standalone: true,
  imports: [CommonModule, MatIconModule, FormsModule],
  templateUrl: './registrar-trabajo.component.html',
  styleUrls: ['./registrar-trabajo.component.css']
})
export class RegistrarTrabajoComponent implements OnInit, OnDestroy {

  listaSolicitudes: Solicitud[] = [];

  nombreColaborador: string = '';
  fechaActual: Date = new Date();

  mensajeError: string = '';
  mensajeExito: string = '';
  mostrarMensaje: boolean = false;

  nuevoRegistro: RegistroTrabajoDATA = {
    nIdSolicitud: 0,
    nIdColaborador: 0,
    sDescripcion: '',
    dFechaRegistro: null,
    nHorasTrabajadas: 0,
    sObservacion: '',
    bEstado: true
  };

  private subscriptions: Subscription[] = [];
  
  constructor(private registroService: RegistroService, private solicitudService: SolicitudService, private router: Router) {}

    ngOnInit(): void {
      const idColaborador = Number(localStorage.getItem('nIdColaborador'));
      const nombreColaborador = localStorage.getItem('sNombreColaborador') || '';
      
      this.nuevoRegistro.nIdColaborador = idColaborador;
      this.nombreColaborador = nombreColaborador;

      this.solicitudService.getListaTotalSolicitudes().subscribe(res => {
        if (res.success && res.response?.data) {
            this.listaSolicitudes = res.response.data;
        }
      });
    }

    crearRegistro(): void {
      const descripcion = this.nuevoRegistro.sDescripcion.trim();
      const observacion = this.nuevoRegistro.sObservacion.trim();

      if (!this.nuevoRegistro.nIdSolicitud || !this.nuevoRegistro.nIdColaborador || !this.nuevoRegistro.sDescripcion.trim() || !this.nuevoRegistro.sObservacion.trim() || this.nuevoRegistro.nHorasTrabajadas === null || this.nuevoRegistro.nHorasTrabajadas === undefined){
        this.mostrarError('Todos los campos son obligatorios.');
        return;
      }

      if (this.nuevoRegistro.nHorasTrabajadas <= 0) {
        this.mostrarError('Las horas trabajadas debe ser como minimo 1.');
        return;
      }

      this.fechaActual = new Date();
      this.nuevoRegistro.dFechaRegistro = new Date();

      this.registroService.postInsertarRegistroTrabajo(this.nuevoRegistro).subscribe(res => {
        
      if (res?.nRetorno === 0 && res?.sRetorno) {
          this.mostrarError(res.sRetorno);
          return;
      }

      this.mostrarMensajeExito(res?.sRetorno ?? 'Registro de Trabajo realizado correctamente');
      
      // resetear form
      this.nuevoRegistro = {
        nIdSolicitud: 0,
        nIdColaborador: Number(localStorage.getItem('nIdColaborador')) || 0,
        sDescripcion: '',
        dFechaRegistro: null,
        nHorasTrabajadas: 0,
        sObservacion: '',
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