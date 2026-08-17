namespace _260816_awards;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


public static partial class DirectoryHelper
{

  /// <summary>
  /// Returns file/directory entries names (not full paths), same as python`s os.listdir.
  /// </summary>
  /// <param name="dirPath"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentException"></exception>
  public static string?[] FindFiles(string dirPath)
  {
    if (string.IsNullOrWhiteSpace(dirPath))
      throw new ArgumentException("Directory path is required.", nameof(dirPath));

    // Check existence
    if (!Directory.Exists(dirPath))
    {
      Console.WriteLine($"Директорія '{dirPath}' не існує!");
      return [];
    }

    // Check it is a directory
    var attrs = File.GetAttributes(dirPath);
    if (!attrs.HasFlag(FileAttributes.Directory))
    {
      Console.WriteLine($"'{dirPath}' не є директорією!");
      return [];
    }

    try
    {
      // Return only files, not directories
      var files = Directory.GetFiles(dirPath);

      if (files.Length == 0)
        Console.WriteLine("У директорії немає файлів.");

      return [.. files.Select(Path.GetFileName)];
    }
    catch (Exception e)
    {
      Console.WriteLine($"Помилка при читанні директорії: {e.Message}");
      return []; // unreachable, but keeps compiler happy
    }
  }


  /// <summary>
  /// Returns: dictionary key = "YYYY-MM-DD", value = full path to the matching .xlsx file
  /// </summary>
  /// <param name="xlsxFiles"></param>
  /// <param name="directoryPath"></param>
  /// <returns></returns>
  public static Dictionary<string, string> CheckMatching(string[] xlsxFiles, string directoryPath)
  {
    var pattern = FilePatternRegex();
    var matchingFiles = xlsxFiles.Where(f => pattern.IsMatch(Path.GetFileName(f))).ToList();

    if (matchingFiles.Count == 0)
    {
      Console.WriteLine("У директорії немає файлів .xlsx, які починаються з 6 цифр.");
      return [];
    }

    Dictionary<string, string> dict = [];

    foreach (var file in matchingFiles)
    {
      var fileName = Path.GetFileName(file);
      if (fileName.Length < 6) continue;

      var dateStr = fileName[..6];

      if (dateStr.Length == 6 && dateStr.All(char.IsDigit))
      {
        try
        {
          var dateObj = DateTime.ParseExact(dateStr, "yyMMdd", null);
          var dateKey = dateObj.ToString("yyyy-MM-dd");

          dict[dateKey] = Path.Combine(directoryPath, fileName);
        }
        catch (FormatException)
        {
          // invalid date -> skip
          Console.WriteLine($"Помилка читання {fileName}.");
          continue;
        }
      }
    }

    // Sort by keys (dates)
    var sorted = dict.OrderBy(kv => kv.Key, StringComparer.Ordinal)
                      .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.Ordinal);

    Console.WriteLine("Знайдені файли .xlsx (відсортовані за датою):");
    foreach (var (k, v) in sorted) Console.WriteLine($"{k}: {v}");

    return sorted;
  }


  /// <summary>
  /// Pattern: starts with 6 digits, then anything, then "-ШПС.xlsx" (case-insensitive)
  /// </summary>
  /// <returns></returns>
  [GeneratedRegex(@"^\d{6}.*\-ШПС\.xlsx$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
  private static partial Regex FilePatternRegex();


  /// <summary>
  /// Returns a list containing all keys from the supplied dictionary.
  /// </summary>
  /// <param name="dict">Dictionary whose keys are needed.</param>
  /// <returns>List of keys.</returns>
  public static string[] MakeArrayOfKeys(Dictionary<string, string> dict)
  {
    return [.. dict.Keys];
  }
}