using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe_WPF
{

    public enum EstadoCita
    {
        Activa = 0,
        Cancelada = 1
    }

    public class Cita
    {
        [Key]
        public int IdCita { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public EstadoCita Estado { get; set; }

        // FK Paciente
        [Required]
        public int IdPaciente { get; set; }

        [ForeignKey(nameof(IdPaciente))]
        public Paciente Paciente { get; set; } = null!;

        // FK Usuario (Personal)
        [Required]
        public int IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;
    }


}
