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
    public class PagoController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/pago/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from pago", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Pago> result = new List<Pago>();
                    while (reader.Read())
                    {
                        Pago pago = new Pago();
                        pago.Id = (int)reader.GetInt64(0);

                        var fecha = reader.IsDBNull(1) ? "01/01/01" : reader.GetString(1);
                        pago.Fecha = DateTime.ParseExact(fecha, "dd/MM/yy", null);

                        var fechaPago = reader.IsDBNull(2) ? "01/01/01" : reader.GetString(2);
                        pago.FechaPago = DateTime.ParseExact(fechaPago, "dd/MM/yy", null);

                        var fechaLimite = reader.IsDBNull(3) ? "01/01/01" : reader.GetString(3);
                        pago.FechaLimite = DateTime.ParseExact(fechaLimite, "dd/MM/yy", null);

                        pago.MetodoPago = reader.GetString(4);
                        pago.EstadoPago = (int)reader.GetInt64(5);
                        pago.MontoTotal = (int)reader.GetInt64(6);
                        pago.ContratoId = (int)reader.GetInt64(7);
                        result.Add(pago);
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
        // api/pago/create
        public IActionResult Create(Pago pago)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_PAGO.SP_CREATE_PAGO", connection);
                    command.Parameters.Add("fechaLimite", pago.FechaLimite);
                    command.Parameters.Add("montoTotal", pago.MontoTotal);
                    command.Parameters.Add("contratoId", pago.ContratoId);
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
            Pago pago = new Pago { Id = Id };
            Respuesta respuesta = GetPago(pago);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/pago/update
        public IActionResult Update(Pago pago)
        {
            var read = GetPago(pago);
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
                    OracleCommand command = new OracleCommand("PCK_PAGO.SP_UPDATE_PAGO", connection);
                    command.Parameters.Add("id", pago.Id);
                    command.Parameters.Add("fechaPago", pago.FechaPago);
                    command.Parameters.Add("fechaLimite", pago.FechaLimite);
                    command.Parameters.Add("metodoPago", pago.MetodoPago);
                    command.Parameters.Add("estado", pago.EstadoPago);
                    command.Parameters.Add("montoTotal", pago.MontoTotal);
                    command.Parameters.Add("contratoId", pago.ContratoId);
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
        // api/pago/Delete
        public IActionResult Delete(int Id)
        {
            Pago pago = new Pago {Id = Id };
            var read = GetPago(pago);
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
                    OracleCommand command = new OracleCommand("PCK_PAGO.SP_DELETE_PAGO", connection);
                    command.Parameters.Add("id", pago.Id);
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

        

        public Respuesta GetPago(Pago pago)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from pago where ID_pago = " + pago.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    Pago result = new Pago();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);

                            var fecha = reader.IsDBNull(1) ? "01/01/01" : reader.GetString(1);
                            result.Fecha = DateTime.ParseExact(fecha, "dd/MM/yy", null);

                            var fechaPago = reader.IsDBNull(2) ? "01/01/01" : reader.GetString(2);
                            result.FechaPago = DateTime.ParseExact(fechaPago, "dd/MM/yy", null);

                            var fechaLimite = reader.IsDBNull(3) ? "01/01/01" : reader.GetString(3);
                            result.FechaLimite = DateTime.ParseExact(fechaLimite, "dd/MM/yy", null);

                            result.MetodoPago = reader.GetString(4);
                            result.EstadoPago = (int)reader.GetInt64(5);
                            result.MontoTotal = (int)reader.GetInt64(6);
                            result.ContratoId = (int)reader.GetInt64(7);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró el pago";
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
        [Route("ConfirmarPago")]
        // api/pago/ConfirmarPago
        public IActionResult ConfirmarPago(Pago pago)
        {
            var read = GetPago(pago);
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
                    OracleCommand command = new OracleCommand("PCK_PAGO.SP_CONFIRMAR_PAGO", connection);
                    command.Parameters.Add("id", pago.Id);
                    command.Parameters.Add("metodoPago", pago.MetodoPago);

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
