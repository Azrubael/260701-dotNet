
class Program
{
  static bool showAll = false;
  static bool longFormat = false;

  static void Main(string[] args)
  {
    try
    {
      string targetPath = Directory.GetCurrentDirectory();

      foreach (var arg in args)
      {
        switch (arg)
        {
          case "-a" or "--all":
            showAll = true; break;

          case "-l":
            longFormat = true; break;

          case "-la" or "-al":
            showAll = true; longFormat = true; break;

          case "--help":
            Console.WriteLine("Usage: ls [OPTION]... [FILE]...");
            Console.WriteLine("List information about the FILEs (the current directory by default).");
            Console.WriteLine("  -a, --all            do not ignore entries starting with .");
            Console.WriteLine("  -l                   use a long listing format");
            Environment.Exit(0);
            break;

          default:
            if (!arg.StartsWith('-')) targetPath = arg;
            break;
        }
      }

      if (!Directory.Exists(targetPath))
      {
        Console.WriteLine($"ls: cannot access '{targetPath}': No such file or directory");
        Environment.Exit(1);
      }

      var entries = Directory.GetFileSystemEntries(targetPath);

      // Фільтруємо приховані файли
      if (!showAll)
      {
        entries = entries.Where(e =>
        {
          var fileInfo = new FileInfo(e);
          return (fileInfo.Attributes & FileAttributes.Hidden) == 0;
        }).ToArray();
      }

      Array.Sort(entries, StringComparer.OrdinalIgnoreCase);

      foreach (var entry in entries)
      {
        var fileInfo = new FileInfo(entry);
        var isDir = (fileInfo.Attributes & FileAttributes.Directory) != 0;
        string name = Path.GetFileName(entry);

        if (longFormat)
        {
          string permissions = GetPermissions(fileInfo);
          string sizeOrDash = isDir ? "-" : fileInfo.Length.ToString();
          string modifiedDate = fileInfo.LastWriteTime.ToString("MMM dd HH:mm");
          if (isDir) name += "/";
          Console.WriteLine($"{permissions}  1  user  group  {sizeOrDash,10}  {modifiedDate}  {name}");
        }
        else
        {
          if (isDir) name += "/";
          Console.Write(name.PadRight(20));
        }
      }

      if (!longFormat && entries.Length > 0) Console.WriteLine();
    }
    catch (Exception ex)
    {
      Console.WriteLine($"ls: error: {ex.Message}");
      Environment.Exit(1);
    }
  }

  static string GetPermissions(FileInfo fileInfo)
  {
    string typeChar = (fileInfo.Attributes & FileAttributes.Directory) != 0 ? "d" : "-";
    return $"{typeChar}rw-r--r--";
  }
}
