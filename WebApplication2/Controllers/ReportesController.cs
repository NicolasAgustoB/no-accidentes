using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using AppWeb.Models;
using System.Text;
using AppWeb.Models.ViewModels.Administrador;
using AppWeb.Models.ViewModel.Profesional;
using System.Reflection;
using System.Linq;
using AppWeb.Models.ViewModels.Reporte;
using static System.Net.Mime.MediaTypeNames;

namespace AppWeb.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return new ViewAsPdf("Index");
        }

        public IActionResult ActividadesSucursal()
        {
            var servicios = ReadAllServicios().Result;
            var tiposervicios = ReadAllTipoServicio().Result;
            var profesionales = ReadAllProfesionales().Result;
            var empresas = ReadAllEmpresas().Result;
            var sucursales = ReadAllSucursales().Result;
            var clientes = ReadAllClientes().Result;
            List<IndexAdmServicioViewModel> tablaServicioViewModel = new List<IndexAdmServicioViewModel>();
            foreach (var item in servicios)
            {
                IndexAdmServicioViewModel servicio = new IndexAdmServicioViewModel();
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
                servicio.Adicional = item.Adicional == 0 ? false : true;
                tablaServicioViewModel.Add(servicio);
            }
            var tiposervicio = tablaServicioViewModel.GroupBy(tipo => tipo.TipoServicio);
            return new ViewAsPdf("ActividadesSucursal", tablaServicioViewModel);
        }
        public IActionResult EmpresasAllReporte()
        {
            var reporteActividades = ReadAllReportesActividades();
            reporteActividades.Empresas.RemoveAll(d => d.EstadoEmpresa != 1);
            reporteActividades.Empresas.ForEach(d => d.Sucursales.RemoveAll(x => x.EstadoSucursal != 1));
            reporteActividades.Empresas.RemoveAll(d => d.Sucursales.Count() == 0);
            return new ViewAsPdf("EmpresasServiciosReporte", reporteActividades);
        }
        public IActionResult EmpresasReadReporte(Empresa empresa, Sucursal sucursal)
        {
            empresa.Id = 1;
            sucursal.Id = 2;
            var reporteActividades = ReadAllReportesActividades();
            if (empresa.Id != 0)
            {
                reporteActividades.Empresas.RemoveAll(d => d.IdEmpresa != empresa.Id);
            }
            if (sucursal.Id != 0)
            {
                if (sucursal.EmpresaId != 0)
                {
                    reporteActividades.Empresas.RemoveAll(d => d.IdEmpresa != sucursal.EmpresaId);
                }
                reporteActividades.Empresas.ForEach(d => d.Sucursales.RemoveAll(x => x.IdSucursal != sucursal.Id));
            }

            reporteActividades.Empresas.RemoveAll(d => d.EstadoEmpresa != 1);
            reporteActividades.Empresas.ForEach(d => d.Sucursales.RemoveAll(x => x.EstadoSucursal != 1));
            reporteActividades.Empresas.RemoveAll(d => d.Sucursales.Count() == 0);
            return new ViewAsPdf("EmpresasServiciosReporte", reporteActividades);
        }


        public IActionResult CalcularAccidentabilidad()
        {
            var tiposervicios = ReadAllTipoServicio().Result;
            var profesionales = ReadAllProfesionales().Result;
            var empresas = ReadAllEmpresas().Result;
            var suc = ReadAllSucursales().Result;
            var serv = ReadAllServicios().Result;
            var clientes = ReadAllClientes().Result;
            ReporteActividadesVM reporteActividades = new ReporteActividadesVM();
            List<EmpresaReporteVM> empresaReportes = new List<EmpresaReporteVM>();


            foreach (var empresa in empresas)
            {
                EmpresaReporteVM empresaReporteVM = new EmpresaReporteVM();

                empresaReporteVM.NombreEmpresa = empresa.Nombre;
                empresaReporteVM.EmailEmpresa = empresa.Email;
                empresaReporteVM.EstadoEmpresa = empresa.Estado;
                List<Sucursal> sucursales = new List<Sucursal>(suc);
                sucursales.RemoveAll(d => d.EmpresaId != empresa.Id);
                List<SucursalReporteVM> sucursalReportes = new List<SucursalReporteVM>();
                foreach (var sucursal in sucursales)
                {
                    SucursalReporteVM sucursalReporteVM = new SucursalReporteVM();
                    sucursalReporteVM.NombreSucursal = sucursal.Nombre;
                    sucursalReporteVM.DireccionSucursal = sucursal.Direccion;
                    sucursalReporteVM.TelefonoSucursal = sucursal.Telefono;
                    sucursalReporteVM.EstadoSucursal = sucursal.Estado;
                    List<Servicio> servicios = new List<Servicio>(serv);

                    foreach (var item in servicios)
                    {
                        ServicioReporteVM servicio = new ServicioReporteVM();
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
                                servicio.Profesional = item3.Nombre + ' ' + item3.Apellidos;
                            }
                        }
                        foreach (var item4 in clientes)
                        {
                            if (item4.Id == item.ClienteId)
                            {
                                foreach (var item5 in sucursales)
                                {
                                    if (item5.Id == item4.SucursalId)
                                    {
                                        servicio.Sucursal = item5.Nombre;
                                    }
                                }
                                servicio.Cliente = item4.Nombre + ' ' + item4.Apellidos;
                                servicio.TelefonoCliente = item4.Telefono;
                            }
                        }
                        servicio.Fecha = item.Fecha.ToShortDateString();
                        servicio.HoraInicio = item.HoraInicio.ToString("HH:mm");
                        servicio.HoraTermino = item.HoraTermino.ToString("HH:mm");
                        servicio.Adicional = item.Adicional == 0 ? false : true;
                        if (servicio.Sucursal == sucursal.Nombre)
                        {
                            sucursalReporteVM.Servicios.Add(servicio);
                        }
                    }
                    empresaReporteVM.Sucursales.Add(sucursalReporteVM);
                }
                reporteActividades.Empresas.Add(empresaReporteVM);
            }
            reporteActividades.Empresas.RemoveAll(d => d.EstadoEmpresa != 1);
            reporteActividades.Empresas.ForEach(d => d.Sucursales.RemoveAll(x => x.EstadoSucursal != 1));
            reporteActividades.Empresas.RemoveAll(d => d.Sucursales.Count() == 0);
            return new ViewAsPdf(reporteActividades);
        }


        public IActionResult Test()
        {
            Test test = new Test();
            test.ResultadoTareas = new List<ResultadoTarea>();
            return View(test);
        }








        public async Task<List<Usuario>> ReadAllClientes()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await client.GetAsync("https://localhost:44319/api/usuario/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(respuesta.Data.ToString());
                usuarios.RemoveAll(d => d.RolId != 3);
                return usuarios;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<Usuario>> ReadAllProfesionales()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await client.GetAsync("https://localhost:44319/api/usuario/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(respuesta.Data.ToString());
                usuarios.RemoveAll(d => d.RolId != 2);
                return usuarios;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<TipoServicio>> ReadAllTipoServicio()
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await tipo.GetAsync("https://localhost:44319/api/TipoServicio/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var TipoServicios = JsonConvert.DeserializeObject<List<TipoServicio>>(respuesta.Data.ToString());
                ViewBag.TipoServicios = TipoServicios;
                return TipoServicios;
            }
            catch (System.Exception)
            {
                return null;

            }

        }
        public async Task<List<TipoSolicitud>> ReadAllTipoSolicitud()
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await tipo.GetAsync("https://localhost:44319/api/TipoSolicitud/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var TipoSolicitud = JsonConvert.DeserializeObject<List<TipoSolicitud>>(respuesta.Data.ToString());

                return TipoSolicitud;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<Rol>> ReadAllRoles()
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await tipo.GetAsync("https://localhost:44319/api/rol/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var rol = JsonConvert.DeserializeObject<List<Rol>>(respuesta.Data.ToString());

                return rol;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<Empresa>> ReadAllEmpresas()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await client.GetAsync("https://localhost:44319/api/empresa/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var empresas = JsonConvert.DeserializeObject<List<Empresa>>(respuesta.Data.ToString());
                empresas.RemoveAll(x => x.Id == 0);
                return empresas;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<Sucursal>> ReadAllSucursales()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await client.GetAsync("https://localhost:44319/api/sucursal/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var sucursales = JsonConvert.DeserializeObject<List<Sucursal>>(respuesta.Data.ToString());
                sucursales.RemoveAll(x => x.Id == 0);
                return sucursales;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<Contrato>> ReadAllContratos()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await client.GetAsync("https://localhost:44319/api/contrato/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var contratos = JsonConvert.DeserializeObject<List<Contrato>>(respuesta.Data.ToString());
                return contratos;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<Tarea>> ReadAllTareas()
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await tipo.GetAsync("https://localhost:44319/api/tarea/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var tareas = JsonConvert.DeserializeObject<List<Tarea>>(respuesta.Data.ToString());

                return tareas;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<ResultadoTarea>> ReadAllResultadoTareas()
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await tipo.GetAsync("https://localhost:44319/api/resultadotarea/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var resultadoTareas = JsonConvert.DeserializeObject<List<ResultadoTarea>>(respuesta.Data.ToString());

                return resultadoTareas;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<ContratoTarea>> ReadAllContratoTarea()
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await tipo.GetAsync("https://localhost:44319/api/contratotarea/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var contratoTareas = JsonConvert.DeserializeObject<List<ContratoTarea>>(respuesta.Data.ToString());

                return contratoTareas;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<Servicio>> ReadAllServicios()
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await tipo.GetAsync("https://localhost:44319/api/servicio/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var servicios = JsonConvert.DeserializeObject<List<Servicio>>(respuesta.Data.ToString());

                return servicios;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<ServiciosTablasVM>> ReadAllProximosServicios()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await client.GetAsync("https://localhost:44319/api/servicio/readall/proximos", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var proximosServicios = JsonConvert.DeserializeObject<List<ServiciosTablasVM>>(respuesta.Data.ToString());
                return proximosServicios;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
        public async Task<List<ResultadoTarea>> ReadAllT()
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await tipo.GetAsync("https://localhost:44319/api/resultadotarea/readallt", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var tareas = JsonConvert.DeserializeObject<List<ResultadoTarea>>(respuesta.Data.ToString());

                return tareas;
            }
            catch (System.Exception)
            {
                return null;

            }
        }

        public async Task<List<Usuario>> ReadAllAccidentes()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await client.GetAsync("https://localhost:44319/api/usuario/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(respuesta.Data.ToString());
                usuarios.RemoveAll(d => d.RolId != 3);
                return usuarios;
            }
            catch (System.Exception)
            {
                return null;

            }
        }


        public ReporteActividadesVM ReadAllReportesActividades()
        {
            var tiposervicios = ReadAllTipoServicio().Result;
            var profesionales = ReadAllProfesionales().Result;
            var empresas = ReadAllEmpresas().Result;
            var suc = ReadAllSucursales().Result;
            var serv = ReadAllServicios().Result;
            var clientes = ReadAllClientes().Result;
            ReporteActividadesVM reporteActividades = new ReporteActividadesVM();
            List<EmpresaReporteVM> empresaReportes = new List<EmpresaReporteVM>();


            foreach (var empresa in empresas)
            {
                EmpresaReporteVM empresaReporteVM = new EmpresaReporteVM();

                empresaReporteVM.NombreEmpresa = empresa.Nombre;
                empresaReporteVM.EmailEmpresa = empresa.Email;
                empresaReporteVM.EstadoEmpresa = empresa.Estado;
                empresaReporteVM.IdEmpresa = empresa.Id;
                List<Sucursal> sucursales = new List<Sucursal>(suc);
                sucursales.RemoveAll(d => d.EmpresaId != empresa.Id);
                List<SucursalReporteVM> sucursalReportes = new List<SucursalReporteVM>();
                foreach (var sucursal in sucursales)
                {
                    SucursalReporteVM sucursalReporteVM = new SucursalReporteVM();
                    sucursalReporteVM.NombreSucursal = sucursal.Nombre;
                    sucursalReporteVM.DireccionSucursal = sucursal.Direccion;
                    sucursalReporteVM.TelefonoSucursal = sucursal.Telefono;
                    sucursalReporteVM.EstadoSucursal = sucursal.Estado;
                    sucursalReporteVM.IdSucursal = sucursal.Id;
                    List<Servicio> servicios = new List<Servicio>(serv);

                    foreach (var item in servicios)
                    {
                        ServicioReporteVM servicio = new ServicioReporteVM();
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
                                servicio.Profesional = item3.Nombre + ' ' + item3.Apellidos;
                            }
                        }
                        foreach (var item4 in clientes)
                        {
                            if (item4.Id == item.ClienteId)
                            {
                                foreach (var item5 in sucursales)
                                {
                                    if (item5.Id == item4.SucursalId)
                                    {
                                        servicio.Sucursal = item5.Nombre;
                                    }
                                }
                                servicio.Cliente = item4.Nombre + ' ' + item4.Apellidos;
                                servicio.TelefonoCliente = item4.Telefono;
                            }
                        }
                        servicio.Fecha = item.Fecha.ToShortDateString();
                        servicio.HoraInicio = item.HoraInicio.ToString("HH:mm");
                        servicio.HoraTermino = item.HoraTermino.ToString("HH:mm");
                        servicio.Adicional = item.Adicional == 0 ? false : true;
                        if (servicio.Sucursal == sucursal.Nombre)
                        {
                            sucursalReporteVM.Servicios.Add(servicio);
                        }
                    }
                    empresaReporteVM.Sucursales.Add(sucursalReporteVM);
                }
                reporteActividades.Empresas.Add(empresaReporteVM);
            }
            return reporteActividades;
        }
    }
}

