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

        /// <summary>
        /// Obtiene las solicitudes asignadas al usuario autenticado, con la opción de filtrar por estado y prioridad.
        /// </summary>
        /// <param name="estado"></param>
        /// <param name="prioridad"></param>
        /// <returns></returns>
        [HttpGet("filtro")]
        public async Task<ActionResult<IEnumerable<SolicitudDto>>> GetByUsuario(
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

        /// <summary>
        /// Obtiene una solicitud por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SolicitudDto>> GetById(int id)
        {
            var solicitud = await _solicitudService.GetByIdAsync(id);
            if (solicitud == null) return NotFound($"No se encontró la solicitud con ID {id}.");
            return Ok(solicitud);
        }

        /// <summary>
        /// Obtiene una solicitud por su código.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpGet("codigo/{codigo}")]
        public async Task<ActionResult<SolicitudDto>> GetByCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return BadRequest("El código es requerido.");

            var solicitud = await _solicitudService.GetByCodigoAsync(codigo);
            if (solicitud == null) return NotFound($"No existe la solicitud con código '{codigo}'.");

            return Ok(solicitud);
        }

        /// <summary>
        /// Obtiene todas las solicitudes con datos del responsable, con la opción de filtrar por estado y prioridad. Solo accesible para administradores.
        /// </summary>
        /// <param name="estado"></param>
        /// <param name="prioridad"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador")]
        [HttpGet("todas")]
        public async Task<ActionResult<IEnumerable<SolicitudDto>>> GetTodas(
            [FromQuery] string? estado = null,
            [FromQuery] string? prioridad = null)
        {
            var solicitudes = await _solicitudService.GetSolicitudesConResponsableAsync(estado, prioridad);
            return Ok(solicitudes);
        }

        /// <summary>
        /// Crea una nueva solicitud. El código de la solicitud se genera automáticamente y es único.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<SolicitudDto>> Crear([FromBody] CrearSolicitudDto request)
        {
            var nuevaSolicitud = await _solicitudService.CrearSolicitudAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = nuevaSolicitud.Id }, nuevaSolicitud);
        }

        /// <summary>
        /// Cambia el estado de una solicitud existente.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("{id:int}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] ActualizarEstadoDto request)
        {
            var resultado = await _solicitudService.CambiarEstadoAsync(id, request.NuevoEstado);
            if (!resultado) return BadRequest(new { mensaje = "No se pudo actualizar el estado." });

            return Ok(new { mensaje = "Estado actualizado exitosamente." });
        }

        /// <summary>
        /// Actualiza los detalles de una solicitud existente.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarSolicitudDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exito = await _solicitudService.ActualizarSolicitudAsync(id, dto);
            if (!exito)
            {
                return NotFound(new { mensaje = $"No se encontró la solicitud con el ID consultado para actualizar." });
            }

            return Ok(new { mensaje = "Solicitud actualizada correctamente." });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador")]
        [HttpPatch("{id:int}/asignar/{usuarioId:int}")]
        public async Task<IActionResult> AsignarResponsable(int id, int usuarioId)
        {
            var resultado = await _solicitudService.AsignarResponsableAsync(id, usuarioId);
            if (!resultado) return BadRequest(new { mensaje = "No se pudo asignar el responsable." });

            return Ok(new { mensaje = "Responsable asignado exitosamente." });
        }

        /// <summary>
        /// Elimina una solicitud existente. Solo accesible para administradores.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _solicitudService.EliminarSolicitudAsync(id);
            if (!resultado) return NotFound(new { mensaje = $"No se encontró la solicitud con el ID indicado para eliminar." });

            return Ok(new { mensaje = "Solicitud eliminada correctamente." });
        }
    }
}

