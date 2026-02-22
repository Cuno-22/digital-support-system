import { Colaborador } from './IColaborador';
import { ColaboradorDATA } from './IColaborador';
import { RolColaborador } from './IColaborador';
import { RolColaboradorDATA } from './IColaborador';

export interface Response<T> {
  data: T[];
}

export interface IColaboradorResponse {
  success: boolean;
  response?: Response<Colaborador>;
  errors?: { code: number; message: string }[];
}

export interface IRolColaboradorResponse {
  success: boolean;
  response?: Response<RolColaborador>;
  errors?: { code: number; message: string }[];
}

export interface IColaboradorDataResponse {
  success: boolean;
  response?: Response<ColaboradorDATA>;
  errors?: { code: number; message: string }[];
}

export interface IRolColaboradorDataResponse {
  success: boolean;
  response?: Response<RolColaboradorDATA>;
  errors?: { code: number; message: string }[];
}

export interface IRetornoDigitalSupportV2 {
  nRetorno: number;
  sRetorno: string;
}