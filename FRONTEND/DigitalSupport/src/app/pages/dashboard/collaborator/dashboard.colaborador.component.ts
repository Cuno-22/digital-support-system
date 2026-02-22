import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavComponent } from '../../../shared/nav/nav.component';
import { Router, RouterModule, ActivatedRoute, NavigationEnd } from '@angular/router';
import { Subscription, forkJoin } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { LoginService } from '../../../services/auth/login.service';
import { SolicitudService } from '../../../services/request/solicitud.service';
import { Solicitud, SolicitudDATA, TipoSolicitud } from '../../../services/request/ISolicitud';
import { IRetorno_DigitalSupportV3 } from '../../../services/auth/IRetornoResponse';
import { ClienteService } from '../../../services/clients/cliente.service';
import { UsuarioCliente } from '../../../services/clients/ICliente';
import { AplicativoService } from '../../../services/application/aplicativo.service';
import { Aplicativo } from '../../../services/application/IAplicativo';

@Component({
  selector: 'app-dashboard-colaborador',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule, FormsModule],
  templateUrl: './dashboard.colaborador.component.html',
  styleUrls: ['./dashboard.colaborador.component.css']
})
export class DashboardColaboradorComponent implements OnInit, OnDestroy {
  userData?: IRetorno_DigitalSupportV3;

  mostrarUserInfo= false;
  mostrarNotificaciones = false;
  mostrarPanelColaborador = false;

  listaTotalSolicitudes: Solicitud[] = [];
  listaSolicitudesFaltantesPendientes: Solicitud[] = [];
  listaSolicitudesFinalizadas: Solicitud[] = [];

  listaUsuarioCliente: UsuarioCliente[] = [];
  listaAplicativo: Aplicativo[] = [];
  listaTipoSolicitud: TipoSolicitud[] = [];

  filtroActivo: 'todas' | 'pendientes' | 'finalizadas' = 'todas';

  solicitudSeleccionada?: Solicitud;
  actualizarSolicitudForm: SolicitudDATA = {
    nIdUsuarioCliente: 0,
    nIdAplicativo: 0,
    nIdTipoSolicitud: 0,
    sMotivo: '',
    dFechaCreacion: null,
    dFechaFinalizacion: null,
    sEstado: '',
    bEstado: true
  };

  mostrarFormularioEdicion = false;
  mensajeError: string = '';
  mensajeExito: string = '';
  mostrarMensaje: boolean = false;

  fechaActual: Date = new Date();
  private subscriptions: Subscription[] = [];

  constructor(private loginService: LoginService, private solicitudService: SolicitudService, private clienteService: ClienteService, private aplicativoService: AplicativoService, private router: Router) {}
  
