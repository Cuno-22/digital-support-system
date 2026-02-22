import { IHorasTrabajadasColaborador } from './IRegistroTrabajo';
import { AsignacionSolicitud } from './IRegistroTrabajo';
import { RegistroTrabajo } from './IRegistroTrabajo';
import { RegistroTrabajoDATA } from './IRegistroTrabajo';
import { AsignacionSolicitudEspecifica } from './IRegistroTrabajo';

export interface Response<T> {
  data: T[];
}

export interface IHorasTrabajadasResponse {
  success: boolean;
  response?: Response<IHorasTrabajadasColaborador>;
  errors?: { code: number; message: string }[];
}

export interface IRegistroTrabajoResponse {
  success: boolean;
  response?: Response<RegistroTrabajo>;
  errors?: { code: number; message: string }[];
}

export interface IAsignacionSolicitudEspecificaResponse {
  success: boolean;
  response?: Response<AsignacionSolicitudEspecifica>;
  errors?: { code: number; message: string }[];
}

export interface IRetornoDigitalSupportV2 {
  nRetorno: number;
  sRetorno: string;
}