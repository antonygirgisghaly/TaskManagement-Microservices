using Identity.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
