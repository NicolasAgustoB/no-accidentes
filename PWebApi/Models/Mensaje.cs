using System;

namespace PWebApi.Models
{
    public class Mensaje
    {
        public int Id { get; set; }
        public string Cuerpo { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public int AccidenteId { get; set; }
        public int UsuarioId { get; set; }
    }
}