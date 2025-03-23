using AwesomeGithubPortfolio.Core.Models.Responses;

namespace AwesomeGithubPortfolio.Core.Models;

public record SkillSection(
    bool Override,
    List<LanguageUsage> UsedLanguages,
    List<ServiceCategory> ServiceCategories);