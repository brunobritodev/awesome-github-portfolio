namespace AwesomeGithubPortfolio.Core.Store;

public class UserSummary
{
    public int Id { get; set; }
    public string Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdate { get; set; }
    public string Summary { get; set; }
    public string Culture { get; set; }
    public string Model { get; set; }

    public UserSummary() { }

    public UserSummary(string summary, string username, string culture, string openAiModel)
    {
        Summary = summary;
        Username = username;
        Culture = culture;
        Model = openAiModel;
        CreatedAt = DateTime.UtcNow;
        LastUpdate = CreatedAt;
    }


    public void Update(string summary, string openAiModel, string culture)
    {
        Summary = summary;
        Model = openAiModel;
        Culture = culture;
        LastUpdate = DateTime.UtcNow;
    }
}