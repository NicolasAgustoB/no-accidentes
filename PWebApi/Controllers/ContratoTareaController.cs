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
    public class ContratoTareaController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/contratotarea/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from contrato_tarea", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<ContratoTarea> result = new List<ContratoTarea>();
                    while (reader.Read())
                    {
                        ContratoTarea contratoTarea = new ContratoTarea();
                        contratoTarea.Id = (int)reader.GetInt64(0);
                        contratoTarea.TareaId = (int)reader.GetInt64(1);
                        contratoTarea.ContratoId = (int)reader.GetInt64(2);
                        contratoTarea.Estado = (int)reader.GetInt64(3);
                        contratoTarea.Nombre = reader.IsDBNull(4)? "N/A": reader.GetString(4);
                        result.Add(contratoTarea);
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
        // api/contratotarea/create
        public IActionResult Create(ContratoTarea contratoTarea)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_CONTRATO_TAREA.SP_CREATE_CONTRATO_TAREA", connection);
                    command.Parameters.Add("tareaId", contratoTarea.TareaId);
                    command.Parameters.Add("estado", contratoTarea.Estado);
                    command.Parameters.Add("contratoId", contratoTarea.ContratoId);
                    command.Parameters.Add("nombre", contratoTarea.Nombre);
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
        // api/contratotarea/read
        public IActionResult Read(int Id)
        {
            ContratoTarea contratoTarea = new ContratoTarea { Id = Id };
            Respuesta respuesta = GetContratoTarea(contratoTarea);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);

        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/contratotarea/update
        public IActionResult Update(ContratoTarea contratoTarea)
        {
            var read = GetContratoTarea(contratoTarea);
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
                    OracleCommand command = new OracleCommand("PCK_CONTRATO_TAREA.SP_UPDATE_CONTRATO_TAREA", connection);
                    command.Parameters.Add("id", contratoTarea.Id);
                    command.Parameters.Add("tareaId", contratoTarea.TareaId);
                    command.Parameters.Add("contratoId", contratoTarea.ContratoId);
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
            ContratoTarea contratoTarea = new ContratoTarea();
            contratoTarea.Id = Id;
            var read = GetContratoTarea(contratoTarea);
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
                    OracleCommand command = new OracleCommand("PCK_CONTRATO_TAREA.SP_DELETE_CONTRATO_TAREA", connection);
                    command.Parameters.Add("id", contratoTarea.Id);
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

        public Respuesta GetContratoTarea(ContratoTarea contratoTarea)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from contrato_tarea where ID_CTO_TAREA = " + contratoTarea.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    ContratoTarea result = new ContratoTarea();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.TareaId = (int)reader.GetInt64(1);
                            result.ContratoId = (int)reader.GetInt64(2);
                            contratoTarea.Estado = (int)reader.GetInt64(3);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró la tarea del contrato";
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
        [Route("UpdateEstado")]
        // api/contratotarea/update
        public IActionResult UpdateEstado(ContratoTarea contratoTarea)
        {
            var read = GetContratoTarea(contratoTarea);
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
                    OracleCommand command = new OracleCommand("PCK_CONTRATO_TAREA.SP_UPDATE_CONTRATO_TAREA_ESTADO", connection);
                    command.Parameters.Add("id", contratoTarea.Id);
                    command.Parameters.Add("estado", contratoTarea.Estado);
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
