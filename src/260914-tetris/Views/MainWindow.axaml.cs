using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using System.Threading.Tasks;
using System;
using _260914_tetris.Models;

namespace _260914_tetris.Views;

public partial class MainWindow : Window
{
  private bool _isPaused = false;
  private readonly DispatcherTimer _timer;
  private Window? _gameOverDialog;

  public MainWindow()
  {

    InitializeComponent();
    _timer = new DispatcherTimer
    {
      Interval = TimeSpan.FromMilliseconds(50)
    };
    _timer.Tick += OnTimerTick;
  }


  private void OnNewGameClick(object? sender, RoutedEventArgs e)
  {

  }


  private void OnTimerTick(object? sender, EventArgs e)
  {
  }


  private void OnPauseClick(object? sender, RoutedEventArgs e)
  {
    // TogglePause();
  }


  private void OnExitClick(object? sender, RoutedEventArgs e)
  {
    _timer.Stop();

    _gameOverDialog?.Close();
    _gameOverDialog = null;

    Close();
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

}