import { RolUsuario } from './IAdmin';
import { Administrador } from './IAdmin';

export interface Response<T> {
  data: T[];
}

export interface IRolUsuarioResponse {
  success: boolean;
  response?: Response<RolUsuario>;
  errors?: { code: number; message: string }[];
}

export interface IAdministradorResponse {
  success: boolean;
  response?: Response<Administrador>;
  errors?: { code: number; message: string }[];
}