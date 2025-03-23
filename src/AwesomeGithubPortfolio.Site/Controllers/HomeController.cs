using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AwesomeGithubPortfolio.Core.Interfaces;
using AwesomeGithubPortfolio.Core.Services;
using AwesomeGithubPortfolio.Site.BackgroundTasks;
using AwesomeGithubPortfolio.Site.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace AwesomeGithubPortfolio.Site.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPortfolioService _portfolioService;
    private readonly IGithubService _githubService;
    private readonly IConfiguration _configuration;
    private readonly HybridCache _hybridCache;
    private readonly IStringLocalizer<HomeController> _localizer;

    public HomeController(ILogger<HomeController> logger,
        IPortfolioService portfolioService,
        IGithubService githubService,
        IConfiguration configuration,
        HybridCache hybridCache,
        IStringLocalizer<HomeController> localizer)
    {
        _logger = logger;
        _portfolioService = portfolioService;
        _githubService = githubService;
        _configuration = configuration;
        _hybridCache = hybridCache;
        _localizer = localizer;
    }

    [HttpGet, Route("")]
    public async Task<IActionResult> Index(bool? ready, string username)
    {
        if (username.IsPresent())
        {
            var portfolioReady = await _portfolioService.IsPortfolioReady(username);
            if (portfolioReady)
                return RedirectToAction("Portfolio", new { username });
        }

        return View(new IndexViewModel { IsReady = ready, Username = username });
    }

    [HttpGet, Route("portfolio/{username}")]
    public async Task<IActionResult> GeneratePortfolio(string username)
    {
        var entryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(1),
            LocalCacheExpiration = TimeSpan.FromMinutes(1)
        };

        var userStarredRepository = await _hybridCache.GetOrCreateAsync(
            $"{username}en-US",
            async cancel => await _githubService.UserHasStarred(username),
            entryOptions
        );

        if (!userStarredRepository)
            return BadRequest(new ProblemDetails() { Detail = $"User {username} need to Star \u2b50 our repository first." });

        PortfolioGeneratorService.ScheduleTask(new PortfolioTaskInfo(username, "en-US"));
        return Ok();
    }

    [HttpGet, Route("portfolio/{username}/status")]
    public async Task<IActionResult> PortfolioStatus(string username)
    {
        var portfolioReady = await _portfolioService.IsPortfolioReady(username);
        return Ok(new { IsReady = portfolioReady });
    }

    [HttpGet, Route("{username}")]
    public async Task<IActionResult> Portfolio(string username)
    {
        var isPortfolioReady = await _portfolioService.IsPortfolioReady(username);
        if (!isPortfolioReady)
            return RedirectToAction(nameof(Index), new { ready = false, username });

        var entryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(60),
            LocalCacheExpiration = TimeSpan.FromMinutes(60)
        };

        var resume = await _hybridCache.GetOrCreateAsync(
            $"{username}en-US",
            async cancel => await FetchResumeViewModel(username),
            entryOptions
        );

        return View(resume);
    }

    [HttpGet, Route("portfolio/{username}/download")]
    public async Task<IActionResult> Download(string username)
    {
        var isPortfolioReady = await _portfolioService.IsPortfolioReady(username);
        if (!isPortfolioReady)
            return NotFound();

        var entryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(60),
            LocalCacheExpiration = TimeSpan.FromMinutes(60)
        };

        var resume = await _hybridCache.GetOrCreateAsync(
            $"{username}en-US",
            async cancel => await _portfolioService.GetPortfolioAsync(username),
            entryOptions
        );
        var json = JsonSerializer.Serialize(resume);

        return File(Encoding.UTF8.GetBytes(json), "application/json", $"{username}.json");
    }

    private async Task<ResumeViewModel> FetchResumeViewModel(string username)
    {
        // Actually only en-US is available
        //var currentUICulture = Thread.CurrentThread.CurrentUICulture.Name;

        var githubUserData = await _portfolioService.GetPortfolioAsync(username);

        return ResumeViewModel.ToModel(githubUserData, _localizer);
    }
}