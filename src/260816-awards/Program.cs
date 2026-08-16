namespace _260816_awards;

using static DirectoryHelper;

class Program
{

  static void Main()
  {
    string directoryPath = "d:/tmp/experiment/";
    string?[]? filesList = FindFiles(directoryPath);
    string[] xlsxFiles = [];
    Dictionary<string, string> matchedFiles;

    if (filesList == null || filesList.Length == 0)
    {
      Console.WriteLine($"Директорія {directoryPath} не містить жодного файлу, що підходить");
      Environment.Exit(1);
    }

    filesList.ToList().ForEach(Console.WriteLine);
    xlsxFiles = [.. filesList
        .Where(f => f != null)    // Filter out null elements
        .Select(f => f!)];
    matchedFiles = CheckMatching(xlsxFiles, directoryPath);
    string?[] dates = MakeArrayOfKeys(matchedFiles);

  }


}