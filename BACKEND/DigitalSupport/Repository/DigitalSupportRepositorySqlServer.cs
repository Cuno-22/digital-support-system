using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebApiDigitalSupport.Domain;
using WebApiDigitalSupport.Repository;
using WebApiDigitalSupport.Repository.Interface;


namespace WebApiDigitalSupport.Repository.SqlServer
{
    public class DigitalSupportRepository : IDigitalSupportRepository
    {
        protected readonly IConfiguration _configuration;
        public DigitalSupportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IRetorno_DigitalSupport> getListaCliente(int nIdTipoCliente)
        {
            try
            {
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 1);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdTipoCliente", nIdTipoCliente, DbType.Int32);

                    if (nIdTipoCliente == 1)
                    {
                        //CLIENTES TIPO EMPRESA
                        var listaClienteEmpresa = await connection.QueryAsync<IClienteEmpresa>(
                            storeprocedure, 
                            parameters, 
                            commandType: CommandType.StoredProcedure
                        );

                        return new IRetorno_DigitalSupport
                        {
                            sRetorno = "",
                            Data = listaClienteEmpresa.ToList<object>()
                        };
                    }

                    else if (nIdTipoCliente == 2) 
                    {
                        //CLIENTES TIPO PERSONA NATURAL
                        var listaClientePersona = await connection.QueryAsync<IClientePersonaNatural>(
                            storeprocedure, 
                            parameters, 
                            commandType: CommandType.StoredProcedure
                        );

                        return new IRetorno_DigitalSupport
                        {
                            sRetorno = "",
                            Data = listaClientePersona.ToList<object>()
                        };
                    }

                    else
                    {
                        var resultado = await connection.QueryFirstOrDefaultAsync<dynamic>(
                            storeprocedure, 
                            parameters, 
                            commandType: CommandType.StoredProcedure
                        );

                        string mensajeRetorno = resultado?.sRetorno ?? "Respuesta inesperada de la base de datos.";

                        return new IRetorno_DigitalSupport
                        {
                            sRetorno = mensajeRetorno,
                            Data = new List<object>()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ITipoCliente>> getListaTipoCliente()
        {
            try
            {
                IEnumerable<ITipoCliente> lista = new List<ITipoCliente>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 2);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<ITipoCliente>(
                        storeprocedure,
                        new { },
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IUsuarioClienteDTO>> getListaUsuarioCliente()
        {
            try
            {
                IEnumerable<IUsuarioClienteDTO> lista = new List<IUsuarioClienteDTO>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 3);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<IUsuarioClienteDTO>(
                        storeprocedure,
                        new { },
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IAplicativoDTO>> getListaAplicativo(int nIdUsuarioCliente)
        {
            try
            {
                IEnumerable<IAplicativoDTO> lista = new List<IAplicativoDTO>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 4);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdUsuarioCliente", nIdUsuarioCliente, DbType.Int32);

                    lista = await connection.QueryAsync<IAplicativoDTO>(
                        storeprocedure,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IRetorno_DigitalSupport> getListaSolicitud(int nIdTipoSolicitud)
        {
            try
            {
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 5);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdTipoSolicitud", nIdTipoSolicitud, DbType.Int32);

                    // Validamos localmente (coincide con la lógica del proc)
                    if (nIdTipoSolicitud == 1 || nIdTipoSolicitud == 2 || nIdTipoSolicitud == 3)
                    {
                        var lista = await connection.QueryAsync<ISolicitudDTO>(
                            storeprocedure,
                            parameters,
                            commandType: CommandType.StoredProcedure
                        );

                        return new IRetorno_DigitalSupport
                        {
                            sRetorno = "",
                            Data = lista.ToList<object>()
                        };
                    }

                    else
                    {
                        var resultado = await connection.QueryFirstOrDefaultAsync<dynamic>(
                            storeprocedure,
                            parameters,
                            commandType: CommandType.StoredProcedure
                        );

                        string mensajeRetorno = resultado?.sRetorno ?? "Respuesta inesperada de la base de datos.";
                        
                        return new IRetorno_DigitalSupport
                        {

                            sRetorno = mensajeRetorno,
                            Data = new List<object>()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ITipoSolicitud>> getListaTipoSolicitud()
        {
            try
            {
                IEnumerable<ITipoSolicitud> lista = new List<ITipoSolicitud>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 6);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<ITipoSolicitud>(
                        storeprocedure,
                        new { },
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<INotificacionDTO>> getListaNotificacion()
        {
            try
            {
                IEnumerable<INotificacionDTO> lista = new List<INotificacionDTO>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 7);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<INotificacionDTO>(
                        storeprocedure,
                        new { },
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IColaboradorDTO>> getListaColaborador(int nIdRolColaborador)
        {
            try
            {
                IEnumerable<IColaboradorDTO> lista = new List<IColaboradorDTO>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 8);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdRolColaborador", nIdRolColaborador, DbType.Int32);

                    lista = await connection.QueryAsync<IColaboradorDTO>(
                        storeprocedure,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IRolColaboradorDTO>> getListaRolColaborador()
        {
            try
            {
                IEnumerable<IRolColaboradorDTO> lista = new List<IRolColaboradorDTO>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 9);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<IRolColaboradorDTO>(
                        storeprocedure,
                        new { },
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IRegistroTrabajoDTO>> getListaRegistroTrabajo(int nIdColaborador)
        {
            try
            {
                IEnumerable<IRegistroTrabajoDTO> lista = new List<IRegistroTrabajoDTO>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 10);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdColaborador", nIdColaborador, DbType.Int32);

                    lista = await connection.QueryAsync<IRegistroTrabajoDTO>(
                        storeprocedure,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ISolicitudColaborador>> getSolicitudFinalizadaPorColaborador()
        {
            try
            {
                IEnumerable<ISolicitudColaborador> lista = new List<ISolicitudColaborador>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 11);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<ISolicitudColaborador>(storeprocedure, new { }, commandType: CommandType.StoredProcedure);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ITopColaborador>> getTop5ColaboradorSolicitud()
        {
            try
            {
                IEnumerable<ITopColaborador> lista = new List<ITopColaborador>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 12);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<ITopColaborador>(storeprocedure, new { }, commandType: CommandType.StoredProcedure);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ITopColaborador>> getColaboradorConMas5Solicitudes()
        {
            try
            {
                IEnumerable<ITopColaborador> lista = new List<ITopColaborador>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 13);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<ITopColaborador>(storeprocedure, new { }, commandType: CommandType.StoredProcedure);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ISolicitudUsuarioCliente>> getSolicitudUsuarioCliente()
        {
            try
            {
                IEnumerable<ISolicitudUsuarioCliente> lista = new List<ISolicitudUsuarioCliente>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 14);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<ISolicitudUsuarioCliente>(storeprocedure, new { }, commandType: CommandType.StoredProcedure);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IHorasTrabajadasColaborador>> getPromedioHorasTrabajadasColaborador()
        {
            try
            {
                IEnumerable<IHorasTrabajadasColaborador> lista = new List<IHorasTrabajadasColaborador>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 15);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<IHorasTrabajadasColaborador>(storeprocedure, new { }, commandType: CommandType.StoredProcedure);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IHistorialSolicitudUC>> getHistorialSolicitudUsuarioCliente(int nIdUsuarioCliente)
        {
            try
            {
                IEnumerable<IHistorialSolicitudUC> lista = new List<IHistorialSolicitudUC>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 16);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdUsuarioCliente", nIdUsuarioCliente, DbType.Int32);

                    lista = await connection.QueryAsync<IHistorialSolicitudUC>(
                        storeprocedure,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ISolicitudPendiente>> getListaSolicitudEnProcesoPendiente()
        {
            try
            {
                IEnumerable<ISolicitudPendiente> lista = new List<ISolicitudPendiente>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 17);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<ISolicitudPendiente>(storeprocedure, new { }, commandType: CommandType.StoredProcedure);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ISolicitudFinalizada>> getListaSolicitudFinalizada()
        {
            try
            {
                IEnumerable<ISolicitudFinalizada> lista = new List<ISolicitudFinalizada>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 18);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<ISolicitudFinalizada>(storeprocedure, new { }, commandType: CommandType.StoredProcedure);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IRetorno_DigitalSupport> getSolicitudPorMeses(int nMesesAntes)
        {
            try
            {
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 19);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nMesesAntes", nMesesAntes, DbType.Int32);

                    if (nMesesAntes > 0)
                    {
                        var listaSolicitudes = await connection.QueryAsync<IFechaSolicitud>(
                            storeprocedure,
                            parameters,
                            commandType: CommandType.StoredProcedure
                        );

                        return new IRetorno_DigitalSupport
                        {
                            sRetorno = "",
                            Data = listaSolicitudes.ToList<object>()
                        };
                    }
                    else
                    {
                        var resultado = await connection.QueryFirstOrDefaultAsync<dynamic>(
                            storeprocedure,
                            parameters,
                            commandType: CommandType.StoredProcedure
                        );

                        string mensajeRetorno = resultado?.sRetorno ?? "Respuesta inesperada de la base de datos.";

                        return new IRetorno_DigitalSupport
                        {
                            sRetorno = mensajeRetorno,
                            Data = new List<object>()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IFechaSolicitud>> getSolicitudFechaEspecifica(DateTime dFechaCreacion)
        {
            try
            {
                IEnumerable<IFechaSolicitud> lista = new List<IFechaSolicitud>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 20);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@dFechaCreacion", dFechaCreacion, DbType.Date);

                    lista = await connection.QueryAsync<IFechaSolicitud>(
                        storeprocedure,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IColaboradorDTO>> getListaColaboradoresActivos()
        {
            try
            {
                IEnumerable<IColaboradorDTO> lista = new List<IColaboradorDTO>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 21);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<IColaboradorDTO>(storeprocedure, new { }, commandType: CommandType.StoredProcedure);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ISolicitudCompleta>> getListaTotalSolicitudes()
        {
            try
            {
                IEnumerable<ISolicitudCompleta> lista = new List<ISolicitudCompleta>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 22);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<ISolicitudCompleta>(storeprocedure, new { }, commandType: CommandType.StoredProcedure);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IRolUsuario>> getListaRolUsuario()
        {
            try
            {
                IEnumerable<IRolUsuario> lista = new List<IRolUsuario>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 23);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<IRolUsuario>(storeprocedure, new { }, commandType: CommandType.StoredProcedure);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IRetorno_DigitalSupport> getListaClienteporUC(int nIdUsuarioCliente)
        {
            try
            {
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 24);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdUsuarioCliente", nIdUsuarioCliente, DbType.Int32);

                    if (nIdUsuarioCliente > 0)
                    {
                        var listaClientes = await connection.QueryAsync<IClienteUC>(
                            storeprocedure,
                            parameters,
                            commandType: CommandType.StoredProcedure
                        );

                        return new IRetorno_DigitalSupport
                        {
                            sRetorno = "",
                            Data = listaClientes.ToList<object>()
                        };
                    }
                    else
                    {
                        var resultado = await connection.QueryFirstOrDefaultAsync<dynamic>(
                            storeprocedure,
                            parameters,
                            commandType: CommandType.StoredProcedure
                        );

                        string mensajeRetorno = resultado?.sRetorno ?? "Respuesta inesperada de la base de datos.";

                        return new IRetorno_DigitalSupport
                        {
                            sRetorno = mensajeRetorno,
                            Data = new List<object>()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ISolicitudUC>> getSolicitudEspecificoUC(int nIdUsuarioCliente)
        {
            try
            {
                IEnumerable<ISolicitudUC> lista = new List<ISolicitudUC>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 25);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdUsuarioCliente", nIdUsuarioCliente, DbType.Int32);

                    lista = await connection.QueryAsync<ISolicitudUC>(
                        storeprocedure,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IAdministrador>> getListaAdministrador()
        {
            try
            {
                IEnumerable<IAdministrador> lista = new List<IAdministrador>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 26);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<IAdministrador>(storeprocedure, new { }, commandType: CommandType.StoredProcedure);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IRetorno_DigitalSupport> getPorcentajeAtencionColaborador(int nIdColaborador)
        {
            try
            {
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 27);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdColaborador", nIdColaborador, DbType.Int32);

                    if (nIdColaborador > 0)
                    {
                        var listaPorcentajes = await connection.QueryAsync<IEstadisticaColaborador>(
                            storeprocedure,
                            parameters,
                            commandType: CommandType.StoredProcedure
                        );

                        return new IRetorno_DigitalSupport
                        {
                            sRetorno = "",
                            Data = listaPorcentajes.ToList<object>()
                        };
                    }
                    else
                    {
                        var resultado = await connection.QueryFirstOrDefaultAsync<dynamic>(
                            storeprocedure,
                            parameters,
                            commandType: CommandType.StoredProcedure
                        );

                        string mensajeRetorno = resultado?.sRetorno ?? "Respuesta inesperada de la base de datos.";

                        return new IRetorno_DigitalSupport
                        {
                            sRetorno = mensajeRetorno,
                            Data = new List<object>()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IEstadisticaColaborador>> getListaPorcentajeColaboradorAtencion()
        {
            try
            {
                IEnumerable<IEstadisticaColaborador> lista = new List<IEstadisticaColaborador>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 28);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    lista = await connection.QueryAsync<IEstadisticaColaborador>(
                        storeprocedure,
                        new { },
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<INotificacionDTO>> getListaNotificacionesUC(int nIdUsuarioCliente)
        {
            try
            {
                IEnumerable<INotificacionDTO> lista = new List<INotificacionDTO>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 50);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdUsuarioCliente", nIdUsuarioCliente, DbType.Int32);

                    lista = await connection.QueryAsync<INotificacionDTO>(
                        storeprocedure,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IMensajeUC>> getMensajeNotificacionUC(int nIdNotificacion)
        {
            try
            {
                IEnumerable<IMensajeUC> lista = new List<IMensajeUC>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 51);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdNotificacion", nIdNotificacion, DbType.Int32);

                    lista = await connection.QueryAsync<IMensajeUC>(
                        storeprocedure,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<IAsignacionSolicitudDTO>> getAsignacionSolicitudEspecifica(int nIdSolicitud, int nIdColaborador)
        {
            try
            {
                IEnumerable<IAsignacionSolicitudDTO> lista = new List<IAsignacionSolicitudDTO>();
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 53);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@nIdSolicitud", nIdSolicitud, DbType.Int32);
                    parameters.Add("@nIdColaborador", nIdColaborador, DbType.Int32);

                    lista = await connection.QueryAsync<IAsignacionSolicitudDTO>(
                        storeprocedure,
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarCliente(ICliente data) 
        { 
            try 
            { 
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted; 

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled)) 
                { 
                    // Proc Insert Cabecera/ruta
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 29); 
                    
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    { 
                        // Insertar Cabecera
                        DynamicParameters parametros = new DynamicParameters(); 
                        parametros.Add("@sNombre", data.sNombre); 
                        parametros.Add("@sApellido", data.sApellido); 
                        parametros.Add("@nEdad", data.nEdad); 
                        parametros.Add("@dFechaNacimiento", data.dFechaNacimiento); 
                        parametros.Add("@sEmail", data.sEmail); 
                        parametros.Add("@sContrasena", data.sContrasena); 
                        parametros.Add("@nIdTipoCliente", data.nIdTipoCliente); 
                        parametros.Add("@bEstado", data.bEstado); 
                        
                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure); 
                    } 
                    transaction.Complete(); 
                    return retorno; 
                } 
            } 
            catch (TransactionException ex) 
            { 
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                }; 
            } 
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarUsuarioCliente(IUsuarioCliente data) 
        { 
            try 
            { 
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled)) 
                {
                    //Proc Insert Cabecera/ruta
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 30); 
                    
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport"))) 
                    { 

                        DynamicParameters parametros = new DynamicParameters(); 
                        parametros.Add("@sNombre", data.sNombre); 
                        parametros.Add("@sApellido", data.sApellido); 
                        parametros.Add("@sEmail", data.sEmail); 
                        parametros.Add("@sContrasena", data.sContrasena); 
                        parametros.Add("@nIdCliente", data.nIdCliente); 
                        parametros.Add("@bEstado", data.bEstado); 
                        
                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure); 
                    } 
                    transaction.Complete(); 
                    return retorno; 
                } 
            } 
            catch (TransactionException ex) 
            { 
                return new IRetorno_DigitalSupportV2 
                {
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                }; 
            } 
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarAplicativo(IAplicativo data) 
        { 
            try 
            { 
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled)) 
                { 
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 31); 
                    
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport"))) 
                    { 
                        DynamicParameters parametros = new DynamicParameters(); 
                        parametros.Add("@sNombreAplicativo", data.sNombreAplicativo); 
                        parametros.Add("@sDescripcion", data.sDescripcion); 
                        parametros.Add("@dFechaLanzamiento", data.dFechaLanzamiento); 
                        parametros.Add("@sVersion", data.sVersion); 
                        parametros.Add("@nIdCliente", data.nIdCliente); 
                        parametros.Add("@bEstado", data.bEstado); 
                        
                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure); 
                    } 
                    transaction.Complete();
                    return retorno; 
                } 
            } 
            catch (TransactionException ex) 
            { 
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                }; 
            } 
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarSolicitud(ISolicitud data) 
        { 
            try 
            { 
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled)) 
                { 
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 32); 

                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport"))) 
                    { 
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdUsuarioCliente", data.nIdUsuarioCliente);
                        parametros.Add("@nIdAplicativo", data.nIdAplicativo);
                        parametros.Add("@nIdTipoSolicitud", data.nIdTipoSolicitud);
                        parametros.Add("@sMotivo", data.sMotivo); 
                        parametros.Add("@bEstado", data.bEstado); 
                        
                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure); 
                    } 
                    transaction.Complete(); 
                    return retorno; 
                } 
            } 
            catch (TransactionException ex) 
            { 
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                }; 
            } 
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarNotificacion(INotificacion data) 
        { 
            try 
            { 
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled)) 
                { 
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 33); 
                    
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport"))) 
                    { 
                        DynamicParameters parametros = new DynamicParameters();

                        parametros.Add("@nIdSolicitud", data.nIdSolicitud);
                        parametros.Add("@sDescripcion", data.sDescripcion); 
                        parametros.Add("@bEstado", data.bEstado); 
                        
                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure); 
                    } 
                    
                    transaction.Complete(); return retorno; 
                } 
            } 
            catch (TransactionException ex) 
            { 
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                }; 
            } 
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarColaborador(IColaborador data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 34);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@sNombre", data.sNombre);
                        parametros.Add("@sApellido", data.sApellido);
                        parametros.Add("@sContrasena", data.sContrasena);
                        parametros.Add("@nIdRolColaborador", data.nIdRolColaborador);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarRolColaborador(IRolColaborador data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 35);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@sDescripcion", data.sDescripcion);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarAsignacionSolicitud(IAsignacionSolicitud data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 36);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdSolicitud", data.nIdSolicitud);
                        parametros.Add("@nIdColaborador", data.nIdColaborador);
                        parametros.Add("@bEsCoordinador", data.bEsCoordinador);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postInsertarRegistroTrabajo(IRegistroTrabajo data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 37);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdSolicitud", data.nIdSolicitud);
                        parametros.Add("@nIdColaborador", data.nIdColaborador);
                        parametros.Add("@sDescripcion", data.sDescripcion);
                        parametros.Add("@nHorasTrabajadas", data.nHorasTrabajadas);
                        parametros.Add("@sObservacion", data.sObservacion);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarCliente(IClienteDATA data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 38);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdCliente", data.nIdCliente);
                        parametros.Add("@sNombre", data.sNombre);
                        parametros.Add("@sApellido", data.sApellido);
                        parametros.Add("@nEdad", data.nEdad);
                        parametros.Add("@dFechaNacimiento", data.dFechaNacimiento);
                        parametros.Add("@sEmail", data.sEmail);
                        parametros.Add("@sContrasena", data.sContrasena);
                        parametros.Add("@nIdTipoCliente", data.nIdTipoCliente);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarUsuarioCliente(IUsuarioClienteDATA data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 39);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdUsuarioCliente", data.nIdUsuarioCliente);
                        parametros.Add("@sNombre", data.sNombre);
                        parametros.Add("@sApellido", data.sApellido);
                        parametros.Add("@sEmail", data.sEmail);
                        parametros.Add("@sContrasena", data.sContrasena);
                        parametros.Add("@nIdCliente", data.nIdCliente);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarAplicativo(IAplicativoDATA data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 40);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdAplicativo", data.nIdAplicativo);
                        parametros.Add("@sNombreAplicativo", data.sNombreAplicativo);
                        parametros.Add("@sDescripcion", data.sDescripcion);
                        parametros.Add("@dFechaLanzamiento", data.dFechaLanzamiento);
                        parametros.Add("@dFechaModificacion", data.dFechaModificacion);
                        parametros.Add("@sVersion", data.sVersion);
                        parametros.Add("@nIdCliente", data.nIdCliente);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarSolicitud(ISolicitudDATA data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 41);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdSolicitud", data.nIdSolicitud);
                        parametros.Add("@nIdUsuarioCliente", data.nIdUsuarioCliente);
                        parametros.Add("@nIdAplicativo", data.nIdAplicativo);
                        parametros.Add("@sMotivo", data.sMotivo);
                        parametros.Add("@dFechaCreacion", data.dFechaCreacion);
                        parametros.Add("@dFechaFinalizacion", data.dFechaFinalizacion);
                        parametros.Add("@sEstado", data.sEstado);
                        parametros.Add("@nIdTipoSolicitud", data.nIdTipoSolicitud);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarNotificacion(INotificacionDATA data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 42);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdNotificacion", data.nIdNotificacion);
                        parametros.Add("@nIdSolicitud", data.nIdSolicitud);
                        parametros.Add("@sDescripcion", data.sDescripcion);
                        parametros.Add("@dFechaEnvio", data.dFechaEnvio);
                        parametros.Add("@bLeido", data.bLeido);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 { nRetorno = -1, sRetorno = ex.Message };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarColaborador(IColaboradorDATA data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 43);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdColaborador", data.nIdColaborador);
                        parametros.Add("@sNombre", data.sNombre);
                        parametros.Add("@sApellido", data.sApellido);
                        parametros.Add("@sEmail", data.sEmail);
                        parametros.Add("@sContrasena", data.sContrasena);
                        parametros.Add("@nIdRolColaborador", data.nIdRolColaborador);
                        parametros.Add("@nIdRolUsuario", data.nIdRolUsuario);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarRolColaborador(IRolColaboradorDATA data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 44);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdRolColaborador", data.nIdRolColaborador);
                        parametros.Add("@sDescripcion", data.sDescripcion);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarAsignacionSolicitud(IAsignacionSolicitudDATA data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 45);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdAsignacionSolicitud", data.nIdAsignacionSolicitud);
                        parametros.Add("@nIdSolicitud", data.nIdSolicitud);
                        parametros.Add("@nIdColaborador", data.nIdColaborador);
                        parametros.Add("@bEsCoordinador", data.bEsCoordinador);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postActualizarRegistroTrabajo(IRegistroTrabajoDATA data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 46);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdRegistroTrabajo", data.nIdRegistroTrabajo);
                        parametros.Add("@nIdSolicitud", data.nIdSolicitud);
                        parametros.Add("@nIdColaborador", data.nIdColaborador);
                        parametros.Add("@sDescripcion", data.sDescripcion);
                        parametros.Add("@dFechaRegistro", data.dFechaRegistro);
                        parametros.Add("@nHorasTrabajadas", data.nHorasTrabajadas);
                        parametros.Add("@sObservacion", data.sObservacion);
                        parametros.Add("@bEstado", data.bEstado);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postEliminarNotificacion(INotificacionDLT data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 47);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdNotificacion", data.nIdNotificacion);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postEliminarAsignacionSolicitud(IAsignacionSolicitudDLT data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 48);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdAsignacionSolicitud", data.nIdAsignacionSolicitud);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV2> postEliminarRegistroTrabajo(IRegistroTrabajoDLT data)
        {
            try
            {
                IRetorno_DigitalSupportV2 retorno = new IRetorno_DigitalSupportV2();

                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;

                // Cabecera
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                {
                    var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 49);
                    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                    {
                        DynamicParameters parametros = new DynamicParameters();
                        parametros.Add("@nIdRegistroTrabajo", data.nIdRegistroTrabajo);

                        retorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV2>(storeprocedure, parametros, commandType: CommandType.StoredProcedure);
                    }
                    transaction.Complete();
                    return retorno;
                }
            }
            catch (TransactionException ex)
            {
                return new IRetorno_DigitalSupportV2 
                { 
                    nRetorno = -1, 
                    sRetorno = ex.Message 
                };
            }
        }

        public async Task<IRetorno_DigitalSupportV3> postAutenticacionUsuario(ILoginRequest data)
        {
            try
            {
                var storeprocedure = String.Format("{0};{1}", "[DigitalSuport].[pa_SoporteDigital]", 52);

                // Cabecera
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("cnDigitalSupport")))
                {
                    DynamicParameters parametros = new DynamicParameters();
                    parametros.Add("@sEmail", data.sEmail, DbType.String);
                    parametros.Add("@sContrasena", data.sContrasena, DbType.String);

                    var mensajeRetorno = await connection.QueryFirstOrDefaultAsync<IRetorno_DigitalSupportV3>(
                        storeprocedure,
                        parametros,
                        commandType: CommandType.StoredProcedure
                    );
                    if (mensajeRetorno == null)
                    {
                        return new IRetorno_DigitalSupportV3
                        {
                            sRetorno = "Error: No se recibió respuesta del procedimiento almacenado."
                        };
                    }
                    return mensajeRetorno;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
