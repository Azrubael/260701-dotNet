namespace _260816_awards;

using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using static XlsxRegexHelper;

public class XlsxHandler
{

  /// <summary>
  /// Головна структура даних для збереження детальної інформації
  /// для кожного співпрацівника.
  /// </summary>
  public sealed class Shpk
  {
    public Dictionary<string, Person> PersonalData { get; } = [];

    public bool AddToShpk(string name, Person person) =>
        PersonalData.TryAdd(name, person);

  }


  /// <summary>
  /// Cтруктура даних що відпувідає за збереження інформації щодо кожної особи.
  /// </summary>
  public sealed class Person
  {
    private static int _nextCount;

    public int Count { get; }
    public string Rank { get; set; }
    public string Department { get; set; }
    public string Ipn { get; set; }
    public string Vacation { get; set; }
    public string Note { get; set; }

    public Dictionary<string, string> Awards { get; init; }

    // Простий конструктор, який ініціалізує порожній словник.
    public Person()
    {
      Rank = string.Empty;
      Department = string.Empty;
      Ipn = string.Empty;
      Vacation = string.Empty;
      Note = string.Empty;
      Awards = [];
      Count = Interlocked.Increment(ref _nextCount);
    }

    public void AddAward(string day, string status) =>
        Awards.TryAdd(day, status);

  }


  /// <summary>
  /// Очищає рядок від зайвих пробілів і переносів рядків.
  /// </summary>
  /// <returns>Повертає повертає «чисте» повне ПІБ або пустий рядок.</returns>
  public static string CleanFullName(string? fullName)
  {
    if (string.IsNullOrWhiteSpace(fullName))
      return string.Empty;

    try
    {
      var parts = fullName
          .Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
      return string.Join(" ", parts).Trim();
    }
    catch (Exception e)
    {
      Console.WriteLine($"Помилка при очищенні імені: {e}");
      return string.Empty;
    }
  }


  /// <summary>
  /// Читання даних файлу ШПК 'shpkFilePath' за один визначений день 'day'
  /// і збереження їх до 'shpk'.
  /// </summary>
  /// <param name="day"></param>
  /// <param name="shpkFilePath"></param>
  /// <param name="shpk"></param>
  public static void ReadShpk(string day, string shpkFilePath, Shpk shpk)
  {
    XLWorkbook? wb = ReadFileShpkBook(shpkFilePath)
        ?? throw new InvalidOperationException(
          $"Файл {shpkFilePath} не відповідає формату xlsx.");

    IXLWorksheet? ws = wb.Worksheet("ШПС")
        ?? throw new InvalidOperationException(
          $"В файлі {shpkFilePath} не знайдено аркуш 'ШПС'."); ;

    string fullName;
    for (int rowNum = 4; rowNum <= 630; rowNum++)
    {
      IXLRow? row = ws.Row(rowNum);
      fullName = row.Cell(9).GetString();                 // стовпчик I
      if (fullName == string.Empty)
        continue;

      string cleanedName = CleanFullName(fullName);

      if (shpk.PersonalData.TryGetValue(cleanedName, out Person? person))
      {
        person.AddAward(day, row.Cell(19).GetString());
      }
      else
      {
        Person newPerson = new()
        {
          Rank = row.Cell(8).GetString(),                 // стовпчик H
          Department = GetCompany(
              row.Cell(11).GetString(), cleanedName),     // стовпчик K
          Ipn = row.Cell(15).GetString(),                 // стовпчик O
          Vacation = row.Cell(24).GetString()             // стовпчик X
        };
        newPerson.AddAward(day, row.Cell(19).GetString());
        shpk.AddToShpk(cleanedName, newPerson);
      }
    }
  }


  /// <summary>
  /// Зчитує файли *.xlsx визначеного формату і створює нову структуру даних
  /// для збереження в новому файлі звіту *.xlsx.
  /// </summary>
  /// <param name="matchedFiles"></param>
  /// <returns>Створений об'єкт XLWorkbook</returns>
  public static List<object[]> CreateLongReportObj(string[] dates, Shpk shpk)
  {
    string[] header = ["# з/п", "Звання", "ПІБ", "Підрозділ", "РНОКПП", "Примітка"];
    List<object[]> longReportTable = [];
    longReportTable.Add([$"Звіт стосовно нарахування премії з {dates[0]} по {dates[^1]}."]);
    longReportTable.Add([.. header, .. dates]);

    foreach ((string name, Person person) in shpk.PersonalData)
    {
      string[]? awards = new string[dates.Length];
      for (int i = 0; i < dates.Length; i++)
      {
        awards[i] = person.Awards.GetValueOrDefault(dates[i], "немає");
      }

      longReportTable.Add([
        person.Count,
        person.Rank,
        name,
        person.Department,
        person.Ipn,
        person.Note,
        .. awards
        // .. person.Awards.Values
        ]);
    }

    return longReportTable;
  }


  /// <summary>
  /// Читає xlsx файл, в якому має бути аркуш "ШПС".
  /// </summary>
  /// <param name="xlsxFilePath"></param>
  /// <returns>Повертає зчитаний аркуш xlsx в форматі об'єкту IXLWorksheet або null.</returns>
  public static XLWorkbook? ReadFileShpkBook(string xlsxFilePath)
  {
    try
    {
      if (!File.Exists(xlsxFilePath))
      {
        return null;
      }

      return new XLWorkbook(xlsxFilePath);
    }
    catch (Exception e)
    {
      Console.WriteLine($"Помилка при читанні {xlsxFilePath}: {e.Message}");
      return null;
    }
  }


  /// <summary>
  /// Зберігає файл *.xlsx
  /// </summary>
  /// <param name="wb"></param>
  /// <param name="reportFilePath"></param>
  public static void SaveXlsx(List<object[]> longReportTable, string reportFilePath)
  {
    try
    {
      using var wb = new XLWorkbook();
      IXLWorksheet ws = wb.Worksheets.Add("LongReport");

      for (int rowIndex = 0; rowIndex < longReportTable.Count; rowIndex++)
      {
        object[] row = longReportTable[rowIndex];

        for (int columnIndex = 0; columnIndex < row.Length; columnIndex++)
        {
          object? value = row[columnIndex];

          ws.Cell(rowIndex + 1, columnIndex + 1).Value =
              value?.ToString() ?? string.Empty;
        }
      }

      wb.SaveAs(reportFilePath);
      Console.WriteLine($"Файл {reportFilePath} успішно збережений.");

    }
    catch (Exception e)
    {
      Console.WriteLine($"Помилка при збереженні {reportFilePath}: {e.Message}");
    }
  }
};
