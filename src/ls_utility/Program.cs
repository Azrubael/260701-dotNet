// A simple analig of Unix 'ls' utility but intended to work in Windows 10+

class Program
{

  //New data structure to replace string entries
  class FileSystemEntry
  {
    public string FullPath { get; set; }
    public string Name { get; set; }
    public bool IsDirectory { get; set; }
    public bool IsHidden { get; set; }
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public FileAttributes Attributes { get; set; }

    public FileSystemEntry(string fullPath)
    {
      FullPath = fullPath;
      Name = Path.GetFileName(fullPath);

      var fileInfo = new FileInfo(fullPath);
      IsDirectory = (fileInfo.Attributes & FileAttributes.Directory) != 0;
      IsHidden = (fileInfo.Attributes & FileAttributes.Hidden) == 0;
      Size = IsDirectory ? 0 : fileInfo.Length;
      LastModified = fileInfo.LastWriteTime;
      Attributes = fileInfo.Attributes;
    }
  }

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

      // Get entries and convert to FileSystemEntry objects
      var entries = Directory.GetFileSystemEntries(targetPath)
        .Select(e => new FileSystemEntry(e))
        .ToArray();

      // Filter hidden files
      if (!showAll) entries = [.. entries.Where(e => e.IsHidden)];

      // Array.Sort(entries, StringComparer.OrdinalIgnoreCase);
      FileSystemEntry[] sortedEntries = SortDirName(entries);

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
          : fsInfo.LastWriteTime.ToString("MMM dd yyyy");

      if (entry.IsDirectory)
        Console.ForegroundColor = ConsoleColor.White;
      else
        Console.ForegroundColor = ConsoleColor.Yellow;

      Console.WriteLine($"{GetPermissions(fsInfo)}  {hidden}  {size,10}  {dateStr}  {name}");
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

  static FileSystemEntry[] SortDirName(FileSystemEntry[] entries)
  {
    return [.. entries
      .OrderByDescending(e => e.IsDirectory)
      .ThenBy(e => e.Name, StringComparer.OrdinalIgnoreCase)];
  }

}
