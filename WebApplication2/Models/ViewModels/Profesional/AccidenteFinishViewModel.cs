using System;

namespace AppWeb.Models
{
    public class AccidenteFinishViewModel
    {
        public int Id { get; set; }
        public string Comentario { get; set; }
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }

    }
}
