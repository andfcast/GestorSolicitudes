using GestorSolicitudes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Domain.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByUsuarioOrEmailAsync(string usuarioOrEmail);
        Task<IEnumerable<Usuario>> GetUsuariosAsync(string? nombre, string? rol);
    }
}
