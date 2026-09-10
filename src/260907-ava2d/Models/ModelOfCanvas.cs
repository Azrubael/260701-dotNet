using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;


namespace _260907_ava2d.Models;

public sealed class ModelOfCanvas(
  int width,
  int height,
  string background = "darkgray")
{
  public int Width { get; init; } = width;
  public int Height { get; init; } = height;
  public string Background { get; init; } = background;

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

  private readonly Random _random = new();


  public void ShowRandomIcon(Image iconImage)
  {
    if (_iconUris.Length == 0)
      return;

    string uri = _iconUris[_random.Next(_iconUris.Length)];

    try
    {
      Stream stream = AssetLoader.Open(new Uri(uri));
      iconImage.Source = new Bitmap(stream);
    }
    catch (Exception exception)
    {
      Console.WriteLine($"Unable to load icon '{uri}': {exception}");
      iconImage.IsVisible = false;
      return;
    }

    if (!double.IsFinite(Width) ||
        !double.IsFinite(Height) ||
        Width <= 0 ||
        Height <= 0)
    {
      iconImage.IsVisible = false;
      return;
    }

    double iconWidth = iconImage.Bounds.Width;
    double iconHeight = iconImage.Bounds.Height;

    if (!double.IsFinite(iconWidth) || iconWidth <= 0)
      iconWidth = 30;

    if (!double.IsFinite(iconHeight) || iconHeight <= 0)
      iconHeight = 30;

    double availableWidth = Width - iconWidth;
    double availableHeight = Height - iconHeight;

    if (availableWidth <= 0 || availableHeight <= 0)
    {
      iconImage.IsVisible = false;
      return;
    }

    double left = _random.NextDouble() * availableWidth;
    double top = _random.NextDouble() * availableHeight;

    Canvas.SetLeft(iconImage, left);
    Canvas.SetTop(iconImage, top);
    iconImage.IsVisible = true;

  }
}