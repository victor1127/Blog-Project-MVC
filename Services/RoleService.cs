using BlogProjectMVC.Data;
using BlogProjectMVC.Enums;
using BlogProjectMVC.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC.Services
{
    public class RoleService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public RoleService(ApplicationDbContext dbContext,
                           RoleManager<IdentityRole> roleManager,
                           UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManageUserRoles()
        {
            await CreateRolesAsync();
            await CreateUserAsync();
        }

        private async Task CreateRolesAsync()
        {
            if (_dbContext.Roles.Any()) return;

            foreach (var role in Enum.GetNames(typeof(BlogRoles)))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task CreateUserAsync()
        {
            if (_dbContext.Users.Any()) return;

            var admUser = new BlogUser
            {
                Email = "vsosa01127@gmail.com",
                UserName= "vsosa01127@gmail.com",
                FirstName = "Victor",
                LastName = "Sosa",
                EmailConfirmed = true
            };

            var modUser = new BlogUser
            {
                Email = "vsosa@gmail.com",
                UserName= "vsosa@gmail.com",
                FirstName = "Manuel",
                LastName = "De Armas",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(admUser, "Victor01127*");
            await _userManager.AddToRoleAsync(admUser, BlogRoles.Administrador.ToString());

            await _userManager.CreateAsync(modUser, "Victor01127*");
            await _userManager.AddToRoleAsync(modUser, BlogRoles.Moderador.ToString());

        }

    }
}
