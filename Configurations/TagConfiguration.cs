using BlogProjectMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectMVC.Configurations
{
    public class TagConfiguration
    {
        public TagConfiguration(ModelBuilder builder)
        {
            builder.Entity<Tag>()
                   .Property(t => t.Title)
                   .IsRequired();

            builder.Entity<Tag>()
                   .HasOne(t => t.Post)
                   .WithMany(p => p.Tags)
                   .HasForeignKey(t => t.PostId);


        }
    }
}
