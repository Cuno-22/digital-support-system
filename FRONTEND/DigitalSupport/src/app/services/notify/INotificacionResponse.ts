import { Notificacion } from './INotificacion';
import { MensajeUC } from './INotificacion';

export interface Response<T> {
  data: T[];
}

export interface INotificacionResponse {
  success: boolean;
  response?: Response<Notificacion>;
  errors?: { code: number; message: string }[];
}

export interface IMensajeUCResponse {
  success: boolean;
  response?: Response<MensajeUC>;
  errors?: { code: number; message: string }[];
}

export interface IRetornoDigitalSupportV2 {
  nRetorno: number;
  sRetorno: string;
}