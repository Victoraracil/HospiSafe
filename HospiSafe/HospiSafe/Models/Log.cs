using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospiSafe.Models
{
    public class Log
    {
        [Key]
        public int IdLog { get; set; }
        public int? IdUsuario { get; set; } //puede ser null para cuando registrar acciones que no son de usuario
        public string Accion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow; //fecha actual, pasaremos siempre default y asi coge getdate de sql
    }
}
