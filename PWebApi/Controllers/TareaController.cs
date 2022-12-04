using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PWebApi.Models;
using PWebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace PWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/tarea/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from tarea", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Tarea> result = new List<Tarea>();
                    while (reader.Read())
                    {
                        Tarea tarea = new Tarea();
                        tarea.Id = (int)reader.GetInt64(0);
                        tarea.Nombre = reader.GetString(1);
                        tarea.Descripcion = reader.GetString(2);
                        tarea.Estado = (int)reader.GetInt64(3);
                        result.Add(tarea);
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
        // api/tarea/create
        public IActionResult Create(Tarea tarea)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_TAREA.SP_CREATE_TAREA", connection);
                    command.Parameters.Add("nombre", tarea.Nombre);
                    command.Parameters.Add("descripcion", tarea.Descripcion);
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
            Tarea tarea = new Tarea { Id = Id };
            Respuesta respuesta = GetTarea(tarea);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/tarea/update
        public IActionResult Update(Tarea tarea)
        {
            var read = GetTarea(tarea);
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
                    OracleCommand command = new OracleCommand("PCK_TAREA.SP_UPDATE_TAREA", connection);
                    command.Parameters.Add("id", tarea.Id);
                    command.Parameters.Add("nombre", tarea.Nombre);
                    command.Parameters.Add("descripcion", tarea.Descripcion);
                    command.Parameters.Add("estado", tarea.Estado);
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
        // api/tarea/Delete
        public IActionResult Delete(int Id)
        {
            Tarea tarea = new Tarea{Id=Id};
            var read = GetTarea(tarea);
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
                    OracleCommand command = new OracleCommand("PCK_TAREA.SP_DELETE_TAREA", connection);
                    command.Parameters.Add("id", tarea.Id);
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

        public Respuesta GetTarea(Tarea tarea)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from tarea where ID_tarea = " + tarea.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    Tarea result = new Tarea();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Nombre = reader.GetString(1);
                            result.Descripcion = reader.GetString(2);
                            result.Estado = (int)reader.GetInt64(3);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró la tarea";
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
