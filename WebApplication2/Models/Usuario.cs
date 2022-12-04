using System;
using System.ComponentModel.DataAnnotations;

namespace AppWeb.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Rut { get; set; }
        public string Telefono { get; set; }
        public int RolId { get; set; }
        public int SucursalId { get; set; }
        public string Token { get; set; }
        public int Estado { get; set; }
    }

    }
