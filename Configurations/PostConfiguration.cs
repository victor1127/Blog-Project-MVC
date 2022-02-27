using BlogProjectMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC.Configurations
{
    public class PostConfiguration
    {
        public PostConfiguration(ModelBuilder builder)
        {
            builder.Entity<Post>()
                   .HasKey(k => k.Id);

            builder.Entity<Post>()
                   .Property(t => t.Title)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Entity<Post>()
                   .Property(a => a.Abstract)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Entity<Post>()
                   .Property(c => c.Content)
                   .IsRequired();

            builder.Entity<Post>()
                   .Ignore(i => i.ImageFile);

            builder.Entity<Post>()
                   .HasOne(p => p.Blog)
                   .WithMany(b => b.Posts)
                   .HasForeignKey(p => p.BlogId);

            builder.Entity<Post>()
                   .HasOne(p => p.Author)
                   .WithMany(u => u.Posts)
                   .HasForeignKey(p => p.AuthorId);

        }

    }
}
