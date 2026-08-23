using Avalonia.Controls;
using Avalonia.Interactivity;

namespace _260820_ava3d.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

    }

    // Handles the 'Open' menu item
    private void OnOpenClick(object? sender, RoutedEventArgs e)
    {
        // Logic to trigger FileOpenDialog goes here
    }

    // Handles the 'Exit' menu item
    private void OnExitClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

}