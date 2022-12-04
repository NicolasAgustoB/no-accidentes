using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models.ViewModels.Administrador
{
    public class ProfesionalViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre")]
        [RegularExpression(@"^[a-zA-Z-\s]+$", ErrorMessage = "Solo se permiten letras")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese un apellido")]
        [RegularExpression(@"^[a-zA-Z-\s]+$", ErrorMessage = "Solo se permiten letras")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre de usuario")]
        [RegularExpression(@"^[a-zA-Z0-9_.-]*$", ErrorMessage = "Solo se permiten letras, números, guiones y puntos ")]
        [Remote(action: "UsuarioValidarNombreUsuario", controller: "Validaciones", AdditionalFields = "Id")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Ingrese una contraseña")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Ingrese un email")]
        [EmailAddress]
        [Remote(action: "UsuarioValidarEmail", controller: "Validaciones", AdditionalFields = "Id")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Ingrese un Rut")]
        [Remote(action: "UsuarioValidarRut", controller: "Validaciones", AdditionalFields = "Id")]
        public string Rut { get; set; }
        [Required(ErrorMessage = "Ingrese un numero de telefono")]
        [RegularExpression(@"^+?569?[0-9]{8}$", ErrorMessage = "El formato para el telefono es +569xxxxxxx")]
        public string Telefono { get; set; }
        public string rolId { get; set; }
        public string SucursalId { get; set; }
        public int Estado { get; set; }
    }
}
