using EventSystem.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string CreateAccessToken(ApplicationUserDto user);
    }
}