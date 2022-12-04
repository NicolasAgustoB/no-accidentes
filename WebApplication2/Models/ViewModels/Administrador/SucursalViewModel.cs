using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace AppWeb.Models.ViewModels.Administrador
{
    public class SucursalViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        [RegularExpression(@"^[a-zA-Z-\s]+$", ErrorMessage = "Solo se permiten letras")]
        [Remote(action: "SucursalValidarNombre", controller: "Validaciones", AdditionalFields = "Id")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese el numero de trabajadores")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public int Trabajadores { get; set; }
        [Required(ErrorMessage = "Ingrese una dirección")]
        [RegularExpression(@"^([a-zA-Z]+\s#?[0-9]+,\s[a-zA-Z]+)$", ErrorMessage = "El formato para la direccion es: 'Calle #13, Maipu'")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Ingrese un numero de telefono")]
        [RegularExpression(@"^+?569?[0-9]{8}$", ErrorMessage = "El formato para el telefono es: '+569xxxxxxx'")]
        [Remote(action: "SucursalValidarTelefono", controller: "Validaciones", AdditionalFields = "Id")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Seleccione una empresa")]
        public int EmpresaId { get; set; }
        public int Estado { get; set; }
    }
}

