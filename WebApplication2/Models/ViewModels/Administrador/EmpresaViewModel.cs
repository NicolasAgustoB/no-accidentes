using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models.ViewModels.Administrador
{
    public class EmpresaViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo se permiten letras")]
        [Remote(action: "EmpresaValidarNombre", controller: "Validaciones", AdditionalFields = "Id")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese un Rut")]
        [Remote(action: "EmpresaValidarRut", controller: "Validaciones", AdditionalFields = "Id")]
        public string Rut { get; set; }
        [Required(ErrorMessage = "Ingrese un Rubro")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo se permiten letras")]
        public string Rubro { get; set; }
        [Required(ErrorMessage = "Ingrese un email")]
        [EmailAddress]
        [Remote(action: "EmpresaValidarEmail", controller: "Validaciones", AdditionalFields = "Id")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Ingrese una dirección")]
        [RegularExpression(@"^([a-zA-Z]+\s#?[0-9]+,\s[a-zA-Z]+)$", ErrorMessage = "El formato para la direccion es: 'Calle #13, Maipu'")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Ingrese un numero de telefono")]
        [RegularExpression(@"^+?569?[0-9]{8}$", ErrorMessage = "El formato para el telefono es +569xxxxxxx")]
        [Remote(action: "EmpresaValidarTelefono", controller: "Validaciones", AdditionalFields = "Id")]
        public string Telefono { get; set; }
        public int Estado { get; set; }
    }
}
