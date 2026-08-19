namespace _260816_awards;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


public static partial class DirectoryHelper
{

  /// <summary>
  /// Повертае імена файлів і директорій (не повні шляхи).
  /// </summary>
  /// <param name="dirPath"></param>
  /// <returns>Пустий або заповнений масив з іменами файлів.</returns>
  /// <exception cref="ArgumentException"></exception>
  public static string?[] FindFiles(string dirPath)
  {
    if (string.IsNullOrWhiteSpace(dirPath))
      throw new ArgumentException($"'{dirPath}' взагалі не шлях!", nameof(dirPath));

    if (!Directory.Exists(dirPath))
    {
      Console.WriteLine($"Директорія '{dirPath}' не існує!");
      return [];
    }

    // Перевіряє, чи є визначений об'єкт директорією.
    var attrs = File.GetAttributes(dirPath);
    if (!attrs.HasFlag(FileAttributes.Directory))
    {
      Console.WriteLine($"'{dirPath}' не є директорією!");
      return [];
    }

    try
    {
      // Повертає тільки файли
      var files = Directory.GetFiles(dirPath);

      if (files.Length == 0)
        Console.WriteLine($"В директорії '{dirPath}' жодного файлу.");

      return [.. files.Select(Path.GetFileName)];
    }
    catch (Exception e)
    {
      Console.WriteLine($"Помилка при читанні директорії: {e.Message}");
      return []; // не має спрацювате, але компілятор вимагає
    }
  }


  /// <summary>
  /// Повертає словник в форматі {"YYYY-MM-DD" : повний шлях до .xlsx файлу}
  /// </summary>
  /// <param name="xlsxFiles"></param>
  /// <param name="directoryPath"></param>
  /// <returns>Словник, який містить пари {'дата': 'ім`я файлу .xlsx'}.</returns>
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
          Console.WriteLine($"Помилка читання {fileName}.");
          continue;
        }
      }
    }

    // Сортую ключі, як дати
    var sorted = dict.OrderBy(kv => kv.Key, StringComparer.Ordinal)
                      .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.Ordinal);

    Console.WriteLine("Знайдені файли .xlsx (відсортовані за датою):");
    foreach (var (k, v) in sorted) Console.WriteLine($"{k}: {v}");

    return sorted;
  }


  /// <summary>
  /// Паттерн для перевірки імен файлів: вони мають починатись з 6 цифр і закінчуватись "-ШПС.xlsx"
  /// </summary>
  /// <returns></returns>
  [GeneratedRegex(@"^\d{6}.*\-ШПС\.xlsx$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
  private static partial Regex FilePatternRegex();


  /// <summary>
  /// Повертає одномірний масив ключів переданого словника.
  /// </summary>
  /// <param name="dict">Словник, чиї ключі потрібні.</param>
  /// <returns>Масив, створений з ключів переданого для обробки словника.</returns>
  public static string[] MakeArrayOfKeys(Dictionary<string, string> dict)
  {
    return [.. dict.Keys];
  }
}