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
    public class SolicitudRepository : Repository<Solicitud>, ISolicitudRepository
    {
        public SolicitudRepository(AppDbContext context) : base(context) { }

        public async Task<Solicitud?> GetByCodigoAsync(string codigo)
        {
            return await _context.Solicitudes
                .Include(s => s.UsuarioResponsable)
                .FirstOrDefaultAsync(s => s.Codigo == codigo);
        }

        public async Task<IEnumerable<Solicitud>> GetSolicitudesConResponsableAsync()
        {
            return await _context.Solicitudes
                .Include(s => s.UsuarioResponsable)
                .ToListAsync();
        }

        public async Task<IEnumerable<Solicitud>> GetByUsuarioResponsableAsync(int usuarioId, string? estado = null, string? prioridad = null)
        {
            var query = _context.Solicitudes
                .Where(s => s.UsuarioResponsableId == usuarioId)
                .AsQueryable();

            // Filtro dinámico por Estado
            if (!string.IsNullOrWhiteSpace(estado) && Enum.TryParse<EstadoSolicitud>(estado, true, out var estadoEnum))
            {
                query = query.Where(s => s.Estado == estadoEnum);
            }

            // Filtro dinámico por Prioridad
            if (!string.IsNullOrWhiteSpace(prioridad) && Enum.TryParse<PrioridadSolicitud>(prioridad, true, out var prioridadEnum))
            {
                query = query.Where(s => s.Prioridad == prioridadEnum);
            }

            return await query
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Solicitud?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Solicitudes
                .Include(s => s.UsuarioResponsable)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
