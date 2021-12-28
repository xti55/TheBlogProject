using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogProject.Data;
using TheBlogProject.Enums;
using TheBlogProject.Models;

namespace TheBlogProject.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext dbcontext,
                           RoleManager<IdentityRole> roleManager,
                           UserManager<BlogUser> userManager)
        {
            _dbcontext = dbcontext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManageDataAsync()
        {
            await _dbcontext.Database.MigrateAsync();

            await SeedRolesAsync();

            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (_dbcontext.Roles.Any())
            {
                return;
            }

            foreach(var role in Enum.GetNames(typeof(BlogRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task SeedUsersAsync()
        {
            if (_dbcontext.Users.Any())
            {
                return;
            }

            var adminUser = new BlogUser()
            {
                Email = "mtaylor@redshiftsol.com",
                UserName = "mtaylor@redshiftsol.com",
                FirstName = "Matthew",
                LastName = "Taylor",
                DisplayName = "MTaylor",
                PhoneNumber = "(123) 456-7899",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(adminUser, "Abc&123!");
            await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());

            //Create a moderator user
            var modUser = new BlogUser()
            {
                Email = "xti55@redshiftsol.com",
                UserName = "xti55@redshiftsol.com",
                FirstName = "xti55",
                LastName = "user",
                DisplayName = "xti55",
                PhoneNumber = "(123) 456-7800",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(modUser, "Abc&123!");
            await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());
        }
    }
}
