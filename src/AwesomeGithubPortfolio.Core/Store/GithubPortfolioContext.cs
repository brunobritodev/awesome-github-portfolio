using AwesomeGithubPortfolio.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AwesomeGithubPortfolio.Core.Store;

public class GithubPortfolioContext : DbContext
{
    public GithubPortfolioContext(DbContextOptions<GithubPortfolioContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var userSummarySetup=builder.Entity<UserSummary>(); 
        userSummarySetup.HasKey(k => k.Id);
        userSummarySetup.HasIndex(p => p.Username).IsUnique();
        userSummarySetup.Property(p => p.Username).IsRequired().HasMaxLength(64);
        userSummarySetup.Property(p => p.Model).HasMaxLength(32).IsRequired();
        userSummarySetup.Property(p => p.Culture).HasMaxLength(10).IsRequired();
        

        var githubData = builder.Entity<GitHubPublicData>();
        githubData.HasKey(k => k.Id);
        githubData.HasIndex(p => p.Username).IsUnique();
        githubData.Property(p => p.Username).IsRequired().HasMaxLength(64);
        
        base.OnModelCreating(builder);
    }

    public DbSet<UserSummary> Summaries { get; set; }
    public DbSet<GitHubPublicData> GitHubRaw { get; set; }
}