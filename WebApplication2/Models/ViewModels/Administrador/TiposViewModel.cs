using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models.ViewModels.Administrador
{
    public class TiposViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debe ingresar un valor")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese un valor valido")]
        public int Valor { get; set; }
        [Required]
        public int Estado { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public string EstadoStr { get; set; }
    }
}
