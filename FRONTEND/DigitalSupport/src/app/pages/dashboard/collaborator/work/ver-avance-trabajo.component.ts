import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavComponent } from '../../../shared/nav/nav.component';
import { Router, RouterModule, ActivatedRoute, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { RegistroService } from '../../../../services/register/registro.service';
import { RegistroTrabajo, RegistroTrabajoDATA, RegistroTrabajoDLT } from '../../../../services/register/IRegistroTrabajo';
import { SolicitudService } from '../../../../services/request/solicitud.service';
import { Solicitud } from '../../../../services/request/ISolicitud';

@Component({
  selector: 'ver-avance-trabajo',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule, MatButtonModule, FormsModule, MatTableModule, MatSortModule, MatPaginatorModule],
  templateUrl: './ver-avance-trabajo.component.html',
  styleUrls: ['./ver-avance-trabajo.component.css']
})
export class VerAvanceTrabajoComponent implements OnInit, OnDestroy {

  listaRegistros: RegistroTrabajo[] = [];
  registros: RegistroTrabajo[] = [];
  registrosFiltrados: RegistroTrabajo[] = [];
  listaSolicitudes: Solicitud[] = [];
  nombreColaborador: string = '';
  hoy: string = new Date().toISOString().split('T')[0];
  filtro: string = '';
  filtroEstado: string = 'activo';
  columnas: string[] = [
    'sDetalleTrabajo',
    'sNombreColaborador',
    'sMotivo',
    'dFechaRegistro',
    'nHorasTrabajadas',
    'sObservacion',
    'bEstado',
    'Extra'
  ];

  mensajeError: string = '';
  mensajeExito: string = '';
  mostrarMensaje: boolean = false;

  mostrarConfirmacionEliminar: boolean = false;
  registroEliminar: RegistroTrabajoDLT | null = null;

