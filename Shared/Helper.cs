namespace Shared;

public static class Helper
{
    public static string MergeIntoFullName(string name, string firstSurname, string? secondSurname) =>
    string.IsNullOrEmpty(secondSurname) ? $"{name} {firstSurname}" : $"{name} {firstSurname} {secondSurname}";
}