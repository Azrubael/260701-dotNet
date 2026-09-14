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
  public ModelOfCanvas ThisCanvas { get; } = new(640, 480, "#fffbd4");
  public double WindowHeight => ThisCanvas.Height + 30;
  public ModelOfSnake Snake { get; } = new();

  private readonly DispatcherTimer _timer;
  public readonly DispatcherTimer IconTimer;

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

    IconTimer = new DispatcherTimer
    {
      Interval = TimeSpan.FromSeconds(30)
    };
    IconTimer.Tick += OnIconTimerTick;

    Closing += OnMainWindowClosing;
  }


  private void OnNewGameClick(object? sender, RoutedEventArgs e)
  {
    _isPaused = false;
    StartMessage.IsVisible = false;
    _score = 0;
    ScoreText.Text = $"Score: {_score,-5}";
    _timer.Stop();
    IconTimer.Stop();

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

    IconTimer.Start();
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
      IconTimer.Stop();
    }
    else
    {
      _timer.Start();
      IconTimer.Start();
    }
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

    if (ThisCanvas.CheckIconCollision(IconImage, Snake, IconTimer))
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
    IconTimer.Stop();

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
    IconTimer.Stop();

    _gameOverDialog?.Close();
    _gameOverDialog = null;
  }


  private void OnExitClick(object? sender, RoutedEventArgs e)
  {
    _timer.Stop();
    IconTimer.Stop();

    _gameOverDialog?.Close();
    _gameOverDialog = null;

    Close();
  }
}