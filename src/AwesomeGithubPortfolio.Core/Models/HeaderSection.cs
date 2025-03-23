namespace AwesomeGithubPortfolio.Core.Models;

public record HeaderSection(
    bool Override,
    string Bio,
    string Picture,
    string GitHubUrl,
    string LinkedinUrl,
    string InstagramUrl,
    IEnumerable<string> OtherSocialAccounts)
{
    public bool HasLinkedin => !string.IsNullOrEmpty(LinkedinUrl);
    public bool HasInstagram => !string.IsNullOrEmpty(InstagramUrl);
}