export interface Aplicativo {
  nIdAplicativo: number;
  sNombreAplicativo: string;
  sInformacionApp: string;
  dFechaLanzamiento: Date;
  sVersion: string;
  nIdCliente: number;
  sNombreCliente: string;
  bEstado: boolean;
}

export interface AplicativoDATA {
  nIdAplicativo?: number;
  sNombreAplicativo: string;
  sDescripcion: string;
  dFechaLanzamiento: Date;
  dFechaModificacion?: Date;
  sVersion: string;
  nIdCliente: number;
  bEstado: boolean;
}