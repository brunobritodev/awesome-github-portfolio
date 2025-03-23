namespace AwesomeGithubPortfolio.Core.Models;

/// <summary>
/// This class represent a Repository.
/// The goald is to be a LightWeight object to not send OpenAI bunch of unused and duplicated data
/// </summary>
/// 
public record GitHubRepository(string NameWithOwner, string Name, string Description, string Url, int StarCount, int ForkCount, int? Commits, int? FirstContributionYear);
