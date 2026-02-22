export interface RolUsuario { //PARA LISTAR DATOS
    nIdRolUsuario: number;
    sNombreRolUsuario: string;
}

export interface Administrador { //PARA LISTAR DATOS
    nIdAdministrador: number;
    sNombreAdmin: string;
    sEmail: string;
    sContrasena: string;
    bEstado: Boolean
}