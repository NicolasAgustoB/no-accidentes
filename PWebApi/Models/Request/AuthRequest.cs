using System.ComponentModel.DataAnnotations;

namespace PWebApi.Models.Request
{
    public class AuthRequest
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
