using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospiSafe_WPF.Models
{
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

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
