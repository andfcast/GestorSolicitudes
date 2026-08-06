using GestorSolicitudes.Domain.Entities;
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
    }
}
