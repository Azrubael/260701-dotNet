using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using _260907_ava2d.Models;

namespace _260907_ava2d.Views;


public partial class MainWindow : Window
{
  public ModelOfCanvas ThisCanvas { get; } = new(640, 480, "#f5f5e0");
  public double WindowHeight => ThisCanvas.Height + 30;
  public ModelOfSnake Snake { get; } = new();

  private readonly DispatcherTimer _timer;
  private readonly DispatcherTimer _iconTimer;

  private bool _isPaused;
  private Window? _gameOverDialog;

  private int _score;


  public MainWindow()
  {
    InitializeComponent();
    DataContext = this;
    Snake.SnakePaths.Add(SnakePath);

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
      Snake.SnakePaths.Add(path);
    }

    GameCanvas.Children.Remove(IconImage);
    GameCanvas.Children.Add(IconImage);
    GameCanvas.ClipToBounds = true;

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
    _isPaused = false;
    StartMessage.IsVisible = false;
    _score = 0;
    ScoreText.Text = $"Score: {_score,-5}";
    _timer.Stop();
    _iconTimer.Stop();

    if (ThisCanvas.Width <= 0 || ThisCanvas.Height <= 0 ||
        double.IsNaN(ThisCanvas.Width) || double.IsNaN(ThisCanvas.Height))
    {
      return;
    }

    Snake.CreateSnake(ThisCanvas, LeftEye, RightEye);

    SnakePath.IsVisible = true;
    SnakeHead.IsVisible = true;
    LeftEye.IsVisible = true;
    RightEye.IsVisible = true;
    IconImage.IsVisible = false;

    Snake.UpdateSnake(ThisCanvas, LeftEye, RightEye);
    UpdateSnakeHeadVisual();

    // Temporarily comment this out while testing.
    ThisCanvas.ShowRandomIcon(IconImage);

    _iconTimer.Start();
    _timer.Start();

    Focus();
  }


  private void OnPauseClick(object? sender, RoutedEventArgs e)
  {
    TogglePause();
  }


  private void OnIconTimerTick(object? sender, EventArgs e)
  {
    IconImage.IsVisible = false;
    ThisCanvas.ShowRandomIcon(IconImage);
  }


  private void OnWindowKeyDown(object? sender, KeyEventArgs e)
  {
    switch (e.Key)
    {
      case Key.Q:
        OnExitClick(sender, e);
        return;

      case Key.N:
        OnNewGameClick(sender, e);
        e.Handled = true;
        return;

      case Key.P:
        TogglePause();
        e.Handled = true;
        return;

      case Key.Left:
        Snake.RotateLeft();
        e.Handled = true;
        break;

      case Key.Right:
        Snake.RotateRight();
        e.Handled = true;
        break;
    }
  }


  private void TogglePause()
  {
    _isPaused = !_isPaused;

    if (_isPaused)
    {
      _timer.Stop();
      _iconTimer.Stop();
    }
    else
    {
      _timer.Start();
      _iconTimer.Start();
    }
  }


  private bool CheckIconCollision()
  {
    // HeadPosition uses continuous coordinates. So after crossing an edge, it may
    // be outside 0..canvas.Width, while the icon remains inside the visible canvas
    static double Wrap(double value, double size)
    {
      double result = value % size;
      return result < 0 ? result + size : result;
    }

    if (!IconImage.IsVisible)
      return false;

    double iconLeft = Canvas.GetLeft(IconImage);
    double iconTop = Canvas.GetTop(IconImage);
    double iconWidth = IconImage.Bounds.Width;
    double iconHeight = IconImage.Bounds.Height;

    if (!double.IsFinite(iconLeft) ||
        !double.IsFinite(iconTop) ||
        !double.IsFinite(iconWidth) ||
        !double.IsFinite(iconHeight) ||
        iconWidth <= 0 ||
        iconHeight <= 0)
    {
      return false;
    }

    const double padding = 3;

    double headX = Wrap(
        Snake.HeadPosition.X,
        ThisCanvas.Width);

    double headY = Wrap(
        Snake.HeadPosition.Y,
        ThisCanvas.Height);

    var iconRect = new Rect(
        iconLeft + padding,
        iconTop + padding,
        Math.Max(0, iconWidth - padding * 2),
        Math.Max(0, iconHeight - padding * 2));

    var headRect = new Rect(
        headX + padding,
        headY + padding,
        ModelOfSnake.SegmentSize - padding * 2,
        ModelOfSnake.SegmentSize - padding * 2);

    bool overlaps =
        headRect.Left < iconRect.Right &&
        headRect.Right > iconRect.Left &&
        headRect.Top < iconRect.Bottom &&
        headRect.Bottom > iconRect.Top;

    if (!overlaps)
      return false;

    IconImage.IsVisible = false;
    _iconTimer.Stop();
    _iconTimer.Start();

    return true;
  }


  private void OnTimerTick(object? sender, EventArgs e)
  {
    Snake.Move();
    Snake.UpdateSnake(ThisCanvas, LeftEye, RightEye);
    UpdateSnakeHeadVisual();

    if (Snake.IsSelfCollision(ThisCanvas))
    {
      GameOver();
      return;
    }

    if (CheckIconCollision())
    {
      _score++;
      ScoreText.Text = $"Score: {_score}";

      Snake.AddLength(ThisCanvas, LeftEye, RightEye, SnakeHead);
      Snake.AddSpeed(_score);
      ThisCanvas.ShowRandomIcon(IconImage);
    }
    ;
  }


  private void UpdateSnakeHeadVisual()
  {
    SnakeHead.Width = Snake.HeadLength;
    SnakeHead.Height = Snake.HeadWidth;

    Canvas.SetLeft(SnakeHead, Snake.HeadRenderPosition.X);
    Canvas.SetTop(SnakeHead, Snake.HeadRenderPosition.Y);
    SnakeHead.RenderTransform = new RotateTransform(Snake.HeadRotation);
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
        Text = "Game over:\nthe Snake bit itself!",
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


  private void OnExitClick(object? sender, RoutedEventArgs e)
  {
    _timer.Stop();
    _iconTimer.Stop();

    _gameOverDialog?.Close();
    _gameOverDialog = null;

    Close();
  }
}