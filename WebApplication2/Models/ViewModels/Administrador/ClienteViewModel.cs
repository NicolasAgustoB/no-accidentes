using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AppWeb.Models.ViewModels.Administrador
{
    public class ClienteViewModel
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
        [RegularExpression(@"^\+?569?[0-9]{8}$", ErrorMessage = "El formato para el telefono es +569xxxxxxx")]
        [Remote(action: "UsuarioValidarTelefono", controller: "Validaciones", AdditionalFields = "Id")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Seleccione una sucursal")]
        public int SucursalId { get; set; }
        [Required(ErrorMessage = "Seleccione una empresa")]
        public int EmpresaId { get; set; }
        public string NombreSucursal { get; set; }
        public string rolId { get; set; }
        public List<Empresa> Empresa { get; set; }
        public List<Sucursal> Sucursal { get; set; }
        public Contrato Contrato { get; set; }
        public ContratoTarea ContratoTarea { get; set; }
    }
}

