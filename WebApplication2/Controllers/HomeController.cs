using AppWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Login(string alerta)
        {
            ViewBag.Alerta = alerta;
            
            return View();
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
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception)
                {
                    return RedirectToAction("Login", "Home", new { alerta = respuesta.Mensaje });
                }
                var usuario = JsonConvert.DeserializeObject<Usuario>(respuesta.Data.ToString());

                HttpContext.Session.SetString("Id", usuario.Id.ToString());
                HttpContext.Session.SetString("Nombre", usuario.Nombre);
                HttpContext.Session.SetString("Apellidos", usuario.Apellidos);
                HttpContext.Session.SetString("UserName", usuario.UserName);
                HttpContext.Session.SetString("Email", usuario.Email);
                HttpContext.Session.SetString("Rut", usuario.Rut);
                HttpContext.Session.SetString("Telefono", usuario.Telefono);
                HttpContext.Session.SetString("RolId", usuario.RolId.ToString());
                HttpContext.Session.SetString("SurcursalId", usuario.SucursalId.ToString());
                HttpContext.Session.SetString("Token", usuario.Token);
                string rol = usuario.RolId == 1 ? "Administrador" : usuario.RolId == 2 ? "Profesional" : "Cliente";
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, usuario.UserName),
                    new Claim(ClaimTypes.Role, rol)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                if (usuario.RolId == 1)
                {
                    return RedirectToAction("IndexAdm", "Administrador");
                }
                else if (usuario.RolId == 2)
                {
                    return RedirectToAction("IndexPro", "Profesional");
                }
                else if (usuario.RolId == 3)
                {
                    return RedirectToAction("IndexCli", "Cliente");
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { alerta = "Error al iniciar Sesion" });
            }
        }


        public IActionResult Logout()
        {

            HttpContext.Session.Remove("Id");
            HttpContext.Session.Remove("Nombre");
            HttpContext.Session.Remove("Apellidos");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Rut");
            HttpContext.Session.Remove("Telefono");
            HttpContext.Session.Remove("RolId");
            HttpContext.Session.Remove("EmpresaId");
            HttpContext.Session.Remove("Token");

            return RedirectToAction("Login", "Home");
        }

    }
}
