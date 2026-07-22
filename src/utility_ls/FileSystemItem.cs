namespace utility_ls;

public readonly struct FileSystemItem
{
  public string FullPath { get; }
  public string Name { get; }
  public bool IsDirectory { get; }
  public bool IsHidden { get; }
  public long Size { get; }
  public DateTime LastModified { get; }
  public FileAttributes Attributes { get; }

  private readonly Lazy<string> _humanReadSize;
  public string HumanReadSize => _humanReadSize.Value;

  public FileSystemItem(string fullPath, bool recursiveSize)
  {
    FullPath = fullPath;
    Name = Path.GetFileName(fullPath);

    var fileInfo = new FileInfo(fullPath);
    IsDirectory = (fileInfo.Attributes & FileAttributes.Directory) != 0;
    IsHidden = (fileInfo.Attributes & FileAttributes.Hidden) != 0;
    Size = (IsDirectory, recursiveSize) switch
    {
      (true, true) => GetDirectorySize(FullPath),
      (true, false) => 0,
      (false, _) => fileInfo.Length,
    };
    LastModified = fileInfo.LastWriteTime;
    Attributes = fileInfo.Attributes;

    var itemSize = Size;
    _humanReadSize = new Lazy<string>(() => FormatBytes(itemSize));
  }


  public static long GetDirectorySize(string directoryPath)
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


  public static string FormatBytes(long bytes)
  {
    string unit = bytes switch
    {
      >= 1_099_511_627_776 => "Ti",
      >= 1_073_741_824 => "Gi",
      >= 1_048_576 => "Mi",
      >= 1024 => "Ki",
      _ => "B"
    };

    double value = bytes switch
    {
      >= 1_099_511_627_776 => bytes / 1_099_511_627_776.0,
      >= 1_073_741_824 => bytes / 1_073_741_824.0,
      >= 1_048_576 => bytes / 1_048_576.0,
      >= 1024 => bytes / 1024.0,
      _ => bytes
    };

    return unit == "B" ? $"{value} {unit}" : $"{value:F2}{unit}";
  }
}