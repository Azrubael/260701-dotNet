using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;

namespace _260901_ava2d.Views;


public partial class MainWindow : Window
{
  private readonly DispatcherTimer _timer;
  private readonly List<Point> _history = [];
  private readonly DispatcherTimer _iconTimer;
  private readonly Random _random = new();
  private readonly List<Polyline> _snakePaths = [];

  private readonly string[] _iconUris =
  [
    "avares://_260901_ava2d/Assets/food_icon_01.png",
    "avares://_260901_ava2d/Assets/food_icon_02.png",
    "avares://_260901_ava2d/Assets/food_icon_03.png",
    "avares://_260901_ava2d/Assets/food_icon_05.png",
    "avares://_260901_ava2d/Assets/food_icon_06.png",
    "avares://_260901_ava2d/Assets/food_icon_07.png",
    "avares://_260901_ava2d/Assets/food_icon_08.png",
    "avares://_260901_ava2d/Assets/food_icon_09.png",
    "avares://_260901_ava2d/Assets/food_icon_10.png",
    "avares://_260901_ava2d/Assets/food_icon_11.png",
    "avares://_260901_ava2d/Assets/food_icon_12.png",
    "avares://_260901_ava2d/Assets/food_icon_13.png",
    "avares://_260901_ava2d/Assets/food_icon_14.png",
    "avares://_260901_ava2d/Assets/food_icon_15.png",
    "avares://_260901_ava2d/Assets/food_icon_16.png"
  ];


  private Vector _direction = new(1, 0);
  private Point _headPosition;

  private const double SegmentSize = 15;
  private const double SnakeSpeed = 90;

  private Window? _gameOverDialog;

  public MainWindow()
  {
    InitializeComponent();

      _snakePaths.Add(SnakePath);

  for (int i = 0; i < 8; i++)
  {
    Polyline path = new()
    {
      Stroke = SnakePath.Stroke,
      StrokeThickness = SnakePath.StrokeThickness,
      StrokeLineCap = SnakePath.StrokeLineCap,
      StrokeJoin = SnakePath.StrokeJoin,
      IsVisible = false
    };

    GameCanvas.Children.Add(path);
    _snakePaths.Add(path);
  }

    _timer = new DispatcherTimer
    {
      Interval = TimeSpan.FromMilliseconds(15)
    };
    _timer.Tick += OnTimerTick;

    _iconTimer = new DispatcherTimer
    {
      Interval = TimeSpan.FromSeconds(30)
    };
    _iconTimer.Tick += OnIconTimerTick;

    Closing += OnMainWindowClosing;
  }


  private void OnNewGameClick(object? sender, RoutedEventArgs e)
  {
    StartMessage.IsVisible = false;
    _timer.Stop();
    _iconTimer.Stop();

    Console.WriteLine(
        $"Canvas: {GameCanvas.Bounds.Width} x {GameCanvas.Bounds.Height}");
    double canvasWidth = GameCanvas.Bounds.Width;
    double canvasHeight = GameCanvas.Bounds.Height;

    if (canvasWidth <= 0 || canvasHeight <= 0 ||
        double.IsNaN(canvasWidth) || double.IsNaN(canvasHeight))
    {
      return;
    }

    _headPosition = new Point(
        canvasWidth / 2 - SegmentSize / 2,
        canvasHeight / 2 - SegmentSize / 2);

    _direction = new Vector(1, 0);

    _history.Clear();
    _history.Add(_headPosition);

    for (int i = 1; i < 6; i++)
    {
      _history.Add(new Point(
          _headPosition.X - i * SegmentSize,
          _headPosition.Y));
    }

    SnakePath.IsVisible = true;
    IconImage.IsVisible = false;

    UpdateSnake();

    // Temporarily comment this out while testing.
    // ShowRandomIcon();

    _iconTimer.Start();
    _timer.Start();

    Focus();
  }



  private void OnIconTimerTick(object? sender, EventArgs e)
  {
    IconImage.IsVisible = false;
    // ShowRandomIcon();
  }


  private void ShowRandomIcon()
  {
    if (_iconUris.Length == 0)
      return;

    string uri = _iconUris[_random.Next(_iconUris.Length)];

    try
    {
      using System.IO.Stream stream = AssetLoader.Open(new Uri(uri));
      IconImage.Source = new Bitmap(stream);
    }
    catch (Exception exception)
    {
      Console.WriteLine($"Unable to load icon '{uri}': {exception}");
      IconImage.IsVisible = false;
      return;
    }

    double canvasWidth = GameCanvas.Bounds.Width;
    double canvasHeight = GameCanvas.Bounds.Height;

    if (!double.IsFinite(canvasWidth) ||
        !double.IsFinite(canvasHeight) ||
        canvasWidth <= 0 ||
        canvasHeight <= 0)
    {
      IconImage.IsVisible = false;
      return;
    }

    double iconWidth = IconImage.Bounds.Width;
    double iconHeight = IconImage.Bounds.Height;

    if (!double.IsFinite(iconWidth) || iconWidth <= 0)
      iconWidth = 32;

    if (!double.IsFinite(iconHeight) || iconHeight <= 0)
      iconHeight = 32;

    double availableWidth = canvasWidth - iconWidth;
    double availableHeight = canvasHeight - iconHeight;

    if (availableWidth <= 0 || availableHeight <= 0)
    {
      IconImage.IsVisible = false;
      return;
    }

    Canvas.SetLeft(
        IconImage,
        _random.NextDouble() * availableWidth);

    Canvas.SetTop(
        IconImage,
        _random.NextDouble() * availableHeight);

    IconImage.IsVisible = true;
  }



