using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Persistence.DataSeed
{
    internal class EventTagSeeder
    {
        private readonly EventSystemDbContext _context;
        public EventTagSeeder(EventSystemDbContext context)
        {
            _context = context;
        }
        public async Task SeedEventTagsAsync()
        {
            if (!_context.Set<Domain.Entities.EventTag>().Any())
            {
                var eventTags = new List<Domain.Entities.EventTag>
                {
                    new Domain.Entities.EventTag
                    {
                        EventId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        TagId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                    },
                    new Domain.Entities.EventTag
                    {
                        EventId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        TagId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
                    },
                    new Domain.Entities.EventTag
                    {
                        EventId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        TagId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc")
                    }
                };
                _context.Set<Domain.Entities.EventTag>().AddRange(eventTags);
                await _context.SaveChangesAsync();
            }
        }
    }
}
