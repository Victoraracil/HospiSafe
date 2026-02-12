using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Models
{
    public enum RolUsuario
    {
        Sin_Asignar = 0,
        Admin = 1,
        Personal = 2,
        Paciente = 3
    }
    public class Usuario : Persona
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public RolUsuario Rol { get; set; }

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
