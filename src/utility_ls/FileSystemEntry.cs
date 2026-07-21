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