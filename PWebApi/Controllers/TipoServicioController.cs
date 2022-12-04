using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PWebApi.Models;
using PWebApi.Models.Response;
using PWebApi.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace PWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoServicioController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/tiposervicio/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from TIPO_SERVICIO", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<TipoServicio> result = new List<TipoServicio>();
                    while (reader.Read())
                    {
                        TipoServicio tipo = new TipoServicio();
                        tipo.Id = (int)reader.GetInt64(0);
                        tipo.Nombre = reader.GetString(1);
                        tipo.Valor = (int)reader.GetInt64(2);
                        tipo.Estado = (int)reader.GetInt64(3);
                        tipo.Descripcion = reader.GetString(4);
                        result.Add(tipo);
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
        // api/tiposervicio/create
        public IActionResult Create(TipoServicio tipo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_TIPO_SERVICIO.SP_CREATE_TIPO_SERVICIO", connection);
                    command.Parameters.Add("nombre", tipo.Nombre);
                    command.Parameters.Add("valor", tipo.Valor);
                    command.Parameters.Add("descripcion", tipo.Descripcion);
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
            TipoServicio tipo = new TipoServicio { Id = Id };
            Respuesta respuesta = GetTipoServicio(tipo);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/tiposervicio/update
        public IActionResult Update(TipoServicio tipo)
        {
            var read = GetTipoServicio(tipo);
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
                    OracleCommand command = new OracleCommand("PCK_TIPO_SERVICIO.SP_UPDATE_TIPO_SERVICIO", connection);

                    command.Parameters.Add("id", tipo.Id);
                    command.Parameters.Add("nombre", tipo.Nombre);
                    command.Parameters.Add("valor", tipo.Valor);
                    command.Parameters.Add("estado", tipo.Estado);
                    command.Parameters.Add("descripcion", tipo.Descripcion);
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
        [Route("Delete")]
        // api/tiposervicio/delete
        public IActionResult Delete(TipoServicio tipo)
        {
            var read = GetTipoServicio(tipo);
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
                    OracleCommand command = new OracleCommand("PCK_TIPO_SERVICIO.SP_DELETE_TIPO_SERVICIO", connection);
                    command.Parameters.Add("id", tipo.Id);
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

        [Authorize]
        [HttpDelete]
        [Route("Delete/{Id}")]
        // api/tiposervicio/delete
        public IActionResult DeleteId(int Id)
        {
            TipoServicio tipo = new TipoServicio {Id = Id};
            var read = GetTipoServicio(tipo);
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
                    OracleCommand command = new OracleCommand("PCK_TIPO_SERVICIO.SP_DELETE_TIPO_SERVICIO", connection);
                    command.Parameters.Add("id", tipo.Id);
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

        public Respuesta GetTipoServicio(TipoServicio tipo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from tipo_servicio where ID_TIPO_SERV = " + tipo.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    TipoServicio result = new TipoServicio();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Nombre = reader.GetString(1);
                            result.Valor = (int)reader.GetInt64(2);
                            result.Estado = (int)reader.GetInt64(3);
                            result.Descripcion = reader.GetString(4);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró el tipo de servicio";
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

