using GestorSolicitudes.Domain.Entities;
using GestorSolicitudes.Domain.Enums;
using GestorSolicitudes.Domain.Repositories;
using GestorSolicitudes.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }

        public async Task<Usuario?> GetByUsuarioOrEmailAsync(string usuarioOrEmail)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == usuarioOrEmail || u.Email == usuarioOrEmail);
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosAsync(string? nombre, string? rol)
        {
            var query = _context.Usuarios.AsQueryable();

            // Filtro dinámico por Nombre
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(u => u.NombreCompleto.Contains(nombre));
            }

            // Filtro dinámico por Rol
            if (!string.IsNullOrWhiteSpace(rol) && Enum.TryParse<RolUsuario>(rol, true, out var rolEnum))
            {
                query = query.Where(u => u.Rol == rolEnum);
            }

            return await query
                .OrderBy(s => s.NombreCompleto)
                .ToListAsync();
        }
    }
}
