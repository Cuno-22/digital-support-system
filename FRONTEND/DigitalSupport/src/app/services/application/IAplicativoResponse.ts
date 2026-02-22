import { Aplicativo } from './IAplicativo';
import { AplicativoDATA } from './IAplicativo';

export interface Response<T> {
  data: T[];
}

export interface IRetornoDigitalSupportV2 {
  nRetorno: number;
  sRetorno: string;
}

export interface IAplicativoResponse {
  success: boolean;
  response?: Response<Aplicativo>;
  errors?: { code: number; message: string }[];
}

export interface IAplicativoDataResponse {
  success: boolean;
  response?: Response<AplicativoDATA>;
  errors?: { code: number; message: string }[];
}