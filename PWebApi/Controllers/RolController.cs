using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PWebApi.Models;
using PWebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;

namespace PWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {

        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/rol/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from Rol", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Rol> result = new List<Rol>();
                    while (reader.Read())
                    {
                        Rol rol = new Rol();
                        rol.Id = (int)reader.GetInt64(0);
                        rol.Nombre = reader.GetString(1);
                        result.Add(rol);
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
        [HttpGet]
        [Route("Read/{Id}")]
        // api/rol/read
        public IActionResult Read(int Id)
        {
            Rol rol = new Rol { Id = Id };
            Respuesta respuesta = GetRol(rol);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
            
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        // api/rol/create
        public IActionResult Create(Rol rol)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_ROL.SP_CREATE_ROL", connection);
                    command.Parameters.Add("nombre", rol.Nombre);
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
        [HttpPut]
        [Route("Update")]
        // api/rol/update
        public IActionResult Update(Rol rol)
        {
            var read = GetRol(rol);
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
                    OracleCommand command = new OracleCommand("PCK_ROL.SP_UPDATE_ROL", connection);
                    command.Parameters.Add("id", rol.Id);
                    command.Parameters.Add("nombre", rol.Nombre);
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
        // api/rol/Delete
        public IActionResult Delete(int Id)
        {
            Rol rol = new Rol();
            rol.Id = Id;
            var read = GetRol(rol);
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
                    OracleCommand command = new OracleCommand("PCK_ROL.SP_DELETE_ROL", connection);
                    command.Parameters.Add("id", rol.Id);
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

        public Respuesta GetRol(Rol rol) {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from Rol where ID_ROL = " + rol.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    Rol result = new Rol();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Nombre = reader.GetString(1);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró el rol";
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
