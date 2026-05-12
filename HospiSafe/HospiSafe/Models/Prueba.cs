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
        En_progreso = 1,
        Completada = 2
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

        [Required]
        public int IdPaciente { get; set; }

        [ForeignKey(nameof(IdPaciente))]
        public virtual Paciente Paciente { get; set; } = null!;

        [Required]
        public int IdUsuario { get; set; }

        //quien la registra
        [ForeignKey(nameof(IdUsuario))]
        public virtual Usuario Usuario { get; set; } = null!;

        // vuelta fk de informe
        [InverseProperty(nameof(Informe.Prueba))]
        public virtual Informe Informe { get; set; } = null!;
    }
}
