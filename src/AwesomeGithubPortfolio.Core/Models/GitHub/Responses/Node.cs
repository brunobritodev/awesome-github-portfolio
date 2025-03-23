namespace AwesomeGithubPortfolio.Core.Models.Responses
{
    public class Collection<T>
    {
        public PageInfo PageInfo { get; set; }
        public List<T> Nodes { get; set; }
        
    }
}