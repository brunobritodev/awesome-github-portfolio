using System.Text.Json;
using AwesomeGithubPortfolio.Core.Interfaces;
using AwesomeGithubPortfolio.Core.Models;
using AwesomeGithubPortfolio.Core.Models.Responses;
using AwesomeGithubPortfolio.Core.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AwesomeGithubPortfolio.Core.Services;

internal class PortfolioService : IPortfolioService
{
    private readonly IConfiguration _configuration;
    private readonly IGithubService _githubService;
    private readonly IOpenApiService _openApiService;
    private readonly GithubPortfolioContext _context;

    public PortfolioService(
        IConfiguration configuration,
        IGithubService githubService,
        IOpenApiService openApiService,
        GithubPortfolioContext context)
    {
        _configuration = configuration;
        _githubService = githubService;
        _openApiService = openApiService;
        _context = context;
    }

    public async Task<PortfolioViewModel> GetPortfolioAsync(string username, string culture = "en-US")
    {
        var summary = await _context.Summaries.FirstOrDefaultAsync(x => x.Username == username && x.Culture == culture);
        var githubData = await _context.GitHubRaw.FirstOrDefaultAsync(x => x.Username == username);

        if (githubData is null || summary is null)
            return null;

        var githubUser = JsonSerializer.Deserialize<GitHubUser>(githubData.Metadata, GithubOptions.DefaultJson);
        var stats = new UserStats(githubUser);

        var userCustomPortfolio = await  _githubService.FetchCustomPortfolioFromFork(githubUser.Login);
        return PortfolioViewModel.Create(githubUser, stats, summary.Summary, userCustomPortfolio);
    }

    public async Task GeneratePortfolioAsync(string username, string culture = "en-US")
    {
        var githubUser = await FetchGithubUserData(username);
        await FetchUserSummary(githubUser, culture);
    }

    private async Task FetchUserSummary(GitHubUser githubUser, string culture)
    {
        var profileStoredData = _context.Summaries.FirstOrDefault(x => x.Username == githubUser.Login && x.Culture == culture);

        if (profileStoredData != null && !ShouldGatherProfileSummay(profileStoredData))
        {
            return;
        }

        var openAiModel = await _githubService.ChooseModel(githubUser.Login);
        var summary = await _openApiService.GetProfessionalSummary(githubUser, culture, openAiModel);
        if (summary.IsMissing())
        {
            summary = await _githubService.FetchUserReadme(githubUser.Login);
            openAiModel = "GitHub Readme";
        }

        if (profileStoredData is null)
            _context.Summaries.Add(new UserSummary(summary, githubUser.Login, culture, openAiModel));
        else
            profileStoredData.Update(summary, openAiModel, culture);

        await _context.SaveChangesAsync();
    }

    private async Task<GitHubUser> FetchGithubUserData(string username)
    {
        var githubStoredData = _context.GitHubRaw.FirstOrDefault(x => x.Username == username);

        GitHubUser githubUserData = null;

        if (githubStoredData != null && !ShouldGatherGithubData(githubStoredData))
        {
            githubUserData = JsonSerializer.Deserialize<GitHubUser>(githubStoredData.Metadata, GithubOptions.DefaultJson);
        }
        else
        {
            githubUserData = await _githubService.FetchUserData(username);
            if (githubStoredData is null)
                _context.GitHubRaw.Add(new GitHubPublicData(username, githubUserData));
            else
                githubStoredData.Update(githubUserData);

            await _context.SaveChangesAsync();
        }

        return githubUserData;
    }


    private bool ShouldGatherProfileSummay(UserSummary profileSummary)
    {
        if (profileSummary.LastUpdate > DateTime.UtcNow.AddMinutes(-120) || profileSummary.Model.Equals("GitHub Readme"))
            return true;
        return false;
    }

    private bool ShouldGatherGithubData(GitHubPublicData githubUser)
    {
        if (githubUser.LastUpdate > DateTime.UtcNow.AddMinutes(-60))
            return false;
        return true;
    }

    public async Task<bool> IsPortfolioReady(string username)
    {
        var summaryReady = await _context.Summaries.AnyAsync(x => x.Username == username);
        var githubDataReady = await _context.GitHubRaw.AnyAsync(x => x.Username == username);

        return summaryReady && githubDataReady;
    }
}