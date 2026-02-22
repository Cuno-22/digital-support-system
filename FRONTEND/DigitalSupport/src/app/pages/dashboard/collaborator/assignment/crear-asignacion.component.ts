import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavComponent } from '../../../shared/nav/nav.component';
import { Router, RouterModule, ActivatedRoute, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { Solicitud } from '../../../../services/request/ISolicitud';
import { AsignacionSolicitud, AsignacionSolicitudEspecifica } from '../../../../services/register/IRegistroTrabajo';
import { RegistroService } from '../../../../services/register/registro.service';
import { SolicitudService } from '../../../../services/request/solicitud.service';

@Component({
  selector: 'crear-asignacion',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule, MatButtonModule, MatFormFieldModule, MatInputModule, FormsModule],
  templateUrl: './crear-asignacion.component.html',
  styleUrls: ['./crear-asignacion.component.css']
})
export class CrearAsignacionComponent implements OnInit, OnDestroy {

    listaSolicitudesTotales: Solicitud[] = [];
    asignacionesDisponibles: { nIdSolicitud: number, nIdAsignacionSolicitud: number }[] = [];
    asignacionSeleccionada: { nIdSolicitud: number, nIdAsignacionSolicitud: number } | null = null;

    nuevaAsignacion: AsignacionSolicitud = {
        nIdSolicitud: 0,
        nIdColaborador: 0,
        bEsCoordinador: false,
        bEstado: true
    };

    actualizarAsignacion: AsignacionSolicitud = {
        nIdAsignacionSolicitud: 0,
        nIdSolicitud: 0,
        nIdColaborador: 0,
        bEsCoordinador: false,
        bEstado: true
    };

    mensajeErrorInsert: string = '';
    mensajeExitoInsert: string = '';
    mensajeErrorUpdate: string = '';
    mensajeExitoUpdate: string = '';
    mensajeErrorDelete: string = '';
    mensajeExitoDelete: string = '';
    mensajeError: string = '';
    mensajeExito: string = '';
    mostrarMensaje: boolean = false;

    mostrarFormularioActualizar: boolean = false;
    mostrarConfirmacionEliminar: boolean = false;
    accionSeleccionada: 'ninguna' | 'actualizar' | 'eliminar' = 'ninguna';
    mensajeSeleccion: string = '';

    nombreColaborador: string = '';
    
    private currentColaboradorId: number = 0;
    private subscriptions: Subscription[] = [];
  
    constructor(private registroService: RegistroService, private solicitudService: SolicitudService, private router: Router) {}

    ngOnInit(): void {
        const idColaborador = Number(localStorage.getItem('nIdColaborador'));
        const nombreColaborador = localStorage.getItem('sNombreColaborador') || '';

        this.currentColaboradorId = idColaborador;
        this.nuevaAsignacion.nIdColaborador = idColaborador;
        this.nombreColaborador = nombreColaborador;

        this.solicitudService.getListaTotalSolicitudes().subscribe(res => {
            if (res.success && res.response?.data) {
                this.listaSolicitudesTotales = res.response.data;

                this.obtenerAsignacionColaborador(idColaborador);
            }
        });
    }

    crearAsignacion(): void {
        if (!this.nuevaAsignacion.nIdSolicitud || this.nuevaAsignacion.nIdSolicitud === 0) {
            this.mostrarError('Debes seleccionar una solicitud', 'insert');
            return;
        }

        if (this.nuevaAsignacion.bEsCoordinador === null || this.nuevaAsignacion.bEsCoordinador === undefined || !this.nuevaAsignacion.nIdColaborador) {
            this.mostrarError('Todos los campos son obligatorios.', 'insert');
            return;
        }

        const existe = this.asignacionesDisponibles.some(a => a.nIdSolicitud === this.nuevaAsignacion.nIdSolicitud);
        if (existe) {
            this.mostrarError('Ya existe una asignación para esta solicitud y colaborador.', 'insert');
            return;
        }

        this.registroService.postInsertarAsignacionSolicitud(this.nuevaAsignacion).subscribe(res => {
        if (res?.nRetorno === 0 && res?.sRetorno) {
            this.mostrarError(res.sRetorno, 'insert');
            return;
        }

        this.mostrarMensajeExito(res?.sRetorno ?? 'Asignación registrada correctamente', 'insert');

        const nuevoId = res?.nRetorno;
        this.obtenerAsignacionColaborador(this.currentColaboradorId);

        if (nuevoId && nuevoId > 0) {
            // seleccionamos el nuevo (si queremos)
            this.asignacionSeleccionada = {
            nIdSolicitud: this.nuevaAsignacion.nIdSolicitud,
            nIdAsignacionSolicitud: nuevoId
            };
            this.mensajeSeleccion = `Se seleccionó la asignación Nro ${nuevoId}`;
        }
        // resetear form
        this.nuevaAsignacion = {
            nIdSolicitud: 0,
            nIdColaborador: 0,
            bEsCoordinador: false,
            bEstado: true
        };
        });
    }

