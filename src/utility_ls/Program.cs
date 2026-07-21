// A simple analog of Unix 'ls' utility but intended to work in Windows 10+

class Program
{
  static bool showAll = false;
  static bool longFormat = false;
  static bool sortBySize = false;
  static bool reverse = false;
  static bool humanRead = false;
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

      // Get entries and convert to FileSystemEntry objects
      var entries = Directory.GetFileSystemEntries(targetPath)
        .Select(e => new FileSystemEntry(e))
        .ToArray();

      // Filter hidden files
      if (!showAll) entries = [.. entries.Where(e => e.IsHidden)];

      FileSystemEntry[] sortedEntries = sortBySize
          ? SortDirSize(entries, reverse)
          : SortDirName(entries, reverse);


      if (longFormat)
        PrintLongFormat(sortedEntries);
      else
        PrintShortFormat(sortedEntries);
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
    \t-a, --all       do not ignore entries starting with '.'
    \t-l, --long      use a long listing format
    \t-h              human readable sizes
    \t-r, --r         use a reverse sorting order
    \t-S              sort by size
    \t-?, --help      display this help and exit
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
      {
        // Support combined short flags like -lS, -alS, -la, -rS, etc.
        for (int i = 1; i < arg.Length; i++)
        {
          char c = arg[i];

          switch (c)
          {
            case 'a': showAll = true; break;
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

      // Non-option token => target path
      targetPath = arg;
    }
  }

  static string GetPermissions(FileSystemInfo fsInfo)
  {
    // Windows doesn't have Unix-style permissions, so we'll use a simplified representation
    string typeChar = fsInfo is DirectoryInfo ? "d" : "-";
    bool isReadOnly = (fsInfo.Attributes & FileAttributes.ReadOnly) != 0;
    string perms = isReadOnly ? "r--r--r--" : "rw-r--r--";
    return $"{typeChar}{perms}";
  }

  static void PrintLongFormat(FileSystemEntry[] entries)
  {
    foreach (var entry in entries)
    {
      FileSystemInfo fsInfo;
      if (entry.IsDirectory)
        fsInfo = new DirectoryInfo(entry.FullPath);
      else
        fsInfo = new FileInfo(entry.FullPath);

      string name = entry.Name + (entry.IsDirectory ? "/" : "");
      string hidden = entry.IsHidden ? "  --  " : "hidden";

      long size = entry.IsDirectory ? 0 : entry.Size; // Directories show 0 size

      // Format date: show time if modified today, otherwise show date
      string dateStr = fsInfo.LastWriteTime.Date == DateTime.Today
          ? fsInfo.LastWriteTime.ToString("MMM dd HH:mm")
          : fsInfo.LastWriteTime.ToString("MMM dd  yyyy");

      if (entry.IsDirectory)
        Console.ForegroundColor = ConsoleColor.White;
      else
        Console.ForegroundColor = ConsoleColor.Yellow;

      const int PermWidth = 11;    // "drw-r--r--" (10) + запас
      const int HiddenWidth = 7;   // "hidden" або "  --  "
      const int SizeWidth = 9;     // ширина під розмір
      const int DateWidth = 15;    // "MMM dd HH:mm" або "MMM dd  yyyy"

      if (humanRead)
      {
        string humanSize = size > 0 ? entry.FormattedSize : "--";
        Console.WriteLine(
            $"{GetPermissions(fsInfo),-PermWidth}" +
            $"{hidden,-HiddenWidth}" +
            $"{humanSize,SizeWidth}" +
            $"  {dateStr,-DateWidth}" +
            $" {name}"
        );
      }
      else
      {
        Console.WriteLine(
            $"{GetPermissions(fsInfo),-PermWidth}" +
            $"{hidden,-HiddenWidth}" +
            $"{size,SizeWidth}" +
            $"  {dateStr,-DateWidth}" +
            $" {name}"
        );
      }
    }
  }

    static void PrintShortFormat(FileSystemEntry[] entries)
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

    static FileSystemEntry[] SortDirName(FileSystemEntry[] entries, bool r)
    {
      if (r)
        return [.. entries
        .OrderByDescending(e => e.Name, StringComparer.OrdinalIgnoreCase)
        .OrderByDescending(e => e.IsDirectory)];

      return [.. entries
        .OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
        .OrderByDescending(e => e.IsDirectory)];
    }

    static FileSystemEntry[] SortDirSize(FileSystemEntry[] entries, bool r)
    {
      if (r)
        return [.. entries
        .OrderByDescending(e => e.Size)
        .OrderByDescending(e => e.IsDirectory)];

      return [.. entries
        .OrderBy(e => e.Size)
        .OrderByDescending(e => e.IsDirectory)];
    }

  }
