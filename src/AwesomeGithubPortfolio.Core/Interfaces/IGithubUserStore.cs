using AwesomeGithubPortfolio.Core.Models.Responses;

namespace AwesomeGithubPortfolio.Core.Interfaces
{
    public interface IGithubUserStore
    {
        Task<GitHubUser> GetUserInformation(string username);
    }
}
