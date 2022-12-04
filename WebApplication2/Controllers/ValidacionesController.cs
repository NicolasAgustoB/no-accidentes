using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppWeb.Models;
using AppWeb.Models.Http;

namespace AppWeb.Controllers
{
    public class ValidacionesController : Controller
    {
        ReadAll readAll = new ReadAll();
        [AcceptVerbs("GET", "POST")]
        public IActionResult ValidarRut(string rut)
        {

            rut = rut.Replace(".", "").ToUpper();
            Regex expresion = new Regex("^(^0?[1-9]{1,2})(?>((\\.\\d{3}){2}\\-)|((\\d{3}){2}\\-)|((\\d{3}){2}))([\\dkK])$");
            string dv = rut.Substring(rut.Length - 1, 1);
            if (!expresion.IsMatch(rut))
            {
                return Json($"El formato para el rut es xx.xxx.xxx-x");
            }
            char[] charCorte = { '-' };
            string[] rutTemp = rut.Split(charCorte);
            if (dv != Digito(int.Parse(rutTemp[0])))
            {
                return Json($"Rut no valido");
            }
            return Json(true);
        }

        public static string Digito(int rut)
        {
            int suma = 0;
            int multiplicador = 1;
            while (rut != 0)
            {
                multiplicador++;
                if (multiplicador == 8)
                    multiplicador = 2;
                suma += (rut % 10) * multiplicador;
                rut = rut / 10;
            }
            suma = 11 - (suma % 11);
            if (suma == 11)
            {
                return "0";
            }
            else if (suma == 10)
            {
                return "K";
            }
            else
            {
                return suma.ToString();
            }
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult HoraTermino(string horatermino, string horainicio)
        {
            var termino = DateTime.Parse(horatermino);
            var inicio = DateTime.Parse(horainicio);
            if (termino < inicio)
            {
                return Json($"La hora de termino debe ser posterior a la hora de inicio");
            }
            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult FechaActual(string fecha)
        {
            var fechaServicio = DateTime.Parse(fecha);
            if (fechaServicio < DateTime.Now.Date)
            {
                return Json($"La hora de termino debe ser posterior a la hora de inicio");
            }
            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult FechaTermino(string FechaTermino)
        {
            var fechaServicio = DateTime.Parse(FechaTermino);
            var xd = DateTime.Now.Date.AddDays(30);
            if (fechaServicio < DateTime.Now.Date.AddDays(30))
            {
                return Json($"La duración mínima del contrato son 30 días");
            }
            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult HiddenInput(int idCliente)
        {
            if (idCliente == 0)
            {
                return Json($"Seleccione un cliente de la tabla");
            }
            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult Disponible(string horainicio, string fecha)
        {
            Servicio servicio = new Servicio() { Fecha = DateTime.Parse(fecha), HoraInicio = DateTime.Parse(horainicio) };
            var empleadosDisponibles = EmpleadosDisponiblesReadAll(servicio).Result;
            if (empleadosDisponibles.Exists(d => d.Id == int.Parse(HttpContext.Session.GetString("Id"))) == false)
            {
                return Json($"Ya cuenta con un servicio en esta fecha y hora");
            }
            return Json(true);
        }


        public async Task<List<Usuario>> EmpleadosDisponiblesReadAll(Servicio servicio)
        {
            var tpJson = JsonConvert.SerializeObject(servicio);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("https://localhost:44319/api/usuario/empleadosdisponibles", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            var empleadodisponible = JsonConvert.DeserializeObject<List<Usuario>>(respuesta.Data.ToString());
            return empleadodisponible;
        }
        // Empresa------------------------------------------
        [AcceptVerbs("GET", "POST")]
        public IActionResult EmpresaValidarNombre(string nombre, int id)
        {
            var empresas = readAll.ReadAllEmpresas(HttpContext.Session.GetString("Token")).Result;
            if (id == 0)
            {
                if (nombre != null)
                {
                    if (empresas.Exists(d => d.Nombre == nombre))
                    {
                        return Json($"Ya existe una empresa con este nombre");
                    }
                }
            }
            else
            {
                empresas.RemoveAll(d => d.Id == id);
                if (empresas.Exists(d => d.Nombre == nombre))
                {
                    return Json($"Ya existe una empresa con este nombre");
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult EmpresaValidarRut(string rut, int id)
        {

            rut = rut.Replace(".", "").ToUpper();
            Regex expresion = new Regex("^(^0?[1-9]{1,2})(?>((\\.\\d{3}){2}\\-)|((\\d{3}){2}\\-)|((\\d{3}){2}))([\\dkK])$");
            string dv = rut.Substring(rut.Length - 1, 1);
            if (!expresion.IsMatch(rut))
            {
                return Json($"El formato para el rut es xx.xxx.xxx-x");
            }
            char[] charCorte = { '-' };
            string[] rutTemp = rut.Split(charCorte);
            if (dv != Digito(int.Parse(rutTemp[0])))
            {
                return Json($"Rut no valido");
            }
            var empresas = readAll.ReadAllEmpresas(HttpContext.Session.GetString("Token")).Result;
            if (id == 0)
            {
                if (rut != null)
                {
                    if (empresas.Exists(d => d.Rut == rut))
                    {
                        return Json($"Ya existe una empresa con este rut");
                    }
                }
            }
            else
            {
                empresas.RemoveAll(d => d.Id == id);
                if (empresas.Exists(d => d.Rut == rut))
                {
                    return Json($"Ya existe una empresa con este rut");
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult EmpresaValidarEmail(string email, int id)
        {
            var empresas = readAll.ReadAllEmpresas(HttpContext.Session.GetString("Token")).Result;
            if (id == 0)
            {
                if (email != null)
                {

                    if (empresas.Exists(d => d.Email == email))
                    {
                        return Json($"Ya existe una empresa con este email");
                    }
                }
            }
            else
            {
                empresas.RemoveAll(d => d.Id == id);
                if (empresas.Exists(d => d.Email == email))
                {
                    return Json($"Ya existe una empresa con este email");
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult EmpresaValidarTelefono(string telefono, int id)
        {
            var empresas = readAll.ReadAllEmpresas(HttpContext.Session.GetString("Token")).Result;
            if (id == 0)
            {
                if (telefono != null)
                {

                    if (empresas.Exists(d => d.Telefono == telefono))
                    {
                        return Json($"Ya existe una empresa con este telefono");
                    }
                }
            }
            else
            {
                empresas.RemoveAll(d => d.Id == id);
                if (empresas.Exists(d => d.Telefono == telefono))
                {
                    return Json($"Ya existe una empresa con este telefono");
                }
            }
            return Json(true);
        }
        //-----------------------------------------------

        // Sucursal -------------------------------------
        [AcceptVerbs("GET", "POST")]
        public IActionResult SucursalValidarNombre(string nombre, int id)
        {
            var sucursales = readAll.ReadAllSucursales(HttpContext.Session.GetString("Token")).Result;
            if (id == 0)
            {
                if (nombre != null)
                {
                    if (sucursales.Exists(d => d.Nombre == nombre))
                    {
                        return Json($"Ya existe una sucursal con este nombre");
                    }
                }
            }
            else
            {
                sucursales.RemoveAll(d => d.Id == id);
                if (sucursales.Exists(d => d.Nombre == nombre))
                {
                    return Json($"Ya existe una sucursal con este nombre");
                }
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult SucursalValidarTelefono(string telefono, int id)
        {
            var sucursales = readAll.ReadAllSucursales(HttpContext.Session.GetString("Token")).Result;
            if (id == 0)
            {
                if (telefono != null)
                {

                    if (sucursales.Exists(d => d.Telefono == telefono))
                    {
                        return Json($"Ya existe una sucursal con este telefono");
                    }
                }
            }
            else
            {
                sucursales.RemoveAll(d => d.Id == id);
                if (sucursales.Exists(d => d.Telefono == telefono))
                {
                    return Json($"Ya existe una sucursal con este telefono");
                }
            }

            return Json(true);
        }
        // ----------------------------------------------

        // USUARIO--------------------------------------
        [AcceptVerbs("GET", "POST")]
        public IActionResult UsuarioValidarNombreUsuario(string username, int id)
        {
            var usuarios = readAll.ReadAllUsuarios(HttpContext.Session.GetString("Token")).Result;
            if (id == 0)
            {
                if (username != null)
                {
                    if (usuarios.Exists(d => d.UserName == username))
                    {
                        return Json($"Nombre de usuario no disponible");
                    }
                }
            }
            else
            {
                usuarios.RemoveAll(d => d.Id == id);
                if (usuarios.Exists(d => d.UserName == username))
                {
                    return Json($"Nombre de usuario no disponible");
                }
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult UsuarioValidarRut(string rut, int id)
        {

            rut = rut.Replace(".", "").ToUpper();
            Regex expresion = new Regex("^(^0?[1-9]{1,2})(?>((\\.\\d{3}){2}\\-)|((\\d{3}){2}\\-)|((\\d{3}){2}))([\\dkK])$");
            string dv = rut.Substring(rut.Length - 1, 1);
            if (!expresion.IsMatch(rut))
            {
                return Json($"El formato para el rut es xx.xxx.xxx-x");
            }
            char[] charCorte = { '-' };
            string[] rutTemp = rut.Split(charCorte);
            if (dv != Digito(int.Parse(rutTemp[0])))
            {
                return Json($"Rut no valido");
            }
            var usuarios = readAll.ReadAllUsuarios(HttpContext.Session.GetString("Token")).Result;
            if (id == 0)
            {
                if (rut != null)
                {
                    if (usuarios.Exists(d => d.Rut == rut))
                    {
                        return Json($"Ya existe un usuario con este rut");
                    }
                }
            }
            else
            {
                usuarios.RemoveAll(d => d.Id == id);
                if (usuarios.Exists(d => d.Rut == rut))
                {
                    return Json($"Ya existe un usuario con este rut");
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult UsuarioValidarEmail(string email, int id)
        {
            var usuarios = readAll.ReadAllUsuarios(HttpContext.Session.GetString("Token")).Result;
            if (id == 0)
            {
                if (email != null)
                {

                    if (usuarios.Exists(d => d.Email == email))
                    {
                        return Json($"Ya existe un usuario con este email");
                    }
                }
            }
            else
            {
                usuarios.RemoveAll(d => d.Id == id);
                if (usuarios.Exists(d => d.Email == email))
                {
                    return Json($"Ya existe un usuario con este email");
                }
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult UsuarioValidarTelefono(string telefono, int id)
        {
            var usuarios = readAll.ReadAllUsuarios(HttpContext.Session.GetString("Token")).Result;
            if (id == 0)
            {
                if (telefono != null)
                {

                    if (usuarios.Exists(d => d.Telefono == telefono))
                    {
                        return Json($"Ya existe un usuario con este telefono");
                    }
                }
            }
            else
            {
                usuarios.RemoveAll(d => d.Id == id);
                if (usuarios.Exists(d => d.Telefono == telefono))
                {
                    return Json($"Ya existe un usuario con este telefono");
                }
            }
            return Json(true);
        }

        //--------------------------------------------
    }

}
