namespace AwesomeGithubPortfolio.Core.Models;

public class LanguageCategoryMap
{
    /// <summary>
    /// Dictionary based on GitHub linguist - https://github.com/github-linguist/linguist
    /// Each category must have his entry at Resources file
    /// </summary>
    public static Dictionary<string, string> Languages = new(StringComparer.OrdinalIgnoreCase)
    {
        // Frontend
        { "HTML", "Frontend" },
        { "CSS", "Frontend" },
        { "SCSS", "Frontend" },
        { "SASS", "Frontend" },
        { "Less", "Frontend" },
        { "Javascript", "Frontend" },
        { "Typescript", "Frontend" },
        { "Vue", "Frontend" },
        { "React", "Frontend" },
        { "Angular", "Frontend" },
        { "Svelte", "Frontend" },
        { "Handlebars", "Frontend" },

        // Backend
        { "C#", "Backend" },
        { "Java", "Backend" },
        { "Ruby", "Backend" },
        { "Smalltalk", "Backend" },
        { "ASP", "Backend" },
        { "ASP.NET", "Backend" },
        { "PHP", "Backend" },
        { "Perl", "Backend" },
        { "Node.js", "Backend" },
        { "Go", "Backend" },
        { "Elixir", "Backend" },
        { "Erlang", "Backend" },
        { "Rust", "Backend" },
        { "C", "Backend" },
        { "C++", "Backend" },
        { "R", "Backend" },
        { "Julia", "Backend" },
        { "Haskell", "Backend" },
        { "Clojure", "Backend" },
        { "Assembly", "Backend" },

        // Devops
        { "Dockerfile", "Devops" },
        { "Shell", "Devops" },
        { "PowerShell", "Devops" },
        { "YAML", "Devops" },
        { "HCL", "Devops" }, // Terraform
        { "Makefile", "Devops" },
        { "Nix", "Devops" },
        { "Groovy", "Devops" }, // Jenkins Pipelines
        { "JSON", "Devops" }, // Configs de cloud / infra-as-code
        { "XML", "Devops" },
        { "Ansible", "Devops" },
        { "Terraform", "Devops" },

        // Infra
        { "Python", "Infra" },
        { "Scala", "Infra" },
        { "Bash", "Infra" },
        { "Lua", "Infra" },
        { "Batchfile", "Infra" },

        // DBA
        { "SQL", "DBA" },
        { "PL/SQL", "DBA" },
        { "T-SQL", "DBA" },
        { "GraphQL", "DBA" },
        { "NoSQL", "DBA" },
        { "MongoDB", "DBA" },
        { "Cassandra", "DBA" },
        { "DynamoDB", "DBA" },
        { "PostgreSQL", "DBA" },
        { "MySQL", "DBA" },
        { "SQLite", "DBA" },
        { "Firebase", "DBA" },

        // Mobile
        { "Swift", "Mobile" },
        { "Objective-C", "Mobile" },
        { "Dart", "Mobile" },
        { "Kotlin", "Mobile" },
        { "React Native", "Mobile" },
    };

    public static Dictionary<string, string> CategoryIconMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Frontend", "fa fa-desktop" },
        { "Backend", "fa fa-code" },
        { "DevOps", "fa fa-cogs" },
        { "Infra", "fa fa-server" }
    };
}