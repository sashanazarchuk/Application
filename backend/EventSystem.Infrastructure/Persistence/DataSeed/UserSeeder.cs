using EventSystem.Application.Exceptions;
using EventSystem.Domain.Entities;
using EventSystem.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Persistence.DataSeed
{
    internal class UserSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EventSystemDbContext _context;
        private readonly ILogger<UserSeeder> _logger;

        public UserSeeder(UserManager<ApplicationUser> userManager, EventSystemDbContext context, ILogger<UserSeeder> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task SeedUserAsync()
        {

            var usersToSeed = new List<(Guid id, string FullName, string Email, string Password)>
            {
                (Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Jane Doe", "janedoe@gmail.com", "Password123!"),
                (Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "John Smith", "johnsmith@gmail.com", "Password123!")
            };

            _logger.LogInformation("Starting user seeding"); 
            foreach (var u in usersToSeed)
            {
                var appUser = await EnsureAppUserExistsAsync(u.id, u.FullName, u.Email, u.Password);
                await EnsureDomainUserExistsAsync(appUser);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("User seeding completed.");
        }


        private async Task<ApplicationUser> EnsureAppUserExistsAsync(Guid id, string fullName, string email, string password)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            if (appUser != null)
            {
                _logger.LogInformation($"Application user {email} already exists.");
                return appUser;
            }
            appUser = new ApplicationUser
            {
                Id = id,
                FullName = fullName,
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(appUser, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException($"Failed to create user {email}: {errors}");
            }

            _logger.LogInformation($"Created application user {email}.");
            return appUser;
        }

        private async Task EnsureDomainUserExistsAsync(ApplicationUser appUser)
        {
            var exists = await _context.DomainUsers.AnyAsync(d => d.Id == appUser.Id);
            if (exists)
            {
                _logger.LogInformation($"Domain user for {appUser.Email} already exists.");
                return;
            }

            _context.DomainUsers.Add(new User
            {
                Id = appUser.Id,
                FullName = appUser.FullName
            });

            _logger.LogInformation($"Created domain user for {appUser.Email}.");
        }
    }
}