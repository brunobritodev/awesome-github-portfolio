namespace AwesomeGithubPortfolio.Core.Models.Responses;

public record FollowCollection(int TotalCount, List<PublicUser> Nodes)
{
    public PageInfo PageInfo { get; set; }
    public List<PublicUser> Shuffle()
    {
        return Nodes?.OrderBy(o => Random.Shared.Next()).ToList();
    }
}

public record PageInfo(string EndCursor, bool HasNextPage);
public record PublicUser(string AvatarUrl, string Login);