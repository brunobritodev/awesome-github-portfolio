using AwesomeGithubPortfolio.Core.Models;
using AwesomeGithubPortfolio.Core.Models.Responses;

namespace AwesomeGithubPortfolio.Core.Interfaces;

public interface IOpenApiService
{
     Task<string> GetProfessionalSummary(GitHubUser user, string culture, string model);
}