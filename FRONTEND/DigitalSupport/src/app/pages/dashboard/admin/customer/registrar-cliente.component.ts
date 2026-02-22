import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { ClienteService } from '../../../../services/clients/cliente.service';
import { Cliente, TipoCliente, ClienteEmpresa, ClientePersonaNatural } from '../../../../services/clients/ICliente';

@Component({
  selector: 'registrar-cliente',
  standalone: true,
  imports: [CommonModule, MatIconModule, FormsModule],
  templateUrl: './registrar-cliente.component.html',
  styleUrls: ['./registrar-cliente.component.css']
})
export class RegistrarClienteComponent implements OnInit, OnDestroy {

  tiposCliente: TipoCliente[] = [];

  listaClienteEmpresa: ClienteEmpresa[] = [];
  listaClientePersonaNatural: ClientePersonaNatural[] = [];

  clientesFiltrados: any[] = [];
  filtroClienteActual: 'Empresa' | 'Persona Natural' | null = null;

  modoEdicion: boolean = false;

  mensajeError: string = '';
  mensajeExito: string = '';
  mostrarMensaje: boolean = false;
  fechaMaxima: string = '';

  nuevoCliente: Cliente = {
    sNombre: '',
    sApellido: null,
    nEdad: null,
    dFechaNacimiento: null,
    sEmail: '',
    sContrasena: '',
    nIdTipoCliente: 0,
    bEstado: true
  };

  private subscriptions: Subscription[] = [];

  constructor(
    private clienteService: ClienteService,
    private router: Router
  ) {}

  // =========================
  // INIT
  // =========================
  ngOnInit(): void {
    const hoy = new Date();
    this.fechaMaxima = hoy.toISOString().split('T')[0];

    this.clienteService.getListaTipoCliente().subscribe(res => {
      if (res.success && res.response?.data) {
        this.tiposCliente = res.response.data;
      }
    });

    this.cargarListasClientes();
    this.filtroClienteActual = 'Empresa';
  }

  // =========================
  // CARGA DE LISTAS
  // =========================
  cargarListasClientes(): void {
    this.clienteService.getListaCliente(1).subscribe(res => {
      if (res.success && res.response?.data?.[0]?.data) {
        this.listaClienteEmpresa = res.response.data[0].data as ClienteEmpresa[];

        if (this.filtroClienteActual === 'Empresa') {
          this.filtrarClientes('Empresa');
        }
      }
    });

    this.clienteService.getListaCliente(2).subscribe(res => {
      if (res.success && res.response?.data?.[0]?.data) {
        this.listaClientePersonaNatural = res.response.data[0].data as ClientePersonaNatural[];

        if (this.filtroClienteActual === 'Persona Natural') {
          this.filtrarClientes('Persona Natural');
        }
      }
    });
  }

  // =========================
  // FILTRAR (SOLO EN MEMORIA)
  // =========================
  filtrarClientes(tipo: 'Empresa' | 'Persona Natural'): void {
    this.filtroClienteActual = tipo;

    if (tipo === 'Empresa') {
      this.clientesFiltrados = this.listaClienteEmpresa.map(e => ({
        nIdCliente: e.nIdCliente,
        nIdTipoCliente: 1,
        nombre: e.sNombreClienteEmpresa,
        usuario: e.sEmail,
        tipo: 'Empresa',
        estado: e.bEstado
      }));
    }

    if (tipo === 'Persona Natural') {
      this.clientesFiltrados = this.listaClientePersonaNatural.map(p => ({
        nIdCliente: p.nIdCliente,
        nIdTipoCliente: 2,
        nombre: p.sNombreClienteNatural,
        edad: p.nEdad,
        fechaNacimiento: p.dFechaNacimiento,
        usuario: p.sEmail,
        tipo: 'Persona Natural',
        estado: p.bEstado
      }));
    }
  }

  // =========================
  // TIPO CLIENTE CHANGE
  // =========================
  onTipoClienteChange(): void {
    if (this.modoEdicion) return; // 🔒 no permitir cambiar tipo en edición

    const tipoSeleccionado = this.nuevoCliente.nIdTipoCliente;

    this.nuevoCliente = {
      sNombre: '',
      sApellido: null,
      nEdad: null,
      dFechaNacimiento: null,
      sEmail: '',
      sContrasena: '',
      nIdTipoCliente: tipoSeleccionado,
      bEstado: true
    };
  }

  // =========================
  // FECHA NACIMIENTO
  // =========================
  onFechaNacimientoChange(): void {
    if (!this.nuevoCliente.dFechaNacimiento) {
      this.nuevoCliente.nEdad = null;
      return;
    }

    const nacimiento = new Date(this.nuevoCliente.dFechaNacimiento);
    const hoy = new Date();

    let edad = hoy.getFullYear() - nacimiento.getFullYear();
    const m = hoy.getMonth() - nacimiento.getMonth();
    if (m < 0 || (m === 0 && hoy.getDate() < nacimiento.getDate())) edad--;

    if (edad < 18) {
      this.mostrarError('El cliente debe ser mayor de edad');
      this.nuevoCliente.dFechaNacimiento = null;
      this.nuevoCliente.nEdad = null;
      return;
    }

    this.nuevoCliente.nEdad = edad;
  }

