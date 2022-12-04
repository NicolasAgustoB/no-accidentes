using System;

namespace AppWeb.Models
{
    public class Accidente
    {
        public int Id { get; set; }
        public int Estado { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int Accidentados { get; set; }
        public string Comentario { get; set; }
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }

    }
}
