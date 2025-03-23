namespace AwesomeGithubPortfolio.Core.Models;

public record WorkerSection( 
    bool Override,
    Dictionary<string, GitHubRepository> Repositories,
    Dictionary<string, HashSet<string>> RepositoriesFromCategory,
    Dictionary<string, List<string>> LanguagesFromRepository);