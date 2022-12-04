using Microsoft.VisualBasic;
using System;

namespace AppWeb.Models
{
    public class SolicitudReadViewModel
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public string Empleado { get; set; }
        public string Respuesta { get; set; }
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }
        public int Situacion { get; set; }
    }
}
