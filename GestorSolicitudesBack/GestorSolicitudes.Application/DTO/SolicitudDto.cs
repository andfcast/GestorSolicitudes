using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Application.DTO
{
    public class SolicitudDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string Prioridad { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string NombreUsuarioResponsable { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public int? UsuarioResponsableId { get; internal set; }
    }
}
