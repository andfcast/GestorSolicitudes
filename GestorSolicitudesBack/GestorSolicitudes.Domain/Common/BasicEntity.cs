using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Domain.Common
{
    public class BasicEntity : BaseEntity<int>
    {
        public string Descripcion { get; set; } = string.Empty;
    }
}
