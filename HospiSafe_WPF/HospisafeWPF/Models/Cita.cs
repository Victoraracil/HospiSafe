using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospiSafe_WPF.Models
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

        [Required]
        public int IdPaciente { get; set; }

        [ForeignKey(nameof(IdPaciente))]
        public Paciente Paciente { get; set; } = null!;

        [Required]
        public int IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;
    }
}
