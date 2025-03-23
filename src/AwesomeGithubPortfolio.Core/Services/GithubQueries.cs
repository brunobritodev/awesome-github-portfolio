using System.Text.Json;
using System.Text.Json.Serialization;

namespace AwesomeGithubPortfolio.Core.Services
{
    public static class GithubOptions
    {
        public static JsonSerializerOptions DefaultJson = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IncludeFields = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        public static Dictionary<string, long> PersonalAccessTokenUsage { get; set; } = new();

        public const string OperationName = "userInfo";

        public const string UserFollowers = @"
             query userInfo($login: String!) {
                user(login: $login) {
  	              following (first: 100){
                    nodes{
                      login
                    }
                  }
	              }
             }";
        public const string UserStarredRepositories = @"
             query userInfo($login: String!) {
                user(login: $login) {
  	              starredRepositories {
                    nodes {
                      nameWithOwner
                    }
                  }
	              }
             }";
        public const string UserInformationQuery = @"
            query userInfo($login: String!) {
                user(login: $login) {
                  name
                  login
                  avatarUrl
                  bio
                  company
                  location
                  websiteUrl
                  url
                  createdAt
                  socialAccounts(first: 10) {
                    nodes {
                      provider
                      url
                    }
                  }
                  contributionsCollection {
                    contributionYears
                  }
                  followers(first: 50) {
                    totalCount
                    nodes {
                      avatarUrl
                    }
                  }
                  following(first: 5) {
                    totalCount
                    nodes {
                      avatarUrl
                    }
                  }
                  pullRequests(first: 1) {
                          totalCount
                  }
                  issues(first: 1) {
                      totalCount
                  }
                  organizations(first: 10) {
                  	nodes {
                      name
                      description
                      avatarUrl
                    }
  								}
                  pinnedItems(first: 6, types: [REPOSITORY]) {
                    nodes {
                      ... on Repository {
                        name
                        description
                        url
                        stargazerCount
                        forkCount
                        primaryLanguage {
                          name
                          color
                        }
                      }
                    }
                  }
              }
          }
";
        public const string ContributionsFromYear = @"
            query userInfo($login: String!) {
                user(login: $login) {
                    contributionsCollection(from: ""{year}-01-01T00:00:00Z"", to: ""{next_year}-01-01T00:00:00Z"") {
                        totalCommitContributions
                        totalRepositoryContributions
                        restrictedContributionsCount
                        pullRequestContributionsByRepository{
                          contributions{
                            totalCount
                          }
                          repository {
                            name
                            nameWithOwner
                            description
                            stargazerCount
                            forkCount
                            url
                            languages(first:5){
                              nodes {
                                name
                              }
                            }
                          }
                        }
                        commitContributionsByRepository{
                          contributions {
                            totalCount
                          }
                          repository {
                            name
                            nameWithOwner
                            description
                            stargazerCount
                            forkCount
                            url
                            languages(first:5){
                              nodes {
                                name
                              }
                            }
                          }
                        }
                    }
                }
            }
";

        

        public static string NextPat()
        {
            var pat = PersonalAccessTokenUsage.OrderByDescending(o => o.Value).First();
            var patValue = pat.Value;
            PersonalAccessTokenUsage[pat.Key] = Interlocked.Increment(ref patValue);
            return pat.Key;
        }
    }
}