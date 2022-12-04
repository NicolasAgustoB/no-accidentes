using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Oracle.ManagedDataAccess.Client;
using PWebApi.Models;
using PWebApi.Models.Response;
using PWebApi.Models.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;

namespace PWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // api/servicio/readall
        public IActionResult ReadAll()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from servicio", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Servicio> result = new List<Servicio>();
                    while (reader.Read())
                    {
                        Servicio servicio = new Servicio();
                        servicio.Id = (int)reader.GetInt64(0);
                        servicio.Estado = (int)reader.GetInt64(1);
                        servicio.HoraInicio = DateTime.ParseExact(reader.GetString(2), "HH:mm", null);
                        servicio.HoraTermino = DateTime.ParseExact(reader.GetString(3), "HH:mm", null);
                        servicio.Fecha = DateTime.ParseExact(reader.GetString(4), "dd/MM/yy", null);
                        servicio.Adicional = (int)reader.GetInt64(5);
                        servicio.Descripcion = reader.GetString(6);
                        servicio.Informe = reader.GetString(7);
                        servicio.Comentario = reader.GetString(8);
                        servicio.Asistentes = (int)reader.GetInt64(9);
                        servicio.Material = reader.GetString(10);
                        servicio.TipoServicioId = (int)reader.GetInt64(11);
                        servicio.ClienteId = (int)reader.GetInt64(12);
                        servicio.EmpleadoId = (int)reader.GetInt64(13);
                        servicio.PagoId = reader.IsDBNull(14) ? 0 : (int)reader.GetInt64(14);
                        result.Add(servicio);
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
        // api/servicio/create
        public IActionResult Create(Servicio servicio)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_SERVICIO.SP_CREATE_SERVICIO", connection);
                    var horaInicioFix = servicio.HoraInicio.ToString("HH:mm");
                    var horaTerminoFix = servicio.HoraTermino.ToString("HH:mm");
                    command.Parameters.Add("horaInicio", horaInicioFix);
                    command.Parameters.Add("horaTermino", horaTerminoFix);
                    command.Parameters.Add("fecha",servicio.Fecha);
                    command.Parameters.Add("adicional", servicio.Adicional);
                    command.Parameters.Add("descripcion", servicio.Descripcion);
                    command.Parameters.Add("asistentes", servicio.Asistentes);
                    command.Parameters.Add("material", servicio.Material);
                    command.Parameters.Add("tipoServicioId", servicio.TipoServicioId);
                    command.Parameters.Add("clienteId", servicio.ClienteId);
                    command.Parameters.Add("empleadoId", servicio.EmpleadoId);
                    command.Parameters.Add("id", OracleDbType.Int32, size: 10);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    var idServicio = command.Parameters["id"].Value.ToString();
                    foreach (var item in servicio.ResultadoTareas )
                    {
                        OracleCommand commandTarea = new OracleCommand("PCK_RESULTADO_TAREA.SP_CREATE_RESULTADO_TAREA", connection);
                        commandTarea.Parameters.Add("servicioId", idServicio);
                        commandTarea.Parameters.Add("contratoTareaId", item.ContratoTareaId);
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
        // api/servicio/read
        public IActionResult Read(int Id)
        {
            Servicio servicio = new Servicio { Id = Id };
            Respuesta respuesta = GetServicio(servicio);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/servicio/update
        public IActionResult Update(Servicio servicio)
        {
            var read = GetServicio(servicio);
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
                    OracleCommand command = new OracleCommand("PCK_SERVICIO.SP_UPDATE_SERVICIO", connection);
                    var horaInicioFix = servicio.HoraInicio.ToString("HH:mm");
                    var horaTerminoFix = servicio.HoraTermino.ToString("HH:mm");
                    command.Parameters.Add("id", servicio.Id);
                    command.Parameters.Add("horaInicio", horaInicioFix);
                    command.Parameters.Add("horaTermino", horaTerminoFix);
                    command.Parameters.Add("fecha", servicio.Fecha);
                    command.Parameters.Add("adicional", servicio.Adicional);
                    command.Parameters.Add("descripcion", servicio.Descripcion);
                    command.Parameters.Add("asistentes", servicio.Asistentes);
                    command.Parameters.Add("material", servicio.Material);
                    command.Parameters.Add("tipoServicioId", servicio.TipoServicioId);
                    command.Parameters.Add("empleadoId", servicio.EmpleadoId);
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
        // api/servicio/Delete
        public IActionResult Delete(int Id)
        {
            Servicio servicio = new Servicio();
            servicio.Id = Id;
            var read = GetServicio(servicio);
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
                    OracleCommand command = new OracleCommand("PCK_SERVICIO.SP_DELETE_SERVICIO", connection);
                    command.Parameters.Add("id", servicio.Id);
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
        public Respuesta GetServicio(Servicio servicio)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from servicio where ID_SERVICIO = " + servicio.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    Servicio result = new Servicio();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Estado = (int)reader.GetInt64(1);
                            result.HoraInicio = DateTime.ParseExact(reader.GetString(2), "HH:mm", null);
                            result.HoraTermino = DateTime.ParseExact(reader.GetString(3), "HH:mm", null);
                            result.Fecha = DateTime.ParseExact(reader.GetString(4), "dd/MM/yy", null);
                            result.Adicional = (int)reader.GetInt64(5);
                            result.Descripcion = reader.GetString(6);
                            result.Informe = reader.GetString(7);
                            result.Comentario = reader.GetString(8);
                            result.Asistentes = (int)reader.GetInt64(9);
                            result.Material = reader.GetString(10);
                            result.TipoServicioId = (int)reader.GetInt64(11);
                            result.ClienteId = (int)reader.GetInt64(12);
                            result.EmpleadoId = (int)reader.GetInt64(13);
                            result.PagoId = reader.IsDBNull(14) ? 0 : (int)reader.GetInt64(14);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró el servicio";
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
        [Route("ReadAll/Proximos")]
        // api/servicio/readall
        public IActionResult ReadAllProximos()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    var select = "SELECT s.id_servicio, \r\n    ts.nombre,\r\n    ue.nombre ||' '|| ue.apellidos as empleado,\r\n    e.nombre as empresa,\r\n    s.fecha,\r\n    s.estado_servicio,\r\n    s.adicional, s.hora_inicio, s.hora_termino, ue.id_usuario, uc.id_usuario, uc.nombre, e.id_empresa, suc.nombre , suc.id_sucursal \r\nFROM\r\n         servicio s\r\n    INNER JOIN tipo_servicio ts ON s.tipo_servicio_id_tipo_serv = ts.id_tipo_serv\r\n    INNER JOIN usuario       ue ON s.usuario_id_empleado = ue.id_usuario\r\n    INNER JOIN usuario       uc ON s.usuario_id_cliente = uc.id_usuario\r\n    INNER JOIN sucursal      suc  ON uc.sucursal_id_sucursal = suc.id_sucursal\r\n    INNER JOIN empresa       e ON suc.empresa_id_empresa = e.id_empresa\r\nWHERE\r\n    s.fecha > sysdate";
                    OracleCommand command = new OracleCommand(select, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<ServiciosTablasVM> result = new List<ServiciosTablasVM>();
                    while (reader.Read())
                    {
                        ServiciosTablasVM servicio = new ServiciosTablasVM();
                        servicio.Id = (int)reader.GetInt64(0);
                        servicio.TipoServicio = reader.GetString(1);
                        servicio.EmpleadoNombre = reader.GetString(2);
                        servicio.EmpresaNombre = reader.GetString(3);
                        var fecha = DateTime.ParseExact(reader.GetString(4), "dd/MM/yy", null).ToShortDateString();
                        servicio.Fecha = fecha;
                        servicio.EstadoServicio = (int)reader.GetInt64(5);
                        servicio.EstadoServicioStr = servicio.EstadoServicio == 0 ? "Pendiente" : servicio.EstadoServicio == 1 ? "Realizado" : servicio.EstadoServicio == 2 ? "Cancelado" : "Atrasado";
                        servicio.Adicional = (int)reader.GetInt64(6);
                        servicio.HoraInicio = reader.GetString(7);
                        servicio.HoraTermino = reader.GetString(8);
                        servicio.EmpleadoId = (int)reader.GetInt64(9);
                        servicio.ClienteId = (int)reader.GetInt64(10);
                        servicio.ClienteNombre = reader.GetString(11);
                        servicio.EmpresaId = (int)reader.GetInt64(12);
                        servicio.SucursalNombre = reader.GetString(13);
                        servicio.SucursalId = (int)reader.GetInt64(14);
                        result.Add(servicio);
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
        [Route("ReadAll/Atrasadas")]
        // api/servicio/readall/atrasadas
        public IActionResult ReadAllAtrasadas()
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    var select = "SELECT s.id_servicio, \r\n    ts.nombre,\r\n    ue.nombre ||' '|| ue.apellidos as empleado,\r\n    e.nombre as empresa,\r\n    s.fecha,\r\n    s.estado_servicio,\r\n    s.adicional, s.hora_inicio, s.hora_termino, ue.id_usuario, uc.id_usuario, uc.nombre, e.id_empresa, suc.nombre , suc.id_sucursal \r\nFROM\r\n         servicio s\r\n    INNER JOIN tipo_servicio ts ON s.tipo_servicio_id_tipo_serv = ts.id_tipo_serv\r\n    INNER JOIN usuario       ue ON s.usuario_id_empleado = ue.id_usuario\r\n    INNER JOIN usuario       uc ON s.usuario_id_cliente = uc.id_usuario\r\n     INNER JOIN usuario       uc ON s.usuario_id_cliente = uc.id_usuario\r\n    INNER JOIN sucursal      suc  ON uc.sucursal_id_sucursal = suc.id_sucursal\r\n    INNER JOIN empresa       e ON suc.empresa_id_empresa = e.id_empresa\r\nWHERE\r\n    s.fecha < sysdate and s.estado_servicio = 0";
                    OracleCommand command = new OracleCommand(select, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<ServiciosTablasVM> result = new List<ServiciosTablasVM>();
                    while (reader.Read())
                    {
                        ServiciosTablasVM servicio = new ServiciosTablasVM();
                        servicio.Id = (int)reader.GetInt64(0);
                        servicio.TipoServicio = reader.GetString(1);
                        servicio.EmpleadoNombre = reader.GetString(2);
                        servicio.EmpresaNombre = reader.GetString(3);
                        var fecha = DateTime.ParseExact(reader.GetString(4), "dd/MM/yy", null).ToShortDateString();
                        servicio.Fecha = fecha;
                        servicio.EstadoServicio = (int)reader.GetInt64(5);
                        servicio.Adicional = (int)reader.GetInt64(6);
                        servicio.HoraInicio = reader.GetString(7);
                        servicio.HoraTermino = reader.GetString(8);
                        servicio.EmpleadoId = (int)reader.GetInt64(9);
                        servicio.ClienteId = (int)reader.GetInt64(10);
                        servicio.ClienteNombre = reader.GetString(11);
                        servicio.EmpresaId = (int)reader.GetInt64(12);
                        servicio.SucursalNombre = reader.GetString(13);
                        servicio.SucursalId = (int)reader.GetInt64(14);
                        result.Add(servicio);
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
        [Route("UpdateAdmin")]
        // api/servicio/updateadmin
        public IActionResult UpdateAdmin(Servicio servicio)
        {
            var read = GetServicio(servicio);
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
                    OracleCommand command = new OracleCommand("PCK_SERVICIO.SP_UPDATE_SERVICIO_ADMIN", connection);
                    var horaInicioFix = servicio.HoraInicio.ToString("HH:mm");
                    var horaTerminoFix = servicio.HoraTermino.ToString("HH:mm");
                    command.Parameters.Add("id", servicio.Id);
                    command.Parameters.Add("horaInicio", horaInicioFix);
                    command.Parameters.Add("horaTermino", horaTerminoFix);
                    command.Parameters.Add("fecha", servicio.Fecha);
                    command.Parameters.Add("adicional", servicio.Adicional);
                    command.Parameters.Add("descripcion", servicio.Descripcion);
                    command.Parameters.Add("asistentes", servicio.Asistentes);
                    command.Parameters.Add("material", servicio.Material);
                    command.Parameters.Add("tipoServicioId", servicio.TipoServicioId);
                    command.Parameters.Add("empleadoId", servicio.EmpleadoId);
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
        [HttpPut]
        [Route("FinishServicio")]
        // api/servicio/finishservicio
        public IActionResult FinishServicio(Servicio servicio)
        {
            var read = GetServicio(servicio);
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
                    OracleCommand command = new OracleCommand("PCK_SERVICIO.SP_FINISH_SERVICIO", connection);
                    command.Parameters.Add("id", servicio.Id);
                    command.Parameters.Add("informe", servicio.Informe);
                    command.Parameters.Add("comentario", servicio.Comentario);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    if (servicio.TipoServicioId == 1)
                    {
                        foreach (var item in servicio.ResultadoTareas)
                        {
                            OracleCommand commandResultado = new OracleCommand("PCK_RESULTADO_TAREA.SP_UPDATE_RESULTADO_TAREA_PROFESIONAL", connection);
                            commandResultado.Parameters.Add("id", item.Id);
                            commandResultado.Parameters.Add("estado", item.Estado);
                            commandResultado.Parameters.Add("comentario", item.Comentario == null? " ": item.Comentario);
                            commandResultado.CommandType = CommandType.StoredProcedure;
                            commandResultado.ExecuteNonQuery();
                        }
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


    }
}
