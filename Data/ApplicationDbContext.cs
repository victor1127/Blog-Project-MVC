using BlogProjectMVC.Configurations;
using BlogProjectMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogProjectMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogUser>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogUser> BlogUsers { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //this.ChangeTracker.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            _ = new TagConfiguration(builder);
            _ = new BlogConfiguration(builder);
            _ = new BlogUserConfiguration(builder);
            _ = new CommentConfiguration(builder);
            _ = new PostConfiguration(builder);

            base.OnModelCreating(builder);

        }
    }
}
