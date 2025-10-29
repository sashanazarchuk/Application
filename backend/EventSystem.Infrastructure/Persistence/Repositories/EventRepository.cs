using EventSystem.Application.Interfaces.Repositories;
using EventSystem.Domain.Entities;
using EventSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Persistence.Repositories
{
    internal class EventRepository : IEventRepository
    {
        private readonly EventSystemDbContext _context;
        public EventRepository(EventSystemDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Event>> GetAllAsync(CancellationToken token)
        {
            return await _context.Events
                .Include(c => c.EventTags)
                    .ThenInclude(et => et.Tag)
                .Include(e => e.Participants)
                    .ThenInclude(u=>u.User)
                .Where(e => e.Type == EventType.Public)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync(token);
        }
        public async Task<Event?> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _context.Events
                .Include(c => c.EventTags)
                    .ThenInclude(et => et.Tag)
               .Include(e => e.Participants)
                    .ThenInclude(c => c.User)
               .AsSplitQuery()
               .FirstOrDefaultAsync(e => e.Id == id, token);
        }
        public async Task<Event> AddAsync(Event entity, CancellationToken token)
        {
            _context.Events.Add(entity);
            await _context.SaveChangesAsync(token);
            return entity;
        }
        public async Task UpdateAsync(Event entity, CancellationToken token)
        {
            _context.Events.Update(entity);
            await _context.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(Event entity, CancellationToken token)
        {
            _context.Events.Remove(entity);
            await _context.SaveChangesAsync(token);
        }

        public async Task JoinEventAsync(Participant participant, CancellationToken token)
        {
            _context.Participants.Add(participant);
            await _context.SaveChangesAsync(token);
        }

        public async Task LeaveEventAsync(Participant participant, CancellationToken token)
        {
            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync(token);
        }

        public async Task<Participant?> GetParticipantAsync(Guid eventId, Guid userId, CancellationToken token)
        {
            return await _context.Participants
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.EventId == eventId && p.UserId == userId, token);
        }

        public async Task<IEnumerable<Event>> FetchUserEventsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Events
                .Include(e => e.Participants)
                    .ThenInclude(p => p.User)
                .Include(e => e.EventTags)
                    .ThenInclude(et => et.Tag)
                .Where(e => e.AdminId == userId || e.Participants.Any(p => p.UserId == userId))
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Event>> GetEventsByTagsAsync(IEnumerable<string> tags, CancellationToken cancellationToken)
        {
            return await _context.Events
                .Include(e => e.EventTags)
                    .ThenInclude(et => et.Tag)
                .Include(p=>p.Participants)
                    .ThenInclude(u=>u.User)
                .Where(e => e.EventTags.Any(et => tags.Contains(et.Tag.Name)))
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}