  mostrarOpcionesExtra: boolean = false; // cuando se hace clic en el "+"
  mostrarFormularioActualizar: boolean = false;
  registroSeleccionado: RegistroTrabajo | null = null;

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
    this.cargaRegistros();
    this.cargarSolicitudes();
  }

  cargaRegistros(): void {
    const idColaborador = Number(localStorage.getItem('nIdColaborador'));
    const nombreColaborador = localStorage.getItem('sNombreColaborador') || '';
    this.nombreColaborador = nombreColaborador;

    const sub = this.registroService.getListaRegistroTrabajo(idColaborador).subscribe(res => {
      if (res.success && res.response?.data) {
        this.listaRegistros = res.response.data;
        this.registros = [...this.listaRegistros];
        this.registrosFiltrados = [...this.listaRegistros];
      }
    });
    this.subscriptions.push(sub);
  }

  cargarSolicitudes(): void {
    this.solicitudService.getListaTotalSolicitudes().subscribe(res => {
      if (res.success && res.response?.data) {
        this.listaSolicitudes = res.response.data;
      }
    });
  }

  abrirOpcionesExtra(registro: RegistroTrabajo): void {
    if (this.registroSeleccionado === registro && this.mostrarOpcionesExtra) {
      this.mostrarOpcionesExtra = false;
      this.registroSeleccionado = null;
    } else {
      // Abrir el panel para este registro
      this.registroSeleccionado = registro;
      this.mostrarOpcionesExtra = true;
    }
  }

  prepararActualizacion(): void {
    if (!this.registroSeleccionado) return;

    // Cerrar panel eliminar si estaba abierto
    this.mostrarConfirmacionEliminar = false

    this.mostrarOpcionesExtra = false;
    this.mostrarFormularioActualizar = true;

    const nIdColaborador = Number(localStorage.getItem('nIdColaborador')) || 0;

    // Buscar el id de solicitud a partir del motivo
    const solicitudEncontrada = this.listaSolicitudes.find(
      s => s.sMotivo === this.registroSeleccionado?.sMotivo
    );

    const nIdSolicitud = solicitudEncontrada?.nIdSolicitud ?? 0;

    this.nuevoRegistro = {
      nIdRegistroTrabajo: this.registroSeleccionado.nIdRegistroTrabajo,
      nIdSolicitud: nIdSolicitud,
      nIdColaborador: nIdColaborador,
      sDescripcion: '',
      dFechaRegistro: new Date(this.registroSeleccionado.dFechaRegistro),
      nHorasTrabajadas: this.registroSeleccionado.nHorasTrabajadas,
      sObservacion: '',
      bEstado: this.registroSeleccionado.bEstado
    };
  }

  prepararEliminacion(): void {
    if (!this.registroSeleccionado) return;
    // Cerrar formulario actualizar si estaba abierto
    this.mostrarFormularioActualizar = false;
    this.mostrarOpcionesExtra = false;
    this.confirmarEliminarRegistro(this.registroSeleccionado);
  }

  confirmarEliminarRegistro(n: RegistroTrabajo): void {
    this.registroEliminar = { nIdRegistroTrabajo: n.nIdRegistroTrabajo };
    // Activa el modal de confirmación en pantalla
    this.mostrarConfirmacionEliminar = true;
  }

  eliminarRegistroConfirmado(): void {
    const registro = this.registroEliminar;
    if (!registro) return;

      this.registroService.postEliminarRegistroTrabajo(registro).subscribe({
        next: (res) => {

          if (!res) {
            this.mostrarError('Error inesperado en el servidor.');
            return;
          }
          if (res.nRetorno === 0 && res.sRetorno) {
            this.mostrarError(res.sRetorno);
            return;
          }

          this.mostrarMensajeExito(res?.sRetorno ?? 'Registro eliminado correctamente');

          this.cargaRegistros();
          this.registroEliminar = null;
          this.mostrarConfirmacionEliminar = false;
        },
        error: (err) => {
          this.mostrarError('Error al eliminar el Registro.');
          console.error(err);
        }
    });
  }

  cancelarEliminacion(): void {
    this.mostrarConfirmacionEliminar = false;
    this.registroEliminar = null;
  }

  cancelarActualizacion(): void {
    this.mostrarFormularioActualizar = false;
    this.registroSeleccionado = null;
  }

  actualizarRegistro(): void {
    if (!this.nuevoRegistro.sDescripcion.trim() ||
        !this.nuevoRegistro.sObservacion.trim() ||
        this.nuevoRegistro.nHorasTrabajadas == null) {
      this.mostrarError('Todos los campos son obligatorios y válidos.');
      return;
    }

    // Validar horas trabajadas (mínimo 1)
    if (this.nuevoRegistro.nHorasTrabajadas < 1) {
      this.mostrarError('Las horas trabajadas deben ser como mínimo 1.');
      return;
    }

    // 🔹 Validar que se haya seleccionado una fecha (obligatoria)
    if (!this.nuevoRegistro.dFechaRegistro) {
      this.mostrarError('Debe seleccionar una fecha de registro.');
      return;
    }

    // Validación de fecha no futura
    const hoy = new Date();
    if (this.nuevoRegistro.dFechaRegistro && new Date(this.nuevoRegistro.dFechaRegistro) > hoy) {
      this.mostrarError('Se debe registrar una fecha de registro y esta no puede ser futura.');
      return;
    }

    // 🔹 Convertir la fecha (asegurar formato Date)
    const fechaRegistro = new Date(this.nuevoRegistro.dFechaRegistro);
    if (isNaN(fechaRegistro.getTime())) {
      this.mostrarError('Debe seleccionar una fecha válida.');
      return;
    }

    // 🔹 Validar que haya cambiado o confirmado la fecha anterior
    const fechaOriginal = this.registroSeleccionado
      ? new Date(this.registroSeleccionado.dFechaRegistro)
      : null;
    if (
      fechaOriginal &&
      fechaOriginal.toDateString() === fechaRegistro.toDateString()
    ) {
      this.mostrarError('Debe confirmar o modificar la fecha antes de guardar.');
      return;
    }

    this.registroService.postActualizarRegistroTrabajo(this.nuevoRegistro).subscribe({
      next: (res) => {
        if (!res) {
          this.mostrarError('Error inesperado en el servidor.');
          return;
        }

        if (res.nRetorno === 0 && res.sRetorno) {
          this.mostrarError(res.sRetorno);
          return;
        }

        this.mostrarMensajeExito(res?.sRetorno ?? 'Registro actualizado correctamente');
        this.mostrarFormularioActualizar = false;
        this.cargaRegistros();
      },
      error: (err) => {
        this.mostrarError('Error al actualizar el registro.');
        console.error(err);
      }
    });
  }

  aplicarFiltro(): void {
    let lista = [...this.registros];

    // Filtro por texto
    if (this.filtro.trim()) {
      const filtroLower = this.filtro.toLowerCase();
      lista = lista.filter(reg =>
        reg.sDetalleTrabajo?.toLowerCase().includes(filtroLower) ||
        reg.sMotivo?.toLowerCase().includes(filtroLower) ||
        reg.sNombreColaborador?.toLowerCase().includes(filtroLower) ||
        reg.sObservacion?.toLowerCase().includes(filtroLower)
      );
    }
    this.registrosFiltrados = lista;
  }

  resaltarSimilitud(texto: string): string {
    if (!this.filtro.trim()) return texto;
    const regex = new RegExp(`(${this.filtro})`, 'gi');
    return texto?.replace(regex, '<strong>$1</strong>') ?? '';
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