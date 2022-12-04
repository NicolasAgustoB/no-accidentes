using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using PWebApi.Models;
using PWebApi.Models.Response;
using PWebApi.Tools;
using PWebApi.Models.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authorization;
using PWebApi.Models.Request;
using PWebApi.Models.Services;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace PWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IUserService _userService;

        public UsuarioController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Autentificar")]
        // "api/usuario/Autentificar"
        public IActionResult Autentificar(Usuario usuario)
        {
            Respuesta respuesta = new Respuesta();
            var userResponse = _userService.Auth(usuario);
            if (userResponse == null)
            {
                respuesta.Mensaje = "Usuario o Contraseña incorrecto";
                return BadRequest(respuesta);
            }
            if (userResponse.Estado == 0)
            {
                respuesta.Mensaje = "Usuario Deshabilitado";
                return BadRequest(respuesta);
            }
            else if (userResponse.Estado == 2)
            {
                respuesta.Mensaje = "Usuario Deshabilitado Por Atraso";
                return BadRequest(respuesta);
            }
            respuesta.Exito = 1;
            respuesta.Data = userResponse;
            return Ok(respuesta);
        }
        [Authorize]
        [HttpGet]
        [Route("ReadAll")]
        // "api/usuario/readall"
        public IActionResult ReadAll() 
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from Usuario", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Usuario> result = new List<Usuario>();
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario();
                        usuario.Id = (int)reader.GetInt64(0);
                        usuario.Nombre = reader.GetString(1);
                        usuario.Apellidos = reader.GetString(2);
                        usuario.UserName = reader.GetString(3);
                        usuario.Email = reader.GetString(5);
                        usuario.Rut = reader.GetString(6);
                        usuario.Telefono = reader.GetString(7);
                        usuario.RolId = reader.GetInt32(8);
                        usuario.SucursalId = reader.GetInt32(9);
                        usuario.Estado = (int)reader.GetInt64(10);
                        result.Add(usuario);
                    }
                    respuesta.Exito = 1;
                    respuesta.Data = result;
                }
                catch(System.Exception ex)
                {
                    respuesta.Mensaje = ex.Message;
                    return BadRequest(respuesta);
                }
                if (respuesta.Exito == 0)
                {
                    return BadRequest(respuesta);
                }
                return Ok(respuesta);
            }
        }


        [Authorize]
        [HttpPost]
        [Route("Create")]
        // api/usuario/create
        public IActionResult Create(Usuario usuario)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_USUARIO.SP_CREATE_USUARIO", connection);
                    command.Parameters.Add("nombre", usuario.Nombre);
                    command.Parameters.Add("apellidos", usuario.Apellidos);
                    command.Parameters.Add("user_name", usuario.UserName);
                    command.Parameters.Add("password", Encrypt.GetSHA256(usuario.Password).ToString());
                    command.Parameters.Add("email", usuario.Email);
                    command.Parameters.Add("rut", usuario.Rut);
                    command.Parameters.Add("telefono", usuario.Telefono);
                    command.Parameters.Add("rolId", usuario.RolId);
                    command.Parameters.Add("sucursalId", usuario.SucursalId);
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
        // api/usuario/read
        public IActionResult Read(int Id)
        {
            Usuario usuario = new Usuario{Id = Id};
            Respuesta respuesta = GetUsuario(usuario);
            if (respuesta.Exito == 1)
            {
                return Ok(respuesta);
            }
            return BadRequest(respuesta);
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        // api/usuario/update
        public IActionResult Update(Usuario usuario)
        {
            var read = GetUsuario(usuario);
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
                    OracleCommand command = new OracleCommand("PCK_USUARIO.SP_UPDATE_USUARIO", connection);
                    command.Parameters.Add("id", usuario.Id);
                    command.Parameters.Add("nombre", usuario.Nombre);
                    command.Parameters.Add("apellidos", usuario.Apellidos);
                    command.Parameters.Add("user_name", usuario.UserName);
                    command.Parameters.Add("email", usuario.Email);
                    command.Parameters.Add("rut", usuario.Rut);
                    command.Parameters.Add("telefono", usuario.Telefono);
                    command.Parameters.Add("rolId", usuario.RolId);
                    command.Parameters.Add("sucursalId", usuario.SucursalId);
                    command.Parameters.Add("estado", usuario.Estado);
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
        [Route("UpdatePassword")]
        // api/usuario/updatepassword
        public IActionResult UpdatePassword(Usuario usuario)
        {
            var read = GetUsuario(usuario);
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
                    OracleCommand command = new OracleCommand("PCK_USUARIO.SP_UPDATE_PASSWORD_USUARIO", connection);
                    command.Parameters.Add("id", usuario.Id);                    
                    command.Parameters.Add("password", Encrypt.GetSHA256(usuario.Password).ToString());                   
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
        // api/usuario/delete
        public IActionResult Delete(int Id)
        {
            Usuario usuario = new Usuario {Id=Id};
            var read = GetUsuario(usuario);
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
                    OracleCommand command = new OracleCommand("PCK_USUARIO.SP_DELETE_USUARIO", connection);
                    command.Parameters.Add("id", usuario.Id);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                    OracleCommand val = new OracleCommand("select * from usuario where id_usuario = " + usuario.Id, connection);
                    OracleDataReader reader = val.ExecuteReader();
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

        public Respuesta GetUsuario(Usuario usuario)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from USUARIO where ID_USUARIO = " + usuario.Id, connection);
                    OracleDataReader reader = command.ExecuteReader();
                    Usuario result = new Usuario();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Id = (int)reader.GetInt64(0);
                            result.Nombre = reader.GetString(1);
                            result.Apellidos = reader.GetString(2);
                            result.UserName = reader.GetString(3);
                            result.Email = reader.GetString(5);
                            result.Rut = reader.GetString(6);
                            result.Telefono = reader.GetString(7);
                            result.RolId = reader.GetInt32(8);
                            result.SucursalId = reader.GetInt32(9);
                            result.Estado = (int)reader.GetInt64(10);
                        }
                        respuesta.Data = result;
                        respuesta.Exito = 1;
                        return respuesta;
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró el usuario";
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
        [HttpPost]
        [Route("EmpleadosDisponibles")]
        // "api/usuario/empleadosdisponibles"
        public IActionResult EmpleadosDisponibles(Servicio servicio)
        {
            Respuesta respuesta = new Respuesta();
            using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
            {
                try
                {
                    connection.Open();
                    OracleCommand command = new OracleCommand("select * from Usuario", connection);
                    OracleDataReader reader = command.ExecuteReader();
                    List<Usuario> resultAll = new List<Usuario>();
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario();
                        usuario.Id = (int)reader.GetInt64(0);
                        usuario.Nombre = reader.GetString(1);
                        usuario.Apellidos = reader.GetString(2);
                        usuario.UserName = reader.GetString(3);
                        usuario.Email = reader.GetString(5);
                        usuario.Rut = reader.GetString(6);
                        usuario.Telefono = reader.GetString(7);
                        usuario.RolId = reader.GetInt32(8);
                        usuario.SucursalId = reader.GetInt32(9);
                        resultAll.Add(usuario);
                    }
                    var fecha = servicio.Fecha.ToString("dd/MM/yyyy");
                    var horaInicio = servicio.HoraInicio.ToString("HH:mm");
                    var fechaHora = fecha + " " + horaInicio;
                    fechaHora.Replace("-", "/");
                    var a = fechaHora.Replace("-", "/");
                    OracleCommand commandOcupados = new OracleCommand("select usuario_id_empleado from servicio where To_char(TO_TIMESTAMP('" + a + "','DD/MM/YYYY HH24:MI')) between To_char(TO_TIMESTAMP(fecha||' '||hora_inicio,'DD/MM/YYYY HH24:MI')) and To_char(TO_TIMESTAMP(fecha||' '||hora_TERMINO,'DD/MM/YYYY HH24:MI'))", connection);
                    OracleDataReader readerOcupados = commandOcupados.ExecuteReader();
                    List<Usuario> resultOcupados = new List<Usuario>();
                    while (readerOcupados.Read())
                    {
                        Usuario usuario = new Usuario();
                        usuario.Id = (int)readerOcupados.GetInt64(0);
                        resultOcupados.Add(usuario);
                    }
                    resultAll.RemoveAll(d => d.RolId != 2);
                    foreach (var item in resultOcupados)
                    {
                        resultAll.RemoveAll(d => d.Id == item.Id);
                    }
                    respuesta.Exito = 1;
                    respuesta.Data = resultAll;

                }
                catch (System.Exception ex)
                {
                    respuesta.Mensaje = ex.Message;
                    return BadRequest(respuesta);
                }
                if (respuesta.Exito == 0)
                {
                    return BadRequest(respuesta);
                }
                return Ok(respuesta);



            }
        }
        //[HttpPost]
        //[Route("ValidarUsuario")]
        //// "api/login/ValidarUsuario"
        //public IActionResult ValidarUsuario(Usuario usuario)
        //{
        //    Respuesta respuesta = new Respuesta();
        //    using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
        //    {
        //        var a = Encrypt.GetSHA256(usuario.Contraseña).ToString();
        //        connection.Open();
        //        OracleCommand command = new OracleCommand("SP_VALIDARUSUARIO", connection);
        //        command.Parameters.Add("email", usuario.Email);
        //        command.Parameters.Add("contraseña", Encrypt.GetSHA256(usuario.Contraseña).ToString());
        //        command.Parameters.Add("id", OracleDbType.Int32, size: 10);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.ExecuteNonQuery();
        //        usuario.Id = int.Parse(command.Parameters["id"].Value.ToString());
        //        if (usuario.Id > 0)
        //        {
        //            command = new OracleCommand("select * from Usuario where ID_USUARIO = " + usuario.Id + "", connection);
        //            OracleDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                usuario.Nombre = reader.GetString(1);
        //                usuario.Apellido = reader.GetString(2);
        //                usuario.Email = reader.GetString(3);
        //                usuario.RolId = (int)reader.GetInt64(4);
        //                usuario.Contraseña = reader.GetString(5);
        //                usuario.Token = GetToken(usuario);
        //            }
        //            respuesta.Exito = 1;
        //            respuesta.Data = usuario;
        //            return Ok(respuesta);

        //        }
        //        else
        //        {
        //            respuesta.Exito = 0;
        //            respuesta.Mensaje = "Usuario o Contraseña Invalida";
        //            return BadRequest(respuesta);
        //        }         
        //    }

        //}

        /*      [Authorize]
        [HttpGet]
        [Route("Get")]
        // api/usuario/get
        public IActionResult Get(Usuario usuario)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConexionBD.Instance.conn))
                {

                    connection.Open();
                    OracleCommand command = new OracleCommand("PCK_USUARIO.SP_READ_USUARIO", connection);
                    command.Parameters.Add("id" ,usuario.Id);
                    command.Parameters.Add("nombre", OracleDbType.Varchar2).Direction = ParameterDirection.Output; 
                    command.Parameters.Add("apellidos", OracleDbType.Varchar2).Direction = ParameterDirection.Output; 
                    command.Parameters.Add("user_name", OracleDbType.Varchar2).Direction = ParameterDirection.Output; 
                    command.Parameters.Add("email", OracleDbType.Varchar2).Direction = ParameterDirection.Output; 
                    command.Parameters.Add("rut", OracleDbType.Varchar2).Direction = ParameterDirection.Output; 
                    command.Parameters.Add("telefono", OracleDbType.Varchar2).Direction = ParameterDirection.Output; 
                    command.Parameters.Add("rolId", OracleDbType.Int32, size:38).Direction = ParameterDirection.Output; 
                    command.Parameters.Add("sucursalId", OracleDbType.Int32, size:38).Direction = ParameterDirection.Output; 

                    command.CommandType = CommandType.StoredProcedure;

                    command.ExecuteNonQuery();

                    Usuario result = new Usuario();

                    result.Nombre = command.Parameters["nombre"].Value.ToString();
                    result.Apellidos = command.Parameters["apellidos"].Value.ToString();
                    result.UserName = command.Parameters["user_name"].Value.ToString();
                    result.Email = command.Parameters["email"].Value.ToString();
                    result.Rut = command.Parameters["rut"].Value.ToString();
                    result.Telefono = command.Parameters["telefono"].Value.ToString();
                    result.RolId = int.Parse(command.Parameters["rolId"].Value.ToString());
                    result.SurcursalId = int.Parse(command.Parameters["sucursalId"].Value.ToString());

                    if (result.RolId > 0)
                    {
                        respuesta.Exito = 1;
                        respuesta.Data = result;
                        return Ok(respuesta);
                    }
                    else
                    {
                        respuesta.Mensaje = "No se encontró el usuario";
                        return BadRequest(respuesta);
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
                return BadRequest(respuesta);
                throw;
            }
        }
*/

    }
}
