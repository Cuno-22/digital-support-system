using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDigitalSupport.Domain

{
    public class IClienteDATA //ACTUALIZAR
    {
        public int nIdCliente { get; set; }
        public string sNombre { get; set; }
        public string? sApellido { get; set; } // NULL si es Cliente Empresa
        public int? nEdad { get; set; } // NULL si es Cliente Empresa
        public string? dFechaNacimiento { get; set; } // NULL si es Cliente Empresa
        public string sEmail { get; set; }
        public string sContrasena { get; set; }
        public int nIdTipoCliente { get; set; } // 1 o 2
        public Boolean bEstado { get; set; }
    }

    public class ICliente //INSERTAR
    {
        public string sNombre { get; set; }
        public string? sApellido { get; set; }          // Solo si es Persona Natural
        public int? nEdad { get; set; }                 // Solo si es Persona Natural
        public string? dFechaNacimiento { get; set; }   // Solo si es Persona Natural
        public string sEmail { get; set; }
        public string sContrasena { get; set; }
        public int nIdTipoCliente { get; set; }         // 1 = Empresa, 2 = Persona Natural
        public Boolean bEstado { get; set; }
    }

    public class IClienteEmpresa //LISTAR
    { 
        public int nIdCliente { get; set; } 
        public string sNombreClienteEmpresa { get; set; } 
        public string sEmail { get; set; } 
        public string sContrasena { get; set; } 
        public Boolean bEstado { get; set; } 
    }

    public class IClientePersonaNatural //LISTAR
    {
        public int nIdCliente { get; set; }
        public string sNombreClienteNatural { get; set; }
        public int nEdad { get; set; }
        public string dFechaNacimiento { get; set; } // Fecha como string
        public string sEmail { get; set; }
        public string sContrasena { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class ITipoCliente //LISTAR
    { 
        public int nIdTipoCliente { get; set; } 
        public string sTipoCliente { get; set; } 
        public Boolean bEstado { get; set; } 
    }

    public class IUsuarioClienteDATA //ACTUALIZAR
    {
        public int nIdUsuarioCliente { get; set; }
        public string sNombre { get; set; }
        public string sApellido { get; set; }
        public string sEmail { get; set; }
        public string sContrasena { get; set; }
        public int nIdCliente { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IUsuarioCliente //INSERTAR
    {
        public string sNombre { get; set; }
        public string sApellido { get; set; }
        public string sEmail { get; set; }
        public string sContrasena { get; set; }
        public int nIdCliente { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IUsuarioClienteDTO //LISTAR
    { 
        public int nIdUsuarioCliente { get; set; } 
        public string sNombreUsuarioCliente { get; set; } 
        public string sEmail { get; set; } 
        public string sContrasena { get; set; } 
        public Boolean bEstado { get; set; } 
    }

    public class IAplicativoDATA //ACTUALIZAR
    {
        public int nIdAplicativo { get; set; }
        public string sNombreAplicativo { get; set; }
        public string sDescripcion { get; set; }
        public string dFechaLanzamiento { get; set; }
        public string dFechaModificacion { get; set; }
        public string sVersion { get; set; }
        public int nIdCliente { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IAplicativo //INSERTAR
    {
        public string sNombreAplicativo { get; set; }
        public string sDescripcion { get; set; }
        public string dFechaLanzamiento { get; set; }
        public string sVersion { get; set; }
        public int nIdCliente { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IAplicativoDTO //LISTAR
    { 
        public int nIdAplicativo { get; set; } 
        public string sNombreAplicativo { get; set; } 
        public string sInformacionApp { get; set; } 
        public string dFechaLanzamiento { get; set; } 
        public string dFechaModificacion { get; set; }
        public string sNombreCliente { get; set; }
        public string sVersion { get; set; } 
        public Boolean bEstado { get; set; } 
    }

    public class ISolicitudDATA //ACTUALIZAR
    {
        public int nIdSolicitud { get; set; }
        public int nIdUsuarioCliente { get; set; }
        public int nIdAplicativo { get; set; }
        public int nIdTipoSolicitud { get; set; }
        public string sMotivo { get; set; }
        public string dFechaCreacion { get; set; }
        public string? dFechaFinalizacion { get; set; }
        public string sEstado { get; set; } // Pendiente, En Proceso, Finalizado
        public Boolean bEstado { get; set; }
    }

    public class ISolicitud //INSERTAR
    {
        public int nIdUsuarioCliente { get; set; }
        public int nIdAplicativo { get; set; }
        public int nIdTipoSolicitud { get; set; }
        public string sMotivo { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class ISolicitudDTO //LISTAR
    {
        public int nIdSolicitud { get; set; } 
        public string sMotivo { get; set; } 
        public string dFechaCreacion { get; set; } 
        public string dFechaFinalizacion { get; set; } 
        public string sEstado { get; set; } 
        public Boolean bEstado { get; set; } 
    }

    public class ITipoSolicitud //LISTAR
    { 
        public int nIdTipoSolicitud { get; set; } 
        public string sTipoSolicitud { get; set; } 
        public Boolean bEstado { get; set; } 
    }

    public class INotificacionDATA //ACTUALIZAR
    {
        public int nIdNotificacion { get; set; }
        public int nIdSolicitud { get; set; }
        public string sDescripcion { get; set; }
        public string dFechaEnvio { get; set; }
        public Boolean bLeido { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class INotificacion //INSERTAR
    {
        public int nIdSolicitud { get; set; }
        public string sDescripcion { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class INotificacionDTO //LISTAR
    {
        public int nIdNotificacion { get; set; }
        public string sMotivoSolicitud { get; set; }
        public string sEstadoSolicitud { get; set; }
        public string sInformacionNotificada { get; set; }
        public string dFechaEnvio { get; set; }
        public Boolean bLeido { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class INotificacionDLT //ELIMINAR
    {
        public int nIdNotificacion { get; set; }
    }

        public class IColaboradorDATA //ACTUALIZAR
    {
        public int nIdColaborador { get; set; }
        public string sNombre { get; set; }
        public string sApellido { get; set; }
        public string sEmail { get; set; }
        public string sContrasena { get; set; }
        public int nIdRolColaborador { get; set; }
        public int nIdRolUsuario { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IColaborador //INSERTAR
    {
        public string sNombre { get; set; }
        public string sApellido { get; set; }
        public string sContrasena { get; set; }
        public int nIdRolColaborador { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IRolColaboradorDATA //ACTUALIZAR
    {
        public int nIdRolColaborador { get; set; }
        public string sDescripcion { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IColaboradorDTO //LISTAR
    { 
        public int nIdColaborador { get; set; } 
        public string sNombreColaborador { get; set; } 
        public string sEmail { get; set; } 
        public string sContrasena { get; set; } 
        public Boolean bEstado { get; set; } 
    }

    public class IRolColaborador //INSERTAR
    {
        public string sDescripcion { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IRolColaboradorDTO //LISTAR
    { 
        public int nIdRolColaborador { get; set; } 
        public string sFuncionColaborador { get; set; }
        public Boolean bEstado { get; set; } 
    }

    public class IAsignacionSolicitudDATA //ACTUALIZAR
    {
        public int nIdAsignacionSolicitud { get; set; }
        public int nIdSolicitud { get; set; }
        public int nIdColaborador { get; set; }
        public Boolean bEsCoordinador { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IAsignacionSolicitud //INSERTAR
    {
        public int nIdSolicitud { get; set; }
        public int nIdColaborador { get; set; }
        public Boolean bEsCoordinador { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IAsignacionSolicitudDTO //LISTAR
    {
        public int nIdAsignacionSolicitud { get; set; }
    }

    public class IAsignacionSolicitudDLT //ELIMINAR
    {
        public int nIdAsignacionSolicitud { get; set; }
    }

    public class IRegistroTrabajoDATA //ACTUALIZAR
    {
        public int nIdRegistroTrabajo { get; set; }
        public int nIdSolicitud { get; set; }
        public int nIdColaborador { get; set; }
        public string sDescripcion { get; set; }
        public string dFechaRegistro { get; set; }
        public decimal nHorasTrabajadas { get; set; }
        public string sObservacion { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IRegistroTrabajo //INSERTAR
    {
        public int nIdSolicitud { get; set; }
        public int nIdColaborador { get; set; }
        public string sDescripcion { get; set; }
        public decimal nHorasTrabajadas { get; set; }
        public string sObservacion { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IRegistroTrabajoDTO //LISTAR
    {
        public int nIdRegistroTrabajo { get; set; }
        public string sDetalleTrabajo { get; set; }
        public string sNombreColaborador { get; set; }
        public string sMotivo { get; set; }
        public string dFechaRegistro { get; set; }
        public int nHorasTrabajadas { get; set; }
        public string sObservacion { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IRegistroTrabajoDLT //ELIMINAR
    {
        public int nIdRegistroTrabajo { get; set; }
    }

    public class ISolicitudColaborador //LISTAR
    {
        public int nIdColaborador { get; set; }
        public string sNombreColaborador { get; set; }
        public string sFuncionColaborador { get; set; }
        public string sEstadoColaborador { get; set; }
        public int nSolicitudesResueltas { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class ITopColaborador //LISTAR
    {
        public int nIdColaborador { get; set; }
        public string sNombreColaborador { get; set; }
        public string sFuncionColaborador { get; set; }
        public int nSolicitudesResueltas { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class ISolicitudUsuarioCliente //LISTAR
    {
        public int nIdUsuarioCliente { get; set; }
        public string sNombreUsuarioCliente { get; set; }
        public int nSolicitudesEstablecidas { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IHorasTrabajadasColaborador //LISTAR
    {
        public int nIdColaborador { get; set; }
        public string sNombreColaborador { get; set; }
        public string sFuncionColaborador { get; set; }
        public int nPromedioHorasResolucion { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IHistorialSolicitudUC //LISTAR
    {
        public int nIdSolicitud { get; set; }
        public string sNombreAplicativo { get; set; }
        public string sTipoSolicitud { get; set; }
        public string sMotivo { get; set; }
        public string dFechaCreacion { get; set; }
        public string dFechaFinalizacion { get; set; }
        public string sEstado { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class ISolicitudPendiente //LISTAR
    {
        public int nIdSolicitud { get; set; }
        public int nIdUsuarioCliente { get; set; }
        public string sNombreUsuarioCliente { get; set; }
        public string sNombreAplicativo { get; set; }
        public string sTipoSolicitud { get; set; }
        public string sMotivo { get; set; }
        public string dFechaCreacion { get; set; }
        public string sEstado { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class ISolicitudFinalizada //LISTAR
    {
        public int nIdSolicitud { get; set; }
        public int nIdUsuarioCliente { get; set; }
        public string sNombreUsuarioCliente { get; set; }
        public string sNombreAplicativo { get; set; }
        public string sTipoSolicitud { get; set; }
        public string sMotivo { get; set; }
        public string dFechaCreacion { get; set; }
        public string dFechaFinalizacion { get; set; }
        public Boolean bEstado { get; set; }
    }
    public class IFechaSolicitud //LISTAR
    {
        public int nIdSolicitud { get; set; }
        public string sNombreUsuarioCliente { get; set; }
        public string sNombreAplicativo { get; set; }
        public string sTipoSolicitud { get; set; }
        public string sMotivo { get; set; }
        public string dFechaCreacion { get; set; }
        public string dFechaFinalizacion { get; set; }
        public string sEstado { get; set; }
        public bool bEstado { get; set; }
    }

    public class ISolicitudCompleta //LISTAR
    {
        public int nIdSolicitud { get; set; }
        public int nIdUsuarioCliente { get; set; }
        public string sNombreUsuarioCliente { get; set; }
        public string sNombreAplicativo { get; set; }
        public string sTipoSolicitud { get; set; }
        public string sMotivo { get; set; }
        public string dFechaCreacion { get; set; }
        public string dFechaFinalizacion { get; set; }
        public string sEstado { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IRolUsuario //LISTAR
    {
        public int nIdRolUsuario { get; set; }
        public string sNombreRolUsuario { get; set; }
    }

    public class IClienteUC //LISTAR
    {
        public int nIdCliente { get; set; }
        public string sNombreCliente { get; set; }
        public string sEmail { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class ISolicitudUC //LISTAR
    {
        public int nIdSolicitud { get; set; }
        public string sTipoSolicitud { get; set; }
        public string sNombreAplicativo { get; set; }
        public string sMotivo { get; set; }
        public string dFechaCreacion { get; set; }
        public string dFechaFinalizacion { get; set; }
        public string sEstado { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IAdministrador //LISTAR
    {
        public int nIdAdministrador { get; set; }
        public string sNombreAdmin { get; set; }
        public string sEmail { get; set; }
        public Boolean bEstado { get; set; }
    }

    public class IEstadisticaColaborador //LISTAR
    {
        public int nIdColaborador { get; set; }
        public string sNombreColaborador { get; set; }
        public int nPorcentajeAtencion { get; set; }
    }

    public class IMensajeUC //LISTAR
    {
        public string sNombreUsuarioCliente { get; set; }
        public string sEmailUsuarioCliente { get; set; }
        public string sEstadoSolicitud { get; set; }
        public string sInformacionNotificada { get; set; }
    }

    public class ILoginRequest //LOGIN
    {
        public string sEmail { get; set; }
        public string sContrasena { get; set; }
    }

    public class IRetorno_DigitalSupport //MENSAJE DEL SISTEMA AL USUARIO EN CASO DE METODOS GET
    { 
        public string sRetorno { get; set; } 
        public List<object> Data { get; set; } 
    }

    public class IRetorno_DigitalSupportV2 //MENSAJE DEL SISTEMA AL USUARIO EN CASO DE METODOS POST
    { 
        public int nRetorno { get; set; } 
        public string sRetorno { get; set; } 
    }

    public class IRetorno_DigitalSupportV3 //LOGIN
    {
        public int nIdUsuario { get; set; }
        public string sRetorno { get; set; }
        public string sNombreUsuario { get; set; }
        public string sPerfil { get; set; }
    }

    public class INotificacionEmailUC //MENSAJE EMAIL

    {
        public string sNombreUsuarioCliente { get; set; }
        public string sEmailUsuarioCliente { get; set; }
        public string sEstadoSolicitud { get; set; }
        public string sInformacionNotificada { get; set; }
    }
}
