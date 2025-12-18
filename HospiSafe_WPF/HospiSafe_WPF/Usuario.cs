using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe_WPF
{
    /*
     * Hereda de persona con todos sus atributos
     */

    public enum RolUsuario
    {
        Admin,
        Personal
    }
    public class Usuario : Persona
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public RolUsuario Rol { get; set; }

        
        // Un usuario puede atender muchas citas
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }

}
