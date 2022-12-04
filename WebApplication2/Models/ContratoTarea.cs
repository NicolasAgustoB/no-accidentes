namespace AppWeb.Models
{
    public class ContratoTarea
    {
        public int Id { get; set; }
        public int Estado { get; set; }
        public int TareaId { get; set; }
        public int ContratoId { get; set; }
        public string Nombre { get; set; }
    }
}
