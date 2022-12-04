using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models.ViewModels.Profesional
{
    public class ServicioViewModel
    {
        public int Id { get; set; }
        public int Estado { get; set; }
        [Required(ErrorMessage = "Ingrese una hora de inicio")]
        [Remote(action: "Disponible", controller: "Validaciones", AdditionalFields = "Fecha")]
        public DateTime HoraInicio { get; set; }
        [Required(ErrorMessage = "Ingrese una hora de termino")]
        [Remote(action: "Horatermino", controller: "Validaciones", AdditionalFields = "HoraInicio")]
        public DateTime HoraTermino { get; set; }
        [Required(ErrorMessage = "Seleccione una fecha")]
        [Remote(action: "FechaActual", controller: "Validaciones")]
        public DateTime Fecha { get; set; }
        public int Adicional { get; set; }
        [Required(ErrorMessage = "Ingrese una descripcion")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Ingrese la cantidad de asistentes ")]
        public int Asistentes { get; set; }
        [Required(ErrorMessage = "Ingrese materiales necesarios ")]
        public string Material { get; set; }
        [Required(ErrorMessage = "Seleccione una actividad ")]
        public int TipoServicioId { get; set; }
        [Required(ErrorMessage = "Seleccione un cliente ")]
        [Remote(action: "HiddenInput", controller: "Validaciones")]
        public int ClienteId { get; set; }
        [Required(ErrorMessage = "Seleccione un profesional ")]

        public int EmpleadoId { get; set; }
        [Required(ErrorMessage = "Seleccione una empresa ")]
        public int EmpresaId { get; set; }
        [Required(ErrorMessage = "Seleccione una sucursal ")]
        public int SucursalId {get; set; }

        public string FechaHora { get; set; }
    }


}