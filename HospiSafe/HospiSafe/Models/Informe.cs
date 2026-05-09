using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Models
{
    public class Informe
    {
        [Key]
        public int IdInforme { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // descripción del informe
        [Required]
        [MaxLength(5000)]
        public string Contenido { get; set; } = string.Empty;

        // FK a Prueba
        [Required]
        public int IdPrueba { get; set; }

        [ForeignKey(nameof(IdPrueba))]
        public virtual Prueba Prueba { get; set; } = null!;

        // FK a paciente para la vista de informes
        [Required]
        public int IdPaciente { get; set; }

        [ForeignKey(nameof(IdPaciente))]
        public virtual Paciente Paciente { get; set; } = null!;

        // meter fk a doctor cuando se implemente,
        // todos los informes llevan doctor
    }
}
