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
    public class TipoSolicitudController : ControllerBase
    {

        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/tiposolicitud/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from TIPO_SOLICITUD", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<TipoSolicitud> result = new List<TipoSolicitud>();
                    while (reader.Read())
                    {
                        TipoSolicitud tipo = new TipoSolicitud();
                        tipo.Id = (int)reader.GetInt64(0);
                        tipo.Nombre = reader.GetString(1);
                        tipo.Estado = (int)reader.GetInt64(2);
                        tipo.Descripcion = reader.GetString(3);
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
        public IActionResult Create(TipoSolicitud tipo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_TIPO_SOLICITUD.SP_CREATE_TIPO_SOLICITUD", connection);

                    command.Parameters.Add("nombre", tipo.Nombre);
                    command.Parameters.Add("descripcion", tipo.Descripcion);

                    command.CommandType = CommandType.StoredProcedure;

                    var filas = command.ExecuteNonQuery();

                    if (filas == 0)
                    {
                        respuesta.Mensaje = "Error al crear";
                        return BadRequest(respuesta);
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
        public IActionResult Read(int Id)
        {
            TipoSolicitud tipo = new TipoSolicitud { Id = Id };
            Respuesta respuesta = GetTipoSolicitud(tipo);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/tiposolicitud/update
        public IActionResult Update(TipoSolicitud tipo)
        {
            var read = GetTipoSolicitud(tipo);
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
                    OracleCommand command = new OracleCommand("PCK_TIPO_SOLICITUD.SP_UPDATE_TIPO_SOLICITUD", connection);

                    command.Parameters.Add("id", tipo.Id);
                    command.Parameters.Add("nombre", tipo.Nombre);
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
        [Route("Delete/{Id}")]
        // api/tiposolicitud/delete
        public IActionResult Delete(int Id)
        {
            TipoSolicitud tipo = new TipoSolicitud{Id=Id};
            var read = GetTipoSolicitud(tipo);
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
                    OracleCommand command = new OracleCommand("PCK_TIPO_SOLICITUD.SP_DELETE_TIPO_SOLICITUD", connection);
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

        public Respuesta GetTipoSolicitud(TipoSolicitud tipo)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from tipo_solicitud where ID_TIPO_SOL = " + tipo.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    TipoSolicitud result = new TipoSolicitud();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Nombre = reader.GetString(1);
                            result.Estado = (int)reader.GetInt64(2);
                            result.Descripcion = reader.GetString(3);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró el tipo de solicitud";
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
