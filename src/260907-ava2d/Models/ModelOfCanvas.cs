using Avalonia;
using System;
using System.Collections.Generic;

namespace _260907_ava2d.Models;

public class ModelOfCanvas
{
  public static int Width => 600;
  public static int Height => 480;
  public string Background { get; init; } = "darkgray";

  public static bool IsSelfCollision(
      IReadOnlyList<Point> history,
      Point headPosition,
      double segmentSize,
      int bodySegments)
  {

    if (history.Count == 0 || bodySegments <= 2)
      return false;

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
          Width);

      double dy = WrappedDifference(
          headPosition.Y,
          bodyPoint.Y,
          Height);

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
