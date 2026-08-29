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
    private static int _nextCount;

    public int Count { get; }
    public string Rank { get; set; }
    public string Department { get; set; }
    public string Ipn { get; set; }
    public string Note { get; set; }

    public Dictionary<string, string> Awards { get; init; }
    public List<(string Begin, string End)> AccuralPeriods { get; init; }

    // Простий конструктор, який ініціалізує порожній словник.
    public Person()
    {
      Rank = string.Empty;
      Department = string.Empty;
      Ipn = string.Empty;
      Note = string.Empty;
      Awards = [];
      AccuralPeriods = [];
      Count = Interlocked.Increment(ref _nextCount);
    }

    public void AddAward(string day, string status) =>
      Awards.TryAdd(day, status);

    /// <summary>
    /// Визначення змісту для поля "Примітка" в звітах.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="vacation"></param>
    /// <param name="szch"></param>
    public void UpdateNote(string status, string vacation, string szch)
    {
      if (!string.IsNullOrEmpty(szch) || (status.Contains("сзч") && !Note.Contains("сзч")))
      {
        Note = "сзч";
        return;
      }
      if (status.Contains("бр") && !Note.Contains("була бр"))
      {
        Note += ", була брка";
        return;
      }
      if (!string.IsNullOrEmpty(vacation) && !Note.Contains("була відпустка"))
      {
        Note += ", була відпустка";
        return;
      }
    }

    /// <summary>
    /// Визначення періодів нарахування премії 10к.
    /// </summary>
    public void DefineAccuralPeriods()
    {
      string? periodStart = null;
      string? periodEnd = null;

      foreach ((string day, string status) in Awards)
      {
        if (status.Contains('+'))
        {
          periodStart ??= day;
          periodEnd = day;
        }
        else if (periodStart is not null && periodEnd is not null)
        {
          AccuralPeriods.Add((periodStart, periodEnd));
          periodStart = null;
          periodEnd = null;
        }
      }

      // Додаємо період, якщо він закінчується останньою датою.
      if (periodStart is not null && periodEnd is not null)
      {
        AccuralPeriods.Add((periodStart, periodEnd));
      }
    }
  }


  /// <summary>
  /// Очищає рядок від зайвих пробілів і переносів рядків.
  /// </summary>
  /// <param name="fullName"></param>
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
    using XLWorkbook wb = ReadFileShpkBook(shpkFilePath)
        ?? throw new InvalidOperationException(
          $"Файл {shpkFilePath} не відповідає формату xlsx.");

    if (!wb.Worksheets.TryGetWorksheet("ШПС", out IXLWorksheet? ws))
    {
      throw new InvalidOperationException(
        $"В файлі {shpkFilePath} не знайдено аркуш 'ШПС'.");
    }

    string fullName;
    for (int rowNum = 4; rowNum <= 630; rowNum++)
    {
      IXLRow row = ws.Row(rowNum);
      fullName = row.Cell(9).GetString();                 // стовпчик I
      if (string.IsNullOrWhiteSpace(fullName))
        continue;

      string cleanedName = CleanFullName(fullName);
      string currentStatus = row.Cell(19).GetString();
      string currentVacation = row.Cell(24).GetString();
      string szch = row.Cell(27).GetString();
      string ipn = row.Cell(15).GetString();

      if (shpk.PersonalData.TryGetValue(cleanedName, out Person? person))
      {
        person.AddAward(day, currentStatus);
        person.UpdateNote(currentStatus, currentVacation, szch);
        if (string.IsNullOrWhiteSpace(person.Ipn) &&
          !string.IsNullOrWhiteSpace(ipn))
        {
          person.Ipn = ipn;
        }
      }
      else
      {
        Person newPerson = new()
        {
          Rank = row.Cell(8).GetString(),                 // стовпчик H
          Department = GetCompany(
              row.Cell(11).GetString(), cleanedName),     // стовпчик K
          Ipn = ipn,                 // стовпчик O
        };
        newPerson.AddAward(day, currentStatus);
        newPerson.UpdateNote(currentStatus, currentVacation, szch);
        shpk.AddToShpk(cleanedName, newPerson);
      }
    }
  }


  /// <summary>
  /// Зчитує файли *.xlsx визначеного формату і створює нову структуру даних
  /// для збереження в новому файлі звіту *.xlsx.
  /// </summary>
  /// <param name="dates"></param>
  /// <param name="shpk"></param>
  /// <returns>Створений двомірний масив для додавання до XLWorkbook</returns>
  public static List<object[]> CreateLongReportObj(string[] dates, Shpk shpk)
  {
    string[] header = ["# з/п", "Звання", "ПІБ", "Підрозділ", "РНОКПП", "Примітка"];
    List<object[]> longReportTable = [];
    longReportTable.Add([$"Детальний звіт стосовно нарахування премії з {dates[0]} по {dates[^1]}."]);
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
        person.Note.TrimStart(',', ' '),
        .. awards
        ]);
    }

    return longReportTable;
  }

  /// <summary>
  /// Зчитує файли *.xlsx визначеного формату і створює нову структуру даних
  /// для збереження в новому файлі звіту *.xlsx.
  /// </summary>
  /// <param name="shpk"></param>
  /// <returns>Створений двомірний масив для додавання до XLWorkbook</returns>
  public static List<object[]> CreateShortReportObj(string[] dates, Shpk shpk)
  {
    List<object[]> shortReportTable = [];
    shortReportTable.Add([$"Скорочений звіт стосовно нарахування премії з {dates[0]} по {dates[^1]}."]);
    string[] header = ["# з/п", "Звання", "ПІБ", "Підрозділ", "РНОКПП", "Примітка", "Початок", "Кінець"];
    shortReportTable.Add([.. header]);

    foreach ((string name, Person person) in shpk.PersonalData)
    {
      person.DefineAccuralPeriods();
      string starts = "";
      string finishes = "";

      foreach ((string Begin, string End) in person.AccuralPeriods)
      {
        starts += $", {Begin}";
        finishes += $", {End}";
      }

      shortReportTable.Add([
        person.Count,
        person.Rank,
        name,
        person.Department,
        person.Ipn,
        person.Note.TrimStart(',', ' '),
        starts.TrimStart(',', ' '),
        finishes.TrimStart(',', ' ')
        ]);

    }
    return shortReportTable;
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
  /// Зберігає файл таблиці звіту longReportTable і shortReportTable до *.xlsx
  /// </summary>
  /// <param name="longReportTable"></param>
  /// <param name="shortReportTable"></param>
  /// <param name="reportFilePath"></param>
  public static void SaveXlsx(
    List<object[]> longReportTable,
    List<object[]> shortReportTable,
    string reportFilePath)
  {
    try
    {
      using var wb = new XLWorkbook();
      IXLWorksheet ws1 = wb.Worksheets.Add("Детально");

      int maxColumnCount = 0;

      for (int rowIndex = 0; rowIndex < longReportTable.Count; rowIndex++)
      {
        object[] row = longReportTable[rowIndex];
        maxColumnCount = Math.Max(maxColumnCount, row.Length);

        for (int columnIndex = 0; columnIndex < row.Length; columnIndex++)
        {
          object? value = row[columnIndex];

          ws1.Cell(rowIndex + 1, columnIndex + 1).Value =
              value?.ToString() ?? string.Empty;
        }
      }

      // Встановлення ширини для колонок.
      ws1.Column(1).Width = 5.0;
      if (maxColumnCount > 1)
      {
        ws1.Columns(2, maxColumnCount).AdjustToContents();
      }

      // Горизонтальне вирівнювання змісту по центру, починаючи з колонки D.
      if (maxColumnCount >= 4)
      {
        ws1.Columns(4, maxColumnCount)
          .Style
          .Alignment
          .Horizontal = XLAlignmentHorizontalValues.Center;
      }

      IXLWorksheet ws2 = wb.Worksheets.Add("Скорочено");

      maxColumnCount = 0;
      for (int rowIndex = 0; rowIndex < shortReportTable.Count; rowIndex++)
      {
        object[] row = shortReportTable[rowIndex];
        maxColumnCount = Math.Max(maxColumnCount, row.Length);

        for (int columnIndex = 0; columnIndex < row.Length; columnIndex++)
        {
          object? value = row[columnIndex];

          ws2.Cell(rowIndex + 1, columnIndex + 1).Value =
              value?.ToString() ?? string.Empty;
        }
      }

      // Встановлення ширини для колонок.
      ws2.Column(1).Width = 5.0;
      if (maxColumnCount > 1)
      {
        ws2.Columns(2, maxColumnCount).AdjustToContents();
      }

      // Горизонтальне вирівнювання змісту по центру, починаючи з колонки D.
      if (maxColumnCount >= 4)
      {
        ws2.Columns(4, maxColumnCount)
          .Style
          .Alignment
          .Horizontal = XLAlignmentHorizontalValues.Center;
      }

      ws1.Cell("A1").Style.Font.Bold = true;
      ws2.Cell("A1").Style.Font.Bold = true;

      wb.SaveAs(reportFilePath);
      Console.WriteLine($"Файл {reportFilePath} успішно збережений.");

    }
    catch (Exception e)
    {
      Console.WriteLine($"Помилка при збереженні {reportFilePath}: {e.Message}");
    }
  }
};
