using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDigitalSupport.Domain;
using WebApiDigitalSupport.BusinessLogic.Interface;
using WebApiDigitalSupport.Repository.Interface;
using System.Net; // NUEVO: Para el correo
using System.Net.Mail; // NUEVO: Para el correo
using Microsoft.Extensions.Configuration; // NUEVO: Para leer el appsettings.json


namespace WebApiDigitalSupport.BusinessLogic
{
    public class DigitalSupportBL : IDigitalSupport
    {
        protected readonly IDigitalSupportRepository _repository;
        private readonly IConfiguration _configuration;
        public DigitalSupportBL(IDigitalSupportRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<IRetorno_DigitalSupport> getListaCliente(int nIdTipoCliente)
        {
            return await _repository.getListaCliente(nIdTipoCliente);
        }

        public async Task<IEnumerable<ITipoCliente>> getListaTipoCliente()
        {
            return await _repository.getListaTipoCliente();
        }

        public async Task<IEnumerable<IUsuarioClienteDTO>> getListaUsuarioCliente()
        {
            return await _repository.getListaUsuarioCliente();
        }

        public async Task<IEnumerable<IAplicativoDTO>> getListaAplicativo(int nIdUsuarioCliente)
        {
            return await _repository.getListaAplicativo(nIdUsuarioCliente);
        }

        public async Task<IRetorno_DigitalSupport> getListaSolicitud(int nIdTipoSolicitud)
        {
            return await _repository.getListaSolicitud(nIdTipoSolicitud);
        }

        public async Task<IEnumerable<ITipoSolicitud>> getListaTipoSolicitud()
        {
            return await _repository.getListaTipoSolicitud();
        }

        public async Task<IEnumerable<INotificacionDTO>> getListaNotificacion()
        {
            return await _repository.getListaNotificacion();
        }

        public async Task<IEnumerable<IColaboradorDTO>> getListaColaborador(int nIdRolColaborador)
        {
            return await _repository.getListaColaborador(nIdRolColaborador);
        }

        public async Task<IEnumerable<IRolColaboradorDTO>> getListaRolColaborador()
        {
            return await _repository.getListaRolColaborador();
        }

        public async Task<IEnumerable<IRegistroTrabajoDTO>> getListaRegistroTrabajo(int nIdColaborador)
        {
            return await _repository.getListaRegistroTrabajo(nIdColaborador);
        }

        public async Task<IEnumerable<ISolicitudColaborador>> getSolicitudFinalizadaPorColaborador()
        {
            return await _repository.getSolicitudFinalizadaPorColaborador();
        }

        public async Task<IEnumerable<ITopColaborador>> getTop5ColaboradorSolicitud()
        {
            return await _repository.getTop5ColaboradorSolicitud();
        }

        public async Task<IEnumerable<ITopColaborador>> getColaboradorConMas5Solicitudes()
        {
            return await _repository.getColaboradorConMas5Solicitudes();
        }

        public async Task<IEnumerable<ISolicitudUsuarioCliente>> getSolicitudUsuarioCliente()
        {
            return await _repository.getSolicitudUsuarioCliente();
        }

        public async Task<IEnumerable<IHorasTrabajadasColaborador>> getPromedioHorasTrabajadasColaborador()
        {
            return await _repository.getPromedioHorasTrabajadasColaborador();
        }

        public async Task<IEnumerable<IHistorialSolicitudUC>> getHistorialSolicitudUsuarioCliente(int nIdUsuarioCliente)
        {
            return await _repository.getHistorialSolicitudUsuarioCliente(nIdUsuarioCliente);
        }

        public async Task<IEnumerable<ISolicitudPendiente>> getListaSolicitudEnProcesoPendiente()
        {
            return await _repository.getListaSolicitudEnProcesoPendiente();
        }

        public async Task<IEnumerable<ISolicitudFinalizada>> getListaSolicitudFinalizada()
        {
            return await _repository.getListaSolicitudFinalizada();
        }

        public async Task<IRetorno_DigitalSupport> getSolicitudPorMeses(int nMesesAntes)
        {
            return await _repository.getSolicitudPorMeses(nMesesAntes);
        }

        public async Task<IEnumerable<IFechaSolicitud>> getSolicitudFechaEspecifica(DateTime dFechaCreacion)
        {
            return await _repository.getSolicitudFechaEspecifica(dFechaCreacion);
        }

        public async Task<IEnumerable<IColaboradorDTO>> getListaColaboradoresActivos() 
        { 
            return await _repository.getListaColaboradoresActivos(); 
        }

        public async Task<IEnumerable<ISolicitudCompleta>> getListaTotalSolicitudes() 
        { 
            return await _repository.getListaTotalSolicitudes(); 
        }

        public async Task<IEnumerable<IRolUsuario>> getListaRolUsuario() 
        { 
            return await _repository.getListaRolUsuario(); 
        }

        public async Task<IRetorno_DigitalSupport> getListaClienteporUC(int nIdUsuarioCliente) 
        {
            return await _repository.getListaClienteporUC(nIdUsuarioCliente); 
        }

        public async Task<IEnumerable<ISolicitudUC>> getSolicitudEspecificoUC(int nIdUsuarioCliente) 
        { 
            return await _repository.getSolicitudEspecificoUC(nIdUsuarioCliente); 
        }

        public async Task<IEnumerable<IAdministrador>> getListaAdministrador()
        { 
            return await _repository.getListaAdministrador(); 
        }

        public async Task<IRetorno_DigitalSupport> getPorcentajeAtencionColaborador(int nIdColaborador) 
        { 
            return await _repository.getPorcentajeAtencionColaborador(nIdColaborador); 
        }

        public async Task<IEnumerable<IEstadisticaColaborador>> getListaPorcentajeColaboradorAtencion() 
        { 
            return await _repository.getListaPorcentajeColaboradorAtencion(); 
        }

        public async Task<IEnumerable<INotificacionDTO>> getListaNotificacionesUC(int nIdUsuarioCliente)
        {
            return await _repository.getListaNotificacionesUC(nIdUsuarioCliente);
        }

        public async Task<IEnumerable<IMensajeUC>> getMensajeNotificacionUC(int nIdNotificacion)
        {
            // 1. Obtenemos la data del repositorio (tu código original)
            var listaMensajes = await _repository.getMensajeNotificacionUC(nIdNotificacion);
            var datosCorreo = listaMensajes.FirstOrDefault();

            // 2. Si la consulta trajo datos y el cliente tiene un correo válido, enviamos el email
            if (datosCorreo != null && !string.IsNullOrEmpty(datosCorreo.sEmailUsuarioCliente))
            {
                try
                {
                    // Leemos tus credenciales de Gmail desde el appsettings.json
                    var smtpConfig = _configuration.GetSection("SmtpSettings");
                    string server = smtpConfig["Server"];
                    int port = int.Parse(smtpConfig["Port"]);
                    string senderName = smtpConfig["SenderName"];
                    string senderEmail = smtpConfig["SenderEmail"];
                    string senderPassword = smtpConfig["SenderPassword"];

                    // Armamos el correo
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(senderEmail, senderName);
                    mail.To.Add(datosCorreo.sEmailUsuarioCliente);
                    mail.Subject = "Actualización de tu Solicitud - Digital Support";
                    mail.IsBodyHtml = true;

                    // Diseño HTML para el cuerpo del correo
                    mail.Body = $@"
                <div style='font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: 0 auto; border: 1px solid #ddd; border-radius: 8px; overflow: hidden;'>
                    <div style='background-color: #00695c; padding: 20px; text-align: center; color: white;'>
                        <h2 style='margin: 0;'>Digital Support</h2>
                    </div>
                    <div style='padding: 20px;'>
                        <h3>Hola, {datosCorreo.sNombreUsuarioCliente}</h3>
                        <p>Te informamos que hay una nueva actualización relacionada con tu solicitud.</p>
                        <table style='width: 100%; border-collapse: collapse; margin-top: 15px;'>
                            <tr>
                                <td style='padding: 10px; border: 1px solid #ddd; background-color: #f4f6f8; font-weight: bold; width: 30%;'>Estado:</td>
                                <td style='padding: 10px; border: 1px solid #ddd;'>{datosCorreo.sEstadoSolicitud}</td>
                            </tr>
                            <tr>
                                <td style='padding: 10px; border: 1px solid #ddd; background-color: #f4f6f8; font-weight: bold;'>Detalle:</td>
                                <td style='padding: 10px; border: 1px solid #ddd;'>{datosCorreo.sInformacionNotificada}</td>
                            </tr>
                        </table>
                        <p style='margin-top: 20px;'>Gracias por confiar en nuestros servicios.</p>
                    </div>
                </div>";

                    // Nos conectamos a Gmail y enviamos
                    using (SmtpClient smtp = new SmtpClient(server, port))
                    {
                        smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtp.EnableSsl = true; // REQUISITO OBLIGATORIO PARA GMAIL
                        await smtp.SendMailAsync(mail);
                    }
                }
                catch (Exception ex)
                {
                    // Si el correo falla (ej. error de internet), lo atrapamos aquí 
                    // para que no rompa el programa y la API siga funcionando.
                    Console.WriteLine("Error al enviar el correo: " + ex.Message);
                }
            }

            // 3. Retornamos la lista original al controlador
            return listaMensajes;
        }

        public async Task<IEnumerable<IAsignacionSolicitudDTO>> getAsignacionSolicitudEspecifica(int nIdSolicitud, int nIdColaborador)
        {
            return await _repository.getAsignacionSolicitudEspecifica(nIdSolicitud, nIdColaborador);
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarCliente(ICliente dato) 
        {
            return await _repository.postInsertarCliente(dato); 
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarUsuarioCliente(IUsuarioCliente dato)
        {
            return await _repository.postInsertarUsuarioCliente(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarAplicativo(IAplicativo dato)
        {
            return await _repository.postInsertarAplicativo(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarSolicitud(ISolicitud dato)
        {
            return await _repository.postInsertarSolicitud(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarNotificacion(INotificacion dato)
        {
            return await _repository.postInsertarNotificacion(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarColaborador(IColaborador dato)
        {
            return await _repository.postInsertarColaborador(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarRolColaborador(IRolColaborador dato)
        {
            return await _repository.postInsertarRolColaborador(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarAsignacionSolicitud(IAsignacionSolicitud dato)
        {
            return await _repository.postInsertarAsignacionSolicitud(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarRegistroTrabajo(IRegistroTrabajo dato)
        {
            return await _repository.postInsertarRegistroTrabajo(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarCliente(IClienteDATA dato)
        {
            return await _repository.postActualizarCliente(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarUsuarioCliente(IUsuarioClienteDATA dato)
        {
            return await _repository.postActualizarUsuarioCliente(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarAplicativo(IAplicativoDATA dato)
        {
            return await _repository.postActualizarAplicativo(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarSolicitud(ISolicitudDATA dato)
        {
            return await _repository.postActualizarSolicitud(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarNotificacion(INotificacionDATA dato)
        {
            return await _repository.postActualizarNotificacion(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarColaborador(IColaboradorDATA dato)
        {
            return await _repository.postActualizarColaborador(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarRolColaborador(IRolColaboradorDATA dato)
        {
            return await _repository.postActualizarRolColaborador(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarAsignacionSolicitud(IAsignacionSolicitudDATA dato)
        {
            return await _repository.postActualizarAsignacionSolicitud(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarRegistroTrabajo(IRegistroTrabajoDATA dato)
        {
            return await _repository.postActualizarRegistroTrabajo(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postEliminarNotificacion(INotificacionDLT dato)
        {
            return await _repository.postEliminarNotificacion(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postEliminarAsignacionSolicitud(IAsignacionSolicitudDLT dato)
        {
            return await _repository.postEliminarAsignacionSolicitud(dato);
        }

        public async Task<IRetorno_DigitalSupportV2> postEliminarRegistroTrabajo(IRegistroTrabajoDLT dato)
        {
            return await _repository.postEliminarRegistroTrabajo(dato);
        }

        public async Task<IRetorno_DigitalSupportV3> postAutenticacionUsuario(ILoginRequest dato) 
        { 
            return await _repository.postAutenticacionUsuario(dato); 
        }
    }
}
