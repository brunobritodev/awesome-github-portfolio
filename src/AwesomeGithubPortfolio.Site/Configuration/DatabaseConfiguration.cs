using AwesomeGithubPortfolio.Core.Store;
using Microsoft.EntityFrameworkCore;

namespace AwesomeGithubPortfolio.Site.Configuration;

public static class DatabaseConfiguration
{
    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue("InMemory", false))
        {
            services.AddDbContext<GithubPortfolioContext>(opt => opt.UseInMemoryDatabase("Avera").EnableSensitiveDataLogging());
        }
        else
        {
            services.AddDbContext<GithubPortfolioContext>(builder =>
                builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => { sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null); }));
        }

        return services;
    }
}