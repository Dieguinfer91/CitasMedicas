using System.ComponentModel.DataAnnotations;

namespace CitasMedicas.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Clave")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Recordar")]
        public bool RememberMe { get; set; }
    }
}
