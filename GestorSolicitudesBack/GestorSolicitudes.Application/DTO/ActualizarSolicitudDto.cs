using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Application.DTO
{
    public class ActualizarSolicitudDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título no puede superar los 150 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        public string Cliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "La prioridad es obligatoria.")]
        public string Prioridad { get; set; } = string.Empty;

        public int? UsuarioResponsableId { get; set; }
    }
}
