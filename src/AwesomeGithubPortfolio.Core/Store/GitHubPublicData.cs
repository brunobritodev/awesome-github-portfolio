using AwesomeGithubPortfolio.Core.Models.Responses;
using AwesomeGithubPortfolio.Core.Services;

namespace AwesomeGithubPortfolio.Core.Store;

public class GitHubPublicData
{
    public int Id { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Metadata { get; set; }
    public DateTime LastUpdate { get; set; }

    public GitHubPublicData()
    {
    }

    public GitHubPublicData(string username, GitHubUser githubUserData)
    {
        CreatedAt = DateTime.UtcNow;
        LastUpdate = DateTime.UtcNow;
        Username = username;
        Metadata = System.Text.Json.JsonSerializer.Serialize(githubUserData, GithubOptions.DefaultJson);
    }


    public void Update(GitHubUser githubUserData)
    {
        Metadata = System.Text.Json.JsonSerializer.Serialize(githubUserData, GithubOptions.DefaultJson);
        CreatedAt = DateTime.UtcNow;
    }
}