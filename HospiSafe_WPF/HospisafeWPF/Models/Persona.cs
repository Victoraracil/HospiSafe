using System;
using System.ComponentModel.DataAnnotations;

namespace HospiSafe_WPF.Models
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
