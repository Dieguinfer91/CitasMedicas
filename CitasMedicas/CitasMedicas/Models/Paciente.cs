using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.Models
{
    public class Paciente
    {
        [Display(Name = "Id Paciente")]
        public int Id { get; set; }
        [Display(Name = "Cédula Paciente")]
        [Cedula(ErrorMessage = "Cédula inválida")]
        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La cédula debe tener 10 dígitos.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "La cédula debe contener solo números.")]
        public string? CedulaPaciente { get; set; }
        [Display(Name = "Nombre Paciente")]
        public string? NombrePaciente { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        [DateRange(-36500, 0, ErrorMessage = "Fecha invalida")]
        public DateTime FechaNacimientoPaciente { get; set; }

        [Display(Name = "Correo Paciente")]
        [DataType(DataType.EmailAddress)]
        public string? CorreoPaciente { get; set; }
        [Display(Name = "Teléfono Paciente")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El teléfono debe tener 10 dígitos.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe contener solo números y debe ser un celular.")]
        public string? TelefonoPaciente { get; set; }

        [ForeignKey("Usuario")]
        public string? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int EdadPaciente { get; set; }
    }
}
