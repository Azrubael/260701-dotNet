namespace _260816_awards;

using static DirectoryHelper;
using static XlsxHandler;


class Program
{

  static void Main()
  {
    string directoryPath = "d:/tmp/experiment/";
    string?[]? filesList = FindFiles(directoryPath);
    string[] xlsxFiles = [];

    if (filesList == null || filesList.Length == 0)
    {
      Console.WriteLine($"Директорія {directoryPath} взагалі не містить жодного файлу.");
      Environment.Exit(1);
    }

    // filesList.ToList().ForEach(Console.WriteLine);
    xlsxFiles = [.. filesList
        .Where(f => f != null)    // Filter out null elements
        .Select(f => f!)];

    // Filter out only matche files
    Dictionary<string, string> matchedFiles = CheckMatching(xlsxFiles, directoryPath);
    if (matchedFiles.Count == 0)
    {
      Console.WriteLine($"Директорія {directoryPath} не містить жодного файлу, що підходить.");
      Environment.Exit(1);
    }
    string[] dates = MakeArrayOfKeys(matchedFiles);

    Shpk shpk = new();
    foreach(var (day, shpkPath) in matchedFiles)
    {
      ReadShpk(day, shpkPath, shpk);
      Console.WriteLine($"Оброблено дані за {day}");
    }
  }


}