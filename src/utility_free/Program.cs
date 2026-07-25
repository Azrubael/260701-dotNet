using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
  static bool _humanReadable = false;
  static bool _showTotal = false;
  static bool _continuous = false;
  static bool _help = false;
  static int _interval = 1; // seconds
  static int _count = -1;   // -1 means infinite


  [StructLayout(LayoutKind.Sequential)]
  private struct MemoryStatusEX
  {
    public uint dwLength;
    public uint dwMemoryLoad;
    public ulong TotalPhys;
    public ulong AvailPhys;
    public ulong TotalPageFile;
    public ulong AvailPageFile;
    public ulong TotalVirtual;
    public ulong AvailVirtual;
    public ulong AvailExtendedVirtual;
    public ulong SystemCache;

    public MemoryStatusEX()
    {
      dwLength = (uint)Marshal.SizeOf<MemoryStatusEX>();
      var perf = GetPerfInfo();
      SystemCache = perf.SystemCache.ToUInt64();
    }
  }


  [StructLayout(LayoutKind.Sequential)]
  private struct PERFORMANCE_INFORMATION
  {
    public uint cb;
    public UIntPtr CommitTotal;
    public UIntPtr CommitLimit;
    public UIntPtr CommitPeak;

    public UIntPtr PhysicalTotal;
    public UIntPtr PhysicalAvailable;

    public UIntPtr SystemCache;
    public UIntPtr KernelTotal;
    public UIntPtr KernelPaged;
    public UIntPtr KernelNonpaged;
    public UIntPtr PageSize;

    public UIntPtr HandleCount;
    public uint ProcessCount;
    public uint ThreadCount;
  }


  [DllImport("kernel32.dll", SetLastError = true)]
  private static extern bool GlobalMemoryStatusEx(ref MemoryStatusEX lpBuffer);


  [DllImport("psapi.dll", SetLastError = true)]
  private static extern bool GetPerformanceInfo(out PERFORMANCE_INFORMATION lpPerformanceInformation, int cb);


  private record SwapInfo
  {
    public ulong CurrentUsage { get; init; }
    public ulong Total { get; init; }
  }


  static void Main(string[] args)
  {
    ParseArguments(args);
    if  (_help)
    {
      ShowHelp();
      Environment.Exit(0);
    }
    // WriteShortReport();
    DisplayMemoryInfo();

  }


  static void ParseArguments(string[] _args)
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
          _help = true;

          break;
        default:
          WriteShortReport();
          Environment.Exit(0);
          break;
      }
    }
  }


  private static void ShowHelp()
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


  static void WriteShortReport()
  {
    // Must pass a real variable to ref parameter
    var memInit = new MemoryStatusEX { dwLength = (uint)Marshal.SizeOf<MemoryStatusEX>() };

    if (!GlobalMemoryStatusEx(ref memInit))
    {
      var err = Marshal.GetLastWin32Error();
      Console.Error.WriteLine($"GlobalMemoryStatusEx failed: {err}");
      return;
    }

    var mem = memInit; // now we have valid initialized struct

    ulong totalPhys = mem.TotalPhys;
    ulong availPhys = mem.AvailPhys;
    ulong systemCache = mem.SystemCache;

    Console.WriteLine($"Total physical:     {FormatBytes(totalPhys)}");
    Console.WriteLine($"Available physical: {FormatBytes(availPhys)}");
    Console.WriteLine($"Approx cache-like (SystemCache): {FormatBytes(systemCache)}");

    long usedNonCacheApprox = (long)totalPhys - (long)availPhys - (long)systemCache;
    if (usedNonCacheApprox < 0) usedNonCacheApprox = 0;
    Console.WriteLine($"Approx used (non-cache):   {FormatBytes((ulong)usedNonCacheApprox)}");
  }


  private static void DisplayMemoryInfo()
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


  private static void DisplaySingleSnapshot()
  {
    try
    {
      var memoryStatus = GetMemoryStatus();
      if (OperatingSystem.IsWindows())
      {
        var swapInfo = GetSwapInfo();
        if (_humanReadable) PrintHumanReadable(memoryStatus, swapInfo);
        else PrintRaw(memoryStatus, swapInfo);

        if (_showTotal) WriteShortReport();
      };
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"Error: {ex.Message}");
    }
  }


  private static MemoryStatusEX GetMemoryStatus()
  {
    var status = new MemoryStatusEX();
    if (!GlobalMemoryStatusEx(ref status))
    {
      throw new InvalidOperationException("Failed to get memory status");
    }
    return status;
  }


  static PERFORMANCE_INFORMATION GetPerfInfo()
  {
    _ = new PERFORMANCE_INFORMATION();
    int cb = Marshal.SizeOf<PERFORMANCE_INFORMATION>();
    if (!GetPerformanceInfo(out PERFORMANCE_INFORMATION pi, cb))
    {
      var err = Marshal.GetLastWin32Error();
      throw new InvalidOperationException($"GetPerformanceInfo failed: {err}");
    }
    return pi;
  }


  [System.Runtime.Versioning.SupportedOSPlatform("windows")]
  private static SwapInfo GetSwapInfo()
  {
    // Windows doesn't expose swap usage directly like Linux, so we approximate
    var status = GetMemoryStatus();
    var totalSwap = status.TotalPageFile;
    var availSwap = status.AvailPageFile;

    // Get page file usage from performance counters
    var category = new PerformanceCounterCategory("Paging File");
    var instances = category.GetInstanceNames();

    ulong currentUsage = totalSwap - availSwap;
    foreach (var instance in instances)
    {
      using var counter = new PerformanceCounter("Paging File", "% Usage", instance);
      currentUsage += (ulong)(counter.NextValue() * totalSwap / 100);
    }

    return new SwapInfo
    {
      CurrentUsage = currentUsage,
      Total = totalSwap
    };
  }


  static string FormatBytes(ulong bytes)
  {
    string[] units = ["B", "KB", "MB", "GB", "TB", "PB"];
    double size = bytes;
    int i = 0;
    while (size >= 1024 && i < units.Length - 1)
    {
      size /= 1024;
      i++;
    }
    return $"{size:0.##} {units[i]}";
  }


  private static void PrintHumanReadable(MemoryStatusEX memoryStatus, SwapInfo swapInfo)
  {
    Console.WriteLine("""
            Type        Total    Used    Free   Buff/Cache  Available
            Mem:        {0,10}  {1,10}  {2,10}  {3,10}     {4,10}
            Swap:       {5,10}  {6,10}  {7,10}
            """,
        FormatBytes(memoryStatus.TotalPhys),
        FormatBytes(memoryStatus.TotalPhys - memoryStatus.AvailPhys),
        FormatBytes(memoryStatus.AvailPhys),
        FormatBytes(memoryStatus.TotalPhys - memoryStatus.AvailPhys - memoryStatus.TotalPageFile + memoryStatus.AvailPageFile),
        FormatBytes(memoryStatus.AvailPageFile),
        FormatBytes(memoryStatus.TotalPageFile),
        FormatBytes(swapInfo.CurrentUsage),
        FormatBytes(memoryStatus.TotalPageFile - memoryStatus.AvailPageFile)
    );
  }


  private static void PrintRaw(MemoryStatusEX memoryStatus, SwapInfo swapInfo)
  {
    Console.WriteLine("""
            Type        Total    Used    Free   Buff/Cache  Available
            Mem:        {0,10}  {1,10}  {2,10}  {3,10}     {4,10}
            Swap:       {5,10}  {6,10}  {7,10}
            """,
        memoryStatus.TotalPhys / 1024,
        (memoryStatus.TotalPhys - memoryStatus.AvailPhys) / 1024,
        memoryStatus.AvailPhys / 1024,
        (memoryStatus.TotalPhys - memoryStatus.AvailPhys - memoryStatus.TotalPageFile + memoryStatus.AvailPageFile) / 1024,
        memoryStatus.AvailPageFile / 1024,
        memoryStatus.TotalPageFile / 1024,
        swapInfo.CurrentUsage,
        (memoryStatus.TotalPageFile - memoryStatus.AvailPageFile) / 1024
    );
  }
}