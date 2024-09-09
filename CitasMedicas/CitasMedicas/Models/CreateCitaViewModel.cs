using System.ComponentModel.DataAnnotations;

namespace CitasMedicas.Models
{
    public class CreateCitaViewModel
    {
        [Display(Name = "Id Paciente")]
        public int PacienteId { get; set; }
        [Display(Name = "Nombre Paciente")]
        public string? PacienteNombre { get; set; }
        [Display(Name = "Especialidad")]
        public int EspecialidadId { get; set; }
    }

}
