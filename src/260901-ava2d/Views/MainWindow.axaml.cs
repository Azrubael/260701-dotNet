using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;

namespace _260901_ava2d.Views;


public partial class MainWindow : Window
{
  private readonly DispatcherTimer _timer;
  private readonly List<Point> _history = [];

  private Vector _direction = new(1, 0);
  private Point _headPosition;

  private const double SegmentSize = 15;
  private const double SnakeSpeed = 30;

  public MainWindow()
  {
    InitializeComponent();
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

    // Pre-populate history with initial snake body segments
    // This creates a snake trail going backwards from the head
    for (int i = 1; i < 6; i++) // 5 body segments
    {
      Point prevSegment = new(
        _headPosition.X - (i * SegmentSize),
        _headPosition.Y);
      _history.Add(prevSegment);
    }

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
      return;

    if (e.Key == Key.Left)
    {
      _direction = new Vector(_direction.Y, -_direction.X);
      e.Handled = true;
    }
    else if (e.Key == Key.Right)
    {
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
    double bodyLength = 5 * SegmentSize *0.9;
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

  private void GameOver()
  {
    _timer.Stop();

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
    Close();
  }
}