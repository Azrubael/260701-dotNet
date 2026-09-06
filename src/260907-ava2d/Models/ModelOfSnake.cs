using Avalonia;
using Avalonia.Controls.Shapes;
using System;
using System.Collections.Generic;


namespace _260907_ava2d.Models;

public class ModelOfSnake
{
  public const int BodySegments = 5;
  public const double SegmentSize = 15;
  public const double SnakeSpeed = 90;
  public readonly List<Point> History = [];
  public readonly List<Polyline> SnakePaths = [];
  public Point HeadPosition;
  public Vector Direction = new(1, 0);


  public Point GetHistoryPoint(
      double distance)
  {
    if (History.Count == 0)
      return HeadPosition;

    if (distance <= 0)
      return History[0];

    double travelled = 0;

    for (int i = 1; i < History.Count; i++)
    {
      Point newer = History[i - 1];
      Point older = History[i];

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

    return History[^1];
  }


  public void UpdateSnake(ModelOfCanvas thisCanvas)
  {
    double canvasWidth = thisCanvas.Width;
    double canvasHeight = thisCanvas.Height;

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

        Polyline path = SnakePaths[pathIndex++];
        path.Points = translatedPoints;
        path.IsVisible = true;
      }
    }
  }

  public bool IsSelfCollision(ModelOfCanvas thisCanvas)
  {
    // Segment 1 is the neck and is connected to the head.
    for (int segmentIndex = 2;
         segmentIndex < BodySegments;
         segmentIndex++)
    {
      Point bodyPoint = GetPointAtDistance(segmentIndex * SegmentSize);

      double dx = WrappedDifference(
          HeadPosition.X,
          bodyPoint.X,
          thisCanvas.Width);

      double dy = WrappedDifference(
          HeadPosition.Y,
          bodyPoint.Y,
          thisCanvas.Height);

      // The head touches a body element.
      if (dx * dx + dy * dy <= SegmentSize * SegmentSize)
        return true;
    }

    return false;
  }


  private Point GetPointAtDistance(double distance)
  {
    if (History.Count == 0 || distance <= 0)
      return History.Count == 0 ? new Point() : History[0];

    double travelled = 0;

    for (int i = 1; i < History.Count; i++)
    {
      Point newer = History[i - 1];
      Point older = History[i];

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

    return History[^1];
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



  public void RotateLeft()
  {
    Direction = new Vector(Direction.Y, -Direction.X);
  }


  public void RotateRight()
  {
    Direction = new Vector(-Direction.Y, Direction.X);
  }

}
