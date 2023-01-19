using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SonOfBlogUpdater.Models;

namespace SonOfBlogUpdater.Data
{
    public class BlogDBContext : IdentityDbContext
    {
        public BlogDBContext(DbContextOptions<BlogDBContext> options)
         : base(options)
        {
        }

        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<MetaTag> MetaTag { get; set; }
        public DbSet<BlogPostMetaTag> BlogPostMetaTag { get; set; }
        public DbSet<BlogPostAuthor> BlogPostAuthor { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlogPostMetaTag>()
                .HasKey(pt => new { pt.BlogPostId, pt.MetaTagId });

            modelBuilder.Entity<BlogPostAuthor>()
                .HasKey(pt => new { pt.BlogPostId, pt.AuthorId });

        

        }

    }
}
