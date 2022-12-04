using System.Data;

namespace AppWeb.Models.ViewModel.Profesional
{
    public class ServiciosTablasVM
    {
        public int Id { get; set; }
        public string TipoServicio { get; set; }
        public string EmpleadoNombre { get; set; }
        public string EmpresaNombre { get; set; }
        public string Fecha { get; set; }
        public int EstadoServicio { get; set; }
        public string EstadoServicioStr { get; set; }
        public int Adicional { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public int EmpleadoId { get; set; }
        public string ClienteNombre { get; set; }
        public int ClienteId { get; set; }
        public int EmpresaId { get; set; }
        public string SucursalNombre { get; set; }
        public int SucursalId { get; set; }
    }
}
