using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PWebApi.Models;
using PWebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Data;

namespace PWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/solicitud/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from solicitud", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Solicitud> result = new List<Solicitud>();
                    while (reader.Read())
                    {
                        Solicitud solicitud = new Solicitud();
                        solicitud.Id = (int)reader.GetInt64(0);
                        solicitud.Situacion = (int)reader.GetInt64(1);
                        solicitud.Descripcion = reader.GetString(2);

                        var fechaInicio = reader.IsDBNull(3) ? "01/01/01" : reader.GetString(3);
                        solicitud.FechaInicio = DateTime.ParseExact(fechaInicio, "dd/MM/yy", null);

                        var fechaTermino = reader.IsDBNull(4) ? "01/01/01" : reader.GetString(4);
                        solicitud.FechaTermino = DateTime.ParseExact(fechaTermino, "dd/MM/yy", null);

                        solicitud.Respuesta = reader.GetString(5);
                        solicitud.TipoSolicitudId = (int)reader.GetInt64(6);

                        var empleadoId = reader.IsDBNull(7) ? 0 : (int)reader.GetInt64(7);
                        solicitud.EmpleadoId = empleadoId;

                        solicitud.ClienteId = (int)reader.GetInt64(8);
                        result.Add(solicitud);
                    }

                    respuesta.Exito = 1;
                    respuesta.Data = result;
                }
                catch (System.Exception ex)
                {
                    respuesta.Mensaje = ex.Message;
                    return BadRequest(respuesta);
                }
            }
            return Ok(respuesta);
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        // api/solicitud/create
        public IActionResult Create(Solicitud solicitud)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_SOLICITUD.SP_CREATE_SOLICITUD", connection);
                    command.Parameters.Add("descripcion", solicitud.Descripcion);
                    command.Parameters.Add("tipoSolicitudId", solicitud.TipoSolicitudId);
                    command.Parameters.Add("clienteId", solicitud.ClienteId);

                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    respuesta.Mensaje = "Añadido correctamente";
                    respuesta.Exito = 1;
                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                return BadRequest(respuesta);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Read/{Id}")]
        public IActionResult Read(int Id)
        {
            Solicitud solicitud = new Solicitud { Id = Id };
            Respuesta respuesta = GetSolicitud(solicitud);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/solicitud/update
        public IActionResult Update(Solicitud solicitud)
        {
            var read = GetSolicitud(solicitud);
            if (read.Exito == 0)
            {
                return BadRequest(read);
            }

            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_SOLICITUD.SP_UPDATE_SOLICITUD", connection);
                    command.Parameters.Add("id", solicitud.Id);
                    command.Parameters.Add("situacion", solicitud.Situacion);
                    command.Parameters.Add("respuesta", solicitud.Respuesta);
                    command.Parameters.Add("tipoSolicitudId", solicitud.TipoSolicitudId);
                    command.Parameters.Add("empleadoId", solicitud.EmpleadoId);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    respuesta.Mensaje = "Editado correctamente";
                    respuesta.Exito = 1;
                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                return BadRequest(respuesta);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{Id}")]
        // api/solicitud/Delete
        public IActionResult Delete(int Id)
        {
            Solicitud solicitud = new Solicitud();
            solicitud.Id = Id;
            var read = GetSolicitud(solicitud);
            if (read.Exito == 0)
            {
                return BadRequest(read);
            }
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_SOLICITUD.SP_DELETE_SOLICITUD", connection);
                    command.Parameters.Add("id", solicitud.Id);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    respuesta.Exito = 1;
                    respuesta.Mensaje = "Eliminado con exito";
                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                return BadRequest(respuesta);
            }
        }

        public Respuesta GetSolicitud(Solicitud solicitud)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from solicitud where ID_solicitud = " + solicitud.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    Solicitud result = new Solicitud();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Situacion = (int)reader.GetInt64(1);
                            result.Descripcion = reader.GetString(2);

                            var fechaInicio = reader.IsDBNull(3) ? "01/01/01" : reader.GetString(3);
                            result.FechaInicio = DateTime.ParseExact(fechaInicio, "dd/MM/yy", null);

                            var fechaTermino = reader.IsDBNull(4) ? "01/01/01" : reader.GetString(4);
                            result.FechaTermino = DateTime.ParseExact(fechaTermino, "dd/MM/yy", null);

                            result.Respuesta = reader.GetString(5);
                            result.TipoSolicitudId = (int)reader.GetInt64(6);

                            var empleadoId = reader.IsDBNull(7) ? 0 : (int)reader.GetInt64(7);
                            result.EmpleadoId = empleadoId;

                            result.ClienteId = (int)reader.GetInt64(8);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró la solicitud";
                        return respuesta;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

        }
        [Authorize]
        [HttpPut]
        [Route("FinishSolicitud")]
        // api/solicitud/finishsolicitud
        public IActionResult FinishSolicitud(Solicitud solicitud)
        {
            var read = GetSolicitud(solicitud);
            if (read.Exito == 0)
            {
                return BadRequest(read);
            }

            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_SOLICITUD.SP_FINISH_SOLICITUD", connection);
                    command.Parameters.Add("id", solicitud.Id);
                    command.Parameters.Add("situacion", solicitud.Situacion);
                    command.Parameters.Add("respuesta", solicitud.Respuesta);
                    command.Parameters.Add("EmpleadoId", solicitud.EmpleadoId);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    respuesta.Mensaje = "Solicitud respondida correctamente";
                    respuesta.Exito = 1;
                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                return BadRequest(respuesta);
            }
        }
    }
}
