using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Persistence.DataSeed
{
    internal class TagSeeder
    {
        private readonly EventSystemDbContext _context;
        public TagSeeder(EventSystemDbContext context)
        {
            _context = context;
        }
        public async Task SeedTagsAsync()
        {
            if (!_context.Tags.Any())
            {
                _context.Tags.AddRange(
                    new Domain.Entities.Tag
                    {
                        Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                        Name = "Technology"
                    },
                    new Domain.Entities.Tag
                    {
                        Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                        Name = "Music"
                    },
                    new Domain.Entities.Tag
                    {
                        Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                        Name = "Art"
                    }
                );
                await _context.SaveChangesAsync();
            }
        }
    }
}
