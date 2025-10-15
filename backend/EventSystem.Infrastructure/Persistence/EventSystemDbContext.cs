using EventSystem.Domain.Entities;
using EventSystem.Infrastructure.Persistence.DataSeed;
using EventSystem.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Persistence
{
    internal class EventSystemDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public EventSystemDbContext(DbContextOptions<EventSystemDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply all configurations from the current assembly
            builder.ApplyConfigurationsFromAssembly(typeof(EventSystemDbContext).Assembly);
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<User> DomainUsers { get; set; }
    }
}