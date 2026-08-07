using GestorSolicitudes.Application.DTO;
using GestorSolicitudes.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestorSolicitudes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuariosController(IUsuarioService usuarioService) {
            _usuarioService = usuarioService;
        }
        [HttpGet("filtro")]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> Filtro(
            [FromQuery] string? nombre = null,
            [FromQuery] string? rol = null)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int usuarioId))
            {
                return Unauthorized(new { mensaje = "Token inválido o usuario no identificado." });
            }

            var solicitudes = await _usuarioService.GetUsuarios(nombre, rol);
            return Ok(solicitudes);
        }
    }
}
