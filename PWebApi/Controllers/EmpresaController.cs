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
    public class EmpresaController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/empresa/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from empresa", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Empresa> result = new List<Empresa>();
                    while (reader.Read())
                    {
                        Empresa empresa = new Empresa();
                        empresa.Id = (int)reader.GetInt64(0);
                        empresa.Nombre = reader.GetString(1);
                        empresa.Rut = reader.GetString(2);
                        empresa.Rubro = reader.GetString(3);
                        empresa.Email = reader.GetString(4);
                        empresa.Direccion = reader.GetString(5);
                        empresa.Telefono = reader.GetString(6);
                        empresa.Estado = (int)reader.GetInt64(7);
                        result.Add(empresa);
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
        // api/empresa/create
        public IActionResult Create(Empresa empresa)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_EMPRESA.SP_CREATE_EMPRESA", connection);
                    command.Parameters.Add("nombre", empresa.Nombre);
                    command.Parameters.Add("rut", empresa.Rut);
                    command.Parameters.Add("rubro", empresa.Rubro);
                    command.Parameters.Add("email", empresa.Email);
                    command.Parameters.Add("direccion", empresa.Direccion);
                    command.Parameters.Add("telefono", empresa.Telefono);
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
        // api/empresa/read
        public IActionResult Read(int Id)
        {
            Empresa empresa = new Empresa {Id=Id};
            Respuesta respuesta = GetEmpresa(empresa);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);

        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/empresa/update
        public IActionResult Update(Empresa empresa)
        {
            var read = GetEmpresa(empresa);
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
                    OracleCommand command = new OracleCommand("PCK_EMPRESA.SP_UPDATE_EMPRESA", connection);
                    command.Parameters.Add("id", empresa.Id);
                    command.Parameters.Add("nombre", empresa.Nombre);
                    command.Parameters.Add("rut", empresa.Rut);
                    command.Parameters.Add("rubro", empresa.Rubro);
                    command.Parameters.Add("email", empresa.Email);
                    command.Parameters.Add("direccion", empresa.Direccion);
                    command.Parameters.Add("telefono", empresa.Telefono);
                    command.Parameters.Add("estado", empresa.Estado);
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
            Empresa empresa = new Empresa();
            empresa.Id = Id;
            var read = GetEmpresa(empresa);
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
                    OracleCommand command = new OracleCommand("PCK_EMPRESA.SP_DELETE_EMPRESA", connection);
                    command.Parameters.Add("id", empresa.Id);
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

        public Respuesta GetEmpresa(Empresa empresa)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from empresa where ID_EMPRESA = " + empresa.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    Empresa result = new Empresa();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Nombre = reader.GetString(1);
                            result.Rut = reader.GetString(2);
                            result.Rubro = reader.GetString(3);
                            result.Email = reader.GetString(4);
                            result.Direccion = reader.GetString(5);
                            result.Telefono = reader.GetString(6);
                            result.Estado = (int)reader.GetInt64(7);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró la empresa";
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
