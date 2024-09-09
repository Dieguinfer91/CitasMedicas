using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CitasMedicas.Models
{
    public class Medicamento
    {
        [Display(Name = "Id Medicamento")]
        public int Id { get; set; }
        [Display(Name = "nombre del Medicamento")]
        public string? NombreMedicamento { get; set; }
        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }
        [Display(Name = "Stock")]
        public int StockMedicamento { get; set; }
    }
}