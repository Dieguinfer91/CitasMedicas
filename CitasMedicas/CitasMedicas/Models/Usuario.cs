using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CitasMedicas.Models
{
    public class Usuario : IdentityUser
    {
        //public int Id { get; set; }
        //public string? NombreUsuario { get; set; }
        //public string? ClaveUsuario { get; set; }

        //public int TipoUsuario { get; set; }

        [Display(Name = "Clave")]
        public override string PasswordHash { get; set; }

        [Required]
        [Display(Name = "Tipo de Usuario")]
        public string TipoUsuario { get; set; }

    }
}