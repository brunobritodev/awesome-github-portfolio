namespace AwesomeGithubPortfolio.Core.Models.Responses;

public record FollowerCollection(int TotalCount, List<PublicUser> Nodes)
{
    public List<PublicUser> Shuffle()
    {
        return Nodes?.OrderBy(o => Random.Shared.Next()).ToList();
    }
}

public record FollowingCollection(int TotalCount, List<PublicUser> Nodes);

public record PublicUser(string AvatarUrl, string Login);