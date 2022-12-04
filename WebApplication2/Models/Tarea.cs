namespace AppWeb.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool isChecked { get; set; } 
        public int Estado { get; set; } 
    }
}
