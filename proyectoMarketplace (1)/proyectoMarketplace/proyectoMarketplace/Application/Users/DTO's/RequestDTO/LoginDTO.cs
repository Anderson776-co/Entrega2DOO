using System.ComponentModel.DataAnnotations;

namespace Application.Users.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Llene el campo con su correo o nombre de usuario")]
        public string Identificador { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo de la contraseña es obligatorio")]
        public string Password { get; set; } = string.Empty;
    }
}