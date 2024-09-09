using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.Models
{
    public class MedicoEnfermera
    {
        public int Id { get; set; }

        [ForeignKey("Medico")]
        public int MedicoId { get; set; }
        public Medico Medico { get; set; }
        [ForeignKey("Enfermera")]
        public int EnfermeraId { get; set; }
        public Enfermera Enfermera { get; set; }

    }
}
