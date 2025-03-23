using System.Text.Json.Serialization;
using AwesomeGithubPortfolio.Core.Models.Responses;

namespace AwesomeGithubPortfolio.Core.Models;

public class UserStats
{
    /// <summary>
    /// This factory organize all bunch of unustrutured data from GitHub API's
    /// The goal is to create useful data to serve application in more easy way
    /// </summary>
    public UserStats(GitHubUser user)
    {
        Name = user.Name;
        Username = user.Login;
        Bio = user.Bio;
        var myLanguages = new List<string>();
        foreach (var yearOfContributions in user.ContributionsThroughYears.OrderBy(o => o.Year))
        {
            foreach (var contribution in yearOfContributions.CommitContributionsByRepository.Concat(yearOfContributions.PullRequestContributionsByRepository).OrderByDescending(o => o.Repository.StargazerCount))
            {
                var repoLanguages = contribution.Repository.Languages.Nodes.Select(o => o.Name).ToList();
                myLanguages.AddRange(repoLanguages);
                LanguagesFromRepository.TryAdd(contribution.Repository.Name, repoLanguages);

                foreach (var language in contribution.Repository.Languages.Nodes)
                {
                    if (LanguageCategoryMap.Languages.ContainsKey(language.Name))
                    {
                        if (!RepositoriesFromCategory.ContainsKey(LanguageCategoryMap.Languages[language.Name]))
                            RepositoriesFromCategory.Add(LanguageCategoryMap.Languages[language.Name], []);

                        RepositoriesFromCategory[LanguageCategoryMap.Languages[language.Name]].Add(contribution.Repository.Name);
                    }
                }

                // User contributions in his own repo
                if (!MyRepositories.ContainsKey(contribution.Repository.Name))
                {
                    if (contribution.Repository.NameWithOwner.Contains(user.Login) ||
                        // If the contribution is in any of user Org, so will be treated as his own repo
                        user.Organizations.Nodes.Any(org => contribution.Repository.NameWithOwner.Contains(org.Name))
                       )
                    {
                        var repo = new GitHubRepository(
                            contribution.Repository.NameWithOwner,
                            contribution.Repository.Name,
                            contribution.Repository.Description,
                            contribution.Repository.Url,
                            contribution.Repository.StargazerCount,
                            contribution.Repository.ForkCount,
                            contribution.Contributions.TotalCount,
                            yearOfContributions.Year
                        );
                        // The repository should not be in Both Plces (RepoContributedFor and MyRepos)
                        MyRepositories.Add(contribution.Repository.Name, repo);

                        // Repositories will have all data. But it's not sent to openai
                        Repositories.Add(repo);
                        continue;
                    }
                }

                // User contributions in other repos
                if (!RepositoriesContributedTo.ContainsKey(contribution.Repository.Name) && !MyRepositories.ContainsKey(contribution.Repository.Name))
                {
                    var repo = new GitHubRepository(
                        contribution.Repository.NameWithOwner,
                        contribution.Repository.Name,
                        contribution.Repository.Description,
                        contribution.Repository.Url,
                        contribution.Repository.StargazerCount,
                        contribution.Repository.ForkCount,
                        contribution.Contributions.TotalCount,
                        yearOfContributions.Year
                    );
                    RepositoriesContributedTo.Add(contribution.Repository.Name, repo);
                    // Repositories will have all data. But it's not sent to openai
                    Repositories.Add(repo);
                }
            }
        }

        MyRepositories.Values.ToDictionary(repo => repo.Name, repo => repo);
        PinnedRepositories = user.PinnedItems.Nodes.Select(pinned =>
            new GitHubRepository(
                pinned.NameWithOwner,
                pinned.Name,
                pinned.Description,
                pinned.Url,
                pinned.StargazerCount,
                pinned.ForkCount,
                null,
                null
            )).ToList();

        CommitCount = user.ContributionsThroughYears.Sum(s => s.TotalCommitContributions + s.RestrictedContributionsCount);
        CreatedRepositories = user.ContributionsThroughYears.Sum(s => s.TotalRepositoryContributions);
        PullRequestsToAnotherRepositories = user.ContributionsThroughYears.SelectMany(s => s.PullRequestContributionsByRepository).Where(w => !w.Repository.NameWithOwner.Contains(user.Login)).Sum(s => s.Contributions.TotalCount);

        DirectStars = MyRepositories.Sum(s => s.Value.StarCount);
        CommitsToMyRepositories = MyRepositories.Sum(s => s.Value.Commits ?? 0);
        CommitsToAnotherRepositories = CommitCount - CommitsToMyRepositories;
        IndirectStars = RepositoriesContributedTo.Sum(s => s.Value.StarCount);
        ContributedTo = Repositories.Count;
        ContributedToNotOwnerRepositories = RepositoriesContributedTo.Count;
        ContributedToOwnRepositories = MyRepositories.Count;

        MostUsedLanguages = myLanguages
            .GroupBy(language => language)
            .Select(group => new LanguageUsage(
                Name: group.Key,
                Count: group.Count(),
                Percentage: (double)group.Count() / myLanguages.Count * 100
            ))
            .OrderByDescending(lang => lang.Count)
            .ToList();

        LastFetch = DateTime.Now;
    }

