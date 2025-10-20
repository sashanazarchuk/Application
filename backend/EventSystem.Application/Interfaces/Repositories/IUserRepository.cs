using EventSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken token);
        Task<User?> GetByIdAsync (Guid id, CancellationToken token);
    }
}