using GestorSolicitudes.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Domain.Entities
{
    public class Usuario : BaseEntity<int>
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
    }
}
