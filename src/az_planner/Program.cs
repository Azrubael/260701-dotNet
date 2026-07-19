class Program
{
  static void Main(string[] args)
  {

    var initMsg = "Message Scheduler started. Press Ctrl+C to exit.";
    Console.WriteLine(initMsg);

    var logger = new Logger("logs", "az_output.log");
    logger.Info(initMsg);

    int interval = GetIntervalFromArgs(args);

    var scheduler = new MessageScheduler(interval, logger);
    scheduler.Start();

    try
    {
      while (true) Thread.Sleep(1000);
    }
    catch (ThreadInterruptedException)
    {
      logger.Warn("Main loop interrupted.");
    }
    catch (Exception ex)
    {
      logger.Error($"Unhandled exception: {ex}");
    }
    finally
    {
      logger.Info("Message Scheduler shutting down.");
      logger.Dispose();
    }
  }

  private static int GetIntervalFromArgs(string[] args)
  {
    if (args.Length == 0)
      return 7200000; // Default: 2 hours

    return args[0] switch
    {
      "--test1" => 120000,  // 2 minutes
      "--test0" => 12000,   // 12 seconds
      _ => throw new ArgumentException(
        "Invalid argument. Use --test0 (12s) or --test1 (2min) or no args (2h)")
    };
  }
}

class MessageScheduler
{
  private readonly int _interval;
  private readonly Timer _timer;
  private readonly Logger _logger;

  public MessageScheduler(int interval, Logger logger)
  {
    _interval = interval;
    _logger = logger;

    // Initialize timer; Start can still hook Ctrl+C.
    _timer = new Timer(
      callback: SendMessage,
      state: null,
      dueTime: 0,
      period: _interval);
  }

  public void Start()
  {
    var firstMsg = $"Configured interval: {TimeSpan.FromMilliseconds(_interval)} ({_interval} ms)";
    _logger.Info(firstMsg);

    Console.CancelKeyPress += (sender, e) =>
    {
      // Prevent forced termination so logs can flush.
      e.Cancel = true;

      try
      {
        _timer.Dispose();
      }
      catch (Exception ex)
      {
        string errMsg = $"Error while disposing timer: {ex}";
        _logger.Error(errMsg);
      }
      string stopMsg = "Scheduler stopped (Ctrl+C).";
      _logger.Info(stopMsg);
      // Give logger a moment to flush if needed; then exit.
      Thread.Sleep(100);
      Environment.Exit(0);
    };
  }

  private void SendMessage(object? state)
  {
    try
    {
      _logger.Info("The sending message: 4.5.0");
    }
    catch (Exception ex)
    {
      _logger.Error($"SendMessage failed: {ex}");
      // Optional: decide whether to stop scheduling:
      _timer.Dispose();
    }
  }
}

sealed class Logger : IDisposable
{
  private readonly StreamWriter _writer;
  private readonly Lock _lock = new();

  public Logger(string directory, string fileName)
  {
    Directory.CreateDirectory(directory);

    var path = Path.Combine(directory, fileName);

    // Append mode so logs keep growing; you can add rolling later if you want.
    _writer = new StreamWriter(new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
    {
      AutoFlush = true
    };

    Info($"Logger initialized. Writing to {path}");
  }

  public void Info(string message) => Write("INFO", message);
  public void Warn(string message) => Write("WARN", message);
  public void Error(string message) => Write("ERROR", message);

  private void Write(string level, string message)
  {
    var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}";
    lock (_lock)
    {
      Console.WriteLine(line);
      _writer.WriteLine(line);
    }
  }

  public void Dispose()
  {
    lock (_lock)
    {
      _writer.Dispose();
    }
  }
}