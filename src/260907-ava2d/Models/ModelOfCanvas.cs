namespace _260907_ava2d.Models;

public readonly struct ModelOfCanvas(
    int width,
    int height,
    string background = "#f5f5e0")
{
  public int Width { get; } = width;
  public int Height { get; } = height;
  public string Background { get; } = background;
}
