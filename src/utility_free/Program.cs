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
    // public ulong SystemCache;

    public MemoryStatusEX()
    {
      dwLength = (uint)Marshal.SizeOf<MemoryStatusEX>();
      // var perf = GetPerfInfo();
      // SystemCache = perf.SystemCache.ToUInt64();
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
    if (_help)
    {
      ShowHelp();
      return;
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
          _help = true;
          Console.Error.WriteLine($"Unknown option: {_args[i]}");
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


  static void WriteShortReport(MemoryStatusEX mem, ulong systemCache)
  {
    ulong totalPhys = mem.TotalPhys;
    ulong availPhys = mem.AvailPhys;

    ulong usedPhys = totalPhys > availPhys ? totalPhys - availPhys : 0;
    ulong usedNonCache = usedPhys > systemCache ? usedPhys - systemCache : 0;

    Console.WriteLine("\n--- Detailed Breakdown ---");
    Console.WriteLine($"Total physical:              {FormatBytes(totalPhys)}");
    Console.WriteLine($"Available physical:          {FormatBytes(availPhys)}");
    Console.WriteLine($"System Cache:                {FormatBytes(systemCache)}");
    Console.WriteLine($"Used physical (non-cache):   {FormatBytes(usedNonCache)}");
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
      if (!OperatingSystem.IsWindows())
      {
        Console.Error.WriteLine("This utility only supports Windows OS.");
        return;
      }

      var mem = GetMemoryStatus();
      ulong systemCache = GetSystemCacheBytes();

      if (_humanReadable)
        PrintFormatted(mem, systemCache, isHumanReadable: true);
      else
        PrintFormatted(mem, systemCache, isHumanReadable: false);

      if (_showTotal)
        WriteShortReport(mem, systemCache);
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
      var err = Marshal.GetLastWin32Error();
      throw new InvalidOperationException($"GlobalMemoryStatusEx failed with Win32 error: {err}");
    }
    return status;
  }


  private static ulong GetSystemCacheBytes()
  {
    int cb = Marshal.SizeOf<PERFORMANCE_INFORMATION>();
    if (!GetPerformanceInfo(out PERFORMANCE_INFORMATION pi, cb))
    {
      return 0;
    }

    // SystemCache is reported in pages, multiply by PageSize to get bytes
    return pi.SystemCache.ToUInt64() * pi.PageSize.ToUInt64();
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


  private static void PrintFormatted(MemoryStatusEX mem, ulong systemCache, bool isHumanReadable)
  {
    ulong totalPhys = mem.TotalPhys;
    ulong availPhys = mem.AvailPhys;
    ulong usedPhys = totalPhys - availPhys;

    // Windows Commit Limit / Usage (RAM + PageFile)
    ulong commitTotal = mem.TotalPageFile - mem.AvailPageFile;
    ulong commitLimit = mem.TotalPageFile;

    Func<ulong, string> fmt = isHumanReadable
        ? FormatBytes
        : (b => (b / 1024).ToString()); // Standard Linux free command defaults to KB

    string label = isHumanReadable ? "" : " (KiB)";

    // Define fixed width per column (12 chars width, left-aligned for labels, right-aligned for values)
    Console.WriteLine($"{label,-8} {"Total",12} {"Used",12} {"Free",12} {"Buff/Cache",12} {"Available",12}");
    Console.WriteLine($"{"Mem:",-8} {fmt(totalPhys),12} {fmt(usedPhys),12} {fmt(availPhys - systemCache),12} {fmt(systemCache),12} {fmt(availPhys),12}");
    Console.WriteLine($"{"Commit:",-8} {fmt(commitLimit),12} {fmt(commitTotal),12} {fmt(commitLimit - commitTotal),12}");
  }
}