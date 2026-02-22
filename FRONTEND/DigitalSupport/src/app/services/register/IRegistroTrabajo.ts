export interface IHorasTrabajadasColaborador {
  nPromedioHorasResolucion: number;
}

export interface AsignacionSolicitud {
  nIdAsignacionSolicitud?: number;
  nIdSolicitud: number;
  nIdColaborador: number;
  bEsCoordinador: boolean;
  bEstado: boolean;
}

export interface RegistroTrabajo{
  nIdRegistroTrabajo: number;
  sDetalleTrabajo: string;
  sNombreColaborador: string;
  sMotivo: string;
  dFechaRegistro: Date;
  nHorasTrabajadas: number;
  sObservacion: string;
  bEstado: boolean;
}

export interface RegistroTrabajoDATA{
  nIdRegistroTrabajo?: number;
  nIdSolicitud: number;
  nIdColaborador: number;
  sDescripcion: string;
  dFechaRegistro: Date | null;
  nHorasTrabajadas: number;
  sObservacion: string;
  bEstado: boolean;
}

export interface AsignacionSolicitudEspecifica {
  nIdAsignacionSolicitud: number;
}

export interface AsignacionSolicitudDLT {
  nIdAsignacionSolicitud: number;
}

export interface RegistroTrabajoDLT{
  nIdRegistroTrabajo: number;
}
