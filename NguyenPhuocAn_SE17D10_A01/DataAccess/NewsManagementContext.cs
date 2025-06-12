using Microsoft.EntityFrameworkCore;
using NguyenPhuocAn_SE17D10_A01.Models;

namespace NguyenPhuocAn_SE17D10_A01.DataAccess
{
    public class NewsManagementContext : DbContext
    {
            public NewsManagementContext(DbContextOptions<NewsManagementContext> options)
                : base(options)
            {
            }

            public DbSet<Account> Accounts { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<NewsArticle> NewsArticles { get; set; }
            public DbSet<Tag> Tags { get; set; }
            public DbSet<NewsArticleTag> NewsArticleTags { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<NewsArticleTag>()
                    .HasKey(nat => new { nat.ArticleID, nat.TagID });

                modelBuilder.Entity<NewsArticleTag>()
                    .HasOne(nat => nat.NewsArticle)
                    .WithMany(na => na.NewsArticleTags)
                    .HasForeignKey(nat => nat.ArticleID);

                modelBuilder.Entity<NewsArticleTag>()
                    .HasOne(nat => nat.Tag)
                    .WithMany(t => t.NewsArticleTags)
                    .HasForeignKey(nat => nat.TagID);
            }
        }
}
