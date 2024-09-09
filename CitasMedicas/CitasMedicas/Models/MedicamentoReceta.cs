using System.ComponentModel.DataAnnotations.Schema;

namespace CitasMedicas.Models
{
    public class MedicamentoReceta
    {
        public int Id { get; set; }

        [ForeignKey("Medicamento")]
        public int MedicamentoId { get; set; }
        public Medicamento Medicamento { get; set; }
        [ForeignKey("Cita")]
        public int CitaId { get; set; }
        public Cita Cita { get; set; }
        public string? Dosis { get; set; }
        public string? Instrucciones { get; set; }
        public int Cantidad { get; set; }
    }
}
