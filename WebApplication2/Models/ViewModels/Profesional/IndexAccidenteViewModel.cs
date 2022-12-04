using System;

namespace AppWeb.Models
{
    public class IndexAccidenteViewModel
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Sucursal { get; set; }
        public string Empresa { get; set; }
        public string Cliente { get; set; }
        public int Accidentados { get; set; }
        public string Fecha { get; set; }
        public string Telefono { get; set; }
        public string Profesional { get; set; }
        public string Estado { get; set; }
    }
}