    [JsonIgnore] public List<GitHubRepository> Repositories { get; } = new();

    [JsonIgnore] public Dictionary<string, List<string>> LanguagesFromRepository { get; } = new();
    [JsonIgnore] public Dictionary<string, HashSet<string>> RepositoriesFromCategory { get; } = new();

    public string Bio { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public Dictionary<string, GitHubRepository> MyRepositories { get; private set; } = new();
    public Dictionary<string, GitHubRepository> RepositoriesContributedTo { get; private set; } = new();
    public List<GitHubRepository> PinnedRepositories { get; }
    public List<LanguageUsage> MostUsedLanguages { get; }
    public int CommitsToMyRepositories { get; set; }


    public UserStats()
    {
        LastFetch = DateTime.Now;
    }

    /// <summary>
    /// When data was fetched
    /// </summary>
    public DateTime LastFetch { get; }

    /// <summary>
    /// How many commits was made
    /// </summary>
    public int CommitCount { get; }

    /// <summary>
    /// Commits made in repositories that user isn't the owner
    /// </summary>
    public int CommitsToAnotherRepositories { get; }

    /// <summary>
    /// How many pr was made for repositories that user isn't the owner
    /// </summary>
    public int PullRequestsToAnotherRepositories { get; }

    /// <summary>
    /// How many repositories user have created
    /// </summary>
    public int CreatedRepositories { get; }

    /// <summary>
    /// Stars from repositories that user has created under his name
    /// </summary>
    public int DirectStars { get; }

    /// <summary>
    /// Stars from repositories that user has contributed for
    /// </summary>
    public int IndirectStars { get; }

    /// <summary>
    /// How many repositories user has contributed
    /// </summary>
    public int ContributedTo { get; }

    /// <summary>
    /// Repositories that user contributed and he/she is the owner
    /// </summary>
    public int ContributedToOwnRepositories { get; }

    /// <summary>
    /// Repositories that user contributed and he/she isn't the owner
    /// </summary>
    public int ContributedToNotOwnerRepositories { get; }

    private static string FormatNumber(int num)
    {
        if (num >= 100000)
        {
            return FormatNumber(num / 1000) + "K";
        }

        if (num >= 10000)
        {
            return (num / 1000D).ToString("0.#") + "K";
        }

        if (num >= 1000)
        {
            return (num / 1000D).ToString("0.#") + "K";
        }

        return num.ToString("#,0");
    }

    /// <summary>
    /// Keep only relevant data. 
    /// </summary>
    public void Clean()
    {
        MyRepositories = MyRepositories.Values.OrderByDescending(b => b.StarCount).Take(30).ToDictionary(repo => repo.Name, repo => repo);
        RepositoriesContributedTo = RepositoriesContributedTo.Values.OrderByDescending(b => b.StarCount).Take(30).ToDictionary(repo => repo.Name, repo => repo);
    }
    public string TotalCommits => FormatNumber(CommitCount);
    public string TotalStars => FormatNumber(DirectStars + IndirectStars);
    public string TotalContributedFor => FormatNumber(ContributedTo);

    public List<GitHubRepository> ListMostFamousRepositories()
    {
        return Repositories
            .OrderByDescending(o => o.StarCount)
            .Take(5)
            .ToList();
    }

    public List<GitHubRepository> MyProjects()
    {
        return MyRepositories.Values
            .OrderByDescending(o => o.StarCount)
            .Take(5)
            .ToList();
    }
}

