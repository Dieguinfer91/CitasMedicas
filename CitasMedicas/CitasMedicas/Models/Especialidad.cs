using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CitasMedicas.Models
{
    public class Especialidad
    {
        [Display(Name = "Id Especialidad")]
        public int Id { get; set; }
        [Display(Name = "Descripción de la Especialidad")]
        public string? DescripcionEspecialidad { get; set; }
        [Display(Name = "Médico")]
        public ICollection<Medico> Medicos { get; set; } = new List<Medico>();


    }
}