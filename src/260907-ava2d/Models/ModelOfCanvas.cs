namespace _260907_ava2d.Models;

public readonly struct ModelOfCanvas(
    int width,
    int height,
    string background = "darkgray")
{
  public int Width { get; } = width;
  public int Height { get; } = height;
  public string Background { get; } = background;
}
