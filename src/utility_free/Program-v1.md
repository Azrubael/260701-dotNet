```csharp
using System.Diagnostics;
using System.Runtime.InteropServices;

var app = new FreeUtility(args);
app.Run();

public class FreeUtility(string[] args)
{
  private readonly string[] _args = args;
  private bool _humanReadable;
  private bool _showTotal;
  private bool _continuous;
  private int _interval = 1; // seconds
  private int _count = -1;   // -1 means infinite

  public void Run()
  {
    ParseArguments();
    DisplayMemoryInfo();
  }

  private void ParseArguments()
  {
    for (int i = 0; i < _args.Length; i++)
    {
      switch (_args[i])
      {
        case "-h" or "--human":
          _humanReadable = true;
          break;
        case "-t" or "--total":
          _showTotal = true;
          break;
        case "-s" or "--seconds":
          if (i + 1 < _args.Length && int.TryParse(_args[++i], out var seconds))
          {
            _interval = seconds;
          }
          _continuous = true;
          break;
        case "-c" or "--count":
          if (i + 1 < _args.Length && int.TryParse(_args[++i], out var count))
          {
            _count = count;
          }
          break;
        case "-?" or "--help":
          ShowHelp();
          Environment.Exit(0);
          break;
      }
    }
  }

  private void ShowHelp()
  {
    Console.WriteLine("""
            free - Display memory usage information

            Usage:
                free [options]

            Options:
                -h, --human     Show human-readable output (KB, MB, GB)
                -t, --total     Show total memory usage
                -s, --seconds N Continuously display every N seconds
                -c, --count N   Display N times (used with -s)
                -?, --help      Show this help message
            """);
  }

  private void DisplayMemoryInfo()
  {
    if (_continuous)
    {
      int currentCount = 0;
      while (_count == -1 || currentCount < _count)
      {
        DisplaySingleSnapshot();
        Thread.Sleep(_interval * 1000);
        currentCount++;
      }
    }
    else
    {
      DisplaySingleSnapshot();
    }
  }

  private void DisplaySingleSnapshot()
  {
    try
    {
      var memoryStatus = GetMemoryStatus();
      if (OperatingSystem.IsWindows())
      {
        var swapInfo = GetSwapInfo();
        if (_humanReadable) PrintHumanReadable(memoryStatus, swapInfo);
        else PrintRaw(memoryStatus, swapInfo);

        if (_showTotal) PrintTotal(memoryStatus, swapInfo);
      };

    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"Error: {ex.Message}");
    }
  }

  private void PrintRaw(MemoryStatusEx memoryStatus, SwapInfo swapInfo)
  {
    Console.WriteLine("""
            Type        Total    Used    Free   Buff/Cache  Available
            Mem:        {0,8}  {1,8}  {2,8}  {3,8}     {4,8}
            Swap:       {5,8}  {6,8}  {7,8}
            """,
        memoryStatus.TotalPhys / 1024,
        (memoryStatus.TotalPhys - memoryStatus.AvailPhys) / 1024,
        memoryStatus.AvailPhys / 1024,
        (memoryStatus.TotalPhys /1024 - memoryStatus.AvailPhys - memoryStatus.TotalPageFile + memoryStatus.AvailPageFile) / 1024,
        // (memoryStatus.TotalPageFile - memoryStatus.AvailPageFile - memoryStatus.TotalPhys + memoryStatus.AvailPhys) / 1024,
        memoryStatus.AvailPageFile / 1024,
        memoryStatus.TotalPageFile / 1024,
        swapInfo.CurrentUsage,
        (memoryStatus.TotalPageFile - memoryStatus.AvailPageFile) / 1024
    );
  }

  private void PrintHumanReadable(MemoryStatusEx memoryStatus, SwapInfo swapInfo)
  {
    var units = new[] { "B", "KB", "MB", "GB", "TB" };
    string FormatSize(ulong bytes)
    {
      int unitIndex = 0;
      double size = bytes;
      while (size >= 1024 && unitIndex < units.Length - 1)
      {
        size /= 1024;
        unitIndex++;
      }
      return $"{size:0.##} {units[unitIndex]}";
    }

    Console.WriteLine("""
            Type     Total        Used        Free      Buff/Cache       Available
            Mem:   {0,10}  {1,10}  {2,10}  {3,10}     {4,10}
            Swap:  {5,10}  {6,10}  {7,10}
            """,
        FormatSize(memoryStatus.TotalPhys),
        FormatSize(memoryStatus.TotalPhys - memoryStatus.AvailPhys),
        FormatSize(memoryStatus.AvailPhys),
        // FormatSize(memoryStatus.TotalPhys - memoryStatus.AvailPhys - memoryStatus.TotalPageFile + memoryStatus.AvailPageFile),
        FormatSize(memoryStatus.TotalPhys - memoryStatus.AvailPhys - memoryStatus.TotalPageFile + memoryStatus.AvailPageFile),
        FormatSize(memoryStatus.AvailPageFile),
        FormatSize(memoryStatus.TotalPageFile),
        FormatSize((ulong)swapInfo.CurrentUsage),
        FormatSize(memoryStatus.TotalPageFile - memoryStatus.AvailPageFile)
    );
  }

  private void PrintTotal(MemoryStatusEx memoryStatus, SwapInfo swapInfo)
  {
    var totalMem = memoryStatus.TotalPhys / 1024;
    var totalUsed = (memoryStatus.TotalPhys - memoryStatus.AvailPhys) / 1024;
    var totalSwap = memoryStatus.TotalPageFile / 1024 / 1024;
    var totalSwapUsed = swapInfo.CurrentUsage;

    Console.WriteLine($"\nTotal: {totalMem + totalSwap} kB (Mem: {totalMem} kB + Swap: {totalSwap} kB)");
    Console.WriteLine($"\nTotal phys used: {totalUsed} kB (Total swap used: {totalSwapUsed} kB)");
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  private struct MemoryStatusEx
  {
    public uint Length;
    public uint MemoryLoad;
    public ulong TotalPhys;
    public ulong AvailPhys;
    public ulong TotalPageFile;
    public ulong AvailPageFile;
    public ulong TotalVirtual;
    public ulong AvailVirtual;
    public ulong AvailExtendedVirtual;

    public MemoryStatusEx()
    {
      Length = (uint)Marshal.SizeOf<MemoryStatusEx>();
    }
  }

  [DllImport("kernel32.dll", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool GlobalMemoryStatusEx(ref MemoryStatusEx lpBuffer);

  private MemoryStatusEx GetMemoryStatus()
  {
    var status = new MemoryStatusEx();
    if (!GlobalMemoryStatusEx(ref status))
    {
      throw new InvalidOperationException("Failed to get memory status");
    }
    return status;
  }

  [System.Runtime.Versioning.SupportedOSPlatform("windows")]
  private SwapInfo GetSwapInfo()
  {
    // Windows doesn't expose swap usage directly like Linux, so we approximate
    var status = GetMemoryStatus();
    var totalSwap = status.TotalPageFile;
    var availSwap = status.AvailPageFile;

    // Get page file usage from performance counters
    var category = new PerformanceCounterCategory("Paging File");
    var instances = category.GetInstanceNames();

    long currentUsage = 0;
    foreach (var instance in instances)
    {
      using var counter = new PerformanceCounter("Paging File", "% Usage", instance);
      currentUsage += (long)(counter.NextValue() * totalSwap / 100);
    }

    return new SwapInfo
    {
      CurrentUsage = currentUsage,
      Total = totalSwap
    };
  }

  private record SwapInfo
  {
    public long CurrentUsage { get; init; }
    public ulong Total { get; init; }
  }
}
```