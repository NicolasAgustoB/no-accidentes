namespace AppWeb.Models
{
    public class Respuesta
    {
        public int Exito { get; set; } = 0;
        public string Mensaje { get; set; }
        public object Data { get; set; }
    }
}
