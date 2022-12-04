using System.Drawing;
using System;
using System.Collections.Generic;

namespace PWebApi.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public int Estado { get; set; }
        public string Cuerpo { get; set; }
        public int Valor { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public int SucursalId { get; set; }
        public List<ContratoTarea> ContratoTareas { get; set; }

    }
}