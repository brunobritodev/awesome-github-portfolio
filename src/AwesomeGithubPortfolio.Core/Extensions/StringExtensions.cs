namespace Microsoft.Extensions.DependencyInjection.Extensions;

public static class StringExtensions
{
    public static bool IsMissing(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
    public static bool IsPresent(this string value)
    {
        return !IsMissing(value);
    }
}