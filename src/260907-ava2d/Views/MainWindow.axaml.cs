using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using System;
using _260907_ava2d.Models;

namespace _260907_ava2d.Views;


public partial class MainWindow : Window
{
  public ModelOfCanvas ThisCanvas { get; } = new(640, 480);
  public double WindowHeight => ThisCanvas.Height + 30;

  private static readonly string[] _iconUris =
  [
    "avares://_260907_ava2d/Assets/food_icon_01.png",
    "avares://_260907_ava2d/Assets/food_icon_02.png",
    "avares://_260907_ava2d/Assets/food_icon_03.png",
    "avares://_260907_ava2d/Assets/food_icon_05.png",
    "avares://_260907_ava2d/Assets/food_icon_06.png",
    "avares://_260907_ava2d/Assets/food_icon_07.png",
    "avares://_260907_ava2d/Assets/food_icon_08.png",
    "avares://_260907_ava2d/Assets/food_icon_09.png",
    "avares://_260907_ava2d/Assets/food_icon_10.png",
    "avares://_260907_ava2d/Assets/food_icon_11.png",
    "avares://_260907_ava2d/Assets/food_icon_12.png",
    "avares://_260907_ava2d/Assets/food_icon_13.png",
    "avares://_260907_ava2d/Assets/food_icon_14.png",
    "avares://_260907_ava2d/Assets/food_icon_15.png",
    "avares://_260907_ava2d/Assets/food_icon_16.png"
  ];

  private readonly DispatcherTimer _timer;
  private readonly DispatcherTimer _iconTimer;
  private readonly Random _random = new();
  private bool _isPaused;
  private Window? _gameOverDialog;
  readonly ModelOfSnake thisSnake = new();


  public MainWindow()
  {
    InitializeComponent();
    DataContext = this;
    thisSnake.SnakePaths.Add(SnakePath);

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
      thisSnake.SnakePaths.Add(path);
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

    thisSnake.HeadPosition = new Point(
        canvasWidth / 2 - ModelOfSnake.SegmentSize / 2,
        canvasHeight / 2 - ModelOfSnake.SegmentSize / 2);

    thisSnake.Direction = new Vector(1, 0);

    thisSnake.History.Clear();
    thisSnake.History.Add(thisSnake.HeadPosition);

    for (int i = 1; i < 6; i++)
    {
      thisSnake.History.Add(new Point(
          thisSnake.HeadPosition.X - i * ModelOfSnake.SegmentSize,
          thisSnake.HeadPosition.Y));
    }

    SnakePath.IsVisible = true;
    IconImage.IsVisible = false;

    thisSnake.UpdateSnake(ThisCanvas);

    // Temporarily comment this out while testing.
    ShowRandomIcon();

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
    ShowRandomIcon();
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
      iconWidth = 30;

    if (!double.IsFinite(iconHeight) || iconHeight <= 0)
      iconHeight = 30;

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
    }

    if (thisSnake.History.Count < 50)
      return;

    switch (e.Key)
    {
      case Key.Left:
        thisSnake.RotateLeft();
        e.Handled = true;
        break;

      case Key.Right:
        thisSnake.RotateRight();
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

  private void OnTimerTick(object? sender, EventArgs e)
  {
    double step = ModelOfSnake.SnakeSpeed * 0.015;

    // Do not clamp or reset this position.
    thisSnake.HeadPosition += thisSnake.Direction * step;

    thisSnake.History.Insert(0, thisSnake.HeadPosition);

    if (thisSnake.History.Count > 200)
      thisSnake.History.RemoveAt(thisSnake.History.Count - 1);

    if (thisSnake.IsSelfCollision(ThisCanvas))
    {
      GameOver();
      return;
    }

    thisSnake.UpdateSnake(ThisCanvas);
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