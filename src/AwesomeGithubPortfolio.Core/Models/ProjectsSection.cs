namespace AwesomeGithubPortfolio.Core.Models;

public record ProjectsSection(
    bool Override,
    List<GitHubRepository> WellKnownRepositories,
    List<GitHubRepository> MyProjects);