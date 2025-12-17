using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe_WPF
{
    public enum EstadoCita
    {
        Activa,
        Cancelada
    }
    public class Cita
    {
        [Key]
        public int IdCita { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int IdPaciente { get; set; }

        [Required]
        public int IdUsuario { get; set; } //El personal que atiende

        [Required]
        public EstadoCita Estado { get; set; }
    }

}
