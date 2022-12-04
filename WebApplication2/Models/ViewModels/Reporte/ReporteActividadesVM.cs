using System;
using System.Collections.Generic;

namespace AppWeb.Models.ViewModels.Reporte
{
    public class ReporteActividadesVM
    {
        public List<EmpresaReporteVM> Empresas { get; set; } = new List<EmpresaReporteVM>();
    }
    public class EmpresaReporteVM {
        public int IdEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string EmailEmpresa { get; set; }
        public int EstadoEmpresa { get; set; }
        public List<SucursalReporteVM> Sucursales { get; set; } = new List<SucursalReporteVM>();
    }

    public class SucursalReporteVM {
        public int IdSucursal { get; set; }
        public string NombreSucursal { get; set; }
        public string TelefonoSucursal { get; set; }
        public string DireccionSucursal { get; set; }
        public int TrabajadoresSucursal { get; set; }
        public int EstadoSucursal { get; set; }
        public List<ServicioReporteVM> Servicios { get; set; } = new List<ServicioReporteVM>();
        public List<AccidenteReporteVM> Accidentes { get; set; } = new List<AccidenteReporteVM>();
    }

    public class ServicioReporteVM
    {
        public int Id { get; set; }
        public string TipoServicio { get; set; }
        public string Profesional { get; set; }
        public string Cliente { get; set; }
        public int IdCliente { get; set; }
        public string Sucursal { get; set; }
        public string Fecha { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public bool Adicional { get; set; }
        public string Estado { get; set; }
        public string TelefonoCliente { get; set; }

    }
    public class AccidenteReporteVM
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string TipoAccidente { get; set; }
        public string Profesional { get; set; }
        public string Cliente { get; set; }
        public int IdCliente { get; set; }
        public string Sucursal { get; set; }
        public string Fecha { get; set; }
        public int Accidentados { get; set; }
        public string Comentario { get; set; }
        public string TelefonoCliente { get; set; }
    }
}
