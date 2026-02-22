using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDigitalSupport.Domain;


namespace WebApiDigitalSupport.BusinessLogic.Interface
{
    public interface IDigitalSupport
    {
        Task<IRetorno_DigitalSupport> getListaCliente(int nIdTipoCliente);
        Task<IEnumerable<ITipoCliente>> getListaTipoCliente();
        Task<IEnumerable<IUsuarioClienteDTO>> getListaUsuarioCliente();
        Task<IEnumerable<IAplicativoDTO>> getListaAplicativo(int nIdUsuarioCliente);
        Task<IRetorno_DigitalSupport> getListaSolicitud(int nIdTipoSolicitud);
        Task<IEnumerable<ITipoSolicitud>> getListaTipoSolicitud();
        Task<IEnumerable<INotificacionDTO>> getListaNotificacion();
        Task<IEnumerable<IColaboradorDTO>> getListaColaborador(int nIdRolColaborador);
        Task<IEnumerable<IRolColaboradorDTO>> getListaRolColaborador();
        Task<IEnumerable<IRegistroTrabajoDTO>> getListaRegistroTrabajo(int nIdColaborador);
        Task<IEnumerable<ISolicitudColaborador>> getSolicitudFinalizadaPorColaborador(); 
        Task<IEnumerable<ITopColaborador>> getTop5ColaboradorSolicitud(); 
        Task<IEnumerable<ITopColaborador>> getColaboradorConMas5Solicitudes(); 
        Task<IEnumerable<ISolicitudUsuarioCliente>> getSolicitudUsuarioCliente(); 
        Task<IEnumerable<IHorasTrabajadasColaborador>> getPromedioHorasTrabajadasColaborador();
        Task<IEnumerable<IHistorialSolicitudUC>> getHistorialSolicitudUsuarioCliente(int nIdUsuarioCliente); 
        Task<IEnumerable<ISolicitudPendiente>> getListaSolicitudEnProcesoPendiente(); 
        Task<IEnumerable<ISolicitudFinalizada>> getListaSolicitudFinalizada();
        Task<IRetorno_DigitalSupport> getSolicitudPorMeses(int nMesesAntes); 
        Task<IEnumerable<IFechaSolicitud>> getSolicitudFechaEspecifica(DateTime dFechaCreacion);
        Task<IEnumerable<IColaboradorDTO>> getListaColaboradoresActivos();
        Task<IEnumerable<ISolicitudCompleta>> getListaTotalSolicitudes();
        Task<IEnumerable<IRolUsuario>> getListaRolUsuario();
        Task<IRetorno_DigitalSupport> getListaClienteporUC(int nIdUsuarioCliente);
        Task<IEnumerable<ISolicitudUC>> getSolicitudEspecificoUC(int nIdUsuarioCliente);
        Task<IEnumerable<IAdministrador>> getListaAdministrador();
        Task<IRetorno_DigitalSupport> getPorcentajeAtencionColaborador(int nIdColaborador);
        Task<IEnumerable<IEstadisticaColaborador>> getListaPorcentajeColaboradorAtencion();
        Task<IEnumerable<INotificacionDTO>> getListaNotificacionesUC(int nIdUsuarioCliente);
        Task<IEnumerable<IMensajeUC>> getMensajeNotificacionUC(int nIdNotificacion);
        Task<IEnumerable<IAsignacionSolicitudDTO>> getAsignacionSolicitudEspecifica(int nIdSolicitud, int nIdColaborador);
        Task<IRetorno_DigitalSupportV2> postInsertarCliente(ICliente dato);
        Task<IRetorno_DigitalSupportV2> postInsertarUsuarioCliente(IUsuarioCliente dato);
        Task<IRetorno_DigitalSupportV2> postInsertarAplicativo(IAplicativo dato);
        Task<IRetorno_DigitalSupportV2> postInsertarSolicitud(ISolicitud dato);
        Task<IRetorno_DigitalSupportV2> postInsertarNotificacion(INotificacion dato);
        Task<IRetorno_DigitalSupportV2> postInsertarColaborador(IColaborador dato);
        Task<IRetorno_DigitalSupportV2> postInsertarRolColaborador(IRolColaborador dato);
        Task<IRetorno_DigitalSupportV2> postInsertarAsignacionSolicitud(IAsignacionSolicitud dato);
        Task<IRetorno_DigitalSupportV2> postInsertarRegistroTrabajo(IRegistroTrabajo dato);
        Task<IRetorno_DigitalSupportV2> postActualizarCliente(IClienteDATA dato);
        Task<IRetorno_DigitalSupportV2> postActualizarUsuarioCliente(IUsuarioClienteDATA dato);
        Task<IRetorno_DigitalSupportV2> postActualizarAplicativo(IAplicativoDATA dato);
        Task<IRetorno_DigitalSupportV2> postActualizarSolicitud(ISolicitudDATA dato);
        Task<IRetorno_DigitalSupportV2> postActualizarNotificacion(INotificacionDATA dato);
        Task<IRetorno_DigitalSupportV2> postActualizarColaborador(IColaboradorDATA dato);
        Task<IRetorno_DigitalSupportV2> postActualizarRolColaborador(IRolColaboradorDATA dato);
        Task<IRetorno_DigitalSupportV2> postActualizarAsignacionSolicitud(IAsignacionSolicitudDATA dato);
        Task<IRetorno_DigitalSupportV2> postActualizarRegistroTrabajo(IRegistroTrabajoDATA dato);
        Task<IRetorno_DigitalSupportV2> postEliminarNotificacion(INotificacionDLT dato);
        Task<IRetorno_DigitalSupportV2> postEliminarAsignacionSolicitud(IAsignacionSolicitudDLT dato);
        Task<IRetorno_DigitalSupportV2> postEliminarRegistroTrabajo(IRegistroTrabajoDLT dato);
        Task<IRetorno_DigitalSupportV3> postAutenticacionUsuario(ILoginRequest dato);
    }
}
