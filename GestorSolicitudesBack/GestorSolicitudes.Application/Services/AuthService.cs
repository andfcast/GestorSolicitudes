using GestorSolicitudes.Application.DTO;
using GestorSolicitudes.Application.Interfaces;
using GestorSolicitudes.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        public AuthService(IUnitOfWork unitOfWork, ITokenService jwtTokenGenerator, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }
        public async Task<AccesoDto?> LoginAsync(LoginDto request)
        {

            var usuario = await _unitOfWork.Usuarios.GetByUsuarioOrEmailAsync(request.UsuarioOrEmail);
            if (usuario == null)
                return null;

            // 2. Verificar contraseña con BCrypt
            bool isPasswordValid = _passwordHasher.Verify(request.Password, usuario.PasswordHash);
            if (!isPasswordValid)
                return null;

            // 3. Delegar la generación del JWT al componente especificado
            var (token, expiracion) = _jwtTokenGenerator.GenerateToken(usuario);

            return new AccesoDto
            {
                Token = token,
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Expiracion = expiracion
            };
        }
    }
}
