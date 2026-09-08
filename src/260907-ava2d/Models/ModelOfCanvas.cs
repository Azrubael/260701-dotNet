namespace _260907_ava2d.Models;

public readonly struct ModelOfCanvas(
    int width,
    int height,
    string background = "#ffffe9")
{
  public int Width { get; } = width;
  public int Height { get; } = height;
  public string Background { get; } = background;
}
