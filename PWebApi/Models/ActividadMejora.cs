namespace PWebApi.Models
{
    public class ActividadMejora
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Situacion { get; set; }
        public string Comentario { get; set; }
        public int ServicioId { get; set; }

    }
}
