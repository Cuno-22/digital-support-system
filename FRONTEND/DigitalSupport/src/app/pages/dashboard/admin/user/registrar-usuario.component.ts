import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
//import { NavComponent } from '../../../../shared/nav/nav.component';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { ClienteService } from '../../../../services/clients/cliente.service';
import { ColaboradorService } from './../../../../services/collab/colaborador.service';
import { AdminService } from '../../../../services/adm/admin.service';
import { Cliente, TipoCliente, UsuarioCliente, UsuarioClienteDATA, ClienteEmpresa, ClientePersonaNatural } from '../../../../services/clients/ICliente';
import { Colaborador, ColaboradorDATA, RolColaborador } from '../../../../services/collab/IColaborador';
import { Administrador, RolUsuario } from '../../../../services/adm/IAdmin';

@Component({
  selector: 'registrar-usuario',
  standalone: true,
  imports: [CommonModule, MatIconModule, FormsModule],
  templateUrl: './registrar-usuario.component.html',
  styleUrls: ['./registrar-usuario.component.css']
})
export class RegistrarUsuarioComponent implements OnInit, OnDestroy {
  tiposCliente: TipoCliente[] = [];
  listaClientes: (ClienteEmpresa | ClientePersonaNatural)[] = [];
  listaAdmins: Administrador[] = [];
  listaColaboradores: Colaborador[] = [];
  listaUsuariosCliente: UsuarioCliente[] = [];
  rolesColaboradores: RolColaborador[] = [];
  listaRoles: { nIdRolUsuario: number, sNombreRolUsuario: string }[] = [];

  mensajeError: string = '';
  mensajeExito: string = '';
  mostrarMensaje: boolean = false;

  modoEdicion: boolean = false;
  usuarioEditando: any = null;

  usuariosFiltrados: any[] = [];
  filtroActual: 'administrador' | 'cliente' | 'colaborador' | null = null;

  nuevoUsuario = {
    nombre: '',
    apellido: '',
    email: '',
    contrasena: '',
    rol: '',
    tipoCliente: 0,
    idClienteSeleccionado: 0,
    idRolColaboradorSeleccionado: 0
  };

  private subscriptions: Subscription[] = [];

  constructor(private clienteService: ClienteService, private colaboradorService: ColaboradorService, private adminService: AdminService, private router: Router) {}

  ngOnInit(): void {
    this.clienteService.getListaTipoCliente().subscribe((res) => {
      if (res.success && res.response?.data) {
        this.tiposCliente = res.response.data;
      }
    });

    this.colaboradorService.getListaRolColaborador().subscribe((res) => {
      if (res.success && res.response?.data) {
        this.rolesColaboradores = res.response.data;
      }
    });

    this.clienteService.getListaUsuarioCliente().subscribe((res) => {
      if (res.success && res.response?.data) {
        this.listaUsuariosCliente = res.response.data;
      }
    });

    this.colaboradorService.getListaColaboradoresActivos().subscribe((res) => {
      if (res.success && res.response?.data) {
        this.listaColaboradores = res.response.data;
      }
    });

    this.adminService.getListaAdministrador().subscribe((res) => {
      if (res.success && res.response?.data) {
        this.listaAdmins = res.response.data;
      }
    });

    this.adminService.getListaRolUsuario().subscribe((res) => {
      if (res.success && res.response?.data) {
        this.listaRoles = res.response.data;
      }
    });

    this.filtrarUsuarios('administrador');
  }

  private separarNombreCompleto(nombreCompleto: string): { nombre: string; apellido: string } {
    if (!nombreCompleto) {
      return { nombre: '', apellido: '' };
    }

    const partes = nombreCompleto.trim().split(' ');

    if (partes.length === 1) {
      return {
        nombre: partes[0],
        apellido: ''
      };
    }

    return {
      nombre: partes[0],
      apellido: partes.slice(1).join(' ')
    };
  }

  onTipoClienteChange(): void {
    if (this.nuevoUsuario.tipoCliente) {
      this.clienteService.getListaCliente(this.nuevoUsuario.tipoCliente).subscribe((res) => {
        if (res.success && res.response?.data?.[0]?.data) {
          this.listaClientes = res.response.data[0].data;
        } else {
          this.listaClientes = [];
        }
      });
    }
  }

