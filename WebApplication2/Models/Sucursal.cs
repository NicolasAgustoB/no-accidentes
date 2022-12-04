namespace AppWeb.Models
{
    public class Sucursal
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Trabajadores { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int EmpresaId { get; set; }
        public int Estado { get; set; }
    }
}
