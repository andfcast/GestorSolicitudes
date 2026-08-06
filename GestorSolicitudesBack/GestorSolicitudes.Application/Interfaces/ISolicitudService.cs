using GestorSolicitudes.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Application.Interfaces
{
    public interface ISolicitudService
    {
        Task<IEnumerable<SolicitudDto>> GetByUsuarioResponsableAsync(int usuarioId, string? estado = null, string? prioridad = null);


        Task<SolicitudDto?> GetByIdAsync(int id);

        Task<SolicitudDto?> GetByCodigoAsync(string codigo);


        Task<IEnumerable<SolicitudDto>> GetSolicitudesConResponsableAsync();


        Task<SolicitudDto> CrearSolicitudAsync(CrearSolicitudDto dto);

        Task<bool> CambiarEstadoAsync(int solicitudId, string nuevoEstado);

        Task<bool> AsignarResponsableAsync(int solicitudId, int usuarioId);
    }
}
