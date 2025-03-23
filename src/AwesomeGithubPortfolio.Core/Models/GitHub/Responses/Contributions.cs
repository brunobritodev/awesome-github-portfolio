namespace AwesomeGithubPortfolio.Core.Models.Responses;

public class Contributions
{
    public IEnumerable<int> ContributionYears { get; set; }
    public int TotalCount { get; set; }
}