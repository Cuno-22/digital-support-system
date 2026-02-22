import { Component, OnInit, OnDestroy} from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavComponent } from '../../../../shared/nav/nav.component';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Notificacion, NotificacionDATA, NotificacionDLT } from '../../../../services/notify/INotificacion';
import { NotificacionService } from '../../../../services/notify/notificacion.service';
import { Solicitud } from '../../../../services/request/ISolicitud';
import { SolicitudService } from '../../../../services/request/solicitud.service';

@Component({
  selector: 'app-dashboard-notificacion-gestionar-visualizar',
  standalone: true,
  imports: [CommonModule, MatIconModule, FormsModule, MatFormFieldModule, MatInputModule, MatTableModule, MatSortModule, MatButtonModule, MatIconModule],
  templateUrl: './notificacion.gestionar-visualizar.component.html',
  styleUrls: ['./notificacion.gestionar-visualizar.component.css']
})

export class NotificacionGestionarVisualizarComponent implements OnInit, OnDestroy {
  
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
    'bEstado',
    'Extra'
  ];

  accionForm: 'agregar' | 'actualizar' | null = null;
  notificacionForm: NotificacionDATA = {
    nIdNotificacion: 0,
    nIdSolicitud: 0,
    sDescripcion: '',
    dFechaEnvio: new Date(),
    bLeido: false,
    bEstado: true
  };

  fechaMaxima: string = '';

  mensajeError: string = '';
  mensajeExito: string = '';
  mostrarMensaje: boolean = false;

  mostrarUserInfo = false;
  mostrarNotificaciones = false;

  mostrarCargando: boolean = false;
  mensajeCarga: string = ''; 
  mostrarConfirmacion: boolean = false;

  listaSolicitud: Solicitud[] = [];
  solicitudSeleccionada: Solicitud | null = null;
  notificacionSeleccionada: Notificacion | null = null;

  mostrarConfirmacionEliminar: boolean = false;
  notificacionEliminar: NotificacionDLT | null = null;
  
  constructor(private notificacionService: NotificacionService, private solicitudService: SolicitudService, private router: Router) {}
  
  private subscriptions: Subscription[] = [];

  ngOnInit(): void {
    const hoy = new Date();
    this.fechaMaxima = hoy.toISOString().split('T')[0];

    this.solicitudService.getListaTotalSolicitudes().subscribe(res => {
        if (res.success && res.response?.data) {
            this.listaSolicitud = res.response.data;
        }
    });
        
    this.cargarNotificaciones();
  }

  cargarNotificaciones(): void {
    const sub1 = this.notificacionService.getListaNotificacion().subscribe();

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

  setAccion(accion: 'agregar' | 'actualizar'): void {
    this.cerrarPanelEliminar();

    this.accionForm = accion;
    this.mensajeError = '';
    this.mensajeExito = '';

    // Resetear formulario según la acción
    this.notificacionForm = {
      nIdNotificacion: 0,
      nIdSolicitud: 0,
      sDescripcion: '',
      dFechaEnvio: new Date(),
      bLeido: false,
      bEstado: true
    };

    // Limpiar selección
    this.notificacionSeleccionada = null;
    this.solicitudSeleccionada = null;
  }

  gestionarNotificacion() {
    if (!this.accionForm) return;

    // Validaciones básicas
    if (!this.notificacionForm.nIdSolicitud) {
      this.mostrarError('Debe seleccionar una solicitud.');
      return;
    }

    if (!this.notificacionForm.sDescripcion.trim()) {
      this.mostrarError('Debe ingresar una descripción para la notificación.');
      return;
    }

    if (this.notificacionForm.dFechaEnvio) {
      const fechaSeleccionada = new Date(this.notificacionForm.dFechaEnvio);
      fechaSeleccionada.setHours(0, 0, 0, 0);

      const hoy = new Date();
      hoy.setHours(0, 0, 0, 0);

      if (fechaSeleccionada > hoy) {
        this.mostrarError('La fecha de envío no puede ser futura.');
        return;
      }
    }

    switch (this.accionForm) {
      case 'agregar':
        this.iniciarCarga('Enviando aviso al Usuario Cliente...');
        
        this.notificacionService.postInsertarNotificacion(this.notificacionForm).subscribe((res: any) => {
          setTimeout(() => this.detenerCarga(), 1500);
          
          // 1. Validamos que la respuesta exista y sea success: true
          if (!res || !res.success) {
            this.mostrarError('Error inesperado en el servidor.');
            return;
          }

          // 2. Extraemos la data exactamente donde viene en tu JSON
          const dataInsert = Array.isArray(res.response?.data) ? res.response.data[0] : res.response?.data;

          if (dataInsert?.nRetorno === 0 && dataInsert?.sRetorno) {
            this.mostrarError(dataInsert.sRetorno);
            return;
          }

          // 3. Mostramos el mensaje ("La Notificacion se registro satisfactoriamente")
          this.mostrarMensajeExito(dataInsert?.sRetorno ?? 'Notificacion registrada correctamente');
          
          // 4. SACAMOS EL ID CORRECTO (Ej: el 17)
          const idNotificacion = dataInsert?.nRetorno;

          if (idNotificacion && idNotificacion > 0) {
            // ¡AHORA SÍ LLAMAMOS AL CORREO CON EL ID CORRECTO!
            this.enviarMensajeAlUC(idNotificacion);
          } else {
            console.warn('No se devolvió un ID válido de notificación. Valor extraído:', idNotificacion);
          }

          // Resetear formulario
          this.notificacionForm = {
            nIdNotificacion: 0,
            nIdSolicitud: 0,
            sDescripcion: '',
            dFechaEnvio: new Date(),
            bLeido: false,
            bEstado: true
          };
          this.solicitudSeleccionada = null;

          this.cargarNotificaciones();
        });
        break;

      case 'actualizar':
        if (typeof this.notificacionForm.dFechaEnvio === 'string') {
          const fecha = new Date(this.notificacionForm.dFechaEnvio);
          const año = fecha.getFullYear();
          const mes = String(fecha.getMonth() + 1).padStart(2, '0');
          const dia = String(fecha.getDate()).padStart(2, '0');
          const hora = String(fecha.getHours()).padStart(2, '0');
          const minuto = String(fecha.getMinutes()).padStart(2, '0');
          const segundo = String(fecha.getSeconds()).padStart(2, '0');

          this.notificacionForm.dFechaEnvio = `${año}-${mes}-${dia} ${hora}:${minuto}:${segundo}` as any;
        }

        this.iniciarCarga('Actualizando y enviando aviso al cliente...');

        this.notificacionService.postActualizarNotificacion(this.notificacionForm).subscribe((res: any) => {
          
          if (!res || !res.success) {
            this.detenerCarga();
            this.mostrarError('Error inesperado en el servidor.');
            return;
          }

          const dataUpdate = Array.isArray(res.response?.data) ? res.response.data[0] : res.response?.data;

          if (dataUpdate?.nRetorno === 0 && dataUpdate?.sRetorno) {
              this.detenerCarga();
              this.mostrarError(dataUpdate.sRetorno);
              return;
          }

          // La notificación se actualizó en BD. Ahora enviamos el correo.
          // Usamos el ID que ya teníamos seleccionado en el formulario
          const idNotificacion = this.notificacionForm.nIdNotificacion;

          if (idNotificacion && idNotificacion > 0) {
            this.enviarMensajeAlUC(idNotificacion);
          } else {
            this.detenerCarga();
            console.warn('No se encontró un ID válido para enviar el correo.');
          }

          this.mostrarMensajeExito(dataUpdate?.sRetorno ?? 'Notificacion actualizada correctamente');
          
          // Resetear formulario
          this.notificacionForm = {
            nIdNotificacion: 0,
            nIdSolicitud: 0,
            sDescripcion: '',
            dFechaEnvio: new Date(),
            bLeido: false,
            bEstado: true
          };

          this.solicitudSeleccionada = null;
          this.notificacionSeleccionada = null;
          
          this.cargarNotificaciones();
        });
        break;
    }
  }

  confirmarEliminarNotificacion(n: Notificacion): void {
    this.cerrarFormulario()
    
    this.notificacionEliminar = { nIdNotificacion: n.nIdNotificacion };
    // Activa el modal de confirmación en pantalla
    this.mostrarConfirmacionEliminar = true;
  }

  eliminarNotificacionConfirmada(): void {
    const notificacion = this.notificacionEliminar;
    if (!notificacion) return;

      this.notificacionService.postEliminarNotificacion(notificacion).subscribe({
        next: (res) => {

          if (!res) {
            this.mostrarError('Error inesperado en el servidor.');
            return;
          }
          if (res.nRetorno === 0 && res.sRetorno) {
            this.mostrarError(res.sRetorno);
            return;
          }

          this.mostrarMensajeExito(res?.sRetorno ?? 'Notificación eliminada correctamente');

          this.cargarNotificaciones();
          this.notificacionEliminar = null;
          this.mostrarConfirmacionEliminar = false;
        },
        error: (err) => {
          this.mostrarError('Error al eliminar la notificación.');
          console.error(err);
        }
    });
  }

  cancelarEliminacion(): void {
    this.mostrarConfirmacionEliminar = false;
    this.notificacionEliminar = null;
  }

  enviarMensajeAlUC(nIdNotificacion: number): void {
    this.notificacionService.getMensajeNotificacionUC(nIdNotificacion).subscribe({
      next: (res) => {
        this.detenerCarga(); // Apagamos el spinner
        if (res?.success) {
          this.mostrarMensajeExito('Se envió exitosamente el correo de notificación al cliente.');
        } else {
          this.mostrarError('No se pudo enviar el correo o el cliente no tiene email.');
        }
      },
      error: (err) => {
        this.detenerCarga(); // Apagamos el spinner si falla
        this.mostrarError('Ocurrió un error al intentar enviar el correo.');
        console.error(err);
      }
    });
  }

  reenviarMensajeCorreo(): void {
    if (!this.notificacionSeleccionada?.nIdNotificacion) return;
    this.enviarMensajeAlUC(this.notificacionSeleccionada.nIdNotificacion);
  }

  onSeleccionarNotificacion(): void {
    if (!this.notificacionSeleccionada) return;

    const n = this.notificacionSeleccionada;

    // Cargar datos al formulario
    this.notificacionForm = {
      nIdNotificacion: n.nIdNotificacion,
      nIdSolicitud: 0, // se reasigna si el usuario selecciona otro
      sDescripcion: n.sInformacionNotificada ?? '',
      dFechaEnvio: new Date(n.dFechaEnvio),
      bLeido: n.bLeido,
      bEstado: n.bEstado
    };
  }

  onSeleccionarSolicitud(): void {
    if (this.solicitudSeleccionada) {
      this.notificacionForm.nIdSolicitud = this.solicitudSeleccionada.nIdSolicitud;
    } else {
      this.notificacionForm.nIdSolicitud = 0;
    }
  }

  cancelarFormulario(): void {
    this.accionForm = null;
  }

  cerrarPanelEliminar(): void {
    if (this.mostrarConfirmacionEliminar) {
      this.mostrarConfirmacionEliminar = false;
      this.notificacionEliminar = null;
    }
  }

  cerrarFormulario(): void {
    if (this.accionForm) {
      this.accionForm = null;
    }
  }

  resaltarSimilitud(texto: string): string {
    if (!this.filtro.trim()) return texto;
    const regex = new RegExp(`(${this.filtro})`, 'gi');
    return texto?.replace(regex, '<strong>$1</strong>') ?? '';
  }

  iniciarCarga(mensaje: string): void {
    this.mostrarCargando = true;
    this.mensajeCarga = mensaje;
  }

  detenerCarga(): void {
    this.mostrarCargando = false;
    this.mensajeCarga = '';
  }

  confirmarReenvio(): void {
    this.mostrarConfirmacion = true;
  }

  confirmarEnvio(): void {
    if (!this.notificacionSeleccionada?.nIdNotificacion) return;
    this.mostrarConfirmacion = false;
    this.mostrarCargando = true;
    this.enviarMensajeAlUC(this.notificacionSeleccionada.nIdNotificacion);
    setTimeout(() => this.mostrarCargando = false, 2500);
  }

  cancelarEnvio(): void {
    this.mostrarConfirmacion = false;
  }

  regresarDashboard(): void {
    if (localStorage.getItem('nIdAdmin')) {
      this.router.navigate(['/dashboard/admin']);
    } 
    else if (localStorage.getItem('nIdColaborador')) {
      this.router.navigate(['/dashboard/colaborador']);
    }
    else {
      // Si no mandamos de vuelta al login
      this.router.navigate(['/iniciar-sesion']);
    }
  }

  cerrarSesion(): void {
    localStorage.clear();
    this.router.navigateByUrl('/iniciar-sesion');
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