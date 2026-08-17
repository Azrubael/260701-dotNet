namespace _260816_awards;

using ClosedXML.Excel;

public class XlsxHandler
{

  /// <summary>
  /// Головна структура даних, яка містить усі дані по кожному співпрацівнику
  /// </summary>
  public sealed record Shpk
  {
    public Dictionary<string, Person> PersonalData { get; init; }

    // Параметричний конструктор, який створює порожній словник.
    public Shpk() : this([]) { }

    private Shpk(Dictionary<string, Person> data) =>
        PersonalData = data;

    public void AddToShpk(string name, Person person) =>
        PersonalData.TryAdd(name, person);

  }


  /// <summary>
  /// Cтруктура даних що відпувідає за збереження інформації щодо кожної особи
  /// </summary>
  public sealed record Person
  {
    public string Rank { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Ipn { get; set; } = string.Empty;
    public string Vacation { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;

    public Dictionary<string, string> Awards { get; set; }

    // "Простий" конструктор, який ініціалізує порожній словник.
    public Person() : this(
        rank: string.Empty,
        department: string.Empty,
        ipn: string.Empty,
        vacation: string.Empty,
        note: string.Empty,
        awards: [])
    { }

    // "Повний" конструктор, який дозволяє задати всі значення під час ініціалізації.
    public Person(
        string rank,
        string department,
        string ipn,
        string vacation,
        string note,
        Dictionary<string, string> awards)
    {
      Rank = rank;
      Department = department;
      Ipn = ipn;
      Vacation = vacation;
      Note = note;
      Awards = awards;
    }

    public void AddAward(string day, string status) =>
        Awards.TryAdd(day, status);
  }


  /// <summary>
  /// Читання даних файлу ШПК 'shpkFilePath' за один визначений день 'day' і збереження їх до 'shpk'.
  /// </summary>
  /// <param name="day"></param>
  /// <param name="shpkFilePath"></param>
  /// <param name="shpk"></param>
  public static void ReadShpk(string day, string shpkFilePath, Shpk shpk)
  {
    IXLWorksheet ws;
    try
    {
      using XLWorkbook? wb = new(shpkFilePath);
      ws = wb.Worksheet("ШПС");

      string fullName;

      for (int rowNum = 4; rowNum <= 630; rowNum++)
      {
        IXLRow? row = ws.Row(rowNum);
        fullName = CleanFullName(row.Cell(9).GetString());   // I
        if (fullName == string.Empty)
          continue;

        if (shpk.PersonalData.TryGetValue(fullName, out Person? person))
        {
          person.AddAward(day, row.Cell(19).GetString());
        }
        else
        {
          Person newPerson = new()
          {
            Rank = row.Cell(8).GetString(),   // F
            Department = row.Cell(11).GetString(),  // K
            Ipn = row.Cell(15).GetString(),  // O
            Vacation = row.Cell(24).GetString() // X (Excel date)
          };
          newPerson.AddAward(day, row.Cell(19).GetString());
          shpk.AddToShpk(fullName, newPerson);
        }
      }


    }
    catch (Exception ex)
    {
      Console.WriteLine($"Помилка відкриття {shpkFilePath}: {ex.Message}");
    }

  }


  /// <summary>
  /// Очищає рядок і повертає «чисте» повне ім'я.
  /// Якщо вхідний рядок null, порожній або містить лише пробіли,
  /// повертає порожній рядок (не null).
  /// </summary>
  public static string CleanFullName(string? fullName)
  {
    if (string.IsNullOrWhiteSpace(fullName))
      return string.Empty;               // гарантовано non‑null

    try
    {
      var parts = fullName
          .Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
      return string.Join(" ", parts).Trim();   // також non‑null
    }
    catch (Exception e)
    {
      Console.WriteLine($"Помилка при очищенні імені: {e}");
      return string.Empty;               // fallback у разі винятка
    }
  }

};
