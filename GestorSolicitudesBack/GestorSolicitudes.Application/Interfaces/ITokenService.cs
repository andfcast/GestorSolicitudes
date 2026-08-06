using GestorSolicitudes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorSolicitudes.Application.Services
{
    public interface ITokenService
    {
        (string Token, DateTime Expiracion) GenerateToken(Usuario u);
    }
}