  ngOnInit(): void {
    const sub1 = this.loginService.userData.subscribe(data => {
      this.userData = data;
    });

    // Cargar listas base de solicitudes y tipos
    const sub2 = forkJoin([
      this.solicitudService.getListaTotalSolicitudes(),
      this.solicitudService.getListaSolicitudEnProcesoPendiente(),
      this.solicitudService.getListaSolicitudFinalizada(),
      this.clienteService.getListaUsuarioCliente(),
      this.solicitudService.getListaTipoSolicitud()
    ]).subscribe(([resTotal, resPend, resFin, resClientes, resTipos]) => {
      if (resTotal.success && resTotal.response?.data)
        this.listaTotalSolicitudes = this.ordenarPorFecha(resTotal.response.data);
      if (resPend.success && resPend.response?.data)
        this.listaSolicitudesFaltantesPendientes = this.ordenarPorFecha(resPend.response.data);
      if (resFin.success && resFin.response?.data)
        this.listaSolicitudesFinalizadas = this.ordenarPorFecha(resFin.response.data);
      if (resClientes.success && resClientes.response?.data)
        this.listaUsuarioCliente = resClientes.response.data;
      if (resTipos.success && resTipos.response?.data)
        this.listaTipoSolicitud = resTipos.response.data;
    });

    this.subscriptions.push(sub1, sub2);

    // Detectar navegación al dashboard
    const sub3 = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.mostrarPanelColaborador = event.urlAfterRedirects === '/dashboard/colaborador';
      }
    });
    this.mostrarPanelColaborador = this.router.url === '/dashboard/colaborador';
    this.subscriptions.push(sub3);
  }

  private ordenarPorFecha(lista: Solicitud[]): Solicitud[] {
    return lista.sort((a, b) => new Date(b.dFechaCreacion).getTime() - new Date(a.dFechaCreacion).getTime());
  }

  cambiarFiltro(filtro: 'todas' | 'pendientes' | 'finalizadas'): void {
    this.filtroActivo = filtro;
  }

  toggleUserMenu() {
    this.mostrarUserInfo = !this.mostrarUserInfo;
    this.mostrarNotificaciones = false;
  }

  toggleNotificaciones() {
    this.mostrarNotificaciones = !this.mostrarNotificaciones;
    this.mostrarUserInfo = false;
  }

  editarSolicitud(solicitud: Solicitud): void {
    this.solicitudSeleccionada = solicitud;

    if (!solicitud.nIdUsuarioCliente) {
      this.mostrarError('La solicitud no tiene asociado un usuario cliente.');
      return;
    }

    // 🔹 Buscar el cliente asociado por su ID
    const cliente = this.listaUsuarioCliente.find(
      (u) => u.nIdUsuarioCliente === solicitud.nIdUsuarioCliente
    );

    // 🔹 Guardar el nombre del cliente para mostrar en HTML
    const nombreCliente = cliente?.sNombreUsuarioCliente ?? solicitud.sNombreUsuarioCliente ?? '';

    // 🔹 Cargar aplicativos asociados al cliente
    this.aplicativoService.getListaAplicativo(solicitud.nIdUsuarioCliente).subscribe((resApp) => {
      if (resApp.success && resApp.response?.data) {
        this.listaAplicativo = resApp.response.data;

        // 🔹 Buscar aplicativo actual (por nombre o ID)
        const nombreBuscado = solicitud.sNombreAplicativo?.trim().toLowerCase();
        const aplicativo = this.listaAplicativo.find((a) => {
          const nombreActual = a.sNombreAplicativo?.trim().toLowerCase();
          return nombreActual === nombreBuscado;
        });

        const nIdAplicativo = aplicativo?.nIdAplicativo ?? 0;

        console.log('Nombre aplicativo de la solicitud:', solicitud.sNombreAplicativo);
        // 🔹 Buscar tipo de solicitud actual (por nombre o ID)
        const tipoSolicitud = this.listaTipoSolicitud.find(
          (t) =>
            t.sTipoSolicitud?.trim().toLowerCase() ===
            solicitud.sTipoSolicitud?.trim().toLowerCase()
        );

        console.log({
  clienteEncontrado: cliente,
  aplicativoEncontrado: aplicativo,
  tipoSolicitudEncontrado: tipoSolicitud,
  listaAplicativo: this.listaAplicativo,
  listaTipoSolicitud: this.listaTipoSolicitud
});

        // 🔹 Armar el formulario para edición
        this.actualizarSolicitudForm = {
          nIdSolicitud: solicitud.nIdSolicitud,
          nIdUsuarioCliente: solicitud.nIdUsuarioCliente ?? cliente?.nIdUsuarioCliente ?? 0,
          nIdAplicativo: nIdAplicativo,
          nIdTipoSolicitud: tipoSolicitud?.nIdTipoSolicitud ?? 0,
          sMotivo: solicitud.sMotivo,
          dFechaCreacion: solicitud.dFechaCreacion, // ⚠️ No se modifica en el front
          dFechaFinalizacion: solicitud.dFechaFinalizacion || null,
          sEstado: solicitud.sEstado || 'Pendiente',
          bEstado: solicitud.bEstado ?? true
        };

        // 🔹 Guardamos también nombres para mostrar en HTML
        this.solicitudSeleccionada = {
          ...solicitud,
          sNombreUsuarioCliente: nombreCliente,
          sNombreAplicativo: aplicativo?.sNombreAplicativo ?? solicitud.sNombreAplicativo,
          sTipoSolicitud: tipoSolicitud?.sTipoSolicitud ?? solicitud.sTipoSolicitud
        };

        this.mostrarFormularioEdicion = true;
      } else {
        this.mostrarError('No se pudieron cargar los aplicativos del usuario cliente.');
      }
    });
  }
  
  cancelarEdicion(): void {
    this.mostrarFormularioEdicion = false;
    this.solicitudSeleccionada = undefined;
  }

  guardarCambios(): void {
    const form = this.actualizarSolicitudForm;
    const solicitudOriginal = this.solicitudSeleccionada;

    // 🔹 Validar existencia de solicitud y datos base
    if (!solicitudOriginal || !form) {
      this.mostrarError('Debe seleccionar una solicitud válida para actualizar.');
      return;
    }

    // 🔹 Validar que los campos ineditables vengan con datos
    if (
      !solicitudOriginal.nIdSolicitud ||
      !solicitudOriginal.nIdUsuarioCliente ||
      !form.nIdAplicativo ||
      !form.nIdTipoSolicitud ||
      !solicitudOriginal.dFechaCreacion
    ) {
      this.mostrarError('Faltan datos obligatorios de la solicitud seleccionada. No se puede actualizar.');
      return;
    }

    // 🔹 Validar campos editables obligatorios
    if (!form.sMotivo?.trim() || !form.sEstado || form.bEstado === null || form.bEstado === undefined) {
      this.mostrarError('Todos los campos editables son obligatorios.');
      return;
    }

    // 🔹 Validar estados permitidos
    const estadosPermitidos = ['Pendiente', 'En Proceso', 'Finalizado'];
    if (!estadosPermitidos.includes(form.sEstado)) {
      this.mostrarError('El estado solo puede ser: Pendiente, En Proceso o Finalizada.');
      return;
    }

    // 🔹 Validar coherencia de fechas
    const fechaCreacion = new Date(solicitudOriginal.dFechaCreacion);
    const fechaActual = new Date();
    let fechaFinalizacion = form.dFechaFinalizacion ? new Date(form.dFechaFinalizacion) : null;

    // Si está Finalizada, debe existir una fecha final (por defecto, hoy)
    if (form.sEstado === 'Finalizado') {
      if (!fechaFinalizacion) {
        fechaFinalizacion = new Date(); // asigna fecha actual si no se ingresó
        form.dFechaFinalizacion = fechaFinalizacion;
      }

      // Validar que no sea anterior a la de creación
      if (fechaFinalizacion.getTime() > Date.now()) {
        this.mostrarError('La fecha de finalización no puede ser futura.');
        return;
      }

      // Validar que no sea futura
      if (fechaFinalizacion > fechaActual) {
        this.mostrarError('La fecha de finalización no puede ser futura.');
        return;
      }
    } else {
      // Si no está finalizada, se borra la fecha final (no aplica)
      form.dFechaFinalizacion = null;
    }

    // 🔹 Reafirmar campos ineditables (no modificables por el front)
    form.nIdSolicitud = solicitudOriginal.nIdSolicitud;
    form.nIdUsuarioCliente = solicitudOriginal.nIdUsuarioCliente;
    form.dFechaCreacion = solicitudOriginal.dFechaCreacion;

    // 🔹 Confirmación lógica extra
    if (form.nIdUsuarioCliente === 0 || form.nIdAplicativo === 0 || form.nIdTipoSolicitud === 0) {
      this.mostrarError('No se pudo determinar los IDs internos de la solicitud. Intente nuevamente.');
      return;
    }

    
    console.log('📦 Datos enviados al backend:', form);
    // 🔹 Enviar datos al backend
    this.solicitudService.postActualizarSolicitud(form).subscribe({
    next: (res: any) => {
      console.log('📥 Respuesta del backend:', res);

      // 👇 Aquí tomamos la parte importante de la respuesta
      const data = res?.response?.data;

      if (!data) {
        this.mostrarError('Respuesta inválida del servidor.');
        return;
      }

      if (data.nRetorno === 0) {
        this.mostrarError(data.sRetorno || 'No se pudo actualizar la solicitud.');
        return;
      }

      this.mostrarMensajeExito(data.sRetorno ?? 'Solicitud actualizada correctamente');
      this.mostrarFormularioEdicion = false;
      this.actualizarFichas();
    },
    error: (err) => {
      console.error('❌ Error al actualizar la solicitud:', err);
      this.mostrarError('Error al actualizar la solicitud.');
    }
  });
  }

  mostrarError(msg: string): void {
    this.mensajeError = msg;
    setTimeout(() => (this.mensajeError = ''), 3500);
  }

  mostrarMensajeExito(msg: string): void {
    console.log('✅ Mostrando mensaje de éxito:', msg);
    this.mensajeExito = msg;
    setTimeout(() => {
      this.mensajeExito = '';
    }, 3500);
  }

  onEstadoChange(nuevoEstado: string): void {
    this.actualizarSolicitudForm.sEstado = nuevoEstado;

    if (nuevoEstado === 'Finalizado') {
      this.actualizarSolicitudForm.dFechaFinalizacion = new Date();
    } else {
      this.actualizarSolicitudForm.dFechaFinalizacion = null;
    }
  }

  actualizarFichas(): void {
    forkJoin([
      this.solicitudService.getListaTotalSolicitudes(),
      this.solicitudService.getListaSolicitudEnProcesoPendiente(),
      this.solicitudService.getListaSolicitudFinalizada()
    ]).subscribe(([resTotal, resPend, resFin]) => {
      if (resTotal.success && resTotal.response?.data)
        this.listaTotalSolicitudes = this.ordenarPorFecha(resTotal.response.data);
      if (resPend.success && resPend.response?.data)
        this.listaSolicitudesFaltantesPendientes = this.ordenarPorFecha(resPend.response.data);
      if (resFin.success && resFin.response?.data)
        this.listaSolicitudesFinalizadas = this.ordenarPorFecha(resFin.response.data);
    });
  }

  cerrarSesion(): void {
    this.loginService.logout();
    this.router.navigateByUrl('/iniciar-sesion');
  }

  gestionarNotificaciones() {
    this.router.navigate(['/dashboard/notificacion/gestionar-visualizar']);
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
