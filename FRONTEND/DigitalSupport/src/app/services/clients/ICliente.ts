export interface UsuarioClienteDATA{
  nIdUsuarioCliente?: number;
  sNombre: string;
  sApellido: string;
  sEmail: string;
  sContrasena: string;
  nIdCliente: number;
  nIdRolUsuario?: number;
  bEstado: boolean;
}

export interface UsuarioCliente{
  nIdUsuarioCliente: number;
  sNombreUsuarioCliente: string;
  sEmail: string;
  sContrasena: string;
  bEstado: boolean;
}

export interface ClienteEmpresa {
  nIdCliente: number;
  sNombreClienteEmpresa: string;
  sEmail: string;
  sContrasena: string;
  bEstado: boolean;
}

export interface ClientePersonaNatural {
  nIdCliente: number;
  sNombreClienteNatural: string;
  nEdad: number;
  dFechaNacimiento: string; // 'dd/MM/yyyy'
  sEmail: string;
  sContrasena: string;
  bEstado: boolean;
}

export interface TipoCliente {
  nIdTipoCliente: number;
  sTipoCliente: string;
  bEstado: boolean;
}

export interface ClienteUC {
  nIdCliente: number;
  sNombreCliente: string;
  sEmail: string;
  bEstado: boolean;
}

export interface Cliente {
  nIdCliente?: number;
  sNombre: string;
  sApellido: string | null;       // Cambiar a `string | null` para aceptar null
  nEdad: number | null;           // Cambiar a `number | null` para aceptar null
  dFechaNacimiento: string | null; // Cambiar a `string | null` para aceptar null
  sEmail: string;
  sContrasena: string;
  nIdTipoCliente: number;         // 1: Empresa, 2: Natural
  bEstado: boolean;
}

export type ClienteTipo = ClienteEmpresa | ClientePersonaNatural;