  private void OnWindowKeyDown(object? sender, KeyEventArgs e)
  {
    if (e.Key == Key.Q)
    {
      OnExitClick(sender, e);
    }
    else if (e.Key == Key.N)
    {
      OnNewGameClick(sender, e);
      e.Handled = true;
      return;
    }

    if (_history.Count < 100)
      return;

    if (e.Key == Key.Left)
    {
      _direction = new Vector(_direction.Y, -_direction.X);
      e.Handled = true;
      return;
    }
    else if (e.Key == Key.Right)
    {
      _direction = new Vector(-_direction.Y, _direction.X);
      e.Handled = true;
      return;
    }
  }

  private void OnTimerTick(object? sender, EventArgs e)
  {
    double step = SnakeSpeed * 0.01;

    Point nextPosition = _headPosition + _direction * step;

    double canvasWidth = GameCanvas.Bounds.Width;
    double canvasHeight = GameCanvas.Bounds.Height;

    if (double.IsNaN(canvasWidth)) canvasWidth = 0;
    if (double.IsNaN(canvasHeight)) canvasHeight = 0;

    double maxX = Math.Max(0, canvasWidth - SegmentSize);
    double maxY = Math.Max(0, canvasHeight - SegmentSize);

    nextPosition = new Point(
        Math.Clamp(nextPosition.X, 0, maxX),
        Math.Clamp(nextPosition.Y, 0, maxY));

    _headPosition = nextPosition;

    _history.Insert(0, _headPosition);

    if (_history.Count > 200)
      _history.RemoveAt(_history.Count - 1);

    if (CheckSelfCollision())
    {
      GameOver();
      return;
    }

    UpdateSnake();
  }


  private bool CheckSelfCollision()
  {
    double bodyLength = 5 * SegmentSize * 0.9;
    double travelled = 0;
    double collisionDistance = SegmentSize;

    for (int i = 1; i < _history.Count; i++)
    {
      Point newer = _history[i - 1];
      Point older = _history[i];

      double dx = newer.X - older.X;
      double dy = newer.Y - older.Y;
      double segmentDistance = Math.Sqrt(dx * dx + dy * dy);

      travelled += segmentDistance;

      if (travelled < bodyLength)
        continue;

      double headDx = _headPosition.X - older.X;
      double headDy = _headPosition.Y - older.Y;
      double distanceSquared = headDx * headDx + headDy * headDy;

      if (distanceSquared < collisionDistance * collisionDistance)
        return true;
    }

    return false;
  }

  private async void GameOver()
  {
    if (_gameOverDialog != null)
      return;

    _timer.Stop();
    _iconTimer.Stop();

    _gameOverDialog = new Window
    {
      Title = "Game Over!",
      Width = 300,
      Height = 150,
      CanResize = false,
      WindowStartupLocation = WindowStartupLocation.CenterOwner,
      Content = new TextBlock
      {
        Text = "You bit yourself!",
        TextAlignment = TextAlignment.Center,
        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
        Foreground = new SolidColorBrush(Colors.Red),
        FontSize = 24,
        FontWeight = FontWeight.Bold
      }
    };

    Window? dialog = _gameOverDialog;

    try
    {
      await dialog.ShowDialog(this);
    }
    finally
    {
      if (ReferenceEquals(_gameOverDialog, dialog))
        _gameOverDialog = null;
    }
  }

  private void OnMainWindowClosing(object? sender, WindowClosingEventArgs e)
  {
    _timer.Stop();
    _iconTimer.Stop();

    _gameOverDialog?.Close();
    _gameOverDialog = null;
  }



  private void UpdateSnake()
  {
    var points = new Points();

    // Add all snake segment positions to the polyline
    for (int segmentIndex = 0; segmentIndex < 5; segmentIndex++)
    {
      Point position = GetHistoryPoint(segmentIndex * SegmentSize);

      points.Add(new Point(
          position.X + SegmentSize / 2,
          position.Y + SegmentSize / 2));
    }

    SnakePath.Points = points;
  }

  private Point GetHistoryPoint(double distance)
  {
    double travelled = 0;

    for (int i = 1; i < _history.Count; i++)
    {
      Point newer = _history[i - 1];
      Point older = _history[i];

      double segmentDistance = Math.Sqrt(
          Math.Pow(newer.X - older.X, 2) +
          Math.Pow(newer.Y - older.Y, 2));

      if (travelled + segmentDistance >= distance)
      {
        double ratio = segmentDistance == 0
            ? 0
            : (distance - travelled) / segmentDistance;

        return new Point(
            newer.X + (older.X - newer.X) * ratio,
            newer.Y + (older.Y - newer.Y) * ratio);
      }

      travelled += segmentDistance;
    }

    return _history[^1];
  }

  private void OnExitClick(object? sender, RoutedEventArgs e)
  {
    _timer.Stop();
    _iconTimer.Stop();

    _gameOverDialog?.Close();
    _gameOverDialog = null;

    Close();
  }
}