using GestorSolicitudes.Application.DTO;
using GestorSolicitudes.Application.Interfaces;
using GestorSolicitudes.Domain.Entities;
using GestorSolicitudes.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<UsuarioDto>> GetUsuarios(string? nombre = null, string? rol = null)
        {
            var usuarios = await _unitOfWork.Usuarios.GetUsuariosAsync(nombre, rol);
            return usuarios.Select(MapToDto);
        }

        private UsuarioDto MapToDto(Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
            };
        }
    }
}
