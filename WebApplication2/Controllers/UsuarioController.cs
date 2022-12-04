using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using AppWeb.Models;

namespace AppWeb.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Login()
        {
            ViewData["Usuario"] = HttpContext.Session.GetString("Nombre");
            return View();
        }

        public IActionResult Usuarios()
        {
            var usuarios = GetAll().Result;
            return View(usuarios);
        }

        [HttpPost]
        public async Task<RedirectToActionResult> ValidarUsuario(Usuario us)
        {
            try
            {
                var usJson = JsonConvert.SerializeObject(us);
                var requestContent = new StringContent(usJson, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                var response = await client.PostAsync("https://localhost:44319/api/usuario/Autentificar", requestContent);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var usuario = JsonConvert.DeserializeObject<Usuario>(respuesta.Data.ToString());


                HttpContext.Session.SetString("Id", usuario.Id.ToString());
                HttpContext.Session.SetString("Nombre", usuario.Nombre);
                HttpContext.Session.SetString("Apellido", usuario.Apellidos);
                HttpContext.Session.SetString("UserName", usuario.UserName);
                HttpContext.Session.SetString("Email", usuario.Email);
                HttpContext.Session.SetString("Rut", usuario.Rut);
                HttpContext.Session.SetString("Telefono", usuario.Telefono);
                HttpContext.Session.SetString("RolId", usuario.RolId.ToString());
                HttpContext.Session.SetString("EmpresaId", usuario.SucursalId.ToString());
                HttpContext.Session.SetString("Token", usuario.Token);

                if (usuario.RolId == 1)
                {
                    return RedirectToAction("Index", "Administrador");
                }
                else if (usuario.RolId == 2)
                {
                    return RedirectToAction("Index", "Profesional");
                }
                else
                {
                    return RedirectToAction("Login", "Usuario");
                }

            }
            catch (System.Exception)
            {
                return RedirectToAction("Login", "Usuario");
                throw;
            }
        }

        [HttpGet]
        public async Task<List<Usuario>> GetAll()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await client.GetAsync("https://localhost:44319/api/usuario/getall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(respuesta.Data.ToString());
                return usuarios;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
    }
}
