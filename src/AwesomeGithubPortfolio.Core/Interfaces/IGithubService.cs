using AwesomeGithubPortfolio.Core.Models;
using AwesomeGithubPortfolio.Core.Models.Responses;

namespace AwesomeGithubPortfolio.Core.Interfaces
{
    public interface IGithubService
    {
        Task<GitHubUser> FetchUserData(string username);
        Task<bool> UserHasStarred(string username);
        Task<string> FetchUserReadme(string githubUserLogin);
        Task<PortfolioViewModel> FetchCustomPortfolioFromFork(string username);
        Task<string> ChooseModel(string username);
    }
}