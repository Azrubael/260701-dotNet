namespace game2_donut;

using System;
using System.Text;
using System.Threading;
using static System.Console;

class Program
{
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
          DrawSphere(
              gradient,
              width,
              height,
              lightAngle
          );
        }
        else
        {
          DrawTorus(
              gradient,
              width,
              height,
              lightAngle
          );
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

    /*
     * Donut dimensions:
     *
     * R = distance from the center of the donut
     * r = radius of the donut tube
     */
    double R = 0.9;
    double r = 0.37;

    // The light travels around the donut.
    double lightX = 2.0 * Math.Cos(lightAngle);
    double lightY = 1.4 * Math.Sin(lightAngle * 0.8);
    double lightZ = 2.0 * Math.Sin(lightAngle);

    // Number of points used to construct the donut.
    int majorSteps = 160;
    int minorSteps = 48;

    // Fixed rotation of the donut so that its hole is visible.
    double rotationX = 0.65;
    double rotationZ = -0.25;

    double cosX = Math.Cos(rotationX);
    double sinX = Math.Sin(rotationX);
    double cosZ = Math.Cos(rotationZ);
    double sinZ = Math.Sin(rotationZ);

    // Scale values map 3D coordinates to console coordinates.
    double scaleX = width * 0.34;
    double scaleY = height * 0.42;

    for (int i = 0; i < majorSteps; i++)
    {
      double u = 2.0 * Math.PI * i / majorSteps;

      for (int j = 0; j < minorSteps; j++)
      {
        double v = 2.0 * Math.PI * j / minorSteps;

        // Point on the torus before rotation.
        double tube = R + r * Math.Cos(v);

        double x = tube * Math.Cos(u);
        double y = tube * Math.Sin(u);
        double z = r * Math.Sin(v);

        // Normal of the torus at this point.
        double nx = Math.Cos(v) * Math.Cos(u);
        double ny = Math.Cos(v) * Math.Sin(u);
        double nz = Math.Sin(v);

        // Rotate point around the X axis.
        double rotatedY = y * cosX - z * sinX;
        double rotatedZ = y * sinX + z * cosX;

        y = rotatedY;
        z = rotatedZ;

        // Rotate point around the Z axis.
        double rotatedX = x * cosZ - y * sinZ;
        rotatedY = x * sinZ + y * cosZ;

        x = rotatedX;
        y = rotatedY;

        // Rotate the normal in exactly the same way.
        rotatedY = ny * cosX - nz * sinX;
        double normalZ = ny * sinX + nz * cosX;

        ny = rotatedY;
        nz = normalZ;

        rotatedX = nx * cosZ - ny * sinZ;
        rotatedY = nx * sinZ + ny * cosZ;

        nx = rotatedX;
        ny = rotatedY;

        /*
         * Convert the 3D point to a console position.
         * Console characters are taller than they are wide,
         * so X and Y use different scale values.
         */
        int screenX = (int)(width / 2.0 + x * scaleX);
        int screenY = (int)(height / 2.0 - y * scaleY);

        if (screenX < 0 ||
            screenX >= width ||
            screenY < 0 ||
            screenY >= height)
        {
          continue;
        }

        // Direction from the surface point to the light.
        double lightDirectionX = lightX - x;
        double lightDirectionY = lightY - y;
        double lightDirectionZ = lightZ - z;

        double lightDistance = Distance3D(
            lightDirectionX,
            lightDirectionY,
            lightDirectionZ
        );

        lightDirectionX /= lightDistance;
        lightDirectionY /= lightDistance;
        lightDirectionZ /= lightDistance;

        /*
         * Lambertian diffuse lighting:
         *
         * The surface is brighter when its normal points
         * toward the moving light source.
         */
        double diffuse =
            nx * lightDirectionX +
            ny * lightDirectionY +
            nz * lightDirectionZ;

        diffuse = Math.Max(0.0, diffuse);

        // A small amount of ambient light prevents the dark
        // side of the donut from disappearing completely.
        double light = 0.10 + diffuse * 0.90;

        // Add a mild distance falloff for a more realistic
        // moving point light.
        double falloff = 1.0 / (1.0 + lightDistance * 0.12);
        light *= falloff;

        // Convert brightness to an index in the ASCII gradient.
        int gradientIndex = Clamp(
            light * (gradient.Length - 1),
            0,
            gradient.Length - 1
        );

        // Depth buffering ensures that the front surface
        // hides the back surface.
        if (z > depthBuffer[screenY, screenX])
        {
          depthBuffer[screenY, screenX] = z;
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

    // The camera looks toward positive Z.
    double cameraZ = -3.0;
    double sphereRadius = 1.0;

    // Console characters are taller than they are wide.
    // This aspect correction keeps the sphere circular.
    double aspect = (double)width / height / 2.0;

    /*
     * Moving point-light position.
     * The negative Z value keeps most of the light on the
     * visible side of the sphere.
     */
    double lightX = 2.0 * Math.Cos(lightAngle);
    double lightY = 1.5 * Math.Sin(lightAngle * 0.8);
    double lightZ = -2.0 + Math.Sin(lightAngle);

    for (int row = 0; row < height; row++)
    {
      for (int col = 0; col < width; col++)
      {
        // Position on the projection plane at z = 0.
        double screenX =
            ((double)col / (width - 1) * 2.0 - 1.0) * aspect;

        double screenY =
            1.0 - (double)row / (height - 1) * 2.0;

        // Ray starts at the camera and passes through
        // the current screen position.
        double originX = 0.0;
        double originY = 0.0;
        double originZ = cameraZ;

        double directionX = screenX;
        double directionY = screenY;
        double directionZ = -cameraZ;

        double directionLength = Distance3D(
            directionX,
            directionY,
            directionZ
        );

        directionX /= directionLength;
        directionY /= directionLength;
        directionZ /= directionLength;

        // Ray-sphere intersection,
        // if the sphere is centered at (0, 0, 0).
        double b =
            2.0 * (
                originX * directionX +
                originY * directionY +
                originZ * directionZ
            );

        double c =
            originX * originX +
            originY * originY +
            originZ * originZ -
            sphereRadius * sphereRadius;

        double discriminant = b * b - 4.0 * c;

        // This ray does not hit the sphere.
        if (discriminant < 0.0)
        {
          frame.Append(' ');
          continue;
        }

        // Use the closest intersection point.
        double distance =
            (-b - Math.Sqrt(discriminant)) / 2.0;

        if (distance < 0.0)
        {
          distance =
              (-b + Math.Sqrt(discriminant)) / 2.0;
        }

        // Calculate the point where the ray hits the sphere.
        double hitX = originX + directionX * distance;
        double hitY = originY + directionY * distance;
        double hitZ = originZ + directionZ * distance;

        // Sphere normal.
        double normalX = hitX / sphereRadius;
        double normalY = hitY / sphereRadius;
        double normalZ = hitZ / sphereRadius;

        // Direction from the surface point to the light.
        double lightDirectionX = lightX - hitX;
        double lightDirectionY = lightY - hitY;
        double lightDirectionZ = lightZ - hitZ;

        double lightDistance =Distance3D(
            lightDirectionX,
            lightDirectionY,
            lightDirectionZ
        );

        lightDirectionX /= lightDistance;
        lightDirectionY /= lightDistance;
        lightDirectionZ /= lightDistance;

        // Diffuse lighting.
        double diffuse =
            normalX * lightDirectionX +
            normalY * lightDirectionY +
            normalZ * lightDirectionZ;

        diffuse = Math.Max(0.0, diffuse);

        // Ambient light keeps the dark side visible.
        double brightness = 0.12 + diffuse * 0.88;

        // Optional specular highlight.
        double viewDirectionX = -hitX;
        double viewDirectionY = -hitY;
        double viewDirectionZ = cameraZ - hitZ;

        double viewLength = Distance3D(
            viewDirectionX,
            viewDirectionY,
            viewDirectionZ
        );

        viewDirectionX /= viewLength;
        viewDirectionY /= viewLength;
        viewDirectionZ /= viewLength;

        // Reflection of the light direction around the normal.
        double reflectionX =
            2.0 * diffuse * normalX - lightDirectionX;

        double reflectionY =
            2.0 * diffuse * normalY - lightDirectionY;

        double reflectionZ =
            2.0 * diffuse * normalZ - lightDirectionZ;

        double specular =
            reflectionX * viewDirectionX +
            reflectionY * viewDirectionY +
            reflectionZ * viewDirectionZ;

        specular = Math.Max(0.0, specular);
        brightness += Math.Pow(specular, 24.0) * 0.35;

        int gradientIndex = Clamp(
            brightness * (gradient.Length - 1),
            0,
            gradient.Length - 1
        );

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


  static double Distance3D(double x, double y, double z) =>
      Math.Sqrt( x * x + y * y + z * z );
}
