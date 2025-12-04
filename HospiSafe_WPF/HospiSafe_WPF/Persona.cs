using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe_WPF
{
    public abstract class Persona
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(9)]
        public string DNI { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Phone]
        public string Telefono { get; set; }

        [EmailAddress]
        public string CorreoElectronico { get; set; }
    }
}

