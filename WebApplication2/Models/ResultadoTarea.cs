using System;

namespace AppWeb.Models
{
    public class ResultadoTarea
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public string Comentario { get; set; }
        public int ServicioId { get; set; }
        public int ContratoTareaId { get; set; }
    }
}