  crearUsuario(): void {
    const { nombre, apellido, email, contrasena, rol } = this.nuevoUsuario;

    if (!rol || !nombre || !apellido) {
      this.mostrarError('Todos los campos comunes son obligatorios.');
      return;
    }

    /* =========================
      CLIENTE
    ========================= */
    if (rol === 'cliente') {

      if (!email || !this.nuevoUsuario.idClienteSeleccionado) {
        this.mostrarError('Completa todos los campos del cliente.');
        return;
      }

      const data: UsuarioClienteDATA = {
        nIdUsuarioCliente: this.modoEdicion ? this.usuarioEditando?.nIdUsuarioCliente : undefined,
        sNombre: nombre,
        sApellido: apellido,
        sEmail: email,
        sContrasena: contrasena ?? '',
        nIdCliente: this.nuevoUsuario.idClienteSeleccionado,
        nIdRolUsuario: 2,
        bEstado: true
      };

      const request$ = this.modoEdicion
        ? this.clienteService.postActualizarUsuarioCliente(data)
        : this.clienteService.postInsertarUsuarioCliente(data);

      request$.subscribe(res => {
        if (res?.nRetorno === 0) {
          this.mostrarError(res.sRetorno);
          return;
        }

        this.mostrarMensajeExito(
          this.modoEdicion ? 'Usuario cliente actualizado correctamente' : 'Usuario cliente registrado correctamente'
        );
        this.resetFormulario();
        this.modoEdicion = false;
        this.usuarioEditando = null;
        this.filtrarUsuarios('cliente', true);
      });
    }

    /* =========================
      COLABORADOR / ADMIN
    ========================= */
    if (rol === 'colaborador' || rol === 'administrador') {

      const idRolColaborador = rol === 'administrador' ? 6 : this.nuevoUsuario.idRolColaboradorSeleccionado;

      if (!idRolColaborador) {
        this.mostrarError('Selecciona un rol de colaborador.');
        return;
      }

      const data: ColaboradorDATA = {
        nIdColaborador: this.modoEdicion ? this.usuarioEditando?.nIdColaborador : undefined,
        sNombre: nombre,
        sApellido: apellido,
        sEmail: email,
        sContrasena: contrasena ?? '',
        nIdRolColaborador: idRolColaborador,
        nIdRolUsuario: rol === 'administrador' ? 1 : 3,
        bEstado: true
      };

      const request$ = this.modoEdicion
        ? this.colaboradorService.postActualizarColaborador(data)
        : this.colaboradorService.postInsertarColaborador(data);

      request$.subscribe(res => {
        if (res?.nRetorno === 0) {
          this.mostrarError(res.sRetorno);
          return;
        }

        this.mostrarMensajeExito(
          this.modoEdicion
            ? 'Usuario actualizado correctamente'
            : rol === 'administrador'
              ? 'Administrador registrado correctamente'
              : 'Colaborador registrado correctamente'
        );

        this.resetFormulario();
        this.modoEdicion = false;
        this.usuarioEditando = null;
        this.filtrarUsuarios(rol === 'administrador' ? 'administrador' : 'colaborador', true);
      });
    }
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
    this.nuevoUsuario = {
      nombre: '',
      apellido: '',
      email: '',
      contrasena: '',
      rol: '',
      tipoCliente: 0,
      idClienteSeleccionado: 0,
      idRolColaboradorSeleccionado: 0
    };
    this.listaClientes = [];
  }

  getNombreCliente(cliente: any): string {
    return cliente.sNombreClienteEmpresa ?? cliente.sNombreClienteNatural ?? 'No registrado';
  }

  onRolUsuarioChange(): void {
    if (this.modoEdicion) return;
    this.nuevoUsuario = {
      nombre: '',
      apellido: '',
      email: '',
      contrasena: '',
      rol: this.nuevoUsuario.rol,
      tipoCliente: 0,
      idClienteSeleccionado: 0,
      idRolColaboradorSeleccionado: 0
    };
    this.listaClientes = [];
    this.mensajeError = '';
  }

