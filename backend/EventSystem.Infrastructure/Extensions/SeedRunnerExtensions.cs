using EventSystem.Infrastructure.Persistence;
using EventSystem.Infrastructure.Persistence.DataSeed;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Extensions
{
    public static class SeedRunnerExtensions
    {
        public static async Task SeedAllAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                // Apply any pending migrations
                var dbContext = services.GetRequiredService<EventSystemDbContext>();
                await dbContext.Database.MigrateAsync();

                // Seed users
                var userSeeder = services.GetRequiredService<UserSeeder>();
                await userSeeder.SeedUserAsync();

                // Seed events
                var eventSeeder = services.GetRequiredService<EventSeeder>();
                await eventSeeder.SeedEventAsync();

                // Seed participant
                var participantSeeder = services.GetRequiredService<ParticipantSeeder>();
                await participantSeeder.SeedParticipantsAsync();

            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<UserSeeder>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
    }
}