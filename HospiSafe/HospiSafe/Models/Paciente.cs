using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Models
{
    // paciente --> persona registrada pero sin acceso directo a la app
    public class Paciente : Persona
    {
        [Key]
        public int IdPaciente { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12)]
        [RegularExpression(@"^\d{12}$")]
        public string NumSS { get; set; }

        // relacion --> paciente puede tener muchas citas
        [InverseProperty(nameof(Cita.Paciente))]
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();

        // inversa hacia usuario asociado (si existe), nullable
        [InverseProperty(nameof(Usuario.Paciente))]
        public virtual Usuario? UsuarioCuenta { get; set; }
    }
}
