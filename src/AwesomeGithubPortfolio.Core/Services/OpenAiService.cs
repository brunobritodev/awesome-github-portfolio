using System.Diagnostics;
using AwesomeGithubPortfolio.Core.Interfaces;
using AwesomeGithubPortfolio.Core.Models;
using AwesomeGithubPortfolio.Core.Models.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;
using Polly;
using Polly.Retry;

namespace AwesomeGithubPortfolio.Core.Services;

internal class OpenAiService : IOpenApiService
{
    private readonly OpenAIClient _api;
    private readonly AsyncRetryPolicy _policy;

    private const string prompt = """
                                  You are a professional AI assistant tasked with generating portfolio-style summaries based on GitHub activity data. 
                                  The following JSON represents a user's open-source contributions, including their personal repositories (`myRepositories`), 
                                  contributions to third-party repositories (`repositoriesContributedTo`), pinned repositories, and language usage statistics. 

                                  Write a fluent and natural presentation in the first person, suitable for a portfolio or professional bio, without any titles. 
                                  Begin by the paragraph highlighting the user most impactful work, especially contributions to widely recognized or industry-relevant projects. 
                                  such as official Microsoft, Google, or Amazon repositories, or high-profile open-source projects (e.g., `dotnet`, `StackExchange`, `Go`, `Angular`, etc.).
                                  Then highlight projects with a high number of stars or forks, and strongly emphasize any contributions to major technology organizations or repositories.

                                  You may use the following fields to guide your response:
                                  - `myRepositories`: personal repositories and projects authored by the user, including `starCount`, `forkCount`, and `commits`.
                                  - `repositoriesContributedTo`: third-party repositories the user has contributed to — identify contributions to notable or high-impact projects here.
                                  - `pinnedRepositories`: projects the user has chosen to showcase.
                                  - `mostUsedLanguages`: overall language experience.

                                  Wrap the response in markdown and include clickable links when referencing repositories.
                                  Keep the response under 1500 characters, Keep the response under 1500 characters, and make it sound like the user is proudly presenting their achievements to a potential employer or client.
                                  The response should be in the following language: {1}
                                  ```json
                                  {0}
                                  ```
                                  """;

    public OpenAiService(IConfiguration configuration, ILogger<OpenAiService> logger)
    {
        _api = new OpenAIClient(configuration["OpenAi:ApiKey"]);

        _policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1)
            }, (exception, timeSpan) => { logger.Log(LogLevel.Error, $"Error trying to get openai data: {exception}"); });
    }

    public async Task<string> GetProfessionalSummary(GitHubUser user, string culture, string model)
    {
//         if (Debugger.IsAttached)
//         {
//             return """
//                    ```markdown
//                    My journey in the open-source world has been incredibly rewarding, with remarkable contributions to high-profile projects that have significantly impacted the industry. Among my notable contributions is to [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis), a widely-used Redis client in the .NET ecosystem. Furthermore, my involvement in the [dotnet/runtime](https://github.com/dotnet/runtime) project showcases my commitment to enhancing .NET's cross-platform capabilities for various applications.
//
//                    In my repository portfolio, my work on [JPProject.IdentityServer4.AdminUI](https://github.com/brunobritodev/JPProject.IdentityServer4.AdminUI) stands out with over 733 stars, reflecting its importance in managing IdentityServer4 and ASP.NET Core Identity. Additionally, the [JPProject.IdentityServer4.SSO](https://github.com/brunobritodev/JPProject.IdentityServer4.SSO) project, with 455 stars, has been pivotal in providing secure single sign-on solutions. One of my personal milestones includes creating [awesome-github-stats](https://github.com/brunobritodev/awesome-github-stats), a motivational tool displaying GitHub contributions, which has garnered 234 stars.
//
//                    I am especially proud of my work on [dev-store](https://github.com/desenvolvedor-io/dev-store), a microservices e-commerce reference application, demonstrating my expertise in building robust ASP.NET applications with 1,080 stars. Additionally, my contributions to the [NetDevPack](https://github.com/NetDevPack/NetDevPack), accumulating over 771 stars, reflect my dedication to enhancing developer productivity through reusable code components.
//
//                    C# is my primary language, constituting around 25% of my work, but I am also proficient in HTML, JavaScript, and various other languages, enabling me to adapt to diverse project needs effectively.
//                    ```
//                    """.Replace("```markdown", string.Empty).Replace("```", string.Empty);
//         }

        var userStats = new UserStats(user);
        userStats.Clean();


        var content = System.Text.Json.JsonSerializer.Serialize(userStats, GithubOptions.DefaultJson);
        var chat = _api.GetChatClient(model);
        var response = await _policy.ExecuteAndCaptureAsync(() => chat.CompleteChatAsync(
            string.Format(prompt, content, culture)
        ));

        if (response.Outcome != OutcomeType.Successful || response.Result is null) return string.Empty;

        return response.Result.Value.Content.LastOrDefault()?.Text.Replace("```markdown", string.Empty).Replace("```", string.Empty);
    }
}