using GestorSolicitudes.Application.DTO;
using GestorSolicitudes.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestorSolicitudes.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesController : ControllerBase
    {
        private readonly ISolicitudService _solicitudService;

        public SolicitudesController(ISolicitudService solicitudService)
        {
            _solicitudService = solicitudService;
        }

        public async Task<ActionResult<IEnumerable<SolicitudDto>>> GetMisSolicitudes(
            [FromQuery] string? estado = null,
            [FromQuery] string? prioridad = null)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int usuarioId))
            {
                return Unauthorized(new { mensaje = "Token inválido o usuario no identificado." });
            }

            var solicitudes = await _solicitudService.GetByUsuarioResponsableAsync(usuarioId, estado, prioridad);
            return Ok(solicitudes);
        }

        // 2. Consulta por ID del usuario
        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<ActionResult<IEnumerable<SolicitudDto>>> GetByUsuario(int usuarioId)
        {
            var solicitudes = await _solicitudService.GetByUsuarioResponsableAsync(usuarioId);
            return Ok(solicitudes);
        }

        // 3. Consulta por ID de solicitud
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SolicitudDto>> GetById(int id)
        {
            var solicitud = await _solicitudService.GetByIdAsync(id);
            if (solicitud == null) return NotFound($"No se encontró la solicitud con ID {id}.");
            return Ok(solicitud);
        }

        // 4. Búsqueda por código único (ej: SOL-20260806-A1B2)
        [HttpGet("codigo/{codigo}")]
        public async Task<ActionResult<SolicitudDto>> GetByCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return BadRequest("El código es requerido.");

            var solicitud = await _solicitudService.GetByCodigoAsync(codigo);
            if (solicitud == null) return NotFound($"No existe la solicitud con código '{codigo}'.");

            return Ok(solicitud);
        }

        // 5. Lista General con datos del Responsable (Vista Administrador)
        [HttpGet("con-responsable")]
        public async Task<ActionResult<IEnumerable<SolicitudDto>>> GetSolicitudesConResponsable()
        {
            var solicitudes = await _solicitudService.GetSolicitudesConResponsableAsync();
            return Ok(solicitudes);
        }

        // 6. Crear Solicitud
        [HttpPost]
        public async Task<ActionResult<SolicitudDto>> Crear([FromBody] CrearSolicitudDto request)
        {
            var nuevaSolicitud = await _solicitudService.CrearSolicitudAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = nuevaSolicitud.Id }, nuevaSolicitud);
        }

        // 7. Cambiar Estado (Pendiente -> EnProceso -> Resuelta)
        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] ActualizarEstadoDto request)
        {
            var resultado = await _solicitudService.CambiarEstadoAsync(id, request.NuevoEstado);
            if (!resultado) return BadRequest(new { mensaje = "No se pudo actualizar el estado." });

            return Ok(new { mensaje = "Estado actualizado exitosamente." });
        }

        // 8. Asignar Responsable
        [HttpPatch("{id:int}/asignar/{usuarioId:int}")]
        public async Task<IActionResult> AsignarResponsable(int id, int usuarioId)
        {
            var resultado = await _solicitudService.AsignarResponsableAsync(id, usuarioId);
            if (!resultado) return BadRequest(new { mensaje = "No se pudo asignar el responsable." });

            return Ok(new { mensaje = "Responsable asignado exitosamente." });
        }
    }
}

