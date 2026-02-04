using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Models
{
    public enum EstadoPrueba
    {
        Pendiente = 0,
        Completada = 1
    }

    public class Prueba
    {
        [Key]
        public int IdPrueba { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [MaxLength(100)]
        public string TipoAnalisis { get; set; } = string.Empty;

        [Required]
        public EstadoPrueba Estado { get; set; }

        [MaxLength(500)]
        public string? Resultados { get; set; }

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
