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
    public class ContratoController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/contrato/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from contrato", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Contrato> result = new List<Contrato>();
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato();
                        contrato.Id = (int)reader.GetInt64(0);
                        contrato.Estado = (int)reader.GetInt64(1);
                        contrato.Cuerpo = reader.GetString(2);
                        contrato.Valor = (int)reader.GetInt64(3);

                        var fechaInicio = reader.IsDBNull(4) ? "01/01/01" : reader.GetString(4);
                        contrato.FechaInicio = DateTime.ParseExact(fechaInicio, "dd/MM/yy", null);

                        var fechaTermino = reader.IsDBNull(5) ? "01/01/01" : reader.GetString(5);
                        contrato.FechaTermino = DateTime.ParseExact(fechaTermino, "dd/MM/yy", null);

                        contrato.SucursalId = (int)reader.GetInt64(6);
                        result.Add(contrato);
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
        // api/contrato/create
        public IActionResult Create(Contrato contrato)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_CONTRATO.SP_CREATE_CONTRATO", connection);
                    command.Parameters.Add("cuerpo", contrato.Cuerpo);
                    command.Parameters.Add("valor", contrato.Valor);
                    command.Parameters.Add("sucursalId", contrato.SucursalId);
                    command.Parameters.Add("fechaTermino", contrato.FechaTermino);
                    command.Parameters.Add("id", OracleDbType.Int32, size: 10);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();                    
                    var idContrato = command.Parameters["id"].Value.ToString();
                    foreach (var item in contrato.ContratoTareas)
                    {
                        OracleCommand commandTarea = new OracleCommand("PCK_CONTRATO_TAREA.SP_CREATE_CONTRATO_TAREA", connection);
                        commandTarea.Parameters.Add("tareaId", item.TareaId);
                        commandTarea.Parameters.Add("contratoId", int.Parse(idContrato));
                        commandTarea.Parameters.Add("estado", item.Estado);
                        commandTarea.Parameters.Add("nombre", item.Nombre);
                        commandTarea.CommandType = CommandType.StoredProcedure;
                        commandTarea.ExecuteNonQuery();
                    }
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
        // api/contrato/read
        public IActionResult Read(int Id)
        {
            Contrato contrato = new Contrato { Id = Id };
            Respuesta respuesta = GetContrato(contrato);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/contrato/update
        public IActionResult Update(Contrato contrato)
        {
            var read = GetContrato(contrato);
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
                    OracleCommand commandContrato = new OracleCommand("PCK_CONTRATO.SP_UPDATE_CONTRATO", connection);
                    commandContrato.Parameters.Add("id", contrato.Id);
                    commandContrato.Parameters.Add("cuerpo", contrato.Cuerpo);
                    commandContrato.Parameters.Add("valor", contrato.Valor);
                    commandContrato.Parameters.Add("fechaTermino", contrato.FechaTermino);
                    commandContrato.CommandType = CommandType.StoredProcedure;
                    commandContrato.ExecuteNonQuery();
                    contrato.ContratoTareas.RemoveAll(t => t.ContratoId == contrato.Id);
                    foreach (var item in contrato.ContratoTareas)
                    {
                        OracleCommand command = new OracleCommand("PCK_CONTRATO_TAREA.SP_UPDATE_CONTRATO_TAREA_ESTADO", connection);
                        command.Parameters.Add("id", item.Id );
                        command.Parameters.Add("estado", item.Estado);
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                    }
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
        [HttpPut]
        [Route("Finalizar")]
        // api/contrato/finalizar
        public IActionResult Finalizar(Contrato contrato)
        {
            var read = GetContrato(contrato);
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
                    var fechaTermino = DateTime.Now.ToShortDateString();
                    OracleCommand commandContrato = new OracleCommand("PCK_CONTRATO.SP_FINALIZAR_CONTRATO", connection);
                    commandContrato.Parameters.Add("id", contrato.Id);
                    commandContrato.CommandType = CommandType.StoredProcedure;
                    commandContrato.ExecuteNonQuery();                   
                    respuesta.Mensaje = "Contrato Finalizado Correctamente";
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
            Contrato contrato = new Contrato();
            contrato.Id = Id;
            var read = GetContrato(contrato);
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
                    OracleCommand command = new OracleCommand("PCK_CONTRATO.SP_DELETE_CONTRATO", connection);
                    command.Parameters.Add("id", contrato.Id);
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
        public Respuesta GetContrato(Contrato contrato)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from contrato where ID_CONTRATO = " + contrato.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    Contrato result = new Contrato();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Estado = (int)reader.GetInt64(1);
                            result.Cuerpo = reader.GetString(2);
                            result.Valor = (int)reader.GetInt64(3);

                            var fechaInicio = reader.IsDBNull(4) ? "01/01/01" : reader.GetString(4);
                            result.FechaInicio = DateTime.ParseExact(fechaInicio, "dd/MM/yy", null);

                            var fechaTermino = reader.IsDBNull(5) ? "01/01/01" : reader.GetString(5);
                            result.FechaTermino = DateTime.ParseExact(fechaTermino, "dd/MM/yy", null);

                            result.SucursalId = (int)reader.GetInt64(6);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró el contrato";
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
