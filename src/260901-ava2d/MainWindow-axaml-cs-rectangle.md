```csharp
using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;

namespace _260901_ava2d.Views;


public partial class MainWindow : Window
{
  private readonly DispatcherTimer _timer;
  private readonly Rectangle[] _segments;
  private readonly List<Point> _history = [];

  private Vector _direction = new(1, 0);
  private Point _headPosition;

  private const double SegmentSize = 15;
  private const double SnakeSpeed = 12; // pixels per second

  public MainWindow()
  {
    InitializeComponent();

    _segments = [Snake0, Snake1, Snake2, Snake3, Snake4];

    _timer = new DispatcherTimer
    {
      Interval = TimeSpan.FromMilliseconds(16)
    };

    _timer.Tick += OnTimerTick;
  }


  private void OnNewGameClick(object? sender, RoutedEventArgs e)
  {
    _headPosition = new Point(75, GameCanvas.Height / 2);
    _direction = new Vector(1, 0);

    _history.Clear();
    _history.Add(_headPosition);

    foreach (Rectangle segment in _segments)
      segment.IsVisible = true;

    SnakePath.IsVisible = true;
    UpdateSnake();

    Focus();
    _timer.Start();
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
    }

    if (_history.Count < 100)
      return; // Ignore input until snake is long enough

    if (e.Key == Key.Left)
    {
      // Relative left turn: (x, y) -> (y, -x)
      _direction = new Vector(_direction.Y, -_direction.X);
      e.Handled = true;
    }
    else if (e.Key == Key.Right)
    {
      // Relative right turn: (x, y) -> (-y, x)
      _direction = new Vector(-_direction.Y, _direction.X);
      e.Handled = true;
    }

  }


  private void OnTimerTick(object? sender, EventArgs e)
  {
    double step = SnakeSpeed * 0.016;

    _headPosition += _direction * step;

    double maxX = GameCanvas.Width - SegmentSize;
    double maxY = GameCanvas.Height - SegmentSize;

    _headPosition = new Point(
        Math.Clamp(_headPosition.X, 0, maxX),
        Math.Clamp(_headPosition.Y, 0, maxY));

    _history.Insert(0, _headPosition);

    if (_history.Count > 2000)
      _history.RemoveAt(_history.Count - 1);

    // NEW: Check for self-collision
    if (CheckSelfCollision())
    {
      GameOver();
      return;
    }

    UpdateSnake();
  }


  private bool CheckSelfCollision()
  {
    // Ignore the trail currently occupied by the snake's own segments.
    double bodyLength = _segments.Length * SegmentSize *0.9;
    double travelled = 0;
    double collisionDistance = SegmentSize * 1.5;

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


  private void GameOver()
  {
    _timer.Stop();

    // Show game over message (you can use a dialog or status text)
    var dialog = new Window
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

    dialog.ShowDialog(this);
  }


  // private void UpdateSnake()
  // {
  //   for (int segmentIndex = 0; segmentIndex < _segments.Length; segmentIndex++)
  //   {
  //     Point position = GetHistoryPoint(segmentIndex * SegmentSize);

  //     Canvas.SetLeft(_segments[segmentIndex], position.X);
  //     Canvas.SetTop(_segments[segmentIndex], position.Y);
  //   }
  // }


  private void UpdateSnake()
  {
    SnakePath.Points.Clear();

    for (int segmentIndex = 0; segmentIndex < _segments.Length; segmentIndex++)
    {
      Point position = GetHistoryPoint(segmentIndex * SegmentSize);

      Canvas.SetLeft(_segments[segmentIndex], position.X);
      Canvas.SetTop(_segments[segmentIndex], position.Y);

      // Exclude Snake0; draw through Snake1–Snake4.
      if (segmentIndex > 0)
      {
        SnakePath.Points.Add(new Point(
          position.X + SegmentSize / 2,
          position.Y + SegmentSize / 2));
      }
    }
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
    Close();
  }
}
```