export interface SolicitudColaborador {
    nIdColaborador: number;
    sNombreColaborador: string;
    sFuncionColaborador: string;
    sEstadoColaborador: string;
    nSolicitudesResueltas: number;
    bEstado: boolean;
}

export interface TopColaborador {
    nIdColaborador: number;
    sNombreColaborador: string;
    sFuncionColaborador: string;
    nSolicitudesResueltas: number;
    bEstado: boolean;
}

export interface AtencionColaborador {
    nIdColaborador: number;
    sNombreColaborador: string;
    nPorcentajeAtencion: number;
}

export interface SolicitudUsuarioCliente {
    nIdUsuarioCliente: number;
    sNombreUsuarioCliente: string;
    nSolicitudesEstablecidas: number;
    bEstado: boolean;
}

export interface RegistroSolicitudMes {
    nIdSolicitud: number;
    sNombreUsuarioCliente: string;
    sNombreAplicativo: string;
    sTipoSolicitud: string;
    sMotivo: string;
    dFechaCreacion: Date;
    dFechaFinalizacion: Date | null;
    bEstado: boolean;
}

export interface SolicitudHistorialUC {
    nIdSolicitud: number;
    sNombreAplicativo: string;
    sTipoSolicitud: string;
    sMotivo: string,
    dFechaCreacion: Date,
    dFechaFinalizacion: Date | null,
    sEstado: string,
    bEstado: true
}