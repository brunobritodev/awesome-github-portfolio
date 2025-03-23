using System.Diagnostics;

namespace AwesomeGithubPortfolio.Core.Models.Responses;

public class Repository
{
    public string Name { get; set; }
    public string NameWithOwner { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public Collection<Language> Languages { get; set; }
    public int ForkCount { get; set; }
    public int StargazerCount { get; set; }
}