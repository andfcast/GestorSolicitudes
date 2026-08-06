using GestorSolicitudes.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Domain.Entities
{
    public class Solicitud
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public PrioridadSolicitud Prioridad { get; set; }
        public EstadoSolicitud Estado { get; set; }
        public int? UsuarioResponsableId { get; set; }
        public Usuario? UsuarioResponsable { get; set; }
    }
}
