using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe_WPF
{

    public enum RolUsuario
    {
        Admin,
        Personal,
        Paciente
    }

    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public string PasswordHash { get; set; }

    }
}
