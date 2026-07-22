// A simple analog of Unix 'ls' utility but intended to work in Windows 10+
namespace utility_ls;


class Program
{
  static bool showAll = false;
  static bool longFormat = false;
  static bool sortBySize = false;
  static bool reverse = false;
  static bool humanRead = false;
  static bool recursiveSize = false;
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

      // Get entries, convert to FileSystemItem objects, filter hidden files
      var entries = Directory.GetFileSystemEntries(targetPath)
        .Select(e => new FileSystemItem(e, recursiveSize))
        .Where(e => showAll || !e.IsHidden)
        .ToArray();

      FileSystemItem[] sortedEntries = sortBySize
          ? SortDirSize(entries, reverse)
          : SortDirName(entries, reverse);

      Console.WriteLine();
      if (longFormat)
        PrintLongFormat(sortedEntries);
      else
        PrintShortFormat(sortedEntries);
      Console.WriteLine();
    }
    catch (Exception ex)
    {
      Console.WriteLine($"ls: error: {ex.Message}");
      Environment.Exit(1);
    }
  }


  static void PrintHelp()
  {
    string helpMsg = """
    Usage: ls [OPTION]... [FILE]...
    List information about the FILEs (the current directory by default).

    Options:
      -a, --all       do not ignore entries starting with '.'
      -D              recursively count directories' sizes (works only with '-l')
      -l, --long      use a long listing format
      -h              human readable sizes
      -r, --r         use a reverse sorting order
      -S              sort by size
      -?, --help      display this help and exit
    """;
    Console.WriteLine(helpMsg);
  }


  static void ParseArguments(string[] args)
  {
    foreach (var arg in args)
    {
      if (string.IsNullOrWhiteSpace(arg))
        continue;

      // Long options
      if (arg.StartsWith("--", StringComparison.Ordinal))
      {
        switch (arg)
        {
          case "--help":
            PrintHelp();
            Environment.Exit(0);
            break;

          case "--all":
            showAll = true;
            break;

          case "--long":
            longFormat = true;
            break;

          case "--r":
            reverse = true;
            break;

          default:
            Console.WriteLine($"ls: invalid option -- '{arg}'");
            Environment.Exit(1);
            break;
        }
        continue;
      }

      // Short options or path
      if (arg.StartsWith('-'))
      { // Support combined short flags like -lS, -alS, -la, -rS, etc.
        for (int i = 1; i < arg.Length; i++)
        {
          char c = arg[i];

          switch (c)
          {
            case 'a': showAll = true; break;
            case 'D': recursiveSize = true; break;
            case 'l': longFormat = true; break;
            case 'h': humanRead = true; break;
            case 'S': sortBySize = true; break;
            case 'r': reverse = true; break;
            case '?': PrintHelp(); break;

            default:
              Console.WriteLine($"ls: invalid option -- '{arg}'");
              Environment.Exit(1);
              break;
          }
        }
        continue;
      }
      targetPath = arg;
    }
  }


  static void PrintLongFormat(FileSystemItem[] entries)
  {
    long totalSize = 0;
    foreach (var entry in entries)
    {
      totalSize += entry.Size;
      FileSystemInfo fsInfo = entry.IsDirectory
        ? new DirectoryInfo(entry.FullPath)
        : new FileInfo(entry.FullPath);

      string name = entry.Name + (entry.IsDirectory ? "/" : "");
      string modeString = entry.WinAttrString;

      string size = humanRead ? entry.HumanReadSize : entry.Size.ToString();

      string dateStr = fsInfo.LastWriteTime.ToString("dd.MM.yyyy HH:mm");

      const int PermWidth = 8;    // "d--h--" + запас
      const int SizeWidth = 16;   // ширина під розмір
      const int DateWidth = 16;   // "dd.MM.yyyy HH:mm"

      Console.ForegroundColor = entry.IsDirectory
        ? ConsoleColor.White
        : ConsoleColor.Yellow;

      Console.WriteLine(
          $"{modeString,-PermWidth}" +
          $"{size,SizeWidth}" +
          $"  {dateStr,-DateWidth}" +
          $"  {name}"
        );
    }

    string totalMsg = FileSystemItem.FormatBytes(totalSize);
    Console.WriteLine("\nThe total size of the current directory is: " +
                        totalMsg);
  }


  static void PrintShortFormat(FileSystemItem[] entries)
  {
    int consoleWidth = Console.WindowWidth;
    int maxLength = entries.Max(e => e.Name.Length) + 2;
    int maxColumns = Math.Max(1, consoleWidth / maxLength);
    int columns = Math.Min(maxColumns, entries.Length);

    for (int i = 0; i < entries.Length; i++)
    {
      var entry = entries[i];
      FileSystemInfo fsInfo = new FileInfo(entry.FullPath);
      string name = entry.Name + (entry.IsDirectory ? "/" : "");
      if (entry.IsDirectory)
        Console.ForegroundColor = ConsoleColor.White;
      else
        Console.ForegroundColor = ConsoleColor.Yellow;
      Console.Write(name.PadRight(maxLength));

      if ((i + 1) % columns == 0 || i == entries.Length - 1)
        Console.WriteLine();
    }
  }


  static FileSystemItem[] SortDirName(FileSystemItem[] entries, bool r)
  {
    if (r)
      return [.. entries
        .OrderByDescending(e => e.Name, StringComparer.OrdinalIgnoreCase)
        .OrderByDescending(e => e.IsDirectory)];

    return [.. entries
        .OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
        .OrderByDescending(e => e.IsDirectory)];
  }


  static FileSystemItem[] SortDirSize(FileSystemItem[] entries, bool r)
  {
    if (r)
      return [.. entries
        .OrderByDescending(e => e.Size)
        .OrderByDescending(e => e.IsDirectory)];

    return [.. entries
        .OrderBy(e => e.Size)
        .OrderByDescending(e => e.IsDirectory)];
  }


  static long GetDirectorySize(string directoryPath)
  {
    try
    {
      var dirInfo = new DirectoryInfo(directoryPath);
      long totalSize = 0;

      // Sum sizes of all files in this directory
      foreach (var file in dirInfo.GetFiles()) totalSize += file.Length;

      // Recursively sum sizes of all subdirectories
      foreach (var subdir in dirInfo.GetDirectories())
        totalSize += GetDirectorySize(subdir.FullName);

      return totalSize;
    }
    catch (Exception ex)  // Handle access denied or other IO exceptions
    {
      Console.WriteLine($"GetDirectorySize: error: {ex.Message}");
      return 0;
    }
  }

}
