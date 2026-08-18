namespace _260816_awards;

using ClosedXML.Excel;
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
    public string Rank { get; set; }
    public string Department { get; set; }
    public string Ipn { get; set; }
    public string Vacation { get; set; }
    public string Note { get; set; }

    public Dictionary<string, string> Awards { get; init; }

    // "Простий" конструктор, який ініціалізує порожній словник.
    public Person()
    {
      Rank = string.Empty;
      Department = string.Empty;
      Ipn = string.Empty;
      Vacation = string.Empty;
      Note = string.Empty;
      Awards = [];
    }

    public void AddAward(string day, string status) =>
        Awards.TryAdd(day, status);

  }


  /// <summary>
  /// Очищає рядок і повертає «чисте» повне ім'я.
  /// Якщо вхідний рядок null, порожній або містить лише пробіли,
  /// повертає порожній рядок (не null).
  /// </summary>
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

    IXLWorksheet? ws = ReadFileShpkSheet(shpkFilePath)
        ?? throw new InvalidOperationException(
          $"Аркуш 'ШПС' в файлі {shpkFilePath} відсутній.");

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


  public static XLWorkbook CreateLongReportObj(Dictionary<string, string> matchedFiles)
  {
    using var wb = new XLWorkbook();

    IXLWorksheet ws = wb.Worksheets.Add("LongReport");
    return wb;
  }


  public static IXLWorksheet? ReadFileShpkSheet(string xlsxFilePath)
  {
    try
    {
      using XLWorkbook? wb = new(xlsxFilePath);
      IXLWorksheet ws = wb.Worksheet("ШПС");
      return ws;
    }
    catch (Exception e)
    {
      Console.WriteLine($"Помилка при читанні {xlsxFilePath}: {e.Message}");
      return null;
    }
  }

  public static void SaveXlsx(XLWorkbook wb, string reportFilePath)
  {
    try
    {
      wb.SaveAs(reportFilePath);
      Console.WriteLine($"Файл {reportFilePath} успішно збережений.");
    }
    catch (Exception e)
    {
      Console.WriteLine($"Помилка при збереженні {reportFilePath}: {e.Message}");
    }
  }
};
