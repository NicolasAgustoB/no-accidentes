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
    public class AccidenteController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/accidente/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from accidente", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Accidente> result = new List<Accidente>();
                    while (reader.Read())
                    {
                        Accidente accidente = new Accidente();
                        accidente.Id = (int)reader.GetInt64(0);
                        accidente.Estado = (int)reader.GetInt64(1);
                        accidente.Tipo = reader.GetString(2);
                        accidente.Descripcion = reader.GetString(3);

                        var fecha = reader.IsDBNull(4) ? "01/01/01 00:00" : reader.GetString(4);
                        accidente.Fecha = DateTime.Parse(fecha);

                        accidente.Accidentados = (int)reader.GetInt64(5);
                        accidente.Comentario = reader.GetString(6);
                        accidente.EmpleadoId = reader.IsDBNull(7) ? 0 : (int)reader.GetInt64(7);
                        accidente.ClienteId = (int)reader.GetInt64(8);

                        result.Add(accidente);
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
        // api/accidente/create
        public IActionResult Create(Accidente accidente)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_ACCIDENTE.SP_CREATE_ACCIDENTE", connection);
                    command.Parameters.Add("tipo", accidente.Tipo);
                    command.Parameters.Add("descripcion", accidente.Descripcion);
                    command.Parameters.Add("accidentados", accidente.Accidentados);
                    command.Parameters.Add("clienteId", accidente.ClienteId);
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
        // api/accidente/read
        public IActionResult Read(int Id)
        {
            Accidente accidente = new Accidente { Id = Id };
            Respuesta respuesta = GetAccidente(accidente);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/accidente/update
        public IActionResult Update(Accidente accidente)
        {
            var read = GetAccidente(accidente);
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
                    OracleCommand command = new OracleCommand("PCK_accidente.SP_UPDATE_accidente", connection);
                    command.Parameters.Add("id", accidente.Id);
                    command.Parameters.Add("estado", accidente.Estado);
                    command.Parameters.Add("comentario", accidente.Comentario);
                    command.Parameters.Add("empleadoId", accidente.EmpleadoId);
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
        // api/accidente/Delete
        public IActionResult Delete(int Id)
        {
            Accidente accidente = new Accidente();
            accidente.Id = Id;
            var read = GetAccidente(accidente);
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
                    OracleCommand command = new OracleCommand("PCK_accidente.SP_DELETE_accidente", connection);
                    command.Parameters.Add("id", accidente.Id);
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

        public Respuesta GetAccidente(Accidente accidente)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from accidente where ID_accidente = " + accidente.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    Accidente result = new Accidente();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Estado = (int)reader.GetInt64(1);
                            result.Tipo = reader.GetString(2);
                            result.Descripcion = reader.GetString(3);

                            var fecha = reader.IsDBNull(4) ? "01/01/01" : reader.GetString(4);
                            result.Fecha = DateTime.Parse(fecha);

                            result.Accidentados = (int)reader.GetInt64(5);
                            result.Comentario = reader.GetString(6);
                            result.EmpleadoId = reader.IsDBNull(7) ? 0 : (int)reader.GetInt64(7);
                            result.ClienteId = (int)reader.GetInt64(8);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró el accidente";
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
        [Route("FinishAccidente")]
        // api/accidente/finishAccidente
        public IActionResult FinishAccidente(Accidente accidente)
        {
            var read = GetAccidente(accidente);
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
                    OracleCommand command = new OracleCommand("PCK_ACCIDENTE.SP_FINISH_ACCIDENTE", connection);
                    command.Parameters.Add("id", accidente.Id);
                    command.Parameters.Add("comentario", accidente.Comentario);
                    command.Parameters.Add("empleadoId", accidente.EmpleadoId);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    respuesta.Mensaje = "Respuesta enviada correctamente";
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
