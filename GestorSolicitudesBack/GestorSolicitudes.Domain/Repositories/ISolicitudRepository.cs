using GestorSolicitudes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Domain.Repositories
{
    public interface ISolicitudRepository : IRepository<Solicitud>
    {
        Task<Solicitud?> GetByCodigoAsync(string codigo);
        Task<IEnumerable<Solicitud>> GetSolicitudesConResponsableAsync();
    }
}
