using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CitasMedicas.Models
{
    public class HistoriaClinica
    {
        [Display(Name = "Identificador")]
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Peso { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Altura { get; set; }
        public string? PresionArterial { get; set; }
        [ForeignKey("Paciente")]
        [Display(Name = "Id Paciente")]
        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }
        
        public ICollection<Cita>? Citas { get; set; }
        public ICollection<Antecedente>? Antecedentes { get; set; }
    }
}