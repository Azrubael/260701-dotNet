namespace game2_donut;

using System;
using System.Text;
using System.Threading;
using static System.Console;

class Program
{
  struct Coordinate3D(double x, double y, double z)
  {
    public double X = x, Y = y, Z = z;

    public readonly double Magnitude() => Math.Sqrt(X * X + Y * Y + Z * Z);

    public readonly Coordinate3D Normalize()
    {
      double mag = Magnitude();
      return new Coordinate3D(X / mag, Y / mag, Z / mag);
    }

    public readonly double DotProduct(Coordinate3D other) =>
        X * other.X + Y * other.Y + Z * other.Z;

    public static Coordinate3D operator +(Coordinate3D a, Coordinate3D b) =>
        new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Coordinate3D operator -(Coordinate3D a, Coordinate3D b) =>
        new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Coordinate3D operator *(Coordinate3D v, double scalar) =>
        new(v.X * scalar, v.Y * scalar, v.Z * scalar);
  }


  static void Main()
  {
    int width = WindowWidth;
    int height = WindowHeight;

    char[] gradient = " .:!/r(l1Z4H9W8#%$@".ToCharArray();

    WriteLine($"Console window width:  {width} columns");
    WriteLine($"Console window height: {height} rows\n");
    WriteLine("""
        Press Q to quit.
        Press S to draw a 3D sphere.
        Press T to draw a torus.
        Press any key to start.
    """);

    CursorVisible = false;
    bool quit = false;
    bool sphereMode = false;
    double lightAngle = 0.0;

    try
    {
      ConsoleKey key = ReadKey(true).Key;

      do
      {
        while (KeyAvailable)
        {
          key = ReadKey(true).Key;

          (quit, sphereMode) = key switch
          {
            ConsoleKey.Q => (true, sphereMode),
            ConsoleKey.S => (false, true),
            ConsoleKey.T => (false, false),
            _ => (quit, sphereMode)
          };

          if (quit)
            break;
        }


        if (sphereMode)
        {
          DrawSphere(gradient, width, height, lightAngle);
        }
        else
        {
          DrawTorus(gradient, width, height, lightAngle);
        }

        lightAngle += 0.06;
      } while (!quit);
    }
    finally
    {
      CursorVisible = true;
      Clear();
      WriteLine("Program closed.");
    }
  }


  static void DrawTorus(
      char[] gradient,
      int width,
      int height,
      double lightAngle)
  {
    int[,] brightness = new int[height, width];
    double[,] depthBuffer = new double[height, width];

    for (int row = 0; row < height; row++)
    {
      for (int col = 0; col < width; col++)
      {
        brightness[row, col] = 0;
        depthBuffer[row, col] = double.NegativeInfinity;
      }
    }

    double R = 0.9;
    double r = 0.37;

    Coordinate3D lightPos = new(
        2.0 * Math.Cos(lightAngle),
        1.4 * Math.Sin(lightAngle * 0.8),
        2.0 * Math.Sin(lightAngle)
    );

    int majorSteps = 160;
    int minorSteps = 48;

    double rotationX = 0.65;
    double rotationZ = -0.25;

    double cosX = Math.Cos(rotationX);
    double sinX = Math.Sin(rotationX);
    double cosZ = Math.Cos(rotationZ);
    double sinZ = Math.Sin(rotationZ);

    double scaleX = width * 0.34;
    double scaleY = height * 0.42;

    for (int i = 0; i < majorSteps; i++)
    {
      double u = 2.0 * Math.PI * i / majorSteps;

      for (int j = 0; j < minorSteps; j++)
      {
        double v = 2.0 * Math.PI * j / minorSteps;

        double tube = R + r * Math.Cos(v);

        Coordinate3D point = new(
            tube * Math.Cos(u),
            tube * Math.Sin(u),
            r * Math.Sin(v)
        );

        Coordinate3D normal = new(
            Math.Cos(v) * Math.Cos(u),
            Math.Cos(v) * Math.Sin(u),
            Math.Sin(v)
        );

        // Rotate around X axis
        double ry = point.Y * cosX - point.Z * sinX;
        double rz = point.Y * sinX + point.Z * cosX;
        point = new(point.X, ry, rz);

        double nry = normal.Y * cosX - normal.Z * sinX;
        double nrz = normal.Y * sinX + normal.Z * cosX;
        normal = new(normal.X, nry, nrz);

        // Rotate around Z axis
        double rx = point.X * cosZ - point.Y * sinZ;
        ry = point.X * sinZ + point.Y * cosZ;
        point = new(rx, ry, point.Z);

        rx = normal.X * cosZ - normal.Y * sinZ;
        ry = normal.X * sinZ + normal.Y * cosZ;
        normal = new(rx, ry, normal.Z);

        int screenX = (int)(width / 2.0 + point.X * scaleX);
        int screenY = (int)(height / 2.0 - point.Y * scaleY);

        if (screenX < 0 || screenX >= width || screenY < 0 || screenY >= height)
          continue;

        Coordinate3D toLight = lightPos - point;
        double lightDist = toLight.Magnitude();
        toLight = toLight.Normalize();

        double diffuse = Math.Max(0.0, normal.DotProduct(toLight));
        double light = 0.10 + diffuse * 0.90;

        double falloff = 1.0 / (1.0 + lightDist * 0.12);
        light *= falloff;

        int gradientIndex = Clamp(
            light * (gradient.Length - 1),
            0,
            gradient.Length - 1
        );

        if (point.Z > depthBuffer[screenY, screenX])
        {
          depthBuffer[screenY, screenX] = point.Z;
          brightness[screenY, screenX] = gradientIndex;
        }
      }
    }

    var frame = new StringBuilder(width * height + height);

    for (int row = 0; row < height; row++)
    {
      for (int col = 0; col < width; col++)
      {
        frame.Append(gradient[brightness[row, col]]);
      }

      if (row < height - 1)
        frame.AppendLine();
    }

    SetCursorPosition(0, 0);
    Write(frame.ToString());

    Thread.Sleep(30);
  }


