using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Models
{
    public class Log
    {
        [Key]
        public int IdLog { get; set; }
        public int? IdUsuario { get; set; } //puede ser null

        [Required]
        [MaxLength(300)]
        public string Accion { get; set; } = string.Empty;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}
