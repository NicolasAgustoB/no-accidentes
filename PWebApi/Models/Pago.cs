using System;

namespace PWebApi.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaLimite { get; set; }
        public string MetodoPago { get; set; }
        public int EstadoPago { get; set; }
        public int MontoTotal { get; set; }
        public int ContratoId { get; set; }
    }
}