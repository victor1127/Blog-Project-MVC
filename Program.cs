using BlogProjectMVC.Data;
using BlogProjectMVC.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var dbContext = host.Services
                                .CreateScope()
                                .ServiceProvider
                                .GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();

            var roleService = host.Services.CreateScope()
                                           .ServiceProvider
                                           .GetRequiredService<RoleService>();
            await roleService.ManageUserRoles();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
