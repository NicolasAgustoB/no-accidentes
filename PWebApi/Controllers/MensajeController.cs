using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PWebApi.Models;
using System.Collections.Generic;
using System;
using PWebApi.Models.Response;
using System.Data;

namespace PWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensajeController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/pago/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from mensaje", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Mensaje> result = new List<Mensaje>();
                    while (reader.Read())
                    {
                        Mensaje mensaje = new Mensaje();
                        mensaje.Id = (int)reader.GetInt64(0);
                        mensaje.Cuerpo = reader.GetString(1);

                        var fecha = reader.IsDBNull(2) ? "01/01/01" : reader.GetString(2);
                        mensaje.Fecha = DateTime.ParseExact(fecha, "dd/MM/yy", null);

                        var hora = reader.IsDBNull(3) ? "00:00" : reader.GetString(3);
                        mensaje.Hora = DateTime.ParseExact("01/01/01 " + hora, "dd/MM/yy HH:mm:ss", null);

                        mensaje.AccidenteId = (int)reader.GetInt64(4);
                        mensaje.UsuarioId = (int)reader.GetInt64(5);

                        result.Add(mensaje);
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
        // api/mensaje/create
        public IActionResult Create(Mensaje mensaje)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_MENSAJE.SP_CREATE_MENSAJE", connection);
                    command.Parameters.Add("cuerpo", mensaje.Cuerpo);
                    command.Parameters.Add("accidenteId", mensaje.AccidenteId);
                    command.Parameters.Add("usuarioId", mensaje.UsuarioId);
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
            Mensaje mensaje = new Mensaje { Id = Id };
            Respuesta respuesta = GetMensaje(mensaje);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/mensaje/update
        public IActionResult Update(Mensaje mensaje)
        {
            var read = GetMensaje(mensaje);
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
                    OracleCommand command = new OracleCommand("PCK_MENSAJE.SP_UPDATE_MENSAJE", connection);
                    command.Parameters.Add("id", mensaje.Id);
                    command.Parameters.Add("cuerpo", mensaje.Cuerpo);
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
        // api/pago/Delete
        public IActionResult Delete(int Id)
        {
            Mensaje mensaje = new Mensaje { Id = Id };
            var read = GetMensaje(mensaje);
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
                    OracleCommand command = new OracleCommand("PCK_MENSAJE.SP_DELETE_MENSAJE", connection);
                    command.Parameters.Add("id", mensaje.Id);
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

        public Respuesta GetMensaje(Mensaje mensaje)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from mensaje where ID_mensaje = " + mensaje.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    Mensaje result = new Mensaje();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Cuerpo = reader.GetString(1);

                            var fecha = reader.IsDBNull(2) ? "01/01/01" : reader.GetString(2);
                            result.Fecha = DateTime.ParseExact(fecha, "dd/MM/yy", null);

                            var hora = reader.IsDBNull(3) ? "00:00" : reader.GetString(3);
                            result.Hora = DateTime.ParseExact("01/01/01 " + hora, "dd/MM/yy HH:mm:ss", null);

                            result.AccidenteId = (int)reader.GetInt64(4);
                            result.UsuarioId = (int)reader.GetInt64(5);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró el mensaje";
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
    }

}
