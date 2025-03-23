using AwesomeGithubPortfolio.Core.Interfaces;
using AwesomeGithubPortfolio.Core.Services;

namespace AwesomeGithubPortfolio.Site.BackgroundTasks;

public class PortfolioGeneratorService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<PortfolioGeneratorService> _logger;

    private static readonly System.Collections.Concurrent.BlockingCollection<PortfolioTaskInfo> _portfolioQueue = new();

    public PortfolioGeneratorService(IServiceProvider services, ILogger<PortfolioGeneratorService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await GeneratePortfoliosQueue(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task GeneratePortfoliosQueue(CancellationToken stoppingToken)
    {
        if (!_portfolioQueue.Any())
            return;

        using var scope = _services.CreateScope();

        var portfolioService = scope.ServiceProvider.GetRequiredService<IPortfolioService>();
        foreach (var task in _portfolioQueue.GetConsumingEnumerable(stoppingToken))
        {
            await portfolioService.GeneratePortfolioAsync(task.Username, task.Culture);
        }
    }

    public static void ScheduleTask(PortfolioTaskInfo portfolioTaskInfo)
    {
        _portfolioQueue.Add(portfolioTaskInfo);
    }
}