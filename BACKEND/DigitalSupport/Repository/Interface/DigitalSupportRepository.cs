using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDigitalSupport.Domain;


namespace WebApiDigitalSupport.Repository.Interface
{
    public interface IDigitalSupportRepository
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
        Task<IRetorno_DigitalSupportV2> postInsertarCliente(ICliente data);
        Task<IRetorno_DigitalSupportV2> postInsertarUsuarioCliente(IUsuarioCliente data);
        Task<IRetorno_DigitalSupportV2> postInsertarAplicativo(IAplicativo data);
        Task<IRetorno_DigitalSupportV2> postInsertarSolicitud(ISolicitud data);
        Task<IRetorno_DigitalSupportV2> postInsertarNotificacion(INotificacion data);
        Task<IRetorno_DigitalSupportV2> postInsertarColaborador(IColaborador data);
        Task<IRetorno_DigitalSupportV2> postInsertarRolColaborador(IRolColaborador data);
        Task<IRetorno_DigitalSupportV2> postInsertarAsignacionSolicitud(IAsignacionSolicitud data);
        Task<IRetorno_DigitalSupportV2> postInsertarRegistroTrabajo(IRegistroTrabajo data);
        Task<IRetorno_DigitalSupportV2> postActualizarCliente(IClienteDATA data);
        Task<IRetorno_DigitalSupportV2> postActualizarUsuarioCliente(IUsuarioClienteDATA data);
        Task<IRetorno_DigitalSupportV2> postActualizarAplicativo(IAplicativoDATA data);
        Task<IRetorno_DigitalSupportV2> postActualizarSolicitud(ISolicitudDATA data);
        Task<IRetorno_DigitalSupportV2> postActualizarNotificacion(INotificacionDATA data);
        Task<IRetorno_DigitalSupportV2> postActualizarColaborador(IColaboradorDATA data);
        Task<IRetorno_DigitalSupportV2> postActualizarRolColaborador(IRolColaboradorDATA data);
        Task<IRetorno_DigitalSupportV2> postActualizarAsignacionSolicitud(IAsignacionSolicitudDATA data);
        Task<IRetorno_DigitalSupportV2> postActualizarRegistroTrabajo(IRegistroTrabajoDATA data);
        Task<IRetorno_DigitalSupportV2> postEliminarNotificacion(INotificacionDLT data);
        Task<IRetorno_DigitalSupportV2> postEliminarAsignacionSolicitud(IAsignacionSolicitudDLT data);
        Task<IRetorno_DigitalSupportV2> postEliminarRegistroTrabajo(IRegistroTrabajoDLT data);
        Task<IRetorno_DigitalSupportV3> postAutenticacionUsuario(ILoginRequest data);
        Task<INotificacionEmailUC> ObtenerDatosNotificacionAsync(int nIdNotificacion);
    }
}
