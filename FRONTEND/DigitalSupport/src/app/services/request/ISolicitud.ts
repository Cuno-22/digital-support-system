export interface Solicitud {
  nIdSolicitud: number;
  nIdUsuarioCliente?: number; 
  sNombreUsuarioCliente?: string;
  sNombreAplicativo: string;
  sTipoSolicitud?: string;
  sMotivo: string;
  dFechaCreacion: Date;
  dFechaFinalizacion?: Date; // solo aplica para finalizadas
  sEstado: string;
  bEstado: boolean;
}

export interface SolicitudDATA {
  nIdSolicitud?: number;
  nIdUsuarioCliente: number;
  nIdAplicativo: number;
  nIdTipoSolicitud: number;
  sMotivo: string;
  dFechaCreacion: Date | null;
  dFechaFinalizacion?: Date | null;
  sEstado?: string;
  bEstado: boolean;
}

export interface TipoSolicitud {
  nIdTipoSolicitud: number;
  sTipoSolicitud: string;
  bEstado: boolean;
}

export interface SolicitudUC {
  nIdSolicitud: number;
  sTipoSolicitud: string;
  sNombreAplicativo: string;
  sMotivo: string;
  dFechaCreacion: Date;
  dFechaFinalizacion?: Date;
  sEstado: string;
  bEstado: boolean;
}