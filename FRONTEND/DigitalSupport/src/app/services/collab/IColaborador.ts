export interface Colaborador {
  nIdColaborador: number;
  sNombreColaborador: string;
  sEmail: string;
  sContrasena: string;
  bEstado: boolean;
}

export interface ColaboradorDATA {
  nIdColaborador?: number;
  sNombre: string;
  sApellido: string;
  sEmail: string;
  sContrasena: string;
  nIdRolColaborador: number;
  nIdRolUsuario?: number;
  bEstado: boolean;
}

export interface RolColaborador {
  nIdRolColaborador: number;
  sFuncionColaborador: string;
  bEstado: boolean;
}

export interface RolColaboradorDATA {
  nIdRolColaborador?: number;
  sDescripcion: string;
  bEstado: boolean;
}