using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace avalonia1_tutorial;

public partial class MainWindow : Window
{
  public MainWindow()
  {
    InitializeComponent();
  }

  private void Button_OnClick(object? sender, RoutedEventArgs e)
  {
    Debug.WriteLine("Click!");
  }
}