using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CitasMedicas.Models
{
    public class Enfermera
    {
        [Display(Name = "Id Enfermera")]
        public int Id { get; set; }
        [Display(Name = "Cédula Enfermera")]
        [Cedula(ErrorMessage = "Cédula inválida")]
        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La cédula debe tener 10 dígitos.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "La cédula debe contener solo números.")]
        public string? CedulaEnfermera { get; set; }
        [Display(Name = "Nombre Enfermera")]
        public string? NombreEnfermera { get; set; }
        [Display(Name = "Apellido Enfermera")]
        public string? ApellidoEnfermera { get; set; }
        [Display(Name = "Correo Enfermera")]
        [DataType(DataType.EmailAddress)]
        public string? CorreoEnfermera { get; set; }
        [Display(Name = "Teléfono Enfermera")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El teléfono debe tener 10 dígitos.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe contener solo números y debe ser un celular.")]
        public string? TelefonoEnfermera { get; set; }
        public ICollection<MedicoEnfermera> Medicos { get; set; } = new List<MedicoEnfermera>();
        [Display(Name = "Fecha Nacimiento")]
        [DateRange(-36500, 0, ErrorMessage = "Fecha invalida")]
        public DateTime? FechaNacimientoEnfermera { get; set; }
        [ForeignKey("Usuario")]
        [Display(Name = "Id de Usuario")]
        public string? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }



        [Display(Name = "Nombre Completo")]
        public string NombreCompleto
        {
            get
            {
                return $"{NombreEnfermera} {ApellidoEnfermera}".Trim();
            }
        }

    }
}