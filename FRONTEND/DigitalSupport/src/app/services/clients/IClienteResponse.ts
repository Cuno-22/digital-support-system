import { Cliente } from './ICliente';
import { UsuarioCliente } from './ICliente';
import { UsuarioClienteDATA } from './ICliente';
import { TipoCliente } from './ICliente';
import { ClienteUC } from './ICliente';
import { ClienteEmpresa, ClientePersonaNatural } from './ICliente';


export interface Response<T> {
  data: T[];
}

export interface IRetornoDigitalSupportV2 {
  nRetorno: number;
  sRetorno: string;
}

export interface IUsuarioClienteDataResponse {
  success: boolean;
  response?: Response<UsuarioClienteDATA>;
  errors?: { code: number; message: string }[];
}

export interface IUsuarioClienteResponse {
  success: boolean;
  response?: Response<UsuarioCliente>;
  errors?: { code: number; message: string }[];
}

export interface ITipoClienteResponse {
  success: boolean;
  response?: Response<TipoCliente>;
  errors?: { code: number; message: string }[];
}

export interface IClienteUCResponse{
  success: boolean;
  response?: Response<ClienteUC>;
  errors?: { code: number; message: string }[];
}

export interface IClienteResponse {
  success: boolean;
  response?: {
    data: IRetornoDigitalSupportCliente[];
  };
  errors?: { code: number; message: string }[];
}

export interface IRetornoDigitalSupportCliente {
  sRetorno: string;
  data: (ClienteEmpresa | ClientePersonaNatural)[];
}

export interface IRetornoDigitalSupportClienteUC {
  sRetorno: string;
  data: ClienteUC[];
}

export interface IClienteUCResponseV2 {
  success: boolean;
  response?: {
    data: IRetornoDigitalSupportClienteUC[];
  };
  errors?: { code: number; message: string }[];
}