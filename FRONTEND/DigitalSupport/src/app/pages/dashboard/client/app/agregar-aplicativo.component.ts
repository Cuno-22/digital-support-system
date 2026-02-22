import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { AplicativoService } from '../../../../services/application/aplicativo.service';
import { ClienteService } from '../../../../services/clients/cliente.service';
import { ClienteUC } from '../../../../services/clients/ICliente';
import { Aplicativo, AplicativoDATA } from '../../../../services/application/IAplicativo';

@Component({
  selector: 'agregar-aplicativo',
  standalone: true,
  imports: [CommonModule, MatIconModule, FormsModule, MatTableModule, MatSortModule, MatPaginatorModule],
  templateUrl: './agregar-aplicativo.component.html',
  styleUrls: ['./agregar-aplicativo.component.css']
})
export class AgregarAplicativoComponent implements OnInit, OnDestroy {

  listaUsuariosCliente: ClienteUC[] = [];
  listaAplicativos: Aplicativo[] = [];

  mensajeError: string = '';
  mensajeExito: string = '';
  mostrarMensaje: boolean = false;
  
  fechaMaxima: string = '';
  filtro: string = '';
  filtroEstado: string = 'activo';
  aplicativos: any[] = [];
  aplicativosFiltrados: any[] = [];

  columnas: string[] = [
    'sNombreAplicativo',
    'sInformacionApp',
    'sVersion',
    'sNombreCliente',
    'dFechaLanzamiento',
    'bEstado',
    'extra'
  ];

  nuevoAplicativo: AplicativoDATA = {
    sNombreAplicativo: '',
    sDescripcion: '',
    dFechaLanzamiento: new Date(),
    sVersion: '',
    nIdCliente: 0,
    bEstado: true
  };

  modoEdicion: boolean = false;
  aplicativoEditando: Aplicativo | null = null;

  private subscriptions: Subscription[] = [];
  
  constructor(private aplicativoService: AplicativoService, private clienteService: ClienteService, private router: Router) {}
  
  ngOnInit(): void {
    const hoy = new Date();
    this.fechaMaxima = hoy.toISOString().split('T')[0];

    const idUsuarioCliente = Number(localStorage.getItem('nIdUsuarioCliente'));

    const sub = this.clienteService.getListaClienteporUC(idUsuarioCliente).subscribe((res) => {
      if (res.success && res.response?.data) {
        this.listaUsuariosCliente = res.response.data[0].data;
      };
    });

    this.subscriptions.push(sub);

    this.cargarAplicativos();
  }

  crearAplicativo(): void {
    // Validaciones básicas
    if (!this.nuevoAplicativo.sNombreAplicativo.trim() ||
        !this.nuevoAplicativo.sDescripcion.trim() ||
        !this.nuevoAplicativo.sVersion.trim() ||
        !this.nuevoAplicativo.nIdCliente) {
      this.mostrarError('Todos los campos son obligatorios.');
      return;
    }

    // Validación de fecha
    if (this.nuevoAplicativo.dFechaLanzamiento) {
      const fechaSeleccionada = new Date(this.nuevoAplicativo.dFechaLanzamiento);
      const hoy = new Date();
      hoy.setHours(0,0,0,0); // normalizamos a medianoche
      if (fechaSeleccionada > hoy) {
        this.mostrarError('La fecha de lanzamiento es obligatoria y no puede ser futura.');
        return;
      }
    }
    
    this.aplicativoService.postInsertarAplicativo(this.nuevoAplicativo).subscribe(res => {
      if (!res) {
        this.mostrarError('Error inesperado en el servidor.');
        return;
      }
      if (res?.nRetorno === 0 && res?.sRetorno) {
          this.mostrarError(res.sRetorno);
          return;
      }

      this.mostrarMensajeExito(res?.sRetorno ?? 'Aplicativo registrado correctamente');

      // Resetear formulario
      this.nuevoAplicativo = {
        sNombreAplicativo: '',
        sDescripcion: '',
        dFechaLanzamiento: new Date(),
        sVersion: '',
        nIdCliente: 0,
        bEstado: true
      };

      this.cargarAplicativos();
    });
  }
  
