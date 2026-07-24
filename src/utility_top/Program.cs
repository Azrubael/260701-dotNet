
using System.Runtime.InteropServices;

class Program
{
    // We’ll report:
    // - total physical
    // - available physical (free-ish)
    // - cache-like approximation:
    //   Standby + Cache-like (from GetPerformanceInfo fields)

    [StructLayout(LayoutKind.Sequential)]
    private struct MemoryStatusEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
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


    static void Main()
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
      var perf = GetPerfInfo();

      ulong totalPhys = mem.ullTotalPhys;
      ulong availPhys = mem.ullAvailPhys;
      ulong systemCache = perf.SystemCache.ToUInt64();

      ulong buffCacheLike = systemCache;

      Console.WriteLine($"Total physical:     {FormatBytes(totalPhys)}");
      Console.WriteLine($"Available physical: {FormatBytes(availPhys)}");
      Console.WriteLine($"Cache-like (SystemCache): {FormatBytes(systemCache)}");
      Console.WriteLine($"Approx buff/cache-like:    {FormatBytes(buffCacheLike)}");

      long usedNonCacheApprox = (long)totalPhys - (long)availPhys - (long)systemCache;
      if (usedNonCacheApprox < 0) usedNonCacheApprox = 0;
      Console.WriteLine($"Approx used (non-cache):   {FormatBytes((ulong)usedNonCacheApprox)}");
  }


    static PERFORMANCE_INFORMATION GetPerfInfo()
    {
        var pi = new PERFORMANCE_INFORMATION();
        int cb = Marshal.SizeOf<PERFORMANCE_INFORMATION>();

        if (!GetPerformanceInfo(out pi, cb))
        {
            var err = Marshal.GetLastWin32Error();
            throw new InvalidOperationException($"GetPerformanceInfo failed: {err}");
        }
        return pi;
    }

    static string FormatBytes(ulong bytes)
    {
        string[] units = { "B", "KB", "MB", "GB", "TB", "PB" };
        double v = bytes;
        int i = 0;
        while (v >= 1024 && i < units.Length - 1)
        {
            v /= 1024;
            i++;
        }
        return $"{v:0.##} {units[i]}";
    }
}
