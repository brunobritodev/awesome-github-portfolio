using AwesomeGithubPortfolio.Core.Models;

namespace AwesomeGithubPortfolio.Core.Interfaces;

public interface IPortfolioService
{
    Task<PortfolioViewModel> GetPortfolioAsync(string username, string culture = "en-US");
    Task GeneratePortfolioAsync(string username, string culture = "en-US");
    Task<bool> IsPortfolioReady(string username);
}