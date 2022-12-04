namespace AppWeb.Models.ViewModels.Administrador
{
    public class IndexAdmClienteViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rut { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public int      IdEmpresa { get; set; }
        public string   NombreEmpresa { get; set; }
        public string   RutEmpresa { get; set; }
        public string   RubroEmpresa { get; set; }
        public string   EmailEmpresa { get; set; }
        public string   DireccionEmpresa { get; set; }
        public string   TelefonoEmpresa { get; set; }
        public int      EstadoEmpresa { get; set; }
        public string   NameEstadoEmpresa { get; set; }
        public int      IdSucursal { get; set; }
        public string   NombreSucursal { get; set; }
        public int      TrabajadoresSucursal { get; set; }
        public string   DireccionSucursal { get; set; }
        public int   EmpresaIdSucursal { get; set; }
        public string   TelefonoSucursal { get; set; }
        public int      EstadoSucursal { get; set; }
        public string   NameEstadoSucursal { get; set; }
    }
}