  filtrarUsuarios(tipo: 'administrador' | 'cliente' | 'colaborador', forzarRecarga: boolean = false) {
    // Evitar recargar si es el mismo filtro
    if (this.filtroActual === tipo && !forzarRecarga) {
      return;
    }
    this.filtroActual = tipo;
    this.usuariosFiltrados = [];

    if (tipo === 'administrador') {
      this.adminService.getListaAdministrador().subscribe(res => {
        if (res.success && res.response?.data) {
          const admins = res.response.data.map(a => ({
            nIdColaborador: a.nIdAdministrador,
            nombreCompleto: a.sNombreAdmin,
            usuario: a.sEmail,
            rol: 'administrador',
            nIdRolColaborador: 6,
            estado: a.bEstado
          }));
          this.usuariosFiltrados.push(...admins);
        }
      });
    }

    if (tipo === 'colaborador') {
      this.colaboradorService.getListaColaboradoresActivos().subscribe(res => {
        if (res.success && res.response?.data) {
          const colaboradores = res.response.data.map(c => ({
            nIdColaborador: c.nIdColaborador,
            nombreCompleto: c.sNombreColaborador,
            usuario: c.sEmail,
            rol: 'colaborador',
            nIdRolColaborador: 6,
            estado: c.bEstado
          }));
          this.usuariosFiltrados.push(...colaboradores);
        }
      });
    }

    if (tipo === 'cliente') {
      this.clienteService.getListaUsuarioCliente().subscribe(res => {
        if (res.success && res.response?.data) {
          const clientes = res.response.data.map(c => ({
            nIdUsuarioCliente: c.nIdUsuarioCliente,
            nombreCompleto: c.sNombreUsuarioCliente,
            usuario: c.sEmail,
            rol: 'cliente',
            estado: c.bEstado   
        }));
        this.usuariosFiltrados.push(...clientes);
      }
      });
    }
  }

  editarUsuario(usuario: any, tipo: 'administrador' | 'colaborador' | 'cliente'): void {

    this.modoEdicion = true;
    this.usuarioEditando = usuario;

    const partes = usuario.nombreCompleto.trim().split(' ');

    this.nuevoUsuario = {
      nombre: partes[0] ?? '',
      apellido: partes.slice(1).join(' ') ?? '',
      email: usuario.usuario,
      contrasena: '',
      rol: tipo,
      tipoCliente: 0,
      idClienteSeleccionado: 0,
      idRolColaboradorSeleccionado: usuario.nIdRolColaborador || 0
    };
  }


  actualizarUsuario(): void {

    this.nuevoUsuario.contrasena = this.nuevoUsuario.contrasena.trim();

    const { nombre, apellido, email, contrasena, rol } = this.nuevoUsuario;

    // ================= VALIDACIONES SOLO PARA ACTUALIZAR =================

    if (!nombre || !apellido) {
      this.mostrarError('Nombre y apellido son obligatorios.');
      return;
    }

    if (rol === 'cliente') {
      if (!email || !email.includes('@')) {
        this.mostrarError('Email inválido.');
        return;
      }
    }

    if (!contrasena || contrasena.trim().length === 0) {
      this.mostrarError('Debes ingresar una contraseña para actualizar el usuario.');
      return;
    }

    /* =========================
        CLIENTE
    ========================= */
    if (rol === 'cliente') {

      if (!this.nuevoUsuario.idClienteSeleccionado) {
        this.mostrarError('Selecciona un cliente.');
        return;
      }

      const data: UsuarioClienteDATA = {
        nIdUsuarioCliente: this.usuarioEditando.nIdUsuarioCliente,
        sNombre: nombre,
        sApellido: apellido,
        sEmail: email,
        sContrasena: contrasena.trim(),
        nIdCliente: this.nuevoUsuario.idClienteSeleccionado,
        nIdRolUsuario: 2,
        bEstado: true
      };

      this.clienteService.postActualizarUsuarioCliente(data).subscribe(res => {
        if (res?.nRetorno === 0) return this.mostrarError(res.sRetorno);

        this.mostrarMensajeExito('Usuario cliente actualizado correctamente');
        this.cancelarEdicionUsuario();
        this.filtrarUsuarios('cliente', true);
      });
    }

    /* =========================
        COLAB / ADMIN
    ========================= */
    if (rol === 'colaborador' || rol === 'administrador') {

      const idRolColaborador = rol === 'administrador'
        ? 6
        : this.nuevoUsuario.idRolColaboradorSeleccionado;

      if (!idRolColaborador) {
        this.mostrarError('Selecciona un rol.');
        return;
      }

      const data: ColaboradorDATA = {
        nIdColaborador: this.usuarioEditando.nIdColaborador,
        sNombre: nombre,
        sApellido: apellido,
        sEmail: email,
        sContrasena: contrasena.trim(),
        nIdRolColaborador: idRolColaborador,
        nIdRolUsuario: rol === 'administrador' ? 1 : 3,
        bEstado: true
      };

      this.colaboradorService.postActualizarColaborador(data).subscribe(res => {
        if (res?.nRetorno === 0) return this.mostrarError(res.sRetorno);

        this.mostrarMensajeExito('Usuario actualizado correctamente');
        this.cancelarEdicionUsuario();
        this.filtrarUsuarios(rol === 'administrador' ? 'administrador' : 'colaborador', true);
      });
    }
  }
  
  cancelarEdicionUsuario(): void {
    this.modoEdicion = false;
    this.usuarioEditando = null;
    this.resetFormulario();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}