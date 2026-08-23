using Avalonia.Controls;
using Avalonia.Interactivity;

namespace avalonia1_tutorial.Views;

public partial class MainWindow : Window
{
  public MainWindow()
  {
    InitializeComponent();
  }

  private void Celsius_TextChanged(object? sender, RoutedEventArgs e)
  {
    if (string.IsNullOrEmpty(Celsius.Text) || Celsius.Text == "-")
    {
      Fahrenheit.Text = "";
    }
    else if (double.TryParse(Celsius.Text, out double C))
    {
      var F = C * (9d / 5d) + 32;
      Fahrenheit.Text = F.ToString("0.0");
    }
    else
    {
      Celsius.Text = "0";
      Fahrenheit.Text = "0";
    }
  }

  private void Button_OnClick(object? sender, RoutedEventArgs e)
  {
    if (double.TryParse(Celsius.Text, out double C))
    {
      var F = C * (9d / 5d) + 32;
      Fahrenheit.Text = F.ToString("0.0");
    }
    else
    {
      Celsius.Text = "0";
      Fahrenheit.Text = "0";
    }
  }
}