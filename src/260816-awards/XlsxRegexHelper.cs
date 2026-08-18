namespace _260816_awards;

using System.Text.RegularExpressions;

public static partial class XlsxRegexHelper
{
  [GeneratedRegex(@"^(1|2|3|4)\/(1|2|3|4)\/3$", RegexOptions.CultureInvariant)]
  private static partial Regex ShooterRegex();

  public static bool IsShooter(string division) =>
      ShooterRegex().IsMatch(division);

  [GeneratedRegex(@"^упр\ (1|2|3|4)\/3 бо$", RegexOptions.CultureInvariant)]
  private static partial Regex CompanyManagerRegex();

  public static bool IsCompanyManager(string division) =>
      CompanyManagerRegex().IsMatch(division);

  [GeneratedRegex(@"^від\.заб\.\/3 бо$", RegexOptions.CultureInvariant)]
  private static partial Regex VidZabRegex();

  public static bool IsVidZab(string division) =>
      VidZabRegex().IsMatch(division);

  [GeneratedRegex(@"^від\.зв\./3 бо$", RegexOptions.CultureInvariant)]
  private static partial Regex VidZvRegex();

  public static bool IsVidZv(string division) =>
      VidZvRegex().IsMatch(division);

  [GeneratedRegex(@"^від\.то\/3 бо$", RegexOptions.CultureInvariant)]
  private static partial Regex VidToRegex();

  public static bool IsVidTo(string division) =>
      VidToRegex().IsMatch(division);

  [GeneratedRegex(@"^м\.п\./3 бо$", RegexOptions.CultureInvariant)]
  private static partial Regex MpRegex();

  public static bool IsMp(string division) =>
      MpRegex().IsMatch(division);

  [GeneratedRegex(@"^упр 3 бо$", RegexOptions.CultureInvariant)]
  private static partial Regex ManagerRegex();

  public static bool IsManager(string division) =>
      ManagerRegex().IsMatch(division);

  public static string GetPlatoonAndCompany(string division)
  {
    var match = ShooterRegex().Match(division);
    return match.Success ? match.Groups[2].Value : string.Empty;
  }

  public static string GetCompanyForManager(string division)
  {
    var match = CompanyManagerRegex().Match(division);
    return match.Success ? match.Groups[1].Value : string.Empty;
  }


  /// <summary>
  /// Визначає підрозділ, в якому рахується персона за допомогою перевірки
  /// відповідності скогоченої назви підрозділу стандартним патернам.
  /// </summary>
  /// <param name="department"></param>
  /// <param name="cleanedName"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentException"></exception>
  public static string GetCompany(string department, string cleanedName)
  {
    return department switch
    {
      var d when IsShooter(d) => GetPlatoonAndCompany(d),
      var d when IsCompanyManager(d) => GetCompanyForManager(d),
      var d when IsVidZab(d) => "від.заб./3 бо",
      var d when IsVidZv(d) => "від.зв./3 бо",
      var d when IsVidTo(d) => "від.то/3 бо",
      var d when IsMp(d) => "м.п./3 бо",
      var d when IsManager(d) => "упр 3 бо",
      _ => throw new ArgumentException(
          $"УВАГА: {cleanedName} вказано невірні дані підрозділу!")
    };
  }

}
