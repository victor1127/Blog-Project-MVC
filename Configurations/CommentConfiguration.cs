using BlogProjectMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC.Configurations
{
    public class CommentConfiguration
    {
        public CommentConfiguration(ModelBuilder builder)
        {
            builder.Entity<Comment>()
                   .HasKey(k => k.Id);

            builder.Entity<Comment>()
                   .Property(c => c.Content)
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Entity<Comment>()
                   .Property(m => m.ModeratedContent)
                   .HasMaxLength(500);

            builder.Entity<Comment>()
                   .HasOne(c => c.Post)
                   .WithMany(p => p.Comments)
                   .HasForeignKey(c => c.PostId);

            builder.Entity<Comment>()
                   .HasOne(c => c.Author)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.AuthorId);

            builder.Entity<Comment>()
                   .HasOne(c => c.Author)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.AuthorId);


        }
    }
}
