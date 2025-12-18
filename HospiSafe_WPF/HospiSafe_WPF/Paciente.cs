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


        // Un paciente puede tener muchas citas
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
