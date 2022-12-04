using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models.ViewModels.Profesional
{
    public class ServicioFinishViewModel
    {
        public int Id { get; set; }
        public int TipoServicioId { get; set; }
        public int Estado { get; set; }
        [Required(ErrorMessage = "Ingrese un informe")]
        public string Informe { get; set; }
        public string Comentario { get; set; }
        public List<ResultadoTarea> ResultadoTareas { get; set; }

    }


}