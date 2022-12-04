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
    public class SucursalController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/sucursal/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from sucursal", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Sucursal> result = new List<Sucursal>();
                    while (reader.Read())
                    {
                        Sucursal sucursal = new Sucursal();
                        sucursal.Id = (int)reader.GetInt64(0);
                        sucursal.Nombre = reader.GetString(1);
                        sucursal.Trabajadores = (int)reader.GetInt64(2);
                        sucursal.Direccion = reader.GetString(3);
                        sucursal.Telefono = reader.GetString(4);
                        sucursal.EmpresaId = (int)reader.GetInt64(5);
                        sucursal.Estado = (int)reader.GetInt64(6);
                        result.Add(sucursal);
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
        // api/sucursal/create
        public IActionResult Create(Sucursal sucursal)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_SUCURSAL.SP_CREATE_SUCURSAL", connection);
                    command.Parameters.Add("nombre", sucursal.Nombre);
                    command.Parameters.Add("trabajadores", sucursal.Trabajadores);
                    command.Parameters.Add("direccion", sucursal.Direccion);
                    command.Parameters.Add("telefono", sucursal.Telefono);
                    command.Parameters.Add("empresaId", sucursal.EmpresaId);
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
            Sucursal sucursal = new Sucursal { Id = Id };
            Respuesta respuesta = GetSucursal(sucursal);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/sucursal/update
        public IActionResult Update(Sucursal sucursal)
        {
            var read = GetSucursal(sucursal);
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
                    OracleCommand command = new OracleCommand("PCK_SUCURSAL.SP_UPDATE_SUCURSAL", connection);
                    command.Parameters.Add("id", sucursal.Id);
                    command.Parameters.Add("nombre", sucursal.Nombre);
                    command.Parameters.Add("trabajadores", sucursal.Trabajadores);
                    command.Parameters.Add("direccion", sucursal.Direccion);
                    command.Parameters.Add("telefono", sucursal.Telefono);
                    command.Parameters.Add("empresaId", sucursal.EmpresaId);
                    command.Parameters.Add("estado", sucursal.Estado);
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
        // api/sucursal/Delete
        public IActionResult Delete(int Id)
        {
            Sucursal sucursal = new Sucursal();
            sucursal.Id = Id;
            var read = GetSucursal(sucursal);
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
                    OracleCommand command = new OracleCommand("PCK_SUCURSAL.SP_DELETE_SUCURSAL", connection);
                    command.Parameters.Add("id", sucursal.Id);
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

        public Respuesta GetSucursal(Sucursal sucursal)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from sucursal where ID_SUCURSAL = " + sucursal.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    Sucursal result = new Sucursal();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Nombre = reader.GetString(1);
                            result.Trabajadores = (int)reader.GetInt64(2);
                            result.Direccion = reader.GetString(3);
                            result.Telefono = reader.GetString(4);
                            result.EmpresaId = (int)reader.GetInt64(5);
                            result.Estado = (int)reader.GetInt64(6);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró la sucursal";
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
