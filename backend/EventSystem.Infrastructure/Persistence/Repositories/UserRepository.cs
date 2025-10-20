using EventSystem.Application.Interfaces.Repositories;
using EventSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Persistence.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly EventSystemDbContext _context;

        public UserRepository(EventSystemDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user, CancellationToken token)
        {
            _context.DomainUsers.Add(user);
            await _context.SaveChangesAsync(token);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _context.DomainUsers.FirstOrDefaultAsync(u => u.Id == id, token);
        }
    }
}