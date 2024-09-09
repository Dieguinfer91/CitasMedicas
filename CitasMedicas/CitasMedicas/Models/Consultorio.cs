using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CitasMedicas.Models
{
    public class Consultorio
    {
        [Display(Name = "Id Consultorio")]
        public int Id { get; set; }
        [Display(Name = "Nombre del Consultorio")]
        public string? NombreConsultorio { get; set; }
        [Display(Name = "Información del Consultorio")]
        public string? DescripcionConsultorio { get; set; }
        [Display(Name = "Médico Asignado")]
        public Medico? Medico { get; set; }
    }
}