  static void DrawSphere(
      char[] gradient,
      int width,
      int height,
      double lightAngle)
  {
    var frame = new StringBuilder(width * height + height);

    double cameraZ = -3.0;
    double sphereRadius = 1.0;
    double aspect = (double)width / height / 2.0;

    Coordinate3D lightPos = new(
        2.0 * Math.Cos(lightAngle),
        1.5 * Math.Sin(lightAngle * 0.8),
        -2.0 + Math.Sin(lightAngle)
    );

    for (int row = 0; row < height; row++)
    {
      for (int col = 0; col < width; col++)
      {
        double screenX = ((double)col / (width - 1) * 2.0 - 1.0) * aspect;
        double screenY = 1.0 - (double)row / (height - 1) * 2.0;

        Coordinate3D rayOrigin = new(0.0, 0.0, cameraZ);
        Coordinate3D rayDir = new(screenX, screenY, -cameraZ);
        rayDir = rayDir.Normalize();

        double b = 2.0 * (rayOrigin.X * rayDir.X + rayOrigin.Y * rayDir.Y + rayOrigin.Z * rayDir.Z);
        double c = rayOrigin.DotProduct(rayOrigin) - sphereRadius * sphereRadius;
        double discriminant = b * b - 4.0 * c;

        if (discriminant < 0.0)
        {
          frame.Append(' ');
          continue;
        }

        double distance = (-b - Math.Sqrt(discriminant)) / 2.0;
        if (distance < 0.0)
          distance = (-b + Math.Sqrt(discriminant)) / 2.0;

        Coordinate3D hitPoint = rayOrigin + rayDir * distance;
        Coordinate3D normal = new(hitPoint.X / sphereRadius, hitPoint.Y / sphereRadius, hitPoint.Z / sphereRadius);

        Coordinate3D toLight = (lightPos - hitPoint).Normalize();
        double diffuse = Math.Max(0.0, normal.DotProduct(toLight));
        double brightness = 0.12 + diffuse * 0.88;

        Coordinate3D toView = (rayOrigin - hitPoint).Normalize();
        Coordinate3D reflection = normal * (2.0 * diffuse) - toLight;
        double specular = Math.Max(0.0, reflection.DotProduct(toView));
        brightness += Math.Pow(specular, 24.0) * 0.35;

        int gradientIndex = Clamp(brightness * (gradient.Length - 1), 0, gradient.Length - 1);
        frame.Append(gradient[gradientIndex]);
      }

      if (row < height - 1)
        frame.AppendLine();
    }

    SetCursorPosition(0, 0);
    Write(frame.ToString());

    Thread.Sleep(30);
  }


  static int Clamp(double value, double min, double max)
  {
    return (int)Math.Max(
        min,
        Math.Min(value, max)
    );
  }

}
