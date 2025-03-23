using AwesomeGithubPortfolio.Core.Models;
using AwesomeGithubPortfolio.Core.Models.Responses;
using AwesomeGithubPortfolio.Core.Store;
using Microsoft.Extensions.Localization;

namespace AwesomeGithubPortfolio.Site.Model;

/// <summary>
/// User cannot customize these settings
/// </summary>
public record HardTranslations(
    string LearnMore,
    string AboutMe,
    string Projects,
    string Work,
    string Services,
    string WellKnowProjects,
    string FirstContribution,
    string MyProjects,
    string FollowersTitle,
    string MoreWork,
    string FollowersMessage,
    string ServiceTitle,
    string ProjectTitle,
    string StartDateMessage,
    string MeInNumbersTitle,
    Dictionary<string, string> CategoryDescription);

public record ResumeViewModel(
    string Name,
    string Login,
    HardTranslations Sections,
    HeaderSection Header,
    AboutSection About,
    SkillSection SkillSection,
    ProjectsSection ProjectsSection,
    FollowerSection FollowersSection,
    WorkerSection WorkerSection)
{
    public static ResumeViewModel ToModel(PortfolioViewModel portfolio, IStringLocalizer localizer)
    {
        string startDate = null;
        if (Random.Shared.Next(0, 2) == 0)
        {
            startDate = localizer.GetString($"StartDate-0", portfolio.CreatedAt.Year);
        }
        else
        {
            var totalDays = DateTime.Now.Subtract(portfolio.CreatedAt).TotalDays;
            startDate = localizer.GetString($"StartDate-1", (int)Math.Round(totalDays > 365 ? totalDays / 365 : 1));
        }

        return new ResumeViewModel(
            portfolio.Name,
            portfolio.Login,
            new HardTranslations(
                localizer.GetString("LearnMore"),
                localizer.GetString("AboutMe"),
                localizer.GetString("Projects"),
                localizer.GetString("Work"),
                localizer.GetString("Services"),
                localizer.GetString("WellKnowProjects"),
                localizer.GetString("FirstContribution"),
                localizer.GetString("MyProjects"),
                localizer.GetString("Followers"),
                localizer.GetString("MoreWork"),
                localizer.GetString("FollowersMessage"),
                localizer.GetString("ServicesWhatIDo"),
                localizer.GetString("ProjectsTitle"),
                startDate,
                localizer.GetString("MeInNumbersTitle"),
                portfolio.SkillSection.ServiceCategories.ToDictionary(s =>  s.Category, cat => localizer.GetString($"Category{cat.Category}")?.Value)
            ),
            portfolio.Header,
            portfolio.About,
            portfolio.SkillSection,
            portfolio.ProjectsSection,
            portfolio.FollowersSection,
            portfolio.WorkerSection
        );
    }

    public string AboutMeTitle()
    {
        return About.CareerStartMessage ?? Sections.StartDateMessage;
    }
}