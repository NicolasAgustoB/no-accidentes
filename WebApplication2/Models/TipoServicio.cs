using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models
{
    public class TipoServicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [Range(0,int.MaxValue,ErrorMessage ="Ingrese un valor valido")]
        public int Valor { get; set; }
        public int Estado { get; set; }
        public string Descripcion { get; set; }
    }
}
