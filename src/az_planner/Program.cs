class Program
{
  static void Main(string[] args)
  {
    Console.WriteLine("Message Scheduler started. Press Ctrl+C to exit.");

    int interval = GetIntervalFromArgs(args);

    var scheduler = new MessageScheduler(interval);
    scheduler.Start();

    // Keep the application running
    while (true)
    {
      Thread.Sleep(1000);
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

class MessageScheduler(int interval)
{
  private readonly int _interval = interval;
  private Timer? _timer;

  public void Start()
  {
    Console.WriteLine($"Scheduling messages every {TimeSpan.FromMilliseconds(_interval)}");

    _timer = new Timer(
        callback: SendMessage,
        state: null,
        dueTime: 0, // Start immediately
        period: _interval);

    // Handle Ctrl+C gracefully
    Console.CancelKeyPress += (sender, e) =>
    {
      _timer?.Dispose();
      Console.WriteLine("Scheduler stopped.");
      Environment.Exit(0);
    };
  }

  private void SendMessage(object? state)
  {
    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Sending message: 4.5.0");
    // Add your actual message sending logic here
  }
}