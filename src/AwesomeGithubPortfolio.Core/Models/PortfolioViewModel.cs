using AwesomeGithubPortfolio.Core.Models.Responses;

namespace AwesomeGithubPortfolio.Core.Models;

public class PortfolioViewModel(
    string name,
    string login,
    DateTime createdAt,
    HeaderSection header,
    AboutSection about,
    SkillSection skillSection,
    ProjectsSection projectsSection,
    FollowerSection followersSection,
    WorkerSection workerSection)
{
    public string Name { get; set; } = name;
    public string Login { get;set; } = login;
    public DateTime CreatedAt { get;set; } = createdAt;
    public HeaderSection Header { get; set;} = header;
    public AboutSection About { get;set; } = about;
    public SkillSection SkillSection { get; set;} = skillSection;
    public ProjectsSection ProjectsSection { get; set;} = projectsSection;
    public FollowerSection FollowersSection { get; set;} = followersSection;
    public WorkerSection WorkerSection { get; set;} = workerSection;

    public static PortfolioViewModel Create(GitHubUser user, UserStats stats, string profileSummary, PortfolioViewModel custowUserPortfolio)
    {
        var portfolio = new PortfolioViewModel(
            user.Name,
            user.Login,
            user.CreatedAt,
            new HeaderSection(
                false,
                user.Bio,
                user.AvatarUrl,
                user.Url,
                user.GetSocialLink("Linkedin"),
                user.GetSocialLink("Instagram"),
                user.ListOtherSocialAccounts()
            ),
            new AboutSection(
                false,
                user.AvatarUrl,
                user.Location,
                null,
                profileSummary
            ),
            new SkillSection(
                false,
                stats.MostUsedLanguages.Take(4).ToList(),
                ListServiceCategories(stats).Take(3)
                    .Select(s => new ServiceCategory(
                            s,
                            LanguageCategoryMap.CategoryIconMap[s]
                        )
                    ).ToList()
            ),
            new ProjectsSection(
                false,
                stats.ListMostFamousRepositories(),
                stats.MyProjects()
            ),
            new FollowerSection(
                user.Followers.Shuffle()
            ),
            new WorkerSection(
                false,
                stats.MyRepositories.Concat(stats.RepositoriesContributedTo).OrderByDescending(o => o.Value.StarCount).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                stats.RepositoriesFromCategory,
                stats.LanguagesFromRepository
            )
        );

        if (custowUserPortfolio != null)
        {
            if (custowUserPortfolio.Header is not null && custowUserPortfolio.Header.Override)
                portfolio.Header = custowUserPortfolio.Header;
            
            if (custowUserPortfolio.About is not null && custowUserPortfolio.About.Override)
                portfolio.About = custowUserPortfolio.About;
            
            if (custowUserPortfolio.SkillSection is not null && custowUserPortfolio.SkillSection.Override)
                portfolio.SkillSection = custowUserPortfolio.SkillSection;
            
            if (custowUserPortfolio.ProjectsSection is not null && custowUserPortfolio.ProjectsSection.Override)
                portfolio.ProjectsSection = custowUserPortfolio.ProjectsSection;
            
            if (custowUserPortfolio.WorkerSection is not null && custowUserPortfolio.WorkerSection.Override)
                portfolio.WorkerSection = custowUserPortfolio.WorkerSection;
        }
        
        return portfolio;
    }

    private static HashSet<string> ListServiceCategories(UserStats stats)
    {
        var categories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        
        foreach (var lang in stats.MostUsedLanguages)
        {
            if (LanguageCategoryMap.Languages.TryGetValue(lang.Name, out var category))
            {
                categories.Add(category);
            }
        }
            
        return categories.ToHashSet();
    }
}