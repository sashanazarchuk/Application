using EventSystem.Domain.Entities;
using EventSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Persistence.DataSeed
{
    internal class EventSeeder
    {
        private readonly EventSystemDbContext _context;

        public EventSeeder(EventSystemDbContext context)
        {
            _context = context;
        }

        public async Task SeedEventAsync()
        {
            if (!_context.Events.Any())
            {
                var events = new List<Event>
            {
                new Event
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    AdminId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Title = "Tech Conference 2025",
                    Description = "A conference about the latest in technology.",
                    Location = "New York",
                    Capacity = 300,
                    Date = DateTime.Parse("2025-04-20T10:00:00").ToUniversalTime(),
                    Type = EventType.Public
                },
                new Event
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    AdminId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Title = "Music Festival",
                    Description = "An outdoor music festival featuring various artists.",
                    Location = "Los Angeles",
                    Capacity = 500,
                    Date = DateTime.Parse("2025-07-15T18:00:00").ToUniversalTime(),
                    Type = EventType.Public
                },
                new Event
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    AdminId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Title = "Art Exhibition",
                    Description = "An exhibition showcasing contemporary art.",
                    Location = "Chicago",
                    Date = DateTime.Parse("2025-11-02T16:00:00").ToUniversalTime(),
                    Type = EventType.Public
                }
            };

                _context.Events.AddRange(events);
                await _context.SaveChangesAsync();
            }
        }
    }
}