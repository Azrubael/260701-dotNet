//A data structure to represent files and directories with their properties
class FileSystemEntry
{
  public string FullPath { get; set; }
  public string Name { get; set; }
  public bool IsDirectory { get; set; }
  public bool IsHidden { get; set; }
  public long Size { get; set; }
  public DateTime LastModified { get; set; }
  public FileAttributes Attributes { get; set; }

  private readonly Lazy<string> _formattedSize;

  public string FormattedSize => _formattedSize.Value;

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

    _formattedSize = new Lazy<string>(() => FormatBytes());
  }

  private string FormatBytes()
  {
    string unit = Size switch
    {
      >= 1_099_511_627_776 => "Ti",
      >= 1_073_741_824 => "Gi",
      >= 1_048_576 => "Mi",
      >= 1024 => "Ki",
      _ => "B"
    };

    double value = Size switch
    {
      >= 1_099_511_627_776 => Size / 1_099_511_627_776.0,
      >= 1_073_741_824 => Size / 1_073_741_824.0,
      >= 1_048_576 => Size / 1_048_576.0,
      >= 1024 => Size / 1024.0,
      _ => Size
    };

    if (unit == "B")
      return $"{value} {unit}";

    return $"{value:F2}{unit}";
  }
}