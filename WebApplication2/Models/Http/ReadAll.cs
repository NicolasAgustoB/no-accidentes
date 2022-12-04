using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AppWeb.Models.ViewModel.Profesional;

namespace AppWeb.Models.Http
{
    public class ReadAll
    {

        public async Task<List<Empresa>> ReadAllEmpresas(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<List<ResultadoTarea>> ReadAllT(string token)
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<List<ContratoTarea>> ReadAllContratoTarea(string token)
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<List<TipoServicio>> ReadAllTipoServicio(string token)
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<List<Usuario>> ReadAllProfesionales(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<List<Contrato>> ReadAllContratos(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<List<ResultadoTarea>> ReadAllResultadoTareas(string token)
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<List<Usuario>> ReadAllClientes(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<List<Usuario>> ReadAllUsuarios(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("https://localhost:44319/api/usuario/readall", HttpCompletionOption.ResponseHeadersRead);
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

        public async Task<List<Sucursal>> ReadAllSucursales(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("https://localhost:44319/api/sucursal/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var sucursales = JsonConvert.DeserializeObject<List<Sucursal>>(respuesta.Data.ToString());
                return sucursales;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<Servicio>> ReadAllServicios(string token)
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<List<ServiciosTablasVM>> ReadAllProximosServicios(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task<List<ActividadMejora>> ReadAllActividadMejora(string token)
        {
            try
            {
                HttpClient tipo = new HttpClient();
                tipo.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await tipo.GetAsync("https://localhost:44319/api/actividadmejora/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var actividadMejora = JsonConvert.DeserializeObject<List<ActividadMejora>>(respuesta.Data.ToString());
                return actividadMejora;
            }
            catch (System.Exception)
            {
                return null;

            }

        }
        public async Task<List<Solicitud>> ReadAllSolicitudes(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("https://localhost:44319/api/solicitud/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var solicitudes = JsonConvert.DeserializeObject<List<Solicitud>>(respuesta.Data.ToString());
                return solicitudes;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<Accidente>> ReadAllAccidentes(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("https://localhost:44319/api/accidente/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var accidentes = JsonConvert.DeserializeObject<List<Accidente>>(respuesta.Data.ToString());
                return accidentes;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<Solicitud>> ReadAllSolicitudesProfesional(string token,Usuario profesional)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("https://localhost:44319/api/contrato/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var solicitudes = JsonConvert.DeserializeObject<List<Solicitud>>(respuesta.Data.ToString());
                solicitudes.RemoveAll(d => d.EmpleadoId != profesional.Id);
                return solicitudes;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
        public async Task<List<TipoSolicitud>> ReadAllTipoSolicitudes(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("https://localhost:44319/api/tiposolicitud/readall", HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<Respuesta>(content);
                var tipoSolicitudes = JsonConvert.DeserializeObject<List<TipoSolicitud>>(respuesta.Data.ToString());
                return tipoSolicitudes;
            }
            catch (System.Exception)
            {
                return null;

            }
        }
    }
}
