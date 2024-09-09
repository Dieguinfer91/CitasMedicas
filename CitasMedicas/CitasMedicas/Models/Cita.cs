using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CitasMedicas.Models
{
    public class Cita
    {
        [Display(Name = "Id de Cita")]
        public int Id { get; set; }
        [Display(Name = "Fecha de Cita")]
        [DateRange(1, 60, ErrorMessage = "Programe fechas futuras hasta máximo 2 meses")]
        public DateTime FechaCita { get; set; }
        [Display(Name = "Hora de Cita")]
        public string? HoraCita { get; set; }

        [ForeignKey("Medico")]
       
        public int MedicoId { get; set; }
        [Display(Name = "Nombre del Médico")]
        public Medico? Medico { get; set; }
        [Display(Name = "Diagnóstico")]
        public string? Diagnostico { get; set; }
        
        [ForeignKey("Paciente")]
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }
        [Display(Name = "Estado de la Cita")]
        public int EstadoCita { get; set; }
        public ICollection<MedicamentoReceta>? Receta { get; set; } = new List<MedicamentoReceta>();
    }
}