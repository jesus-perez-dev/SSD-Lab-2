using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SSDLab1.Models;

namespace SSDLab1.Data
{
    public static class DbInitializer
    {
        public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            // create the database if it doesn't exist
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Check if teams already exist and exit if there are
            if (context.Teams.Any())
                return 5;  // should log an error message here

            // Seed teams
            int result = await SeedTeams(context);
            if (result != 0)
                return 6;  // should log an error message here

            // Check if roles already exist and exit if there are
            if (roleManager.Roles.Count() > 0)
                return 1;  // should log an error message here

            // Seed roles
            result = await SeedRoles(roleManager);
            if (result != 0)
                return 2;  // should log an error message here

            // Check if users already exist and exit if there are
            if (userManager.Users.Count() > 0)
                return 3;  // should log an error message here

            // Seed users
            result = await SeedUsers(userManager);
            if (result != 0)
                return 4;  // should log an error message here
            

            return 0;
        }

        private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Create Manager Role
            var result = await roleManager.CreateAsync(new IdentityRole("Manager"));
            if (!result.Succeeded)
                return 1;  // should log an error message here

            // Create Player Role
            result = await roleManager.CreateAsync(new IdentityRole("Player"));
            if (!result.Succeeded)
                return 2;  // should log an error message here

            return 0;
        }

        private static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Create Manager User
            var managerUser = new ApplicationUser
            {
                UserName = "the.manager@mohawkcollege.ca",
                Email = "the.manager@mohawkcollege.ca",
                FirstName = "The",
                LastName = "Manager",
                EmailConfirmed = true,
                BirthDate = new DateTime(1990, 1, 1)
            };
            var result = await userManager.CreateAsync(managerUser, "Password!1");
            if (!result.Succeeded)
                return 1;  // should log an error message here

            // Assign user to Admin role
            result = await userManager.AddToRoleAsync(managerUser, "Manager");
            if (!result.Succeeded)
                return 2;  // should log an error message here

            // Create Player User
            var playerUser = new ApplicationUser
            {
                UserName = "the.player@mohawkcollege.ca",
                Email = "the.player@mohawkcollege.ca",
                FirstName = "The",
                LastName = "Player",
                EmailConfirmed = true,
                BirthDate = new DateTime(2000, 12, 12)
            };
            result = await userManager.CreateAsync(playerUser, "Password!1");
            if (!result.Succeeded)
                return 3;  // should log an error message here

            // Assign user to Member role
            result = await userManager.AddToRoleAsync(playerUser, "Player");
            if (!result.Succeeded)
                return 4;  // should log an error message here

            return 0;
        }
        
        private static async Task<int> SeedTeams(ApplicationDbContext context)
        {
            // Create Teams
            var team1 = new Team()
            {
                Id = "1",
                Email = "team1@mohawkcollege.ca",
                EstablishedDate = new DateTime(),
                TeamName = "Team 1"
            };
            
            // Create Teams
            var team2 = new Team()
            {
                Id = "2",
                Email = "team2@mohawkcollege.ca",
                EstablishedDate = new DateTime(),
                TeamName = "Team 2"
            };
            
            // Create Teams
            var team3 = new Team()
            {
                Id = "3",
                Email = "team3@mohawkcollege.ca",
                EstablishedDate = new DateTime(),
                TeamName = "Team 3"
            };

            await context.AddAsync(team1);
            await context.AddAsync(team2);
            await context.AddAsync(team3);
            
            await context.SaveChangesAsync();
            
            return 0;
        }
    }
}