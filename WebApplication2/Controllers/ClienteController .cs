using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using AppWeb.Models;
using AppWeb.Models.Http;
using System.Text;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppWeb.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class ClienteController : Controller
    {
        ReadAll readAll = new ReadAll();
        public IActionResult IndexCliente()
        {
            return View();
        }




        [HttpPost]
        public async Task<RedirectToActionResult> CreateMejora(ActividadMejora mejora)
        {
            var tpJson = JsonConvert.SerializeObject(mejora);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("https://localhost:44319/api/actividadmejora/create", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexProMejora", "Profesional", new {alerta = respuesta.Mensaje, servicioId = mejora.ServicioId});
        }

        [HttpPost]
        public ActionResult ReadSucursal(string IdEmpresa)
        {
            try
            {
                var sucursales = readAll.ReadAllSucursales(HttpContext.Session.GetString("Token")).Result;

                sucursales.RemoveAll(a => a.EmpresaId != int.Parse(IdEmpresa));
                var list = JsonConvert.SerializeObject(sucursales, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return Content(list, "application/json");
            }
            catch (System.Exception)
            {
                List<Sucursal> s = new List<Sucursal>();
                return Json(new SelectList(s));

            }
        }
        [HttpPost]
        public ActionResult ReadSucursalDisponible(string IdEmpresa)
        {
            try
            {
                var sucursales = readAll.ReadAllSucursales(HttpContext.Session.GetString("Token")).Result;
                sucursales.RemoveAll(a => a.Estado != 1);
                sucursales.RemoveAll(a => a.EmpresaId != int.Parse(IdEmpresa));
                var list = JsonConvert.SerializeObject(sucursales, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return Content(list, "application/json");
            }
            catch (System.Exception)
            {
                List<Sucursal> s = new List<Sucursal>();
                return Json(new SelectList(s));

            }
        }
        [HttpPost]
        public ActionResult ReadSucursales()
        {
            try
            {
                var sucursales = readAll.ReadAllSucursales(HttpContext.Session.GetString("Token")).Result;
                var list = JsonConvert.SerializeObject(sucursales, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return Content(list, "application/json");
            }
            catch (System.Exception)
            {
                List<Sucursal> s = new List<Sucursal>();
                return Json(new SelectList(s));

            }
        }
        [HttpPost]
        public ActionResult ReadClientes()
        {
            try
            {
                var clientes = readAll.ReadAllClientes(HttpContext.Session.GetString("Token")).Result;
                var list = JsonConvert.SerializeObject(clientes, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return Content(list, "application/json");
            }
            catch (System.Exception)
            {
                List<Usuario> s = new List<Usuario>();
                return Json(new SelectList(s));

            }
        }
        [HttpPost]
        public ActionResult ReadCliente(string IdSucursal)
        {
            try
            {
                var clientes = readAll.ReadAllClientes(HttpContext.Session.GetString("Token")).Result;

                clientes.RemoveAll(a => a.SucursalId != int.Parse(IdSucursal));
                var list = JsonConvert.SerializeObject(clientes, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return Content(list, "application/json");
            }
            catch (System.Exception)
            {
                List<Usuario> s = new List<Usuario>();
                return Json(new SelectList(s));

            }
        }
        [HttpPost]
        public ActionResult ReadMejoras(string IdServicio)
        {
            try
            {
                var mejoras = readAll.ReadAllActividadMejora(HttpContext.Session.GetString("Token")).Result;
                mejoras.RemoveAll(d => d.ServicioId != int.Parse(IdServicio));
                var list = JsonConvert.SerializeObject(mejoras, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return Content(list, "application/json");
            }
            catch (System.Exception)
            {
                List<ActividadMejora> s = new List<ActividadMejora>();
                return Json(new SelectList(s));

            }
        }
        [HttpPost]
        public ActionResult ReadSolicitud(string IdSolicitud)
        {
            try
            {
                var solicitudes = readAll.ReadAllSolicitudes(HttpContext.Session.GetString("Token")).Result;

                solicitudes.RemoveAll(a => a.Id != int.Parse(IdSolicitud));
                var list = JsonConvert.SerializeObject(solicitudes, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return Content(list, "application/json");
            }
            catch (System.Exception)
            {
                List<Sucursal> s = new List<Sucursal>();
                return Json(new SelectList(s));

            }
        }
        [HttpPost]
        public ActionResult ReadAccidente(string IdAccidente)
        {
            try
            {
                var accidentes = readAll.ReadAllAccidentes(HttpContext.Session.GetString("Token")).Result;

                accidentes.RemoveAll(a => a.Id != int.Parse(IdAccidente));
                var list = JsonConvert.SerializeObject(accidentes, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return Content(list, "application/json");
            }
            catch (System.Exception)
            {
                List<Sucursal> s = new List<Sucursal>();
                return Json(new SelectList(s));

            }
        }
        [HttpPost]
        public async Task<RedirectToActionResult> FinishMejora(ActividadMejora mejora)
        {
            var tpJson = JsonConvert.SerializeObject(mejora);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/actividadmejora/finishmejora", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexProMejora", "Profesional", new { alerta = respuesta.Mensaje, servicioId = mejora.ServicioId });
        }

        [HttpPost]
        public async Task<RedirectToActionResult> FinishSolicitud(Solicitud solicitud)
        {
            var tpJson = JsonConvert.SerializeObject(solicitud);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/solicitud/finishsolicitud", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexProSolicitud", "Profesional", new { alerta = respuesta.Mensaje });
        }
        [HttpPost]
        public async Task<RedirectToActionResult> FinishAccidente(Accidente accidente)
        {
            var tpJson = JsonConvert.SerializeObject(accidente);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/accidente/finishaccidente", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexProAccidente", "Profesional", new { alerta = respuesta.Mensaje});
        }
    }

}
