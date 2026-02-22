import { SolicitudColaborador } from './IEstadistica';
import { TopColaborador } from './IEstadistica';
import { AtencionColaborador } from './IEstadistica';
import { RegistroSolicitudMes } from './IEstadistica';
import { SolicitudUsuarioCliente } from './IEstadistica';
import { SolicitudHistorialUC } from './IEstadistica';

export interface Response<T> {
  data: T[];
}

export interface IRetornoDigitalSupport {
  sRetorno: string;
  data: [RegistroSolicitudMes];
}

export interface IRetornoDigitalSupportV2 {
  nRetorno: number;
  sRetorno: string;
}

export interface IRegistroSolicitudMesResponse {
  success: boolean;
  response?: Response<IRetornoDigitalSupport>;
  errors?: { code: number; message: string }[];
}

export interface ISolicitudColaboradorResponse {
  success: boolean;
  response?: Response<SolicitudColaborador>;
  errors?: { code: number; message: string }[];
}

export interface ITopColaboradorResponse {
  success: boolean;
  response?: Response<TopColaborador>;
  errors?: { code: number; message: string }[];
}

export interface IAtencionColaboradorResponse {
  success: boolean;
  response?: Response<AtencionColaborador>;
  errors?: { code: number; message: string }[];
}

export interface ISolicitudUsuarioClienteResponse {
  success: boolean;
  response?: Response<SolicitudUsuarioCliente>;
  errors?: { code: number; message: string }[];
}

export interface ISolicitudHistorialUCResponse {
  success: boolean;
  response?: Response<SolicitudHistorialUC>;
  errors?: { code: number; message: string }[];
}