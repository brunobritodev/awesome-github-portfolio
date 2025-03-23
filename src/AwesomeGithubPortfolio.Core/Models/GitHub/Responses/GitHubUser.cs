namespace AwesomeGithubPortfolio.Core.Models.Responses;

public class GitHubUser
{
    public string Name { get; set; }
    public string Login { get; set; }
    public string Bio { get; set; }
    public string AvatarUrl { get; set; }
    public string Company { get; set; }
    public string Location { get; set; }
    public string WebsiteUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Url { get; set; }
    public ContributionsCollection ContributionsCollection { get; set; }
    public FollowCollection Followers { get; set; }
    public FollowCollection Following { get; set; }
    public Collection<Organization> Organizations { get; set; }
    public Collection<SocialAccount> SocialAccounts { get; set; }
    public Collection<Repository> PinnedItems { get; set; }
    public PullRequests PullRequests { get; set; }
    public Issues Issues { get; set; }
    public Collection<Repository> StarredRepositories { get; set; }
    public bool HasInstagram => SocialAccounts.Nodes.Any(a => a.Provider.ToUpper().Equals("INSTAGRAM"));
    public bool HasLinkedin => SocialAccounts.Nodes.Any(a => a.Provider.ToUpper().Equals("LINKEDIN"));
    public string ProfessionalSummary { get; set; }

    public List<ContributionsCollection> ContributionsThroughYears { get; set; }

    public string GetSocialLink(string provider) => SocialAccounts.Nodes.FirstOrDefault(a => a.Provider.ToUpper().Equals(provider.ToUpper()))?.Url;

    public IEnumerable<string> ListOtherSocialAccounts()
    {
        return SocialAccounts.Nodes.Where(a => a.Provider.ToUpper().Equals("GENERIC")).Select(s => s.Url);
    }
}