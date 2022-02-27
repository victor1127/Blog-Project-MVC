using BlogProjectMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC.Configurations
{
    public class BlogUserConfiguration
    {
        public BlogUserConfiguration(ModelBuilder builder)
        {
            builder.Entity<BlogUser>()
                   .Property(f => f.FirstName)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Entity<BlogUser>()
                   .Property(l => l.LastName)
                   .HasMaxLength(80)
                   .IsRequired();

            builder.Entity<BlogUser>()
                   .Ignore(f => f.FullName);

            builder.Entity<BlogUser>()
                   .Ignore(i => i.ImageFile);

        }
    }
}
