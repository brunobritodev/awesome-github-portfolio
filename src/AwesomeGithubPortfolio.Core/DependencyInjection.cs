using AwesomeGithubPortfolio.Core.Interfaces;
using AwesomeGithubPortfolio.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Configure Notification
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureGithubServices(this IServiceCollection services, IConfiguration configuration, string githubUrl = "https://api.github.com", string githubRawContent = "https://raw.githubusercontent.com")
        {
            var pats = configuration.GetSection("GITHUB_TOKENS").AsEnumerable();
            foreach (var pat in pats)
            {
                if (pat.Value.IsPresent())
                    GithubOptions.PersonalAccessTokenUsage.Add(pat.Value, 0);
            }

            services.AddScoped<IGithubService, GithubService>();
            services.AddScoped<IOpenApiService, OpenAiService>();
            services.AddScoped<IPortfolioService, PortfolioService>();

            services.AddHttpClient("github", c =>
            {
                var pat = GithubOptions.NextPat();
                c.BaseAddress = new Uri(githubUrl);
                // Github requires a user-agent
                c.DefaultRequestHeaders.Add("Authorization", $"bearer {pat}");

                // Github requires a user-agent
                c.DefaultRequestHeaders.Add("User-Agent", "Awesome-Github-Portfolio");
            });

            return services;
        }
    }
}