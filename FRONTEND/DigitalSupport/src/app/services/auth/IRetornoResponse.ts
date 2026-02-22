export interface IRetorno_DigitalSupportV3 {
  nIdUsuario: number;
  sRetorno: string;
  sNombreUsuario: string
  sPerfil: string
}

export interface Response<T> {
  data: T[];
}

export interface IRetornoResponse {
  success: boolean;
  response?: Response<IRetorno_DigitalSupportV3>;
  errors?: { code: number; message: string }[];
}