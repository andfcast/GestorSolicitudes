using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Domain.Enums
{
    public enum EstadoSolicitud
    {
        Nueva = 1,
        Asignada = 2,
        EnProceso = 3,
        Resuelta = 4,
        Cerrada = 5
    }
}
