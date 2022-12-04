using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using AppWeb.Models;
using System;
using System.Text;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppWeb.Models.ViewModels.Administrador;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using AppWeb.Models.ViewModel.Profesional;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;

namespace AppWeb.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministradorController : Controller
    {
        public IActionResult IndexAdm()
        {
            var result = ReadAllProximosServicios().Result;
            var idUsuario = int.Parse(HttpContext.Session.GetString("Id"));
            ViewBag.Proximos = result;

            return View();
        }
        public IActionResult IndexAdmCliente()
        {
            List<IndexAdmClienteViewModel> tablaEmpresaViewModel = new List<IndexAdmClienteViewModel>();
            List<IndexAdmClienteViewModel> tablaSucursalViewModel = new List<IndexAdmClienteViewModel>();
            List<IndexAdmClienteViewModel> tablaClienteViewModel = new List<IndexAdmClienteViewModel>();

            var empresas = ReadAllEmpresas().Result;
            var sucursales = ReadAllSucursales().Result;
            var clientes = ReadAllClientes().Result;
            foreach (var item in empresas)
            {
                IndexAdmClienteViewModel empresa = new IndexAdmClienteViewModel();
                empresa.IdEmpresa = item.Id;
                empresa.NombreEmpresa = item.Nombre;
                empresa.EmailEmpresa = item.Email;
                empresa.RutEmpresa = item.Rut;
                empresa.TelefonoEmpresa = item.Telefono;
                empresa.EstadoEmpresa = item.Estado;
                empresa.NameEstadoEmpresa = item.Estado == 0 ? "Inactiva": "Activa";
                empresa.RubroEmpresa = item.Rubro;
                empresa.DireccionEmpresa = item.Direccion;
                tablaEmpresaViewModel.Add(empresa);
            }
            foreach (var item in sucursales)
            {
                IndexAdmClienteViewModel sucursal = new IndexAdmClienteViewModel();
                sucursal.IdSucursal = item.Id;
                sucursal.NombreSucursal = item.Nombre;
                sucursal.TrabajadoresSucursal = item.Trabajadores;
                sucursal.DireccionSucursal = item.Direccion;
                sucursal.EmpresaIdSucursal = item.EmpresaId;
                sucursal.EstadoSucursal = item.Estado;
                sucursal.NameEstadoSucursal = item.Estado == 0 ? "Inactiva" : "Activa";
                tablaSucursalViewModel.Add(sucursal);

            }
            foreach (var item in clientes)
            {
                IndexAdmClienteViewModel cliente = new IndexAdmClienteViewModel();
                cliente.Id = item.Id;
                cliente.Nombre = item.Nombre +" "+item.Apellidos;
                cliente.Rut = item.Rut;
                cliente.Email = item.Email;
                cliente.Telefono = item.Telefono;
                tablaClienteViewModel.Add(cliente);

            }
            ViewBag.Cliente = tablaClienteViewModel;
            ViewBag.Sucursal = tablaSucursalViewModel;
            ViewBag.Empresa = tablaEmpresaViewModel;
            return View();
        }
        public IActionResult IndexAdmProfesional()
        {
            var profesionales = ReadAllProfesionales().Result;
            List<IndexAdmProfesionalViewModel> tablaProfesionalesViewModel = new List<IndexAdmProfesionalViewModel>();
            foreach (var item in profesionales)
            {
                IndexAdmProfesionalViewModel profesional = new IndexAdmProfesionalViewModel();
                profesional.Id = item.Id;
                profesional.Nombre = item.Nombre+" "+item.Apellidos;
                profesional.UserName = item.UserName;
                profesional.Email = item.Email;
                profesional.Telefono = item.Telefono;
                profesional.Estado = item.Estado == 1 ? "Activo" : "Inactivo";
                tablaProfesionalesViewModel.Add(profesional);
            }
            ViewBag.Contrato = tablaProfesionalesViewModel;
            ViewBag.Profesional = tablaProfesionalesViewModel;
            return View();
        }
        public IActionResult IndexAdmContrato()
        {

            var contratos = ReadAllContratos().Result;
            List<IndexAdmContratoViewModel> tablaContratosViewModel = new List<IndexAdmContratoViewModel>();
            foreach (var item in contratos)
            {
                var sucursales = ReadAllSucursales().Result;
                var empresas = ReadAllEmpresas().Result;
                sucursales.RemoveAll(d => d.Id != item.SucursalId);
                empresas.RemoveAll(d => d.Id != sucursales[0].EmpresaId);
                IndexAdmContratoViewModel contrato = new IndexAdmContratoViewModel();
                contrato.IdContrato = item.Id;
                contrato.EstadoContrato = item.Estado == 0 ? "Inactivo": item.Estado == 1 ? "Activo": "Suspendido";
                contrato.FechaInicio = item.FechaInicio.ToShortDateString();
                contrato.FechaTermino = item.FechaTermino.ToShortDateString() == "01-01-2001" ? "N/A": item.FechaTermino.ToShortDateString();
                contrato.NombreSucursal = sucursales[0].Nombre;
                contrato.NombreEmpresa = empresas[0].Nombre;
                tablaContratosViewModel.Add(contrato);
            }
            ViewBag.Contrato = tablaContratosViewModel;
            return View();

        }
        public IActionResult IndexAdmServicio()
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
                    if(item.TipoServicioId == item2.Id) 
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
                                foreach(var item6 in empresas)
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
                servicio.Adicional = item.Adicional == 0 ? false: true;
                tablaServicioViewModel.Add(servicio);
            }
            ViewBag.Servicio = tablaServicioViewModel;
            return View();
        }
        public IActionResult IndexAdmPago()
        {
            List<IndexAdmPagoViewModel> listPagoVM = new List<IndexAdmPagoViewModel>();
            List<Pago> pag = ReadAllPago().Result;
            List<Contrato> con = ReadAllContratos().Result;
            List<Sucursal> suc = ReadAllSucursales().Result;
            List<Empresa> emp = ReadAllEmpresas().Result;
            foreach (var item in pag)
            {
                List<Contrato> contratos = new List<Contrato>(con);
                List<Sucursal> sucursales = new List<Sucursal>(suc);
                List<Empresa> empresas = new List<Empresa>(emp);

                contratos.RemoveAll(d => d.Id != item.ContratoId);
                sucursales.RemoveAll(d => d.Id != contratos[0].SucursalId);
                empresas.RemoveAll(d => d.Id != sucursales[0].EmpresaId);

                IndexAdmPagoViewModel indexAdmPago = new IndexAdmPagoViewModel();
                indexAdmPago.Id = item.Id;
                indexAdmPago.NombreEmpresa = empresas[0].Nombre;
                indexAdmPago.NombreSucursal = sucursales[0].Nombre;
                indexAdmPago.MetodoPago = item.MetodoPago;
                indexAdmPago.FechaPago = item.FechaPago.ToShortDateString() == "01-01-2001" ? "Pendiente" : item.FechaPago.ToShortDateString();
                indexAdmPago.Fecha = item.Fecha.ToShortDateString();
                indexAdmPago.FechaLimite = item.FechaLimite.ToShortDateString();
                indexAdmPago.EstadoPago = item.EstadoPago == 0 ? "Pendiente" : item.EstadoPago == 1 ? "Realizado" : "Atrasado";
                indexAdmPago.MontoTotal = item.MontoTotal;
                listPagoVM.Add(indexAdmPago);
            }
            ViewBag.Pago = listPagoVM;
            return View();
        }
        public IActionResult AdministrarEmpresa(Empresa empresa)
        {
            var empresas = ReadAllEmpresas().Result;
            empresas.RemoveAll(d => d.Id != empresa.Id);
            if (empresas.Count > 0) {
            ViewBag.Empresa = empresas[0];
            }
            else
            {
                Empresa empresavacia = new Empresa();
                ViewBag.Empresa = empresavacia;
            }
            return View();
        }
        public IActionResult AdministrarSucursal(Sucursal sucursal)
        {
            var empresasCreate = ReadAllEmpresas().Result;
            var empresas = ReadAllEmpresas().Result;
            var sucursales = ReadAllSucursales().Result;
            sucursales.RemoveAll(d => d.Id != sucursal.Id);
            empresasCreate.RemoveAll(d => d.Id != sucursal.EmpresaId);
            if (empresasCreate.Count > 0)
            {
                ViewBag.EmpresaCreate = empresasCreate[0];
            }
            else
            {
                Empresa empresa = new Empresa();
                ViewBag.EmpresaCreate = empresa;
            }
            if (sucursales.Count > 0)
            {
                ViewBag.Sucursal = sucursales[0];
            }
            else
            {
                Sucursal sucursalVacia = new Sucursal();
                ViewBag.Sucursal = sucursalVacia;
            }
            ViewBag.Empresas = empresas;
            ViewBag.ValidacionSucursal = sucursal.Id > 0 ? 1 : 0;
            return View();
        }
        public IActionResult AdministradorContrato(Contrato contrato)
        {
            List<Tarea> tareasList = new List<Tarea>();
            var tareas = ReadAllTareas().Result;
            tareas.RemoveAll(d => d.Estado != 1);
            var sucusales = ReadAllSucursales().Result;
            var contratos = ReadAllContratos().Result;
            var contratoTareas = ReadAllContratoTarea().Result;
            contratos.RemoveAll(d => d.Id != contrato.Id);
            if (contratos.Count > 0){
                contratoTareas.RemoveAll(d => d.ContratoId != contrato.Id);
                foreach (var item in tareas)
                {
                    Tarea tarea = new Tarea();
                    tarea.Id = item.Id;
                    tarea.Nombre = item.Nombre;
                    tarea.Descripcion = item.Descripcion;
                    tarea.isChecked = false;
                    foreach (var item2 in contratoTareas)
                    {
                        if (item2.TareaId == item.Id && item2.Estado == 1)
                        {
                            tarea.isChecked = true;
                         }
                    }
                    tarea.Estado = item.Estado;
                    tareasList.Add(tarea);
                }
                ViewBag.Contrato = contratos[0];
            }
            else
            {
                Contrato c = new Contrato();
                ViewBag.Contrato = c;
            }
            ViewBag.TareasContrato = tareasList;
            ViewBag.Sucursal = sucusales;
            ViewBag.Tareas = tareas;
            return View();
        }
        public IActionResult AdministrarProfesional(Usuario profesional)
        {
            var profesionales = ReadAllProfesionales().Result;
            profesionales.RemoveAll(d => d.Id != profesional.Id);
            if (profesionales.Count > 0)
            {
                ViewBag.Profesional = profesionales[0];
            }
            else
            {
                Usuario c = new Usuario();
                ViewBag.Profesional = c;
            }
            return View();
        }
        public IActionResult AdministrarCliente(Usuario cliente)
        {

            var clientes = ReadAllClientes().Result;
            var empresas = ReadAllEmpresas().Result;
            var empresaRead = ReadAllEmpresas().Result;
            var sucursal = ReadAllSucursales().Result;
            empresas.RemoveAll(d => d.Estado != 1);
            if (cliente.Id != 0)
            {
                clientes.RemoveAll(d => d.Id != cliente.Id);
                sucursal.RemoveAll(d => d.Id != clientes[0].SucursalId);
                empresaRead.RemoveAll(d => d.Id != sucursal[0].EmpresaId);
                ViewBag.Empresa = empresaRead[0];
                ViewBag.Sucursal = sucursal[0];
                ViewBag.Cliente = clientes[0];
            }
            else if (cliente.SucursalId != 0)
            {
                Usuario clienteVacio = new Usuario();
                sucursal.RemoveAll(d => d.Id != cliente.SucursalId);
                empresaRead.RemoveAll(d => d.Id != sucursal[0].EmpresaId);
                ViewBag.Empresa = empresaRead[0];
                ViewBag.Sucursal = sucursal[0];
                ViewBag.Cliente = clienteVacio;
            }
            else
            {
                Sucursal sucursalVacia = new Sucursal();
                Empresa empresaVacia = new Empresa();
                Usuario clienteVacio = new Usuario();
                ViewBag.Sucursal = sucursalVacia;
                ViewBag.Empresa = empresaVacia;
                ViewBag.Cliente = clienteVacio;
            }
            ViewBag.Empresas = empresas;
            return View();

        }
        public IActionResult AdministrarSistema()
        {
            List<TiposViewModel> tipoServicioViewModels = new List<TiposViewModel>();
            List<TiposViewModel> tipoSolicitudViewModels = new List<TiposViewModel>();
            var tiposerv = ReadAllTipoServicio().Result;
            var tiposol = ReadAllTipoSolicitud().Result;
            foreach (var item in tiposerv)
            {
                TiposViewModel tipoServicio = new TiposViewModel();
                tipoServicio.Id = item.Id;
                tipoServicio.Nombre = item.Nombre;
                tipoServicio.Valor = item.Valor;
                tipoServicio.EstadoStr = item.Estado == 0 ? "Deshabilitado" : "Habilitado";
                tipoServicio.Descripcion = item.Descripcion;
                tipoServicioViewModels.Add(tipoServicio);
            }
            foreach (var item in tiposol)
            {
                TiposViewModel tipoSolicitud = new TiposViewModel();
                tipoSolicitud.Id = item.Id;
                tipoSolicitud.Nombre = item.Nombre;
                tipoSolicitud.EstadoStr = item.Estado == 0 ? "Deshabilitado" : "Habilitado";
                tipoSolicitud.Descripcion = item.Descripcion;
                tipoSolicitudViewModels.Add(tipoSolicitud);
            }
            ViewBag.TipoServicio = tipoServicioViewModels;
            ViewBag.TipoSolicitud = tipoSolicitudViewModels;
            return View();
        }

        public IActionResult AdministrarServicio(Servicio servicio)
        {
            var tiposervicios = ReadAllTipoServicio().Result;
            var profesionales = ReadAllProfesionales().Result;
            var empresas = ReadAllEmpresas().Result;
            var sucursales = ReadAllSucursales().Result;
            var clientes = ReadAllClientes().Result;
            var servicios = ReadAllServicios().Result;
            empresas.RemoveAll(d => d.Estado != 1);
            servicios.RemoveAll(d => d.Id != servicio.Id);
            if (servicios.Count > 0)
            {
                ViewBag.Servicio = servicios[0];
            }
            else
            {
                Servicio c = new Servicio();
                ViewBag.Servicio = c;
            }
            ViewBag.Empresas = empresas;
            ViewBag.Sucursales = sucursales;
            ViewBag.Cliente = clientes;
            ViewBag.Profesional = profesionales;
            ViewBag.TipoServicio = tiposervicios;

            return View();
        }
        public IActionResult AdministrarServicioRead(Servicio servicio)
        {
            var tareas = ReadAllT().Result;
            var contratotareas = ReadAllContratoTarea().Result;
            var tiposervicios = ReadAllTipoServicio().Result;
            var profesionales = ReadAllProfesionales().Result;
            var sucursales = ReadAllSucursales().Result;
            var empresas = ReadAllEmpresas().Result;
            var clientes = ReadAllClientes().Result;
            var servicios = ReadAllServicios().Result;
            var contratos = ReadAllContratos().Result;
            var resultadotareas = ReadAllResultadoTareas().Result;
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
            ViewBag.TipoServicio = tiposervicios[0] ;
            ViewBag.Tareas = tareas;

            return View();
        }
        public IActionResult AdministrarPagoRead(Pago pago)
        {
            var pagos = ReadAllPago().Result;
            var contratos = ReadAllContratos().Result;
            var sucursales = ReadAllSucursales().Result;
            var empresas = ReadAllEmpresas().Result;
            pagos.RemoveAll(d => d.Id != pago.Id);
            contratos.RemoveAll(d => d.Id != pagos[0].ContratoId);
            sucursales.RemoveAll(d => d.Id != contratos[0].SucursalId);
            empresas.RemoveAll(d => d.Id != sucursales[0].EmpresaId);

            IndexAdmPagoViewModel indexAdmPago = new IndexAdmPagoViewModel();
            indexAdmPago.Id = pagos[0].Id;
            indexAdmPago.NombreEmpresa = empresas[0].Nombre;
            indexAdmPago.NombreSucursal = sucursales[0].Nombre;
            indexAdmPago.MetodoPago = pagos[0].MetodoPago;
            indexAdmPago.FechaPago = pagos[0].FechaPago.ToShortDateString() == "01-01-2001" ? "Pendiente" : pagos[0].FechaPago.ToShortDateString();
            indexAdmPago.Fecha = pagos[0].Fecha.ToShortDateString();
            indexAdmPago.FechaLimite = pagos[0].FechaLimite.ToShortDateString();
            indexAdmPago.EstadoPago = pagos[0].EstadoPago == 0 ? "Pendiente" : pagos[0].EstadoPago == 1 ? "Realizado" : "Atrasado";
            indexAdmPago.MontoTotal = pagos[0].MontoTotal;

            ViewBag.Pago = indexAdmPago;

            var servicios = ReadAllServicios().Result;
            var tiposervicios = ReadAllTipoServicio().Result;
            var profesionales = ReadAllProfesionales().Result;
            var empresasServ = ReadAllEmpresas().Result;
            var sucursalesServ = ReadAllSucursales().Result;
            var clientes = ReadAllClientes().Result;
            List<PagoReadTablaViewModel> tablaServicioViewModel = new List<PagoReadTablaViewModel>();
            servicios.RemoveAll(d => d.PagoId != pagos[0].Id);
            foreach (var item in servicios)
            {
                PagoReadTablaViewModel servicio = new PagoReadTablaViewModel();
                servicio.Id = item.Id;
                servicio.Estado = item.Estado == 0 ? "Pendiente" : item.Estado == 1 ? "Realizada" : item.Estado == 2 ? "Cancelada" : "Atrasada";
                foreach (var item2 in tiposervicios)
                {
                    if (item.TipoServicioId == item2.Id)
                    {
                        servicio.TipoServicio = item2.Nombre;
                        servicio.Valor = item2.Valor.ToString();
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
                        foreach (var item5 in sucursalesServ)
                        {
                            if (item5.Id == item4.SucursalId)
                            {
                                foreach (var item6 in empresasServ)
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
                servicio.Valor = servicio.Adicional == false ? "Contrato" : servicio.Valor;
                tablaServicioViewModel.Add(servicio);
            }
            ViewBag.Servicio = tablaServicioViewModel;
            return View();
        }

        
        [HttpPost]
        public async Task<RedirectToActionResult> CreateTipoServicio(TipoServicio tpServicio)
        {
            if (ModelState.IsValid)
            {
                var tpJson = JsonConvert.SerializeObject(tpServicio);
                var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await client.PostAsync("https://localhost:44319/api/tiposervicio/create", requestContent);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                return RedirectToAction("AdministrarSistema", "Administrador");

            }
            else
            {
                return RedirectToAction("AdministrarSistema", "Administrador");
            }
        }
        [HttpPost]
        public async Task<RedirectToActionResult> CreateTipoSolicitud(TipoSolicitud tpSolicitud)
        {
            var tpJson = JsonConvert.SerializeObject(tpSolicitud);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("https://localhost:44319/api/tiposolicitud/create", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            Debug.WriteLine(respuesta);
            return RedirectToAction("AdministrarSistema", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> CreateCliente(Usuario cliente)
        {
            var tpJson = JsonConvert.SerializeObject(cliente);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("https://localhost:44319/api/usuario/create", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmCliente", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> CreateProfesional(Usuario profesional)
        {
            var tpJson = JsonConvert.SerializeObject(profesional);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("https://localhost:44319/api/usuario/create", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmProfesional", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> CreateEmpresa(Empresa empresa)
        {
            var tpJson = JsonConvert.SerializeObject(empresa);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("https://localhost:44319/api/empresa/create", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmCliente", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> CreateSucursal(Sucursal sucursal)
        {
            var tpJson = JsonConvert.SerializeObject(sucursal);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("https://localhost:44319/api/sucursal/create", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmCliente", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> CreateContrato(Contrato contrato)
        {
            var tareas = ReadAllTareas().Result;
            tareas.RemoveAll(d => d.Estado != 1);

            List<ContratoTarea> listTareas = new List<ContratoTarea>();
            foreach (var item in tareas)
            {
                ContratoTarea tarea = new ContratoTarea() { TareaId=item.Id, Estado=0, Nombre=item.Nombre};
                foreach (var item2 in contrato.Tareasid) { 
                    if (item.Id == item2)
                        {
                            tarea.Estado = 1;
                        }
                }
                listTareas.Add(tarea);
            }
            contrato.ContratoTareas = listTareas;
            var tpJson = JsonConvert.SerializeObject(contrato);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("https://localhost:44319/api/contrato/create", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmContrato", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> CreateServicio(ServicioViewModel servicio)
        {
            List<ResultadoTarea> listResultadoTareas = new List<ResultadoTarea>();
            var contratos  = ReadAllContratos().Result;
            var clientes = ReadAllClientes().Result;
            clientes.RemoveAll(x => x.Id != servicio.ClienteId);
            var contratotareas = ReadAllContratoTarea().Result;
            contratos.RemoveAll(x => x.Estado != 1);
            contratos.RemoveAll(x => x.SucursalId != clientes[0].SucursalId);
            contratotareas.RemoveAll(x => x.ContratoId != contratos[0].Id);
            contratotareas.RemoveAll(x => x.Estado != 1);
            Servicio serv = new Servicio();
            serv.Id = servicio.Id;
            serv.HoraInicio = servicio.HoraInicio;
            serv.TipoServicioId = servicio.TipoServicioId;
            serv.HoraTermino = servicio.HoraTermino;
            serv.Asistentes = servicio.Asistentes;
            serv.Fecha = servicio.Fecha;
            serv.Adicional = servicio.Adicional;
            serv.Descripcion = servicio.Descripcion;
            serv.Material = servicio.Material == null? "N/A": servicio.Material;
            serv.ClienteId = servicio.ClienteId;
            foreach (var item in contratotareas)
            {
                ResultadoTarea tarea = new ResultadoTarea();
                tarea.ServicioId = servicio.Id;
                tarea.ContratoTareaId = item.Id;
                tarea.Nombre = item.Nombre;
                tarea.Estado = item.Estado;
                listResultadoTareas.Add(tarea);

            }
            serv.ResultadoTareas = listResultadoTareas;
            serv.EmpleadoId = servicio.EmpleadoId;
            var tpJson = JsonConvert.SerializeObject(serv);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PostAsync("https://localhost:44319/api/servicio/create", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmServicio", "Administrador");
        }

        [HttpPost]
        public async Task<RedirectToActionResult> UpdateTipoSolicitud(TipoSolicitud tpSolicitud)
        {
            var tpJson = JsonConvert.SerializeObject(tpSolicitud);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/tiposolicitud/update", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("AdministrarSistema", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> UpdateTipoServicio(TipoServicio tpServicio)
        {
            var tpJson = JsonConvert.SerializeObject(tpServicio);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/tiposervicio/update", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("AdministrarSistema", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> UpdateEmpresa(Empresa empresa)
        {
            var tpJson = JsonConvert.SerializeObject(empresa);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/empresa/update", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmCliente", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> UpdateContrato(Contrato contrato)
        {
            var tareas = ReadAllTareas().Result;
            tareas.RemoveAll(d => d.Estado != 1);
            var contratotareas = ReadAllContratoTarea().Result;
            contratotareas.RemoveAll(d => d.ContratoId != contrato.Id);
            List<ContratoTarea> listTareas = new List<ContratoTarea>();
            foreach (var item in tareas)
            {
                ContratoTarea tarea = new ContratoTarea() { Estado = 0 };
                foreach (var item3 in contratotareas)
                {
                    if(item.Id == item3.TareaId)
                    {
                        tarea.Id = item3.Id;
                    }

                }
                foreach (var item2 in contrato.Tareasid)
                {
                    if (item.Id == item2)
                    {
                        tarea.Estado = 1;
                    }
                }
                listTareas.Add(tarea);
            }
            contrato.ContratoTareas = listTareas;
            var tpJson = JsonConvert.SerializeObject(contrato);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/contrato/update", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmContrato", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> UpdateSucursal(Sucursal sucursal)
        {
            var tpJson = JsonConvert.SerializeObject(sucursal);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/sucursal/update", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmCliente", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> UpdateServicio(Servicio servicio)
        {
            servicio.Material = servicio.Material == null ? "N/A" : servicio.Material;
            var tpJson = JsonConvert.SerializeObject(servicio);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/servicio/updateadmin", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmServicio", "Administrador");
        }
        [HttpPost]
        public async Task<RedirectToActionResult> UpdateProfesional(Usuario profesional)
        {
            var tpJson = JsonConvert.SerializeObject(profesional);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/usuario/update", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmProfesional", "Administrador");
        }
        public async Task<RedirectToActionResult> DeleteTipoServicio(TipoServicio tpServicio)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.DeleteAsync("https://localhost:44319/api/tiposervicio/delete/"+tpServicio.Id);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            Debug.WriteLine(respuesta);
            return RedirectToAction("AdministrarSistema", "Administrador");
        }
        [HttpGet]
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
        public async Task<List<Usuario>> EmpleadosDisponibles(Servicio servicio)
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

        [HttpPost]
        public ActionResult ReadEmpleado(Servicio servicio)
        {
            try
            {
                var empleados = EmpleadosDisponibles(servicio).Result;

                var list = JsonConvert.SerializeObject(empleados, Formatting.None, new JsonSerializerSettings()
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
        public ActionResult ReadSucursal(string IdEmpresa)
        {
            try
            {
                var sucursales = ReadAllSucursales().Result;

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
                var sucursales = ReadAllSucursales().Result;
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
                var sucursales = ReadAllSucursales().Result;
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
                var clientes = ReadAllClientes().Result;
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
                var clientes = ReadAllClientes().Result;

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
        public ActionResult ReadEmpresas()
        {
            try
            {
                var clientes = ReadAllClientes().Result;
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
        public async Task<RedirectToActionResult> ConfirmarPago(Pago pago)
        {
            var tpJson = JsonConvert.SerializeObject(pago);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/pago/confirmarpago", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmPago", "Administrador");
        }
        public async Task<List<Pago>> ReadAllPago()
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
                var response = await tipo.GetAsync("https://localhost:44319/api/pago/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var pago = JsonConvert.DeserializeObject<List<Pago>>(respuesta.Data.ToString());

                return pago;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        [HttpPost]
        public async Task<RedirectToActionResult> FinalizarContrato(Contrato contrato)
        {
            var tpJson = JsonConvert.SerializeObject(contrato);
            var requestContent = new StringContent(tpJson, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var response = await client.PutAsync("https://localhost:44319/api/contrato/finalizar", requestContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
            return RedirectToAction("IndexAdmContrato", "Administrador");
        }

        public RedirectToActionResult NotificarAtraso(Pago pago)
        {

            var origen = "prevencionistas.grupo4@gmail.com";
            var pass = "eafbcunqccfgqsrb";
            var pagos = ReadAllPago().Result;
            var contratos = ReadAllContratos().Result;
            var sucursales = ReadAllSucursales().Result;
            var clientes = ReadAllClientes().Result;
            pagos.RemoveAll(d => d.Id != pago.Id);
            contratos.RemoveAll(d => d.Id != pagos[0].ContratoId);
            sucursales.RemoveAll(d => d.Id != contratos[0].SucursalId);
            clientes.RemoveAll(d => d.SucursalId != sucursales[0].Id);

            foreach (var item in clientes)
            {
                var destino = item.Email;

                MailMessage mailMessage = new MailMessage(origen, destino, "Atraso en el pago", "<p>Aun no ha realizado el pago de su cuenta</p> </br> <b>Su cuenta se inhabilitara hasta que realize el pago por el medio correspondiente</b>");
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(origen, pass);
                smtpClient.Send(mailMessage);
                smtpClient.Dispose();
            }
            return RedirectToAction("IndexAdmPago", "Administrador");
        }


    }
}
