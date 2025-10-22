using EventSystem.Application.DTOs.Auth;
using EventSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Guid> RegisterAsync(User domainUser, string email, string password);
        Task<ApplicationUserDto> LoginAsync(string email, string password);
        Task LogoutAsync(Guid userId);
        Task<TokenDto> GenerateTokensAsync(Guid userId, string email);
    }
}