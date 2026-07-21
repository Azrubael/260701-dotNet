
class Program
{
  static bool showAll = false;
  static bool longFormat = false;
  static string targetPath = Directory.GetCurrentDirectory();

  static void Main(string[] args)
  {
    try
    {

      ParseArguments(args);

      if (!Directory.Exists(targetPath))
      {
        Console.WriteLine($"ls: cannot access '{targetPath}': No such file or directory");
        Environment.Exit(1);
      }

      var entries = Directory.GetFileSystemEntries(targetPath);

      // Фільтруємо приховані файли
      if (!showAll)
      {
        entries = [.. entries.Where(e =>
        {
          var fileInfo = new FileInfo(e);
          return (fileInfo.Attributes & FileAttributes.Hidden) == 0;
        })];
      }

      // Array.Sort(entries, StringComparer.OrdinalIgnoreCase);
      entries = SortDirFile(entries);

      if (longFormat)
        PrintLongFormat(entries);
      else
        PrintShortFormat(entries);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"ls: error: {ex.Message}");
      Environment.Exit(1);
    }
  }

  static void PrintHelp()
  {
    Console.WriteLine("Usage: ls [OPTION]... [FILE]...");
    Console.WriteLine("List information about the FILEs (the current directory by default).");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  -a, --all            do not ignore entries starting with .");
    Console.WriteLine("  -l, --long           use a long listing format");
    Console.WriteLine("  --help               display this help and exit");
  }

  static void ParseArguments(string[] args)
  {
    foreach (var arg in args)
    {
      switch (arg)
      {
        case "-a" or "--all":
          showAll = true;
          break;

        case "-l" or "--long":
          longFormat = true;
          break;

        case "-la" or "-al" or "--all --long" or "--long --all":
          showAll = true;
          longFormat = true;
          break;

        case "--help":
          PrintHelp();
          Environment.Exit(0);
          break;

        default:
          if (!arg.StartsWith('-'))
            targetPath = arg;
          else
          {
            Console.WriteLine($"ls: invalid option -- '{arg}'");
            Environment.Exit(1);
          }
          break;
      }
    }
  }

  static string GetPermissions(FileSystemInfo fsInfo)
  {
    // Windows doesn't have Unix-style permissions, so we'll use a simple representation
    // Based on the file's read-only attribute
    string typeChar = fsInfo is DirectoryInfo ? "d" : "-";
    bool isReadOnly = (fsInfo.Attributes & FileAttributes.ReadOnly) != 0;
    string perms = isReadOnly ? "r--r--r--" : "rw-r--r--";
    return $"{typeChar}{perms}";
  }

  static void PrintLongFormat(string[] entries)
  {
    foreach (var entry in entries)
    {
      FileSystemInfo fsInfo;
      bool isDir = (File.GetAttributes(entry) & FileAttributes.Directory) != 0;

      if (isDir)
        fsInfo = new DirectoryInfo(entry);
      else
        fsInfo = new FileInfo(entry);

      string name = Path.GetFileName(entry) + (isDir ? "/" : "");
      long size = isDir ? 0 : ((FileInfo)fsInfo).Length; // Directories show 0 size

      // Format date: show time if modified today, otherwise show date
      string dateStr = fsInfo.LastWriteTime.Date == DateTime.Today
          ? fsInfo.LastWriteTime.ToString("MMM dd HH:mm")
          : fsInfo.LastWriteTime.ToString("MMM dd  yyyy");

      if (isDir)
        Console.ForegroundColor = ConsoleColor.White;
      else
        Console.ForegroundColor = ConsoleColor.Yellow;

      Console.WriteLine($"{GetPermissions(fsInfo)}  1  user  group  {size,10}  {dateStr}  {name}");
    }
  }

  static void PrintShortFormat(string[] entries)
  {
    int consoleWidth = Console.WindowWidth;
    int maxLength = entries.Max(e => Path.GetFileName(e).Length) + 2;
    int maxColumns = Math.Max(1, consoleWidth / maxLength);
    int columns = Math.Min(maxColumns, entries.Length);

    for (int i = 0; i < entries.Length; i++)
    {
      var fileInfo = new FileInfo(entries[i]);
      bool isDir = (fileInfo.Attributes & FileAttributes.Directory) != 0;
      string name = Path.GetFileName(entries[i]) + (isDir ? "/" : "");
      if (isDir)
        Console.ForegroundColor = ConsoleColor.White;
      else
        Console.ForegroundColor = ConsoleColor.Yellow;
      Console.Write(name.PadRight(maxLength));

      if ((i + 1) % columns == 0 || i == entries.Length - 1)
        Console.WriteLine();
    }
  }

  static string[] SortDirFile(string[] entries)
  {
    string[] directories = [];
    string[] files = [];
    foreach (var entry in entries)
    {
      bool isDir = (File.GetAttributes(entry) & FileAttributes.Directory) != 0;
      if (isDir)
      {
        Array.Resize(ref directories, directories.Length + 1);
        directories[^1] = entry;
      } else
      {
        Array.Resize(ref files, files.Length + 1);
        files[^1] = entry;
      }
    }
    Array.Sort(directories, StringComparer.OrdinalIgnoreCase);
    Array.Sort(files, StringComparer.OrdinalIgnoreCase);

    return [.. directories, .. files];
  }

}
