using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;

namespace AwesomeGithubPortfolio.Site.Configuration;

public static class BoostrapConfiguration
{
    public static IServiceCollection ConfigureMvc(this IServiceCollection services, bool isProduction)
    {
        
        if (!isProduction)
        {
            services.AddControllersWithViews(options => options.Filters.Add<LogRequestTimeFilterAttribute>())
                .AddRazorRuntimeCompilation();
        }
        else
        {
            services.AddControllersWithViews();
        }

        services.AddResponseCaching();
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
        });
        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });
        
        return services;
    }
    
}