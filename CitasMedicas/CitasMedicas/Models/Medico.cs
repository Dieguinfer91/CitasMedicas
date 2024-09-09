using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CitasMedicas.Models
{
    public class Medico
    {
        [Display(Name = "Id Médico")]
        public int Id { get; set; }
        [Display(Name = "Cédula Médico")]
        [Cedula(ErrorMessage = "Cédula inválida")]
        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La cédula debe tener 10 dígitos.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "La cédula debe contener solo números.")]
        public string? CedulaMedico { get; set; }
        [Display(Name = "Nombre Médico")]
        public string? NombreMedico { get; set; }
        [Display(Name = "Apellido Médico")]
        public string? ApellidoMedico { get; set; }
        [Display(Name = "Correo Médico")]
        [DataType(DataType.EmailAddress)]
        public string? CorreoMedico { get; set; }
        [Display(Name = "Teléfono Médico")]
        //[StringLength(10, MinimumLength = 10, ErrorMessage = "El teléfono debe tener 10 dígitos.")]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe contener solo números y debe ser un celular.")]
        public string? TelefonoMedico { get; set; }
        public ICollection<Especialidad> Especialidades { get; set; } = new List<Especialidad>();
        [Display(Name = "Fecha Nacimiento")]
        [DateRange(-36500, 0, ErrorMessage = "Fecha invalida")]
        public DateTime? FechaNacimientoMedico { get; set; }
        [ForeignKey("Usuario")]
        [Display(Name = "Id de Usuario")]
        public string? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public ICollection<MedicoEnfermera> Enfermeras { get; set; } = new List<MedicoEnfermera>();

        [ForeignKey("Consultorio")]
        [Display(Name = "Consultorio asignado")]
        public int ConsultorioId { get; set; }
        public Consultorio? Consultorio { get; set; }
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto
        {
            get
            {
                return $"{NombreMedico} {ApellidoMedico}".Trim();
            }
        }

    }
}