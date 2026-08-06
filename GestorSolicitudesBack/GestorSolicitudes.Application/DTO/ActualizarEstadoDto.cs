using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Application.DTO
{
    public class ActualizarEstadoDto
    {
        [Required(ErrorMessage = "El nuevo estado es obligatorio.")]
        public string NuevoEstado { get; set; } = string.Empty; // Asignada, En Proceso, Resuelta, Cerrada
    }
}
