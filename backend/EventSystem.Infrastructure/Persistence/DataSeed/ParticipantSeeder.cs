using EventSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Persistence.DataSeed
{
    internal class ParticipantSeeder
    {
        private readonly EventSystemDbContext _context;

        public ParticipantSeeder(EventSystemDbContext context)
        {
            _context = context; 
        }

        public async Task SeedParticipantsAsync()
        {
            if (!_context.Participants.Any())
            {
                _context.Participants.AddRange(
                    new Participant
                    {
                        Id = Guid.Parse("5f3b630e-b1f7-46a4-90de-5a9fca4ebad3"),
                        UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                        EventId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        JoinedAt = DateTime.Parse("2025-04-10T09:15:00").ToUniversalTime()
                    },
                    new Participant
                    {
                        Id = Guid.Parse("b37c8140-ad33-44cc-91ed-13e4bc09f2ef"),
                        UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                        EventId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        JoinedAt = DateTime.Parse("2025-07-10T12:00:00").ToUniversalTime()
                    },
                    new Participant
                    {
                        Id = Guid.Parse("6055f3e9-b3c9-4364-b22e-f9a375378645"),
                        UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                        EventId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        JoinedAt = DateTime.Parse("2025-10-25T15:30:00").ToUniversalTime()
                    }
                );

                await _context.SaveChangesAsync();
            }
        }
    }
}