using GestorSolicitudes.Application.DTO;
using GestorSolicitudes.Application.Interfaces;
using GestorSolicitudes.Domain.Entities;
using GestorSolicitudes.Domain.Enums;
using GestorSolicitudes.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Application.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SolicitudService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SolicitudDto>> GetByUsuarioResponsableAsync(int usuarioId, string? estado = null, string? prioridad = null)
        {
            var solicitudes = await _unitOfWork.Solicitudes.GetByUsuarioResponsableAsync(usuarioId, estado, prioridad);

            return solicitudes.Select(s => MapToDto(s));
        }

        public async Task<SolicitudDto?> GetByIdAsync(int id)
        {
            var solicitud = await _unitOfWork.Solicitudes.GetByIdWithDetailsAsync(id);
            return solicitud == null ? null : MapToDto(solicitud);
        }

        public async Task<SolicitudDto?> GetByCodigoAsync(string codigo)
        {
            var solicitud = await _unitOfWork.Solicitudes.GetByCodigoAsync(codigo);
            return solicitud == null ? null : MapToDto(solicitud);
        }

        public async Task<IEnumerable<SolicitudDto>> GetSolicitudesConResponsableAsync()
        {
            var solicitudes = await _unitOfWork.Solicitudes.GetSolicitudesConResponsableAsync();
            return solicitudes.Select(MapToDto);
        }

        public async Task<SolicitudDto> CrearSolicitudAsync(CrearSolicitudDto dto)
        {
            var solicitud = new Solicitud
            {
                Codigo = $"SOL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}",
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Cliente = dto.Cliente,
                Estado = EstadoSolicitud.Nueva,
                Prioridad = Enum.Parse<PrioridadSolicitud>(dto.Prioridad, true),
                FechaCreacion = DateTime.UtcNow,
                UsuarioResponsableId = dto.UsuarioResponsableId
            };

            await _unitOfWork.Solicitudes.AddAsync(solicitud);
            await _unitOfWork.CompleteAsync();

            return MapToDto(solicitud);
        }

        public async Task<bool> CambiarEstadoAsync(int solicitudId, string nuevoEstado)
        {
            var solicitud = await _unitOfWork.Solicitudes.GetByIdAsync(solicitudId);
            if (solicitud == null) return false;

            if (Enum.TryParse<EstadoSolicitud>(nuevoEstado, true, out var estadoParsed))
            {
                solicitud.Estado = estadoParsed;

                if (estadoParsed == EstadoSolicitud.Resuelta || estadoParsed == EstadoSolicitud.Cerrada)
                {
                    solicitud.FechaCierre = DateTime.UtcNow;
                }

                _unitOfWork.Solicitudes.Update(solicitud);
                await _unitOfWork.CompleteAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> ActualizarSolicitudAsync(int id, ActualizarSolicitudDto dto)
        {
            var solicitud = await _unitOfWork.Solicitudes.GetByIdAsync(id);
            if (solicitud == null) return false;

            solicitud.Titulo = dto.Titulo;
            solicitud.Descripcion = dto.Descripcion;
            solicitud.Cliente = dto.Cliente;
            solicitud.Prioridad = Enum.Parse<PrioridadSolicitud>(dto.Prioridad, true);
            solicitud.UsuarioResponsableId = dto.UsuarioResponsableId;

            _unitOfWork.Solicitudes.Update(solicitud);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> AsignarResponsableAsync(int solicitudId, int usuarioId)
        {
            var solicitud = await _unitOfWork.Solicitudes.GetByIdAsync(solicitudId);
            if (solicitud == null) return false;

            solicitud.UsuarioResponsableId = usuarioId;
            _unitOfWork.Solicitudes.Update(solicitud);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private static SolicitudDto MapToDto(Solicitud s)
        {
            return new SolicitudDto
            {
                Id = s.Id,
                Codigo = s.Codigo,
                Titulo = s.Titulo,
                Descripcion = s.Descripcion,
                Estado = s.Estado.ToString(),
                Cliente = s.Cliente,
                Prioridad = s.Prioridad.ToString(),
                FechaCreacion = s.FechaCreacion,
                UsuarioResponsableId = s.UsuarioResponsableId,
                NombreUsuarioResponsable = s.UsuarioResponsable != null
                    ? s.UsuarioResponsable.NombreCompleto
                    : "Sin asignar"
            };
        }
    }
}
