using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        TecnicoLaboratorio = 3,
        TecnicoRayos = 4,
        Administracion = 5
    }

    // usuario del sistema: personal que accede a la aplicación
    public class Usuario : Persona
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public RolUsuario Rol { get; set; }

        // relacion opcional con Paciente: si un paciente necesita una cuenta, la vinculamos por este id
        public int? IdPaciente { get; set; }

        [ForeignKey(nameof(IdPaciente))]
        public virtual Paciente? Paciente { get; set; }

        // relacion --> un usuario puede crear/registrar citas
        [InverseProperty(nameof(Cita.Usuario))]
        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
