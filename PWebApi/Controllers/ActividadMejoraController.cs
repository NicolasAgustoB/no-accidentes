using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PWebApi.Models;
using System.Data;
using System;
using PWebApi.Models.Response;
using System.Collections.Generic;

namespace PWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadMejoraController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/actividadmejora/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from actividad_mejora", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<ActividadMejora> result = new List<ActividadMejora>();
                    while (reader.Read())
                    {
                        ActividadMejora actividadMejora = new ActividadMejora();
                        actividadMejora.Id = (int)reader.GetInt64(0);
                        actividadMejora.Nombre = reader.GetString(1);
                        actividadMejora.Descripcion = reader.GetString(2);
                        actividadMejora.Situacion = (int)reader.GetInt64(3);
                        actividadMejora.Comentario = reader.GetString(4);
                        actividadMejora.ServicioId = (int)reader.GetInt64(5);
                        result.Add(actividadMejora);
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
        // api/actividadmejora/create
        public IActionResult Create(ActividadMejora actividadMejora)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_ACTIVIDAD_MEJORA.SP_CREATE_ACTIVIDAD_MEJORA", connection);
                    command.Parameters.Add("nombre", actividadMejora.Nombre);
                    command.Parameters.Add("descripcion", actividadMejora.Descripcion);
                    command.Parameters.Add("servicioId", actividadMejora.ServicioId);
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
        // api/actividadmejora/read
        public IActionResult Read(int Id)
        {
            ActividadMejora actividadMejora = new ActividadMejora { Id = Id };
            Respuesta respuesta = GetActividadMejora(actividadMejora);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/actividadmejora/update
        public IActionResult Update(ActividadMejora actividadMejora)
        {
            var read = GetActividadMejora(actividadMejora);
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
                    OracleCommand command = new OracleCommand("PCK_actividad_mejora.SP_UPDATE_actividad_mejora", connection);
                    command.Parameters.Add("id", actividadMejora.Id);
                    command.Parameters.Add("nombre", actividadMejora.Nombre);
                    command.Parameters.Add("descripcion", actividadMejora.Descripcion);
                    command.Parameters.Add("situacion", actividadMejora.Situacion);
                    command.Parameters.Add("comentario", actividadMejora.Comentario);
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
        // api/empresa/Delete
        public IActionResult Delete(int Id)
        {
            ActividadMejora actividadMejora = new ActividadMejora();
            actividadMejora.Id = Id;
            var read = GetActividadMejora(actividadMejora);
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
                    OracleCommand command = new OracleCommand("PCK_actividad_mejora.SP_DELETE_actividad_mejora", connection);
                    command.Parameters.Add("id", actividadMejora.Id);
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

        public Respuesta GetActividadMejora(ActividadMejora actividadMejora)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from actividad_mejora where id_actividad_mejora = " + actividadMejora.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    ActividadMejora result = new ActividadMejora();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Nombre = reader.GetString(1);
                            result.Descripcion = reader.GetString(2);
                            result.Situacion = (int)reader.GetInt64(3);
                            result.Comentario = reader.GetString(4);
                            result.ServicioId = (int)reader.GetInt64(5);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró la actividad";
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
        [Route("FinishMejora")]
        // api/servicio/finishmejora
        public IActionResult FinishMejora(ActividadMejora mejora)
        {
            var read = GetActividadMejora(mejora);
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
                    OracleCommand command = new OracleCommand("PCK_ACTIVIDAD_MEJORA.SP_FINISH_ACTIVIDAD_MEJORA", connection);
                    command.Parameters.Add("id", mejora.Id);
                    command.Parameters.Add("situacion", mejora.Situacion);
                    command.Parameters.Add("comentario", mejora.Comentario);
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
    }
}
