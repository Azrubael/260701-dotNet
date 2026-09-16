using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using System.Threading.Tasks;
using System;
using _260914_tetris.Models;

namespace _260914_tetris.Views;

public partial class MainWindow : Window
{
  private const int CellSize = 20;
  private const int BoardWidth = 10;
  private const int BoardHeight = 25;
  private GameLogicEngine Game = new(BoardWidth, BoardHeight);
  private readonly DispatcherTimer _timer;
  private Window? _gameOverDialog;
  private bool _isPaused = false;

  public MainWindow()
  {

    InitializeComponent();
    _timer = new DispatcherTimer
    {
      Interval = TimeSpan.FromMilliseconds(500)
    };
    _timer.Tick += OnTimerTick;
  }


  private void OnNewGameClick(object? sender, RoutedEventArgs e)
  {
    Focus();

    _isPaused = false;
    StartMessage.IsVisible = false;
    GameCanvas.IsVisible = true;
    Game = new(10, 25);
    _timer.Start();
    DrawTetromino();
  }


  private void OnTimerTick(object? sender, EventArgs e)
  {
    if (!Game.TryMovePiece(0, 1))
    {
      if (!Game.DropPiece())
      {
        DrawTetromino();
        GameOver();
        return;
      }
    }
    DrawTetromino();
  }


  private void DrawTetromino()
  {
    GameCanvas.Children.Clear();

    var board = Game.BoardSnapshot;

    for (int x = 0; x < BoardWidth; x++)
    {
      for (int y = 0; y < BoardHeight; y++)
      {
        if (board[x, y] == CellState.Empty)
          continue;

        AddCell(x, y, Tetromino.GetColor((TetrominoType)board[x, y]));
      }
    }

    var color = Tetromino.GetColor(Game.CurrentPiece.Type);

    foreach (var (x, y) in Game.CurrentPiece.GetCells())
      AddCell(x, y, color);
  }

  private void AddCell(int x, int y, IBrush color)
  {
    var cell = new Rectangle
    {
      Width = CellSize - 1,
      Height = CellSize - 1,
      Fill = color,
      Stroke = Brushes.LightGray,
      StrokeThickness = 1
    };

    Canvas.SetLeft(cell, x * CellSize);
    Canvas.SetTop(cell, y * CellSize);
    GameCanvas.Children.Add(cell);
  }


  private void WindowKeyDown(object sender, KeyEventArgs e)
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

      case Key.Space:
        if (!Game.DropPiece())
        {
          _timer.Stop();
          GameOver();
        }
        DrawTetromino();
        e.Handled = true;
        return;

      case Key.Left:
        Game.TryMovePiece(-1, 0);
        break;

      case Key.Right:
        Game.TryMovePiece(1, 0);
        break;

      case Key.Down:
        if (!Game.DropPiece())
        {
          _timer.Stop();
          GameOver();
        }
        DrawTetromino();
        e.Handled = true;
        return;

      case Key.Up:
        Game.TryRotate(true);
        break;
    }

    DrawTetromino();
  }


  private void OnPauseClick(object? sender, RoutedEventArgs e)
  {
    TogglePause();
  }


  private void TogglePause()
  {
    _isPaused = !_isPaused;

    if (_isPaused)
    {
      _timer.Stop();
    }
    else
    {
      _timer.Start();
    }
  }


  private async void OnFAQClick(object? sender, RoutedEventArgs e)
  {
    await ShowInfoDialogAsync(
      "Frequently Asked Questions",
      "This game has no defined end.\nPress 'N' to play.\nPress 'P' to pause.\nPress 'Q' to close the application.\nAny other questions are useless.",
      Colors.Blue,
      16);
  }


  private async void OnAboutClick(object? sender, RoutedEventArgs e)
  {
    await ShowInfoDialogAsync(
      "About this game",
      "This application was created\nas a pet project\non 21 September 2026.",
      Colors.Blue,
      16);
  }


  private async Task ShowInfoDialogAsync(
    string title,
    string message,
    Color textColor,
    double fontSize)
  {
    bool wasPaused = _isPaused;
    bool timerWasRunning = _timer.IsEnabled;

    _timer.Stop();
    _isPaused = true;

    Window dialog = new()
    {
      Title = title,
      Width = 300,
      Height = 150,
      CanResize = false,
      WindowStartupLocation = WindowStartupLocation.CenterOwner,
      Content = new TextBlock
      {
        Text = message,
        TextAlignment = TextAlignment.Center,
        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
        Foreground = new SolidColorBrush(textColor),
        FontSize = fontSize
      }
    };

    try
    {
      await dialog.ShowDialog(this);
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"{title} dialog error: {ex}");
    }
    finally
    {
      _isPaused = wasPaused;

      if (timerWasRunning)
        _timer.Start();

    }
  }


  private async void GameOver()
  {
    if (_gameOverDialog != null)
      return;

    _timer.Stop();
    _gameOverDialog = new Window
    {
      Title = "Game Over!",
      Width = 300,
      Height = 150,
      CanResize = false,
      WindowStartupLocation = WindowStartupLocation.CenterOwner,
      Content = new TextBlock
      {
        Text = "Game over!",
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


  private void OnExitClick(object? sender, RoutedEventArgs e)
  {
    _timer.Stop();
    _gameOverDialog?.Close();
    _gameOverDialog = null;
    Close();
  }


  protected override void OnClosed(EventArgs e)
  {
    _timer.Stop();
    _gameOverDialog?.Close();
    _gameOverDialog = null;
    base.OnClosed(e);
  }

}