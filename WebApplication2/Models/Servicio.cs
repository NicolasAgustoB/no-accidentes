using System;
using System.Collections.Generic;

namespace AppWeb.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public int Estado { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraTermino { get; set; }
        public DateTime Fecha { get; set; }
        public int Adicional { get; set; }
        public string Descripcion { get; set; }
        public string Informe { get; set; }
        public string Comentario { get; set; }
        public int Asistentes { get; set; }
        public string Material { get; set; }
        public int TipoServicioId { get; set; }
        public int ClienteId { get; set; }
        public int EmpleadoId { get; set; }
        public int PagoId { get; set; }
        public string FechaHora { get; set; }
        public List<ResultadoTarea> ResultadoTareas { get; set; }
    }
}
