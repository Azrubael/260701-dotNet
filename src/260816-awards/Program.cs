namespace _260816_awards;

using System.Globalization;
using static DirectoryHelper;
using static XlsxHandler;


class Program
{

  static void Main()
  {
    // string directoryPath = "d:/tmp/experiment/";
    string directoryPath = Directory.GetCurrentDirectory();
    string?[] filesList = FindFiles(directoryPath);
    string[] xlsxFiles = [];

    if (filesList.Length == 0)
    {
      Console.WriteLine($"Директорія {directoryPath} взагалі не містить" +
          " жодного файлу.");
      Environment.Exit(1);
    }

    xlsxFiles = [.. filesList
        .Where(f => f != null)
        .Select(f => f!)];

    // Залишає тількі підходящі імена файлів
    Dictionary<string, string> matchedFiles = CheckMatching(xlsxFiles, directoryPath);
    if (matchedFiles.Count == 0)
    {
      Console.WriteLine($"Директорія {directoryPath} не містить" +
          " жодного файлу, що підходить.");
      Environment.Exit(1);
    }
    string[] dates = MakeArrayOfKeys(matchedFiles);

    Shpk shpk = new();
    foreach ((string day, string shpkPath) in matchedFiles)
    {
      ReadShpk(day, shpkPath, shpk);
    }

    DateTime now = DateTime.Now;
    string newFileName = now.ToString("yyMMdd", CultureInfo.InvariantCulture) +
        "-звіт_премії.xlsx";

    SaveXlsx(
        CreateLongReportObj(dates, shpk),
        CreateShortReportObj(dates, shpk),
        Path.Combine(directoryPath, newFileName)
        );
  }

}