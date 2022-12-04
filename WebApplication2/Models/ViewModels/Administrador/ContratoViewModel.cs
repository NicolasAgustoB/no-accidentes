using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models.ViewModels.Administrador
{
    public class ContratoViewModel
    {
        public int Id { get; set; }
        public int Estado { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [RegularExpression(@"^[a-zA-Z-\s]+$", ErrorMessage = "Solo se permiten letras")]
        public string Cuerpo { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public int Valor { get; set; }
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [Remote(action: "FechaTermino", controller: "Validaciones")]
        public DateTime FechaTermino { get; set; }
        [Required(ErrorMessage = "Seleccione una sucursal")]
        public int SucursalId { get; set; }
        public List<int> Tareas { get; set; }
    }
}