  cargarAplicativos(): void {
    const idUsuarioCliente = Number(localStorage.getItem('nIdUsuarioCliente'));

    const sub = this.aplicativoService.getListaAplicativo(idUsuarioCliente).subscribe(res => {
      if (res.success && res.response?.data) {
        this.listaAplicativos = res.response.data;
        this.aplicativos = [...this.listaAplicativos];
        this.aplicarFiltro();
      }
    });
    this.subscriptions.push(sub);
  }

  iniciarEdicion(app: Aplicativo): void {
      this.modoEdicion = true;
      this.aplicativoEditando = app;

      this.nuevoAplicativo = {
        nIdAplicativo: app.nIdAplicativo,
        sNombreAplicativo: app.sNombreAplicativo,
        sDescripcion: app.sInformacionApp,
        dFechaLanzamiento: new Date(app.dFechaLanzamiento),
        sVersion: app.sVersion,
        nIdCliente: app.nIdCliente,
        bEstado: app.bEstado
      };
  }

  cancelarEdicion(): void {
    this.modoEdicion = false;
    this.aplicativoEditando = null;
    this.resetFormulario();
  }

  actualizarAplicativo(): void {
    // Validaciones básicas
    if (!this.nuevoAplicativo.sNombreAplicativo.trim() ||
        !this.nuevoAplicativo.sDescripcion.trim() ||
        !this.nuevoAplicativo.sVersion.trim() ||
        !this.nuevoAplicativo.nIdCliente ||
        !this.nuevoAplicativo.dFechaLanzamiento ||
        !this.nuevoAplicativo.dFechaModificacion) {
      this.mostrarError('Todos los campos son obligatorios.');
      return;
    }

    // 🔸 Normalizamos fechas a medianoche
    const hoy = new Date();
    hoy.setHours(0, 0, 0, 0);

    const fechaLanzamiento = new Date(this.nuevoAplicativo.dFechaLanzamiento);
    fechaLanzamiento.setHours(0, 0, 0, 0);

    const fechaModificacion = new Date(this.nuevoAplicativo.dFechaModificacion);
    fechaModificacion.setHours(0, 0, 0, 0);

    // 🔸 Fecha lanzamiento no puede ser futura
    if (fechaLanzamiento > hoy) {
      this.mostrarError('La fecha de lanzamiento no puede ser futura.');
      return;
    }

    // 🔸 Fecha modificación no puede ser anterior al lanzamiento
    if (fechaModificacion < fechaLanzamiento) {
      this.mostrarError('La fecha de modificación no puede ser anterior a la fecha de lanzamiento.');
      return;
    }

    // 🔸 Fecha modificación no puede ser futura
    if (fechaModificacion > hoy) {
      this.mostrarError('La fecha de modificación no puede ser futura.');
      return;
    }

    this.aplicativoService.postActualizarAplicativo(this.nuevoAplicativo).subscribe(res => {
      if (res?.nRetorno === 0 && res?.sRetorno)
        return this.mostrarError(res.sRetorno);

      this.mostrarMensajeExito(res?.sRetorno ?? 'Aplicativo actualizado correctamente');
      this.cancelarEdicion();
      this.cargarAplicativos();
    });
  }

  aplicarFiltro(): void {
    let lista = [...this.aplicativos];

    // Filtro por texto
    if (this.filtro.trim()) {
      const filtroLower = this.filtro.toLowerCase();
      lista = lista.filter(app =>
        app.sNombreAplicativo?.toLowerCase().includes(filtroLower) ||
        app.sDescripcion?.toLowerCase().includes(filtroLower) ||
        app.sVersion?.toLowerCase().includes(filtroLower) ||
        app.sNombreCliente?.toLowerCase().includes(filtroLower)
      );
    }

    // Filtro por estado
    if (this.filtroEstado === 'activo') {
      lista = lista.filter(app => app.bEstado === true);
    } else if (this.filtroEstado === 'inactivo') {
      lista = lista.filter(app => app.bEstado === false);
    }

    this.aplicativosFiltrados = lista;
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

  resetFormulario(): void {
    this.nuevoAplicativo = {
      sNombreAplicativo: '',
      sDescripcion: '',
      dFechaLanzamiento: new Date(),
      sVersion: '',
      nIdCliente: 0,
      bEstado: true
    };
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}