  // =========================
  // SELECCIONAR PARA EDITAR
  // =========================
  editarClienteUI(c: any): void {
    this.modoEdicion = true;

    if (c.nIdTipoCliente === 1) {
      const empresa = this.listaClienteEmpresa.find(e => e.nIdCliente === c.nIdCliente);
      if (empresa) this.cargarFormularioEmpresa(empresa);
    }

    if (c.nIdTipoCliente === 2) {
      const persona = this.listaClientePersonaNatural.find(p => p.nIdCliente === c.nIdCliente);
      if (persona) this.cargarFormularioPersona(persona);
    }
  }

  cargarFormularioEmpresa(empresa: ClienteEmpresa): void {
    this.nuevoCliente = {
      nIdCliente: empresa.nIdCliente,
      sNombre: empresa.sNombreClienteEmpresa,
      sApellido: null,
      nEdad: null,
      dFechaNacimiento: null,
      sEmail: empresa.sEmail,
      sContrasena: '',
      nIdTipoCliente: 1,
      bEstado: empresa.bEstado
    };
  }

  cargarFormularioPersona(persona: ClientePersonaNatural): void {
    this.nuevoCliente = {
      nIdCliente: persona.nIdCliente,
      sNombre: persona.sNombreClienteNatural,
      sApellido: null,
      nEdad: persona.nEdad,
      dFechaNacimiento: persona.dFechaNacimiento,
      sEmail: persona.sEmail,
      sContrasena: '',
      nIdTipoCliente: 2,
      bEstado: persona.bEstado
    };
  }

  // =========================
  // GUARDAR (REGISTRO O UPDATE)
  // =========================
  guardarCliente(): void {
    if (this.modoEdicion) {
      this.actualizarCliente();
    } else {
      this.registrarCliente();
    }
  }

  // =========================
  // REGISTRAR
  // =========================
  registrarCliente(): void {
    if (!this.validarCliente()) return;

    this.clienteService.postInsertarCliente(this.nuevoCliente).subscribe(res => {
      if (res?.nRetorno === 0) {
        this.mostrarError(res.sRetorno);
        return;
      }

      this.mostrarMensajeExito('Cliente registrado correctamente');
      this.resetFormulario();
      this.cargarListasClientes();
    });
  }

  // =========================
  // ACTUALIZAR
  // =========================
  actualizarCliente(): void {
    if (!this.nuevoCliente.nIdCliente) {
      this.mostrarError('No se pudo identificar el cliente');
      return;
    }

    if (!this.validarCliente()) return;

    this.clienteService.postActualizarCliente(this.nuevoCliente).subscribe(res => {
      if (res?.nRetorno === 0) {
        this.mostrarError(res.sRetorno);
        return;
      }

      this.mostrarMensajeExito('Cliente actualizado correctamente');
      this.resetFormulario();
      this.cargarListasClientes();
    });
  }

  // =========================
  // VALIDACIÓN ÚNICA
  // =========================
  validarCliente(): boolean {
    if (
      !this.nuevoCliente.sNombre ||
      !this.nuevoCliente.sEmail ||
      !this.nuevoCliente.nIdTipoCliente ||
      !this.nuevoCliente.sContrasena ||
      this.nuevoCliente.sContrasena.trim() === ''
    ) {
      this.mostrarError('Todos los campos obligatorios deben estar completos');
      return false;
    }

    if (this.nuevoCliente.nIdTipoCliente === 1) {
      // Empresa
      this.nuevoCliente.sApellido = null;
      this.nuevoCliente.nEdad = null;
      this.nuevoCliente.dFechaNacimiento = null;
    }

    if (this.nuevoCliente.nIdTipoCliente === 2) {
      // Persona Natural
      if (!this.nuevoCliente.dFechaNacimiento) {
        this.mostrarError('Debe ingresar la fecha de nacimiento');
        return false;
      }

      const nacimiento = new Date(this.nuevoCliente.dFechaNacimiento);
      const hoy = new Date();

      if (nacimiento >= hoy) {
        this.mostrarError('La fecha de nacimiento no puede ser futura');
        return false;
      }

      let edad = hoy.getFullYear() - nacimiento.getFullYear();
      const m = hoy.getMonth() - nacimiento.getMonth();
      if (m < 0 || (m === 0 && hoy.getDate() < nacimiento.getDate())) edad--;

      if (edad < 18) {
        this.mostrarError('El cliente debe ser mayor de edad');
        return false;
      }

      this.nuevoCliente.nEdad = edad;
    }

    return true;
  }

  // =========================
  // RESET
  // =========================
  resetFormulario(): void {
    this.nuevoCliente = {
      sNombre: '',
      sApellido: null,
      nEdad: null,
      dFechaNacimiento: null,
      sEmail: '',
      sContrasena: '',
      nIdTipoCliente: 0,
      bEstado: true
    };

    this.modoEdicion = false;
  }

  cancelarEdicion(): void {
    this.resetFormulario();
  }

  // =========================
  // MENSAJES
  // =========================
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

  // =========================
  // DESTROY
  // =========================
  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }
}