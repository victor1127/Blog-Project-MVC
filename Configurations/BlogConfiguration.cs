using BlogProjectMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC.Configurations
{
    public class BlogConfiguration
    {
        public BlogConfiguration(ModelBuilder builder)
        {
            builder.Entity<Blog>()
                   .HasKey(k => k.Id);

            builder.Entity<Blog>()
                   .Property(t => t.Title)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Entity<Blog>()
                   .Property(d => d.Description)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Entity<Blog>()
                   .Ignore(i => i.ImageFile);

            builder.Entity<Blog>()
                   .HasOne(b => b.Author)
                   .WithMany(u => u.Blogs)
                   .HasForeignKey(b => b.AuthorId);

        }
    }
}
