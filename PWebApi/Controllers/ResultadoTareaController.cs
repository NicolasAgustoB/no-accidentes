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
    public class ResultadoTareaController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/resultadotarea/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from resultado_tarea", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<ResultadoTarea> result = new List<ResultadoTarea>();
                    while (reader.Read())
                    {
                        ResultadoTarea resultado = new ResultadoTarea();
                        resultado.Id = (int)reader.GetInt64(0);
                        resultado.Estado = (int)reader.GetInt64(1);
                        resultado.Comentario = reader.GetString(2);
                        resultado.ServicioId = (int)reader.GetInt64(3);
                        resultado.ContratoTareaId = (int)reader.GetInt64(4);
                        resultado.Nombre = reader.IsDBNull(5)? "N/A": reader.GetString(5);
                        result.Add(resultado);
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
        // api/resultadotarea/create
        public IActionResult Create(ResultadoTarea resultado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_RESULTADO_TAREA.SP_CREATE_RESULTADO_TAREA", connection);
                    command.Parameters.Add("servicioId", resultado.ServicioId);
                    command.Parameters.Add("contratoTareaId", resultado.ContratoTareaId);
                    command.Parameters.Add("nombre", resultado.Nombre);
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
        // api/resultadotarea/read
        public IActionResult Read(int Id)
        {
            ResultadoTarea resultado = new ResultadoTarea {Id=Id};
            Respuesta respuesta = GetResultadoTarea(resultado);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);

        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/resultadotarea/update
        public IActionResult Update(ResultadoTarea resultado)
        {
            var read = GetResultadoTarea(resultado);
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
                    OracleCommand command = new OracleCommand("PCK_RESULTADO_TAREA.SP_UPDATE_RESULTADO_TAREA", connection);
                    command.Parameters.Add("id", resultado.Id);
                    command.Parameters.Add("estado", resultado.Estado);
                    command.Parameters.Add("comentario", resultado.Comentario);
                    command.Parameters.Add("servicioId", resultado.ServicioId);
                    command.Parameters.Add("contratoTareaId", resultado.ContratoTareaId);
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
        // api/resultadotarea/Delete
        public IActionResult Delete(int Id)
        {
            ResultadoTarea resultado = new ResultadoTarea();
            resultado.Id = Id;
            var read = GetResultadoTarea(resultado);
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
                    OracleCommand command = new OracleCommand("PCK_RESULTADO_TAREA.SP_DELETE_RESULTADO_TAREA", connection);
                    command.Parameters.Add("id", resultado.Id);
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

        public Respuesta GetResultadoTarea(ResultadoTarea resultado)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from resultado_tarea where ID_RES_TAREA = " + resultado.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    ResultadoTarea result = new ResultadoTarea();                   
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Estado = (int)reader.GetInt64(1);
                            result.Comentario = reader.GetString(2);
                            result.ServicioId = (int)reader.GetInt64(3);
                            result.ContratoTareaId = (int)reader.GetInt64(4);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró el resultado de la tarea";
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
        [HttpGet]
        [Route("ReadAllT")]
        // api/resultadotarea/readall
        public IActionResult ReadAllT()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("SELECT\r\n    rt.id_res_tarea,\r\n    t.nombre,\r\n    rt.estado,\r\n    rt.comentario,\r\n    rt.servicio_id_servicio\r\nFROM\r\n         resultado_tarea rt\r\n    INNER JOIN contrato_tarea ct ON ct.id_cto_tarea = rt.contrato_tarea_id_cto_tarea\r\n    INNER JOIN tarea          t ON ct.tarea_id_tarea = t.id_tarea", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<ResultadoTarea> result = new List<ResultadoTarea>();
                    while (reader.Read())
                    {
                        ResultadoTarea resultado = new ResultadoTarea();
                        resultado.Id = (int)reader.GetInt64(0);
                        resultado.Nombre= reader.GetString(1);
                        resultado.Estado = (int)reader.GetInt64(2);
                        resultado.Comentario = reader.GetString(3);
                        resultado.ServicioId = (int)reader.GetInt64(4);
                        result.Add(resultado);
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
        [HttpPut]
        [Route("UpdateProfesional")]
        // api/resultadotarea/updateprofesional
        public IActionResult UpdateProfesional(ResultadoTarea resultado)
        {
            var read = GetResultadoTarea(resultado);
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
                    OracleCommand command = new OracleCommand("PCK_RESULTADO_TAREA.SP_UPDATE_RESULTADO_TAREA_PROFESIONAL", connection);
                    command.Parameters.Add("id", resultado.Id);
                    command.Parameters.Add("estado", resultado.Estado);
                    command.Parameters.Add("comentario", resultado.Comentario);
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
