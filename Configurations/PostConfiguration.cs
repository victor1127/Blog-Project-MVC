﻿using BlogProjectMVC.Models;
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

        }
    }
}
