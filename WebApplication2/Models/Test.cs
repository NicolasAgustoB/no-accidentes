using System.Collections.Generic;

namespace AppWeb.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ResultadoTarea> ResultadoTareas { get; set; }
    }
}
