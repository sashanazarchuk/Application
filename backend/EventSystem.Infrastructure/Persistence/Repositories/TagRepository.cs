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
    internal class TagRepository : ITagRepository
    {
        private readonly EventSystemDbContext _context;
        public TagRepository(EventSystemDbContext context)
        {
            _context = context;
        }
        public async Task<Tag> AddAsync(Tag entity, CancellationToken token)
        {
            await _context.Tags.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<Tag> entities, CancellationToken token)
        {
            await _context.Tags.AddRangeAsync(entities, token);
            await _context.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(Tag entity, CancellationToken token)
        {
            _context.Tags.Remove(entity);
            await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(CancellationToken token)
        {
            return await _context.Tags.AsNoTracking().ToListAsync(token);
        }

        public async Task<Tag?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _context.Tags.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, token);
        }

        public async Task UpdateAsync(Tag entity, CancellationToken token)
        {
            _context.Tags.Update(entity);
            await _context.SaveChangesAsync(token);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Tags.AnyAsync(t => t.Name == name, cancellationToken);
        }

        public async Task<List<Tag>> GetByNamesAsync(IEnumerable<string> names, CancellationToken token)
        {
            return await _context.Tags
                .Where(t => names.Contains(t.Name))
                .AsNoTracking()
                .ToListAsync(token);
        }
    }
}