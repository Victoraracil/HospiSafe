using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe_WPF
{
    public class Paciente : Persona
    {
        [Key]
        public int IdPaciente { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12)]
        [RegularExpression(@"^\d{12}$")]
        public string NumSS { get; set; }

        /*
        // Relación 1 a 1 con Usuario (opcional)
        public int? IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
        */
    }
}