    obtenerAsignacionColaborador(nIdColaborador: number): void {
        this.asignacionesDisponibles = [];

        // Recorremos todas las solicitudes y verificamos si hay una asignación específica
        this.listaSolicitudesTotales.forEach(solicitud => {
            this.registroService.getAsignacionSolicitudEspecifica(solicitud.nIdSolicitud, nIdColaborador)
            .subscribe(res => {
                const data = res.response?.data;

                // Como el backend devuelve un array, verificamos si existe y si tiene elementos
                if (res.success && Array.isArray(data) && data.length > 0 && data[0].nIdAsignacionSolicitud) {
                this.asignacionesDisponibles.push({
                    nIdSolicitud: solicitud.nIdSolicitud,
                    nIdAsignacionSolicitud: data[0].nIdAsignacionSolicitud
                });
                }
            });
        });
    }

    seleccionarAsignacion(asignacion: { nIdSolicitud: number, nIdAsignacionSolicitud: number } | null): void {
        if (!asignacion) {
            this.asignacionSeleccionada = null;
            this.mensajeSeleccion = '';
            this.mostrarFormularioActualizar = false;
            this.mostrarConfirmacionEliminar = false;
            return;
        }

        this.asignacionSeleccionada = asignacion;
        this.mensajeSeleccion = `Se seleccionó la asignación Nro ${asignacion.nIdAsignacionSolicitud}`;
        this.mostrarFormularioActualizar = false;
        this.mostrarConfirmacionEliminar = false;
    }

    mostrarActualizar(): void {
        if (!this.asignacionSeleccionada) {
            this.mostrarError('Primero selecciona una asignación', 'update');
            return;
        }

        // Reiniciamos mensajes y estados previos
        this.mensajeError = '';
        this.mensajeExito = '';
        this.mostrarMensaje = false;

        this.accionSeleccionada = 'actualizar';
        this.mostrarFormularioActualizar = true;
        this.mostrarConfirmacionEliminar = false;

        this.actualizarAsignacion = {
            nIdAsignacionSolicitud: this.asignacionSeleccionada.nIdAsignacionSolicitud,
            nIdSolicitud: this.asignacionSeleccionada.nIdSolicitud,
            nIdColaborador: this.currentColaboradorId,
            bEsCoordinador: false,
            bEstado: true
        };
    }

