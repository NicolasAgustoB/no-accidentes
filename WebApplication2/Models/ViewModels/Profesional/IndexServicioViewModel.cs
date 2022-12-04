using System;

namespace AppWeb.Models.ViewModels.Profesional
{
    public class IndexServicioViewModel
    {
        public int Id { get; set; }
        public string TipoServicio { get; set; }
        public string Profesional { get; set; }
        public string Empresa { get; set; }
        public string Cliente { get; set; }
        public int IdCliente { get; set; }
        public string Sucursal { get; set; }
        public string Fecha { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public bool Adicional { get; set; }
        public string Estado { get; set; }
    }
}
