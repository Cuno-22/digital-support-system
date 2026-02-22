using WebApiDigitalSupport.Response;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApiDigitalSupport.Domain;
using WebApiDigitalSupport.BusinessLogic.Interface;


namespace WebApiDigitalSupport.Controllers
{
    [Route("api/{v:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DigitalSupportController : Controller
    {
        private readonly IDigitalSupport _digitalSupport;

        public DigitalSupportController(IDigitalSupport digitalSupport)
        {
            _digitalSupport = digitalSupport;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IRetorno_DigitalSupport>>> getListaCliente(int nIdTipoCliente)
        {
            WebApiResponse<IRetorno_DigitalSupport> response = new WebApiResponse<IRetorno_DigitalSupport>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaCliente(nIdTipoCliente);

                response.Success = true;
                response.Response = new Response<IRetorno_DigitalSupport>();
                response.Response.Data = new List<IRetorno_DigitalSupport> { result };
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error> 
                {
                    new Error() 
                    { 
                        Code = StatusCodes.Status500InternalServerError, 
                        Message = ex.Message 
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<ITipoCliente>>> getListaTipoCliente()
        {
            WebApiResponse<ITipoCliente> response = new WebApiResponse<ITipoCliente>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaTipoCliente();

                response.Success = true;
                response.Response = new Response<ITipoCliente>();
                response.Response.Data = result.ToList<ITipoCliente>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error> 
                {
                    new Error() 
                    { 
                        Code = StatusCodes.Status500InternalServerError, 
                        Message = ex.Message 
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IUsuarioClienteDTO>>> getListaUsuarioCliente()
        {
            WebApiResponse<IUsuarioClienteDTO> response = new WebApiResponse<IUsuarioClienteDTO>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaUsuarioCliente();

                response.Success = true;
                response.Response = new Response<IUsuarioClienteDTO>();
                response.Response.Data = result.ToList<IUsuarioClienteDTO>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error> 
                {
                    new Error() 
                    { 
                        Code = StatusCodes.Status500InternalServerError, 
                        Message = ex.Message 
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IAplicativoDTO>>> getListaAplicativo(int nIdUsuarioCliente)
        {
            WebApiResponse<IAplicativoDTO> response = new WebApiResponse<IAplicativoDTO>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaAplicativo(nIdUsuarioCliente);

                // Verificar si result y Data no son nulos
                if (result != null)
                {
                    // Iterar sobre los objetos dentro de Data foreach
                    foreach (var app in result)
                    {
                        if (!string.IsNullOrEmpty(app.dFechaLanzamiento) && DateTime.TryParse(app.dFechaLanzamiento, out var fechaLanzamiento))
                            app.dFechaLanzamiento = fechaLanzamiento.ToString("yyyy-MM-ddTHH:mm:ss");

                        if (!string.IsNullOrEmpty(app.dFechaModificacion) && DateTime.TryParse(app.dFechaModificacion, out var fechaModificacion))
                            app.dFechaModificacion = fechaModificacion.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }

                response.Success = true;
                response.Response = new Response<IAplicativoDTO>();
                response.Response.Data = result.ToList<IAplicativoDTO>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error> 
                {
                    new Error() 
                    { 
                        Code = StatusCodes.Status500InternalServerError, 
                        Message = ex.Message 
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IRetorno_DigitalSupport>>> getListaSolicitud(int nIdTipoSolicitud)
        {
            WebApiResponse<IRetorno_DigitalSupport> response = new WebApiResponse<IRetorno_DigitalSupport>(); 
            try
            {
                response.Errors = new List<Error>(); 
                
                var result = await _digitalSupport.getListaSolicitud(nIdTipoSolicitud); 
                
                // Verificar si result y Data no son nulos
                if (result != null && result.Data != null) 
                { 
                    // Iterar sobre los objetos dentro de Data foreach
                    foreach (var item in result.Data)
                    {
                        if (item is ISolicitudDTO solicitud)
                        {

                            // Convertir dFechaCreacion a formato ISO 8601
                            if (!string.IsNullOrEmpty(solicitud.dFechaCreacion))
                            {
                                DateTime fechaCreacion; 
                                
                                if (DateTime.TryParse(solicitud.dFechaCreacion, out fechaCreacion))
                                {
                                    solicitud.dFechaCreacion = fechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss");
                                }
                            } 
                            
                            // Convertir dFechaFinalizacion a formato ISO 8601
                            if (!string.IsNullOrEmpty(solicitud.dFechaFinalizacion)) 
                            { 
                                DateTime fechaFinalizacion;
                                
                                if (DateTime.TryParse(solicitud.dFechaFinalizacion, out fechaFinalizacion)) 
                                { 
                                    solicitud.dFechaFinalizacion = fechaFinalizacion.ToString("yyyy-MM-ddTHH:mm:ss"); 
                                } 
                            } 
                        } 
                    } 
                } 

                response.Success = true; 
                response.Response = new Response<IRetorno_DigitalSupport>(); 
                response.Response.Data = new List<IRetorno_DigitalSupport> { result }; 
                return StatusCode(200, response); 
            } 
            catch (Exception ex) 
            { 
                response.Success = false; 
                response.Errors = new List<Error> 
                { 
                    new Error() 
                    { 
                        Code = StatusCodes.Status500InternalServerError, 
                        Message = ex.Message 
                    } 
                }; 
                return StatusCode(500, response); 
            } 
        } 

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<ITipoSolicitud>>> getListaTipoSolicitud()
        {
            WebApiResponse<ITipoSolicitud> response = new WebApiResponse<ITipoSolicitud>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaTipoSolicitud();

                response.Success = true;
                response.Response = new Response<ITipoSolicitud>();
                response.Response.Data = result.ToList<ITipoSolicitud>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error> 
                {
                    new Error() 
                    { 
                        Code = StatusCodes.Status500InternalServerError, 
                        Message = ex.Message 
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<INotificacionDTO>>> getListaNotificacion()
        {
            WebApiResponse<INotificacionDTO> response = new WebApiResponse<INotificacionDTO>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaNotificacion();

                if (result != null)
                {
                    foreach (var notify in result)
                    {
                        if (!string.IsNullOrEmpty(notify.dFechaEnvio) && DateTime.TryParse(notify.dFechaEnvio, out var fechaEnvio))
                            notify.dFechaEnvio = fechaEnvio.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }

                response.Success = true;
                response.Response = new Response<INotificacionDTO>();
                response.Response.Data = result.ToList();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
        {
            new Error()
            {
                Code = StatusCodes.Status500InternalServerError,
                Message = ex.Message
            }
        };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IColaboradorDTO>>> getListaColaborador(int nIdRolColaborador)
        {
            WebApiResponse<IColaboradorDTO> response = new WebApiResponse<IColaboradorDTO>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaColaborador(nIdRolColaborador);

                response.Success = true;
                response.Response = new Response<IColaboradorDTO>();
                response.Response.Data = result.ToList<IColaboradorDTO>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error> 
                {
                    new Error() 
                    { 
                        Code = StatusCodes.Status500InternalServerError, 
                        Message = ex.Message 
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IRolColaboradorDTO>>> getListaRolColaborador()
        {
            WebApiResponse<IRolColaboradorDTO> response = new WebApiResponse<IRolColaboradorDTO>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaRolColaborador();

                response.Success = true;
                response.Response = new Response<IRolColaboradorDTO>();
                response.Response.Data = result.ToList<IRolColaboradorDTO>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error> 
                {
                    new Error() 
                    { 
                        Code = StatusCodes.Status500InternalServerError, 
                        Message = ex.Message 
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IRegistroTrabajoDTO>>> getListaRegistroTrabajo(int nIdColaborador)
        {
            WebApiResponse<IRegistroTrabajoDTO> response = new WebApiResponse<IRegistroTrabajoDTO>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaRegistroTrabajo(nIdColaborador);

                if (result != null)
                {
                    foreach (var registro in result)
                    {
                        if (!string.IsNullOrEmpty(registro.dFechaRegistro) && DateTime.TryParse(registro.dFechaRegistro, out var fechaRegistro))
                            registro.dFechaRegistro = fechaRegistro.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }

                response.Success = true;
                response.Response = new Response<IRegistroTrabajoDTO>();
                response.Response.Data = result.ToList<IRegistroTrabajoDTO>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<ISolicitudColaborador>>> getSolicitudFinalizadaPorColaborador()
        {
            WebApiResponse<ISolicitudColaborador> response = new WebApiResponse<ISolicitudColaborador>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getSolicitudFinalizadaPorColaborador();

                response.Success = true;
                response.Response = new Response<ISolicitudColaborador>();
                response.Response.Data = result.ToList<ISolicitudColaborador>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<ITopColaborador>>> getTop5ColaboradorSolicitud()
        {
            WebApiResponse<ITopColaborador> response = new WebApiResponse<ITopColaborador>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getTop5ColaboradorSolicitud();

                response.Success = true;
                response.Response = new Response<ITopColaborador>();
                response.Response.Data = result.ToList<ITopColaborador>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<ITopColaborador>>> getColaboradorConMas5Solicitudes()
        {
            WebApiResponse<ITopColaborador> response = new WebApiResponse<ITopColaborador>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getColaboradorConMas5Solicitudes();

                response.Success = true;
                response.Response = new Response<ITopColaborador>();
                response.Response.Data = result.ToList<ITopColaborador>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<ISolicitudUsuarioCliente>>> getSolicitudUsuarioCliente()
        {
            WebApiResponse<ISolicitudUsuarioCliente> response = new WebApiResponse<ISolicitudUsuarioCliente>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getSolicitudUsuarioCliente();

                response.Success = true;
                response.Response = new Response<ISolicitudUsuarioCliente>();
                response.Response.Data = result.ToList<ISolicitudUsuarioCliente>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IHorasTrabajadasColaborador>>> getPromedioHorasTrabajadasColaborador()
        {
            WebApiResponse<IHorasTrabajadasColaborador> response = new WebApiResponse<IHorasTrabajadasColaborador>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getPromedioHorasTrabajadasColaborador();

                response.Success = true;
                response.Response = new Response<IHorasTrabajadasColaborador>();
                response.Response.Data = result.ToList<IHorasTrabajadasColaborador>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IHistorialSolicitudUC>>> getHistorialSolicitudUsuarioCliente(int nIdUsuarioCliente)
        {
            WebApiResponse<IHistorialSolicitudUC> response = new WebApiResponse<IHistorialSolicitudUC>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getHistorialSolicitudUsuarioCliente(nIdUsuarioCliente);

                // Verificar si result y Data no son nulos
                if (result != null)
                {
                    // Iterar sobre los objetos dentro de Data foreach
                    foreach (var solicitud in result)
                    {
                        if (!string.IsNullOrEmpty(solicitud.dFechaCreacion) && DateTime.TryParse(solicitud.dFechaCreacion, out var fechaCreacion))
                            solicitud.dFechaCreacion = fechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss");

                        if (!string.IsNullOrEmpty(solicitud.dFechaFinalizacion) && DateTime.TryParse(solicitud.dFechaFinalizacion, out var fechaFinalizacion))
                            solicitud.dFechaFinalizacion = fechaFinalizacion.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }

                response.Success = true;
                response.Response = new Response<IHistorialSolicitudUC>();
                response.Response.Data = result.ToList<IHistorialSolicitudUC>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<ISolicitudPendiente>>> getListaSolicitudEnProcesoPendiente()
        {
            WebApiResponse<ISolicitudPendiente> response = new WebApiResponse<ISolicitudPendiente>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaSolicitudEnProcesoPendiente();

                // Verificar si result y Data no son nulos
                if (result != null)
                {
                    // Iterar sobre los objetos dentro de Data foreach
                    foreach (var solicitud in result)
                    {
                        if (!string.IsNullOrEmpty(solicitud.dFechaCreacion) && DateTime.TryParse(solicitud.dFechaCreacion, out var fechaCreacion))
                            solicitud.dFechaCreacion = fechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }

                response.Success = true;
                response.Response = new Response<ISolicitudPendiente>();
                response.Response.Data = result.ToList<ISolicitudPendiente>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<ISolicitudFinalizada>>> getListaSolicitudFinalizada()
        {
            WebApiResponse<ISolicitudFinalizada> response = new WebApiResponse<ISolicitudFinalizada>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaSolicitudFinalizada();

                // Verificar si result y Data no son nulos
                if (result != null)
                {
                    // Iterar sobre los objetos dentro de Data foreach
                    foreach (var solicitud in result)
                    {
                        if (!string.IsNullOrEmpty(solicitud.dFechaCreacion) && DateTime.TryParse(solicitud.dFechaCreacion, out var fechaCreacion))
                            solicitud.dFechaCreacion = fechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss");

                        if (!string.IsNullOrEmpty(solicitud.dFechaFinalizacion) && DateTime.TryParse(solicitud.dFechaFinalizacion, out var fechaFinalizacion))
                            solicitud.dFechaFinalizacion = fechaFinalizacion.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }

                response.Success = true;
                response.Response = new Response<ISolicitudFinalizada>();
                response.Response.Data = result.ToList<ISolicitudFinalizada>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IRetorno_DigitalSupport>>> getSolicitudPorMeses(int nMesesAntes)
        {
            WebApiResponse<IRetorno_DigitalSupport> response = new WebApiResponse<IRetorno_DigitalSupport>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getSolicitudPorMeses(nMesesAntes);

                // Verificar si result y Data no son nulos
                if (result != null && result.Data != null)
                {
                    // Iterar sobre los objetos dentro de Data foreach
                    foreach (var item in result.Data)
                    {
                        if (item is IFechaSolicitud solicitud)
                        {

                            // Convertir dFechaCreacion a formato ISO 8601
                            if (!string.IsNullOrEmpty(solicitud.dFechaCreacion))
                            {
                                DateTime fechaCreacion;

                                if (DateTime.TryParse(solicitud.dFechaCreacion, out fechaCreacion))
                                {
                                    solicitud.dFechaCreacion = fechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss");
                                }
                            }

                            // Convertir dFechaFinalizacion a formato ISO 8601
                            if (!string.IsNullOrEmpty(solicitud.dFechaFinalizacion))
                            {
                                DateTime fechaFinalizacion;

                                if (DateTime.TryParse(solicitud.dFechaFinalizacion, out fechaFinalizacion))
                                {
                                    solicitud.dFechaFinalizacion = fechaFinalizacion.ToString("yyyy-MM-ddTHH:mm:ss");
                                }
                            }
                        }
                    }
                }

                response.Success = true;
                response.Response = new Response<IRetorno_DigitalSupport>();
                response.Response.Data = new List<IRetorno_DigitalSupport> { result };
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IFechaSolicitud>>> getSolicitudFechaEspecifica(DateTime dFechaCreacion)
        {
            WebApiResponse<IFechaSolicitud> response = new WebApiResponse<IFechaSolicitud>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getSolicitudFechaEspecifica(dFechaCreacion);

                // Verificar si result y Data no son nulos
                if (result != null)
                {
                    // Iterar sobre los objetos dentro de Data foreach
                    foreach (var solicitud in result)
                    {
                        if (!string.IsNullOrEmpty(solicitud.dFechaCreacion) && DateTime.TryParse(solicitud.dFechaCreacion, out var fechaCreacion))
                            solicitud.dFechaCreacion = fechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss");

                        if (!string.IsNullOrEmpty(solicitud.dFechaFinalizacion) && DateTime.TryParse(solicitud.dFechaFinalizacion, out var fechaFinalizacion))
                            solicitud.dFechaFinalizacion = fechaFinalizacion.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }

                response.Success = true;
                response.Response = new Response<IFechaSolicitud>();
                response.Response.Data = result.ToList<IFechaSolicitud>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IColaboradorDTO>>> getListaColaboradoresActivos()
        {
            WebApiResponse<IColaboradorDTO> response = new WebApiResponse<IColaboradorDTO>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaColaboradoresActivos();

                response.Success = true;
                response.Response = new Response<IColaboradorDTO>();
                response.Response.Data = result.ToList<IColaboradorDTO>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<ISolicitudCompleta>>> getListaTotalSolicitudes()
        {
            WebApiResponse<ISolicitudCompleta> response = new WebApiResponse<ISolicitudCompleta>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaTotalSolicitudes();

                // Verificar si result y Data no son nulos
                if (result != null)
                {
                    // Iterar sobre los objetos dentro de Data foreach
                    foreach (var solicitud in result)
                    {
                        if (!string.IsNullOrEmpty(solicitud.dFechaCreacion) && DateTime.TryParse(solicitud.dFechaCreacion, out var fechaCreacion))
                            solicitud.dFechaCreacion = fechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss");

                        if (!string.IsNullOrEmpty(solicitud.dFechaFinalizacion) && DateTime.TryParse(solicitud.dFechaFinalizacion, out var fechaFinalizacion))
                            solicitud.dFechaFinalizacion = fechaFinalizacion.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }

                response.Success = true;
                response.Response = new Response<ISolicitudCompleta>();
                response.Response.Data = result.ToList<ISolicitudCompleta>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IRolUsuario>>> getListaRolUsuario()
        {
            WebApiResponse<IRolUsuario> response = new WebApiResponse<IRolUsuario>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaRolUsuario();

                response.Success = true;
                response.Response = new Response<IRolUsuario>();
                response.Response.Data = result.ToList<IRolUsuario>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IRetorno_DigitalSupport>>> getListaClienteporUC(int nIdUsuarioCliente)
        {
            WebApiResponse<IRetorno_DigitalSupport> response = new WebApiResponse<IRetorno_DigitalSupport>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaClienteporUC(nIdUsuarioCliente);

                response.Success = true;
                response.Response = new Response<IRetorno_DigitalSupport>();
                response.Response.Data = new List<IRetorno_DigitalSupport> { result };
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<ISolicitudUC>>> getSolicitudEspecificoUC(int nIdUsuarioCliente)
        {
            WebApiResponse<ISolicitudUC> response = new WebApiResponse<ISolicitudUC>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getSolicitudEspecificoUC(nIdUsuarioCliente);

                // Verificar si result y Data no son nulos
                if (result != null)
                {
                    // Iterar sobre los objetos dentro de Data foreach
                    foreach (var solicitud in result)
                    {
                        if (!string.IsNullOrEmpty(solicitud.dFechaCreacion) && DateTime.TryParse(solicitud.dFechaCreacion, out var fechaCreacion))
                            solicitud.dFechaCreacion = fechaCreacion.ToString("yyyy-MM-ddTHH:mm:ss");

                        if (!string.IsNullOrEmpty(solicitud.dFechaFinalizacion) && DateTime.TryParse(solicitud.dFechaFinalizacion, out var fechaFinalizacion))
                            solicitud.dFechaFinalizacion = fechaFinalizacion.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }

                response.Success = true;
                response.Response = new Response<ISolicitudUC>();
                response.Response.Data = result.ToList<ISolicitudUC>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IAdministrador>>> getListaAdministrador()
        {
            WebApiResponse<IAdministrador> response = new WebApiResponse<IAdministrador>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaAdministrador();

                response.Success = true;
                response.Response = new Response<IAdministrador>();
                response.Response.Data = result.ToList<IAdministrador>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IRetorno_DigitalSupport>>> getPorcentajeAtencionColaborador(int nIdColaborador)
        {
            WebApiResponse<IRetorno_DigitalSupport> response = new WebApiResponse<IRetorno_DigitalSupport>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getPorcentajeAtencionColaborador(nIdColaborador);

                response.Success = true;
                response.Response = new Response<IRetorno_DigitalSupport>();
                response.Response.Data = new List<IRetorno_DigitalSupport> { result };
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IEstadisticaColaborador>>> getListaPorcentajeColaboradorAtencion()
        {
            WebApiResponse<IEstadisticaColaborador> response = new WebApiResponse<IEstadisticaColaborador>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaPorcentajeColaboradorAtencion();

                response.Success = true;
                response.Response = new Response<IEstadisticaColaborador>();
                response.Response.Data = result.ToList<IEstadisticaColaborador>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<INotificacionDTO>>> getListaNotificacionesUC(int nIdUsuarioCliente)
        {
            WebApiResponse<INotificacionDTO> response = new WebApiResponse<INotificacionDTO>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getListaNotificacionesUC(nIdUsuarioCliente);

                // Verificar si result y Data no son nulos
                if (result != null)
                {
                    foreach (var notify in result)
                    {
                        if (!string.IsNullOrEmpty(notify.dFechaEnvio) && DateTime.TryParse(notify.dFechaEnvio, out var fechaEnvio))
                            notify.dFechaEnvio = fechaEnvio.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }

                response.Success = true;
                response.Response = new Response<INotificacionDTO>();
                response.Response.Data = result.ToList<INotificacionDTO>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IMensajeUC>>> getMensajeNotificacionUC(int nIdNotificacion)
        {
            WebApiResponse<IMensajeUC> response = new WebApiResponse<IMensajeUC>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getMensajeNotificacionUC(nIdNotificacion);

                response.Success = true;
                response.Response = new Response<IMensajeUC>();
                response.Response.Data = result.ToList<IMensajeUC>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<WebApiResponse<IAsignacionSolicitudDTO>>> getAsignacionSolicitudEspecifica(int nIdSolicitud, int nIdColaborador)
        {
            WebApiResponse<IAsignacionSolicitudDTO> response = new WebApiResponse<IAsignacionSolicitudDTO>();
            try
            {
                response.Errors = new List<Error>();

                var result = await _digitalSupport.getAsignacionSolicitudEspecifica(nIdSolicitud, nIdColaborador);

                response.Success = true;
                response.Response = new Response<IAsignacionSolicitudDTO>();
                response.Response.Data = result.ToList<IAsignacionSolicitudDTO>();
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> PostInsertarCliente([FromBody] ICliente data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Insertamos
                var result = await _digitalSupport.postInsertarCliente(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postInsertarUsuarioCliente([FromBody] IUsuarioCliente data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Insertamos
                var result = await _digitalSupport.postInsertarUsuarioCliente(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postInsertarAplicativo([FromBody] IAplicativo data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Insertamos
                var result = await _digitalSupport.postInsertarAplicativo(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postInsertarSolicitud([FromBody] ISolicitud data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Insertamos
                var result = await _digitalSupport.postInsertarSolicitud(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postInsertarNotificacion([FromBody] INotificacion data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Insertamos
                var result = await _digitalSupport.postInsertarNotificacion(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postInsertarColaborador([FromBody] IColaborador data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Insertamos
                var result = await _digitalSupport.postInsertarColaborador(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postInsertarRolColaborador([FromBody] IRolColaborador data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Insertamos
                var result = await _digitalSupport.postInsertarRolColaborador(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postInsertarAsignacionSolicitud([FromBody] IAsignacionSolicitud data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Insertamos
                var result = await _digitalSupport.postInsertarAsignacionSolicitud(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postInsertarRegistroTrabajo([FromBody] IRegistroTrabajo data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Insertamos
                var result = await _digitalSupport.postInsertarRegistroTrabajo(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postActualizarCliente([FromBody] IClienteDATA data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Actualizar
                var result = await _digitalSupport.postActualizarCliente(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postActualizarUsuarioCliente([FromBody] IUsuarioClienteDATA data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Actualizar
                var result = await _digitalSupport.postActualizarUsuarioCliente(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postActualizarAplicativo([FromBody] IAplicativoDATA data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Actualizar
                var result = await _digitalSupport.postActualizarAplicativo(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postActualizarSolicitud([FromBody] ISolicitudDATA data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Actualizar
                var result = await _digitalSupport.postActualizarSolicitud(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postActualizarNotificacion([FromBody] INotificacionDATA data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Actualizar
                var result = await _digitalSupport.postActualizarNotificacion(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postActualizarColaborador([FromBody] IColaboradorDATA data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Actualizar
                var result = await _digitalSupport.postActualizarColaborador(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postActualizarRolColaborador([FromBody] IRolColaboradorDATA data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Actualizar
                var result = await _digitalSupport.postActualizarRolColaborador(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postActualizarAsignacionSolicitud([FromBody] IAsignacionSolicitudDATA data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Actualizar
                var result = await _digitalSupport.postActualizarAsignacionSolicitud(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postActualizarRegistroTrabajo([FromBody] IRegistroTrabajoDATA data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Actualizar
                var result = await _digitalSupport.postActualizarRegistroTrabajo(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postEliminarNotificacion([FromBody] INotificacionDLT data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Eliminamos
                var result = await _digitalSupport.postEliminarNotificacion(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postEliminarAsignacionSolicitud([FromBody] IAsignacionSolicitudDLT data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Eliminamos
                var result = await _digitalSupport.postEliminarAsignacionSolicitud(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponseV2<IRetorno_DigitalSupportV2>>> postEliminarRegistroTrabajo([FromBody] IRegistroTrabajoDLT data)
        {
            WebApiResponseV2<IRetorno_DigitalSupportV2> response = new WebApiResponseV2<IRetorno_DigitalSupportV2>();
            try
            {
                // Eliminamos
                var result = await _digitalSupport.postEliminarRegistroTrabajo(data);

                if (result.nRetorno < 0)
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result.sRetorno
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new ResponseV2<IRetorno_DigitalSupportV2>();
                response.Response.Data = result;
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<WebApiResponse<IRetorno_DigitalSupportV3>>> postAutenticacionUsuario([FromBody] ILoginRequest data)
        {
            WebApiResponse<IRetorno_DigitalSupportV3> response = new WebApiResponse<IRetorno_DigitalSupportV3>();

            try
            {
                var result = await _digitalSupport.postAutenticacionUsuario(data);

                if (result == null || result.sRetorno.Contains("Error"))
                {
                    response.Success = false;
                    response.Errors = new List<Error>
                    {
                        new Error()
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Message = result?.sRetorno ?? "Error desconocido durante la autenticación"
                        }
                    };
                    return StatusCode(400, response);
                }

                response.Success = true;
                response.Response = new Response<IRetorno_DigitalSupportV3>();
                response.Response.Data = new List<IRetorno_DigitalSupportV3> { result };

                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Errors = new List<Error>
                {
                    new Error()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = ex.Message
                    }
                };
                return StatusCode(500, response);
            }
        }
    }
}