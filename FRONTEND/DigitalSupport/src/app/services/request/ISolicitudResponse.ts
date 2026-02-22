import { Solicitud } from './ISolicitud';
import { SolicitudDATA } from './ISolicitud';
import { TipoSolicitud } from './ISolicitud';
import { SolicitudUC } from './ISolicitud';

export interface Response<T> {
  data: T[];
}

export interface IRetornoDigitalSupport {
  sRetorno: string;
  data: [Solicitud];
}

export interface IRetornoDigitalSupportV2 {
  nRetorno: number;
  sRetorno: string;
}

export interface ISolicitudResponse {
  success: boolean;
  response?: Response<Solicitud>;
  errors?: { code: number; message: string }[];
}

export interface ISolicitudFiltradaResponse {
  success: boolean;
  response?: Response<IRetornoDigitalSupport>;
  errors?: { code: number; message: string }[];
}

export interface ISolicitudDataResponse {
  success: boolean;
  response?: Response<SolicitudDATA>;
  errors?: { code: number; message: string }[];
}

export interface ITipoSolicitudResponse {
  success: boolean;
  response?: Response<TipoSolicitud>;
  errors?: { code: number; message: string }[];
}

export interface ISolicitudUCResponse {
  success: boolean;
  response?: Response<SolicitudUC>;
  errors?: { code: number; message: string }[];
}