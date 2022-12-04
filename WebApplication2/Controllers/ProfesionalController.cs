using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using AppWeb.Models;
using AppWeb.Models.ViewModels.Profesional;
using AppWeb.Models.ViewModel.Profesional;
using AppWeb.Models.Http;
using System.Text;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppWeb.Controllers
{
    [Authorize(Roles = "Profesional")]
    public class ProfesionalController : Controller
    {
        ReadAll readAll = new ReadAll();
        public IActionResult IndexPro()
        {
            var result = readAll.ReadAllProximosServicios(HttpContext.Session.GetString("Token")).Result;
            result.RemoveAll(d => d.EstadoServicio != 0);
            var idUsuario = int.Parse(HttpContext.Session.GetString("Id"));
            result.RemoveAll(d => d.EmpleadoId != idUsuario);
            ViewBag.Proximos = result;

            return View();
        }
        public IActionResult IndexProCliente()
        {
            List<IndexProCliente> tablaClienteViewModel = new List<IndexProCliente>();
            var clientes = readAll.ReadAllClientes(HttpContext.Session.GetString("Token")).Result;
            var servicios = readAll.ReadAllServicios(HttpContext.Session.GetString("Token")).Result;
            var sucursales = readAll.ReadAllSucursales(HttpContext.Session.GetString("Token")).Result;
            var empresas = readAll.ReadAllEmpresas(HttpContext.Session.GetString("Token")).Result;
            servicios.RemoveAll(d => d.EmpleadoId != int.Parse(HttpContext.Session.GetString("Id")));
            foreach (var item in servicios) {
                IndexProCliente suc = new IndexProCliente();
                foreach (var item2 in clientes)
                {
                    if (item.ClienteId == item2.Id)
                    {
                        suc.Nombre = item2.Nombre;
                        suc.Telefono = item2.Telefono;
                        suc.Id = item2.Id;
                        foreach (var item3 in sucursales)
                        {
                            if (item3.Id == item2.SucursalId)
                            {
                                suc.Sucursal = item3.Nombre;
                                suc.Direccion = item3.Direccion;
                                foreach(var item4 in empresas){
                                    if (item3.EmpresaId == item4.Id)
                                    {
                                        suc.Empresa = item4.Nombre;
                                         suc.Rubro = item4.Rubro;
                                    }
                                }
                            }
                        }
                    }
                }
                tablaClienteViewModel.Add(suc);
            }

            ViewBag.Cliente = tablaClienteViewModel;
            return View();
        }
        public IActionResult IndexProActividades(string alerta) {
            var servicios = readAll.ReadAllServicios(HttpContext.Session.GetString("Token")).Result;
            servicios.RemoveAll(d => d.EmpleadoId != int.Parse(HttpContext.Session.GetString("Id")));
            var tiposervicios = readAll.ReadAllTipoServicio(HttpContext.Session.GetString("Token")).Result;
            var profesionales = readAll.ReadAllProfesionales(HttpContext.Session.GetString("Token")).Result;
            var empresas = readAll.ReadAllEmpresas(HttpContext.Session.GetString("Token")).Result;
            var sucursales = readAll.ReadAllSucursales(HttpContext.Session.GetString("Token")).Result;
            var clientes = readAll.ReadAllClientes(HttpContext.Session.GetString("Token")).Result;
            List<IndexServicioViewModel> tablaServicioViewModel = new List<IndexServicioViewModel>();
            foreach (var item in servicios)
            {
                IndexServicioViewModel servicio = new IndexServicioViewModel();
                servicio.Id = item.Id;
                servicio.Estado = item.Estado == 0 ? "Pendiente" : item.Estado == 1 ? "Realizada" : item.Estado == 2 ? "Cancelada" : "Atrasada";
                foreach (var item2 in tiposervicios)
                {
                    if (item.TipoServicioId == item2.Id)
                    {
                        servicio.TipoServicio = item2.Nombre;
                    }
                }
                foreach (var item3 in profesionales)
                {
                    if (item3.Id == item.EmpleadoId)
                    {
                        servicio.Profesional = item3.Nombre;
                    }
                }

                foreach (var item4 in clientes)
                {
                    if (item4.Id == item.ClienteId)
                    {
                        servicio.Cliente = item4.Nombre +" "+ item4.Apellidos;
                        foreach (var item5 in sucursales)
                        {
                            if (item5.Id == item4.SucursalId)
                            {
                                foreach (var item6 in empresas)
                                {
                                    if (item5.EmpresaId == item6.Id)
                                    {
                                        servicio.Empresa = item6.Nombre;
                                    }
                                }
                                servicio.Sucursal = item5.Nombre;
                            }
                        }
                    }
                }

                servicio.Fecha = item.Fecha.ToShortDateString();
                servicio.HoraInicio = item.HoraInicio.ToString("HH:mm");
                servicio.HoraTermino = item.HoraTermino.ToString("HH:mm");
                servicio.Adicional = item.Adicional == 1 ? true : false;
                tablaServicioViewModel.Add(servicio);
            }
            ViewBag.Servicio = tablaServicioViewModel;
            ViewBag.Alerta = alerta;
            return View();
        }
        public IActionResult IndexProMejora(Servicio servicio,string alerta, int servicioId)
        {
            List<IndexProMejoraViewModel> listaMejora = new List<IndexProMejoraViewModel>();
            var mejoras = readAll.ReadAllActividadMejora(HttpContext.Session.GetString("Token")).Result;
            var servicios = readAll.ReadAllServicios(HttpContext.Session.GetString("Token")).Result;
           
            if (servicioId != 0)
            {
                servicio.Id = servicioId;
            }
            servicios.RemoveAll(d => d.Id != servicio.Id);
            mejoras.RemoveAll(d => d.ServicioId != servicio.Id);
            if (mejoras.Count > 0)
            {
                foreach (var item in mejoras)
                {
                    IndexProMejoraViewModel indexProMejora = new IndexProMejoraViewModel();
                    indexProMejora.Id = item.Id;
                    indexProMejora.Nombre = item.Nombre;
                    indexProMejora.Comentario = item.Comentario;
                    indexProMejora.Descripcion = item.Descripcion;
                    indexProMejora.Situacion = item.Situacion ==0?"Pendiente": item.Situacion==1?"Realizada": "Fallida" ;
                    indexProMejora.ServicioId = item.ServicioId;
                    listaMejora.Add(indexProMejora);
                }
                ViewBag.Mejoras = listaMejora;
             }
                ViewBag.Mejoras = listaMejora;
                ViewBag.Servicio = servicios[0];
                return View();
        }
        public IActionResult IndexProSolicitud(string alerta)
        {
            List<IndexSolicitudViewModel> indexSolicitudViewModels = new List<IndexSolicitudViewModel>();
            var solicitudes = readAll.ReadAllSolicitudes(HttpContext.Session.GetString("Token")).Result;
            var profesionales = readAll.ReadAllProfesionales(HttpContext.Session.GetString("Token")).Result;
            var tiposolicitudes = readAll.ReadAllTipoSolicitudes(HttpContext.Session.GetString("Token")).Result;
            var clientes = readAll.ReadAllClientes(HttpContext.Session.GetString("Token")).Result;
            foreach (var item in solicitudes)
            {
                List<TipoSolicitud> tiposolicitudesList = new List<TipoSolicitud>(tiposolicitudes);
                List<Usuario> profesionalesList = new List<Usuario>(profesionales);
                List<Usuario> clientesList = new List<Usuario>(clientes);
                profesionalesList.RemoveAll(d => d.Id != item.EmpleadoId);
                tiposolicitudesList.RemoveAll(d => d.Id != item.TipoSolicitudId);
                clientesList.RemoveAll(d => d.Id != item.ClienteId);
                IndexSolicitudViewModel solicitud = new IndexSolicitudViewModel();
                solicitud.Id = item.Id;
                solicitud.TipoSolicitud = tiposolicitudesList[0].Nombre;
                solicitud.Empleado = profesionalesList.Count>0?profesionalesList[0].Nombre + " " + profesionalesList[0].Apellidos: " ";
                solicitud.Cliente = clientesList[0].Nombre + " " + clientesList[0].Apellidos;
                solicitud.FechaInicio = item.FechaInicio.ToShortDateString();
                solicitud.FechaTermino = item.FechaTermino.ToShortDateString();
                solicitud.Situacion = item.Situacion == 1 ? "Aceptada" : item.Situacion == 2 ? "Rechazada" : "Pendiente";
                indexSolicitudViewModels.Add(solicitud);
            }
            ViewBag.Solicitudes = indexSolicitudViewModels;
            return View();
        }
        public IActionResult IndexProAccidente(string alerta)
        {
            List<IndexAccidenteViewModel> indexAccidenteViewModel = new List<IndexAccidenteViewModel>();
            var accidentes = readAll.ReadAllAccidentes(HttpContext.Session.GetString("Token")).Result;
            var profesionales = readAll.ReadAllProfesionales(HttpContext.Session.GetString("Token")).Result;
            var clientes = readAll.ReadAllClientes(HttpContext.Session.GetString("Token")).Result;
            var sucursales = readAll.ReadAllSucursales(HttpContext.Session.GetString("Token")).Result;
            var empresas = readAll.ReadAllEmpresas(HttpContext.Session.GetString("Token")).Result;
            foreach (var item in accidentes)
            {
                List<Empresa> empresaList = new List<Empresa>(empresas);
                List<Sucursal> sucursalesList = new List<Sucursal>(sucursales);
                List<Usuario> profesionalesList = new List<Usuario>(profesionales);
                List<Usuario> clientesList = new List<Usuario>(clientes);
                IndexAccidenteViewModel accidente = new IndexAccidenteViewModel();
                clientesList.RemoveAll(d => d.Id != item.ClienteId);
                sucursalesList.RemoveAll(d => d.Id != clientesList[0].SucursalId);
                empresaList.RemoveAll(d => d.Id != sucursalesList[0].EmpresaId);
                accidente.Id = item.Id;
                accidente.Tipo = item.Tipo;
                accidente.Empresa = empresaList[0].Nombre;
                accidente.Sucursal = sucursalesList[0].Nombre;
                accidente.Cliente = clientesList[0].Nombre + " " + clientesList[0].Apellidos;
                accidente.Accidentados = item.Accidentados;
                accidente.Fecha = item.Fecha.ToLongDateString();
                accidente.Telefono = clientesList[0].Telefono;
                accidente.Estado = item.Estado == 1 ?"Concluido": "Pendiente";
                if (accidente.Estado == "Concluido")
                {
                    profesionalesList.RemoveAll(d => d.Id != item.EmpleadoId);
                    accidente.Profesional = profesionalesList[0].Nombre + " " + profesionalesList[0].Apellidos;
                }
                indexAccidenteViewModel.Add(accidente);
            }
            ViewBag.Accidentes = indexAccidenteViewModel;
            return View();
        }
        public IActionResult ProfesionalServicioRead(Servicio servicio)
        {
            List<ResultadoTarea> listaResultadoTareas = new List<ResultadoTarea>();
            var tareas = readAll.ReadAllT(HttpContext.Session.GetString("Token")).Result;
            var contratotareas = readAll.ReadAllContratoTarea(HttpContext.Session.GetString("Token")).Result;
            var tiposervicios = readAll.ReadAllTipoServicio(HttpContext.Session.GetString("Token")).Result;
            var profesionales = readAll.ReadAllProfesionales(HttpContext.Session.GetString("Token")).Result;
            var sucursales = readAll.ReadAllSucursales(HttpContext.Session.GetString("Token")).Result;
            var empresas = readAll.ReadAllEmpresas(HttpContext.Session.GetString("Token")).Result;
            var clientes = readAll.ReadAllClientes(HttpContext.Session.GetString("Token")).Result;
            var servicios = readAll.ReadAllServicios(HttpContext.Session.GetString("Token")).Result;
            var contratos = readAll.ReadAllContratos(HttpContext.Session.GetString("Token")).Result;
            var resultadotareas = readAll.ReadAllResultadoTareas(HttpContext.Session.GetString("Token")).Result;
            servicios.RemoveAll(d => d.Id != servicio.Id);
            tiposervicios.RemoveAll(d => d.Id != servicios[0].TipoServicioId);
            clientes.RemoveAll(d => d.Id != servicios[0].ClienteId);
            profesionales.RemoveAll(d => d.Id != servicios[0].EmpleadoId);
            sucursales.RemoveAll(d => d.Id != clientes[0].SucursalId);
            contratos.RemoveAll(d => d.SucursalId != sucursales[0].Id);
            empresas.RemoveAll(d => d.Id != sucursales[0].EmpresaId);
            contratotareas.RemoveAll(d => d.ContratoId != contratos[0].Id);

            if (servicios.Count > 0)
            {
                ViewBag.Servicio = servicios[0];
            }
            else
            {
                Servicio c = new Servicio();
                ViewBag.Servicio = c;
            }
            resultadotareas.RemoveAll(d => d.ServicioId != servicios[0].Id);
            tareas.RemoveAll(d => d.ServicioId != servicios[0].Id);
 
            ViewBag.ContratoTareas = contratotareas;
            ViewBag.ResultadoTareas = resultadotareas;
            ViewBag.Cliente = clientes[0];
            ViewBag.Sucursal = sucursales[0];
            ViewBag.Empresa = empresas[0];
            ViewBag.Empleado = profesionales[0];
            ViewBag.TipoServicio = tiposervicios[0];
            ViewBag.Tareas = tareas;

            return View();
        }
        public IActionResult ProfesionalServicioCreate( )
        {
            var tiposervicios = readAll.ReadAllTipoServicio(HttpContext.Session.GetString("Token")).Result;
            var empresas = readAll.ReadAllEmpresas(HttpContext.Session.GetString("Token")).Result;
            var sucursales = readAll.ReadAllSucursales(HttpContext.Session.GetString("Token")).Result;
            empresas.RemoveAll(d => d.Estado != 1);
            tiposervicios.RemoveAll(d => d.Estado != 1);
            Usuario usuario = new Usuario();
            usuario.Id = int.Parse(HttpContext.Session.GetString("Id"));
            usuario.Nombre = HttpContext.Session.GetString("Nombre")+" "+ HttpContext.Session.GetString("Apellidos");
            ViewBag.Empleado = usuario;
            ViewBag.TipoServicio = tiposervicios;
            ViewBag.Empresas = empresas;

            return View();
        }
        public IActionResult ProfesionalMejoraRead(ActividadMejora mejora)
        {
            var mejoras = readAll.ReadAllActividadMejora(HttpContext.Session.GetString("Token")).Result;
            mejoras.RemoveAll(d => d.Id != mejora.Id);
            if (mejoras.Count>0)
            {

                    IndexProMejoraViewModel indexProMejora = new IndexProMejoraViewModel();
                    indexProMejora.Id = mejoras[0].Id;
                    indexProMejora.Nombre = mejoras[0].Nombre;
                    indexProMejora.Comentario = mejoras[0].Comentario;
                    indexProMejora.Descripcion = mejoras[0].Descripcion;
                    indexProMejora.Situacion = mejoras[0].Situacion.ToString();
                    indexProMejora.ServicioId = mejoras[0].ServicioId;

                ViewBag.Mejoras = indexProMejora;
            }
            return View();
        }
        public IActionResult ProfesionalServicioFinish(ServicioFinishViewModel servicio)
        {
            ServicioFinishViewModel test = new ServicioFinishViewModel();
            var servicios = readAll.ReadAllServicios(HttpContext.Session.GetString("Token")).Result;
            var contratos = readAll.ReadAllContratos(HttpContext.Session.GetString("Token")).Result;
            var tareas = readAll.ReadAllResultadoTareas(HttpContext.Session.GetString("Token")).Result;
            servicios.RemoveAll(d => d.Id != servicio.Id);
            tareas.RemoveAll(d => d.ServicioId != servicio.Id);
            if (servicios.Count > 0)
            {

                test.Id = servicios[0].Id;
                test.Informe = servicios[0].Informe;
                test.Comentario = servicios[0].Comentario;
                test.ResultadoTareas = tareas;
                test.Estado = servicios[0].Estado;
                test.TipoServicioId = servicios[0].TipoServicioId;
                ViewBag.Servicio = servicios[0];
                ViewBag.Tareas = tareas;
                ViewBag.ServicioFinish = test;
            }
            else
            {
                Servicio c = new Servicio();
                ViewBag.Servicio = c;
            }
            return View(test);
        }
        public IActionResult ProfesionalSolicitudRead(Solicitud solicitud)
        {

        SolicitudReadViewModel solicitudReadViewModels = new SolicitudReadViewModel();
            var clientes = readAll.ReadAllClientes(HttpContext.Session.GetString("Token")).Result;
            var solicitudes = readAll.ReadAllSolicitudes(HttpContext.Session.GetString("Token")).Result;
            var profesionales = readAll.ReadAllProfesionales(HttpContext.Session.GetString("Token")).Result;
            solicitudes.RemoveAll(d => d.Id != solicitud.Id);
            solicitudReadViewModels.Id = solicitudes[0].Id;
            foreach (var item in clientes)
            {
                List<Usuario> cliente = new List<Usuario>(clientes);         
                cliente.RemoveAll(d => d.Id != solicitudes[0].ClienteId);
                solicitudReadViewModels.Cliente = cliente[0].Nombre +" " + cliente[0].Apellidos;
                if (solicitudes[0].Situacion != 0) { 
                List<Usuario> profesional = new List<Usuario>(profesionales);
                profesional.RemoveAll(d => d.Id != solicitudes[0].EmpleadoId);
                solicitudReadViewModels.Empleado = profesional[0].Nombre + " " + profesional[0].Apellidos;
                }

            }
            solicitudReadViewModels.Respuesta = solicitudes[0].Respuesta;
            solicitudReadViewModels.Descripcion = solicitudes[0].Descripcion;
            solicitudReadViewModels.FechaInicio = solicitudes[0].FechaInicio;
            solicitudReadViewModels.FechaTermino = solicitudes[0].FechaTermino;
            solicitudReadViewModels.EmpleadoId = solicitudes[0].EmpleadoId;
            solicitudReadViewModels.ClienteId = solicitudes[0].ClienteId;
            solicitudReadViewModels.Situacion = solicitudes[0].Situacion;

            ViewBag.Solicitud = solicitudReadViewModels;
            return View();
        }
        public IActionResult ProfesionalAccidenteRead(Accidente accidente)
        {
            AccidenteReadViewModel accidenteReadViewModels = new AccidenteReadViewModel();
            var clientes = readAll.ReadAllClientes(HttpContext.Session.GetString("Token")).Result;
            var accidentes = readAll.ReadAllAccidentes(HttpContext.Session.GetString("Token")).Result;
            var profesionales = readAll.ReadAllProfesionales(HttpContext.Session.GetString("Token")).Result;
            accidentes.RemoveAll(d => d.Id != accidente.Id);
            accidenteReadViewModels.Id = accidentes[0].Id;
            foreach (var item in clientes)
            {
                List<Usuario> cliente = new List<Usuario>(clientes);
                cliente.RemoveAll(d => d.Id != accidentes[0].ClienteId);
                accidenteReadViewModels.Cliente = cliente[0].Nombre + " " + cliente[0].Apellidos;
                if (accidentes[0].Estado != 0)
                {
                    List<Usuario> profesional = new List<Usuario>(profesionales);
                    profesional.RemoveAll(d => d.Id != accidentes[0].EmpleadoId);
                    accidenteReadViewModels.Profesional = profesional[0].Nombre + " " + profesional[0].Apellidos;
                }

            }
            accidenteReadViewModels.Accidentados = accidentes[0].Accidentados;
            accidenteReadViewModels.Comentario = accidentes[0].Comentario;
            accidenteReadViewModels.Descripcion = accidentes[0].Descripcion;
            accidenteReadViewModels.Fecha = accidentes[0].Fecha;
            accidenteReadViewModels.EmpleadoId = accidentes[0].EmpleadoId;
            accidenteReadViewModels.ClienteId = accidentes[0].ClienteId;
            accidenteReadViewModels.Estado = accidentes[0].Estado;

            ViewBag.Accidente = accidenteReadViewModels;
            return View();
        }
        public IActionResult ProfesionalSolicitudFinish(Solicitud solicitud)
        {   

            var solicitudes = readAll.ReadAllSolicitudes(HttpContext.Session.GetString("Token")).Result;
            solicitudes.RemoveAll(d => d.Id == solicitud.Id);
            ViewBag.Solicitud = solicitudes[0];
            return View();
        }
        public IActionResult ProfesionalAccidenteFinish(Accidente accidente)
        {
            var accidentes = readAll.ReadAllAccidentes(HttpContext.Session.GetString("Token")).Result;
            accidentes.RemoveAll(d => d.Id != accidente.Id);
            ViewBag.Accidente = accidentes[0];
            return View();
        }
        

        [HttpPost]
        public async Task<RedirectToActionResult> CreateServicio(ServicioViewModel servicio)
        {
            try { 
            List<ResultadoTarea> listResultadoTareas = new List<ResultadoTarea>();
            var contratos = readAll.ReadAllContratos(HttpContext.Session.GetString("Token")).Result;
            var clientes = readAll.ReadAllClientes(HttpContext.Session.GetString("Token")).Result;
            clientes.RemoveAll(x => x.Id != servicio.ClienteId);
            var contratotareas = readAll.ReadAllContratoTarea(HttpContext.Session.GetString("Token")).Result;
                contratos.RemoveAll(x => x.Estado != 1);
            contratotareas.RemoveAll(x => x.Estado != 1);
            contratos.RemoveAll(x => x.SucursalId != clientes[0].SucursalId);
            contratotareas.RemoveAll(x => x.ContratoId != contratos[0].Id);
            Servicio serv = new Servicio();
            serv.Id = servicio.Id;
            serv.EmpleadoId = int.Parse(HttpContext.Session.GetString("Id"));
            serv.HoraInicio = servicio.HoraInicio;
            serv.TipoServicioId = servicio.TipoServicioId;
            serv.HoraTermino = servicio.HoraTermino;
            serv.Asistentes = servicio.Asistentes;
            serv.Fecha = servicio.Fecha;
            serv.Adicional = servicio.Adicional;
            serv.Descripcion = servicio.Descripcion;
            serv.Material = servicio.Material == null ? "N/A" : servicio.Material;
            serv.ClienteId = servicio.ClienteId;
            foreach (var item in contratotareas)
            {
                ResultadoTarea tarea = new ResultadoTarea();
                tarea.ServicioId = servicio.Id;
                tarea.ContratoTareaId = item.Id;
                tarea.Estado = item.Estado;
                tarea.Nombre = item.Nombre;
                listResultadoTareas.Add(tarea);

            }
            serv.ResultadoTareas = listResultadoTareas;
            var tpJson = JsonConvert.SerializeObject(serv);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("https://localhost:44319/api/servicio/create", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexProActividades", "Profesional");
            }
            catch (Exception ex)
            {
                var Mensaje = ex.Message;
                return RedirectToAction("IndexProActividades", "Profesional");
            }
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
        public async Task<RedirectToActionResult> FinishServicio(ServicioFinishViewModel servicio)
        {
            var tpJson = JsonConvert.SerializeObject(servicio);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/servicio/finishservicio", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexProActividades", "Profesional", new {alerta=respuesta.Mensaje});
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
