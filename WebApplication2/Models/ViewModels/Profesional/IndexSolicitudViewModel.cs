using Microsoft.VisualBasic;
using System;

namespace AppWeb.Models
{
    public class IndexSolicitudViewModel
    {
        public int Id { get; set; }
        public string Situacion { get; set; }
        public string Descripcion { get; set; }
        public string FechaInicio { get; set; }
        public string FechaTermino { get; set; }
        public string Respuesta { get; set; }
        public string TipoSolicitud { get; set; }
        public string Empleado { get; set; }
        public string Cliente { get; set; }
    }
}
