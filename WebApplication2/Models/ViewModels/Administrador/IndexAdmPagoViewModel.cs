using System;

namespace AppWeb.Models.ViewModels.Administrador
{
    public class IndexAdmPagoViewModel
    {
        public int      Id { get; set; }
        public string   NombreEmpresa { get; set; }
        public string   NombreSucursal { get; set; }
        public string Fecha { get; set; }
        public string FechaPago { get; set; }
        public string FechaLimite { get; set; }
        public string MetodoPago { get; set; }
        public string EstadoPago { get; set; }
        public int MontoTotal { get; set; }
    }
}
