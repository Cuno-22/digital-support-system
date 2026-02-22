export interface Notificacion {
  nIdNotificacion: number;
  sMotivoSolicitud: string;
  sEstadoSolicitud: string;
  sInformacionNotificada: string;
  dFechaEnvio: Date ;
  bLeido: boolean;
  bEstado: boolean;
}

export interface MensajeUC {
  sNombreUsuarioCliente: string;
  sEmailUsuarioCliente: string;
  sEstadoSolicitud: string;
  sInformacionNotificada: string;
}

export interface NotificacionDATA {
  nIdNotificacion?: number;
  nIdSolicitud: number;
  sDescripcion: string;
  dFechaEnvio?: Date;
  bLeido?: boolean;
  bEstado: boolean;
}

export interface NotificacionDLT {
  nIdNotificacion: number;
}