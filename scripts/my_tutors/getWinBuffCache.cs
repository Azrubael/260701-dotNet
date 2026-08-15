// getWinBuffCache.cs
// Build: dotnet run
using System;
using System.Runtime.InteropServices;

class Program
{
    // We’ll report:
    // - total physical
    // - available physical (free-ish)
    // - cache-like approximation:
    //   Standby + Cache-like (from GetPerformanceInfo fields)
    //
    // NOTE: Windows “cache-like” accounting differs from Linux buff/cache,
    // but these fields are the closest direct equivalents.

    [StructLayout(LayoutKind.Sequential)]
    private struct MEMORYSTATUSEX
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
    private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

    [DllImport("psapi.dll", SetLastError = true)]
    private static extern bool GetPerformanceInfo(out PERFORMANCE_INFORMATION lpPerformanceInformation, int cb);

    static void Main()
    {
        if (!GlobalMemoryStatusEx(ref UnsafeInit()))
        {
            var err = Marshal.GetLastWin32Error();
            Console.Error.WriteLine($"GlobalMemoryStatusEx failed: {err}");
            return;
        }

        var mem = GetMemStatus();
        var perf = GetPerfInfo();

        // GetPerformanceInfo:
        // SystemCache = file cache (cache-like).
        // There isn't a direct "standby" field in this struct.
        // So we approximate buff/cache-like as SystemCache + reclaimable cache-like portion.
        //
        // A commonly used approximation on Windows is:
        //   cache-like ≈ SystemCache
        //
        // If you want “more cache-like”, the best you can do without extra APIs
        // is SystemCache; standby cache accounting requires additional working sets / query APIs.
        //
        // So we’ll output both:
        // - SystemCache
        // - Available
        // and compute:
        // - used_non_cache-ish ≈ total - available - systemCache (may be negative due to different accounting)
        ulong totalPhys = mem.ullTotalPhys;
        ulong availPhys = mem.ullAvailPhys;

        ulong systemCache = perf.SystemCache.ToUInt64();

        ulong buffCacheLike = systemCache;

        Console.WriteLine($"Total physical:     {FormatBytes(totalPhys)}");
        Console.WriteLine($"Available physical: {FormatBytes(availPhys)}");
        Console.WriteLine($"Cache-like (SystemCache): {FormatBytes(systemCache)}");
        Console.WriteLine($"Approx buff/cache-like:    {FormatBytes(buffCacheLike)}");

        long usedNonCacheApprox = (long)totalPhys - (long)availPhys - (long)systemCache;
        Console.WriteLine($"Approx used (non-cache):   {FormatBytes(usedNonCacheApprox > 0 ? (ulong)usedNonCacheApprox : 0)}");
    }

    static MEMORYSTATUSEX UnsafeInit()
    {
        var s = new MEMORYSTATUSEX();
        s.dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>();
        return s;
    }

    static MEMORYSTATUSEX GetMemStatus()
    {
        var m = new MEMORYSTATUSEX { dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>() };
        if (!GlobalMemoryStatusEx(ref m))
        {
            var err = Marshal.GetLastWin32Error();
            throw new InvalidOperationException($"GlobalMemoryStatusEx failed: {err}");
        }
        return m;
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