    actualizarAsignacionSolicitud(): void {
        if (!this.asignacionSeleccionada) {
            this.mostrarError('No hay asignación seleccionada para actualizar.', 'update');
            return;
        }

        // Validar duplicado: si existe otra asignación (distinta id) con la misma solicitud
        const duplicada = this.asignacionesDisponibles.some(a =>
            a.nIdSolicitud === this.actualizarAsignacion.nIdSolicitud &&
            a.nIdAsignacionSolicitud !== this.asignacionSeleccionada!.nIdAsignacionSolicitud
        );

        if (duplicada) {
            this.mostrarError('No se puede actualizar: ya existe otra asignación para esa solicitud y colaborador.', 'update');
            return;
        }

        const data = {
            nIdAsignacionSolicitud: this.asignacionSeleccionada?.nIdAsignacionSolicitud,
            nIdSolicitud: this.actualizarAsignacion.nIdSolicitud,
            nIdColaborador: this.actualizarAsignacion.nIdColaborador,
            bEsCoordinador: this.actualizarAsignacion.bEsCoordinador,
            bEstado: this.actualizarAsignacion.bEstado
        };

        this.registroService.postActualizarAsignacionSolicitud(data).subscribe(res => {
        if (res?.nRetorno === 0) {
            this.mostrarError(res.sRetorno, 'update');
            return;
        }
        this.mostrarMensajeExito(res?.sRetorno ?? 'Asignación actualizada correctamente', 'update');
        this.mostrarFormularioActualizar = false;
        this.obtenerAsignacionColaborador(this.currentColaboradorId);
        this.mensajeSeleccion = `Se actualizó la asignación Nro ${data.nIdAsignacionSolicitud}`;
        });

        this.mostrarFormularioActualizar = false;
        this.accionSeleccionada = 'ninguna';
        this.asignacionSeleccionada = null;
        this.mensajeSeleccion = '';
    }

    mostrarEliminar(): void {
        if (!this.asignacionSeleccionada) {
            this.mostrarError('Primero selecciona una asignación', 'delete');
            return;
        }

        this.mostrarConfirmacionEliminar = true;
        this.mostrarFormularioActualizar = false;
        this.accionSeleccionada = 'eliminar';
    }

    eliminarAsignacionConfirmada(): void {
        if (!this.asignacionSeleccionada) return;

        const id = this.asignacionSeleccionada.nIdAsignacionSolicitud;
        this.registroService.postEliminarAsignacionSolicitud({ nIdAsignacionSolicitud: id }).subscribe(res => {
        if (res?.nRetorno === 0) {
            this.mostrarMensajeEliminacion(res.sRetorno ?? 'No se pudo eliminar', 'error');
            return;
        }

        this.mostrarMensajeEliminacion(res?.sRetorno ?? 'Asignación eliminada correctamente', 'exito');
        this.obtenerAsignacionColaborador(this.currentColaboradorId);
        
        this.asignacionSeleccionada = null;
        this.mostrarConfirmacionEliminar = false;
        this.accionSeleccionada = 'ninguna';
        this.mensajeSeleccion = '';
        });
    }

    cancelarActualizar(): void {
        this.mostrarFormularioActualizar = false;
        this.accionSeleccionada = 'ninguna';
    }

    cancelarEliminacion(): void {
        this.mostrarConfirmacionEliminar = false;
        this.accionSeleccionada = 'ninguna';
    }

    mostrarError(msg: string, tipo: 'insert' | 'update' | 'delete' = 'insert'): void {
        if (tipo === 'insert') {
            this.mensajeErrorInsert = msg;
            setTimeout(() => this.mensajeErrorInsert = '', 3500);
        } else if (tipo === 'update') {
            this.mensajeErrorUpdate = msg;
            setTimeout(() => this.mensajeErrorUpdate = '', 3500);
        } else {
            this.mensajeErrorUpdate = msg;
            setTimeout(() => this.mensajeErrorDelete = '', 3500);
        }
    }

    mostrarMensajeExito(msg: string, tipo: 'insert' | 'update' | 'delete' = 'insert'): void {
        if (tipo === 'insert') {
        this.mensajeExitoInsert = msg;
        setTimeout(() => this.mensajeExitoInsert = '', 3500);
        } else if (tipo === 'update') {
        this.mensajeExitoUpdate = msg;
        setTimeout(() => this.mensajeExitoUpdate = '', 3500);
        } else {
        this.mensajeExitoDelete = msg;
        setTimeout(() => this.mensajeExitoDelete = '', 3500);
        }
    }

    mostrarMensajeEliminacion(msg: string, tipo: 'exito' | 'error'): void {
        if (tipo === 'exito') {
        this.mostrarMensajeExito(msg, 'delete');
        } else {
        this.mostrarError(msg, 'delete');
        }
    }
    ngOnDestroy(): void {
        this.subscriptions.forEach(sub => sub.unsubscribe());
    }
}