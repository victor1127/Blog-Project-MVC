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
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Entity<Blog>()
                   .Property(d => d.Description)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Entity<Blog>()
                   .Ignore(i => i.ImageFile);

            builder.Entity<Blog>()
                   .HasOne(b => b.Author)
                   .WithMany(u => u.Blogs)
                   .HasForeignKey(b => b.AuthorId);

        }
    }
}
