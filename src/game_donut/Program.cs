namespace game_donut;

using System.Threading;
using System.Text;
using static System.Console;

class Program
{

  static void Main()
  {
    int width = WindowWidth;
    int height = WindowHeight - 2;

    WriteLine($"Console window width:  {width} columns");
    WriteLine($"Console window height: {height} rows");

    char[,] outputConsole = new char[width, height];
    for (int row = 0; row < height; row++)
      for (int col = 0; col < width; col++)
        outputConsole[col, row] = ' ';

    PressKey();
    while (true)
    {
      for (int m = 0; m < width; m++)
      {
        double t = Math.Sin(m * 0.1);
        DrawScreen(outputConsole, width, height, t);
      }
      PressKey();
    }
    ;
  }


  static void DrawScreen(char[,] outputConsole, int width, int height, double t)
  {
    var frame = new StringBuilder(width * height + height);
    double aspect = (double)width / height / 2;
    char[] gradient = " .:!=a#%$@".ToCharArray();
    int gradientSize = gradient.Length - 2;

    for (int row = 0; row < height; row++)
    {
      for (int col = 0; col < width; col++)
      {
        double x = ((double)col / width * 2.0 - 1.0) * aspect + t;
        double y = (double)row / height * 2.0 - 1.0;
        double dist = Math.Sqrt(x * x + y * y);
        int color = (int)(1.0 / dist);
        if (color < 0) color = 0;
        else if (color > gradientSize) color = gradientSize;
        frame.Append(
            (x * x + y * y > 0.5)
            ? outputConsole[col, row]
            : gradient[color]
            );
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
}