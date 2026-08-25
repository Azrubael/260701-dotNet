using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using _260820_ava3d.Models;

namespace _260820_ava3d.Views;

public partial class MainWindow : Window
{
  private string? inputFile;

  public MainWindow()
  {
    InitializeComponent();

  }


  // Handles the 'Open' menu item
  private async void OnOpenClick(object? sender, RoutedEventArgs e)
  {
    var files = await StorageProvider.OpenFilePickerAsync(
      new FilePickerOpenOptions
      {
        Title = "Select input PDF",
        AllowMultiple = false,
        FileTypeFilter =
        [
          new FilePickerFileType("PDF files")
          {
            Patterns = ["*.pdf"]
          }
        ]
      });

    if (files.Count > 0)
    {
      inputFile = files[0].Path.LocalPath;
      Console.WriteLine($"Selected file: {inputFile}");
    }
  }


  // Handles the 'Exit' menu item
  private void OnSaveprcClick(object? sender, RoutedEventArgs e)
  {
    if (string.IsNullOrWhiteSpace(inputFile))
    {
      Console.Error.WriteLine("Please select an input PDF first.");
      return;
    }

    try
    {
      string outputFile = System.IO.Path.Combine(
        System.IO.Path.GetDirectoryName(inputFile)!,
        "model.prc");

      Console.WriteLine(
        ExtractPrc.ExtractFile(inputFile, outputFile)
          ? $"See the file {outputFile} with result."
          : $"There is an error of creating {outputFile}."
          );
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine($"Error: {ex.Message}");
    }

  }


  // Handles the 'Exit' menu item
  private void OnExitClick(object? sender, RoutedEventArgs e)
  {
    Close();
  }

}