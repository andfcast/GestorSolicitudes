using GestorSolicitudes.Domain.Repositories;
using GestorSolicitudes.Infrastructure.Persistence.Context;
using GestorSolicitudes.Infrastructure.Persistence.Repositories;

namespace GestorSolicitudes.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUsuarioRepository Usuarios { get; }
        public ISolicitudRepository Solicitudes { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Usuarios = new UsuarioRepository(_context);
            Solicitudes = new SolicitudRepository(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
