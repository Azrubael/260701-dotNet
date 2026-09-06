```csharp
using Avalonia;
using Avalonia.Controls.Shapes;
using System;
using System.Collections.Generic;


namespace _260907_ava2d.Models;

public class ModelOfSnake
{
  public const double SegmentSize = 15;
  public const double SnakeSpeed = 90;
  public readonly List<Polyline> _snakePaths = [];

  private Point GetHistoryPoint(double distance)
  {
    if (_history.Count == 0)
      return _headPosition;

    if (distance <= 0)
      return _history[0];

    double travelled = 0;

    for (int i = 1; i < _history.Count; i++)
    {
      Point newer = _history[i - 1];
      Point older = _history[i];

      double dx = older.X - newer.X;
      double dy = older.Y - newer.Y;
      double segmentLength = Math.Sqrt(dx * dx + dy * dy);

      if (travelled + segmentLength >= distance)
      {
        double remaining = distance - travelled;
        double ratio = segmentLength == 0
            ? 0
            : remaining / segmentLength;

        return new Point(
            newer.X + dx * ratio,
            newer.Y + dy * ratio);
      }

      travelled += segmentLength;
    }

    return _history[^1];
  }


  private void UpdateSnake()
  {
    double canvasWidth = ModelOfCanvas.Width;
    double canvasHeight = ModelOfCanvas.Height;

    if (canvasWidth <= 0 || canvasHeight <= 0)
      return;

    var points = new Points();

    for (int segmentIndex = 0; segmentIndex < 5; segmentIndex++)
    {
      Point position = GetHistoryPoint(segmentIndex * SegmentSize);

      points.Add(new Point(
          position.X + SegmentSize / 2,
          position.Y + SegmentSize / 2));
    }

    // Move the main copy back into the visible canvas.
    double baseOffsetX =
        -Math.Floor(points[0].X / canvasWidth) * canvasWidth;

    double baseOffsetY =
        -Math.Floor(points[0].Y / canvasHeight) * canvasHeight;

    int pathIndex = 0;

    for (int y = -1; y <= 1; y++)
    {
      for (int x = -1; x <= 1; x++)
      {
        Points translatedPoints = [];

        foreach (Point point in points)
        {
          translatedPoints.Add(new Point(
              point.X + baseOffsetX + x * canvasWidth,
              point.Y + baseOffsetY + y * canvasHeight));
        }

        Polyline path = _snakePaths[pathIndex++];
        path.Points = translatedPoints;
        path.IsVisible = true;
      }
    }
  }

  public static bool IsSelfCollision(
    IReadOnlyList<Point> history,
    Point headPosition,
    double segmentSize,
    int bodySegments)
  {
    // Segment 1 is the neck and is connected to the head.
    for (int segmentIndex = 2;
         segmentIndex < bodySegments;
         segmentIndex++)
    {
      Point bodyPoint = GetPointAtDistance(
          history,
          segmentIndex * segmentSize);

      double dx = WrappedDifference(
          headPosition.X,
          bodyPoint.X,
          ModelOfCanvas.Width);

      double dy = WrappedDifference(
          headPosition.Y,
          bodyPoint.Y,
          ModelOfCanvas.Height);

      // The head touches a body element.
      if (dx * dx + dy * dy <= segmentSize * segmentSize)
        return true;
    }

    return false;
  }


  private static Point GetPointAtDistance(
      IReadOnlyList<Point> history,
      double distance)
  {
    if (history.Count == 0 || distance <= 0)
      return history.Count == 0 ? new Point() : history[0];

    double travelled = 0;

    for (int i = 1; i < history.Count; i++)
    {
      Point newer = history[i - 1];
      Point older = history[i];

      double dx = older.X - newer.X;
      double dy = older.Y - newer.Y;
      double length = Math.Sqrt(dx * dx + dy * dy);

      if (travelled + length >= distance)
      {
        double ratio = length == 0
            ? 0
            : (distance - travelled) / length;

        return new Point(
            newer.X + dx * ratio,
            newer.Y + dy * ratio);
      }

      travelled += length;
    }

    return history[^1];
  }


  private static double WrappedDifference(
      double first,
      double second,
      double size)
  {
    double difference = (first - second) % size;

    if (difference > size / 2)
      difference -= size;
    else if (difference < -size / 2)
      difference += size;

    return difference;
  }
}

```