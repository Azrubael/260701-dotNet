namespace _260901_ava2d.Models;

public sealed class ModelOfCanvas
{
  public static int Width => 600;
  public static int Height => 480;
  public string Background { get; init; } = "darkgray";
  public bool ShowStartMessage { get; set; } = true;
  public bool ShowSnake { get; set; }
  public bool ShowIcon { get; set; }
  public int[,] Canvas { get; } = new int[Width, Height];
  public (int, int) IconPosition { get; set; } = new(0, 0);

  public ModelOfCanvas()
  {
    for(int x = 0; x < Width; x++)
      for(int y = 0; y < Height; y++)
        Canvas[x,y] = 0;
  }
}
