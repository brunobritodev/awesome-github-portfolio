namespace AwesomeGithubPortfolio.Core.Models
{
    class DefaultRequest
    {
        public string Query { get; set; }
        public dynamic Variables { get; set; }
        public string OperationName { get; set; }
    }
}
