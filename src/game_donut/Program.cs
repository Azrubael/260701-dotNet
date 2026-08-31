namespace game_donut;

using System.Threading;
using System.Text;
using static System.Console;
using System.Security.Claims;

class Program
{

  static void Main()
  {
    int width = WindowWidth;
    int height = WindowHeight;

    WriteLine($"Console window width:  {width} columns");
    WriteLine($"Console window height: {height} rows");

    PressKey();
    while (true)
    {
      for (int m = 0; m < width; m++)
      {
        double t = Math.Sin(m * 0.1);
        DrawScreen(width, height, t);
      }
      PressKey();
    }
    ;
  }


  static void DrawScreen(int width, int height, double t)
  {
    var frame = new StringBuilder(width * height + height);
    double aspect = (double)width / height / 2;
    char[] gradient = " .:!/r(l1Z4H9W8#%$@".ToCharArray();
    int gradientSize = gradient.Length - 1;

    for (int row = 0; row < height; row++)
    {
      for (int col = 0; col < width; col++)
      {
        double x = ((double)col / width * 2.0 - 1.0) * aspect + t;
        double y = (double)row / height * 2.0 - 1.0;
        double dist = Math.Sqrt(x * x + y * y);
        int color = Clamp(1.37 / dist, 0, gradientSize);
        frame.Append(gradient[color]);
      }
      if (row < height - 1)
        frame.AppendLine();
    }

    SetCursorPosition(0, 0);
    Write(frame.ToString());
    Thread.Sleep(15);
  }


  static void PressKey()
  {
    WriteLine("Press any key to continue or 'q' to exit...");
    if (char.ToLower(ReadKey(true).KeyChar) == 'q')
      Environment.Exit(1); ;
    Clear();
  }


  static int Clamp(double value, double min, double max)
  {
    return (int)Math.Max(Math.Min(value, max), min);
  }
}