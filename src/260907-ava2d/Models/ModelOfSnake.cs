using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;


namespace _260907_ava2d.Models;

public class ModelOfSnake
{
  public const double SegmentSize = 16;
  public const int StartSegmentWidth = 10;
  public IBrush SkinColor { get; } = Brush.Parse("#590992");
  public IBrush EyeColor { get; } = Brush.Parse("#fffb00");
  public int BodyWidth { get; private set; } = StartSegmentWidth;
  public int HeadWidth { get; private set; }
  public int HeadLength { get; private set; }
  public int EyeSizeX { get; private set; } = 4;
  public int EyeSizeY { get; private set; } = 4;
  public double SnakeSpeed { get; private set; } = 1;
  public int BodySegments { get; private set; } = 4;
  public readonly List<Point> History = [];
  public readonly List<Polyline> SnakePaths = [];
  public Point HeadPosition { get; private set; }
  public Point HeadRenderPosition { get; private set; }
  public double HeadRotation { get; private set; }
  public Vector Direction { get; private set; } = new(1, 0);

  public int RequiedHistory { get; private set; }


  public void CreateSnake(
      ModelOfCanvas canvas,
      Ellipse LeftEye,
      Ellipse RightEye)
  {
    History.Clear();

    SetHeadSizes();

    HeadPosition = new Point(
        canvas.Width / 2 - SegmentSize / 2,
        canvas.Height / 2 - SegmentSize / 2);
    Direction = new Vector(1, 0);
    History.Add(HeadPosition);

    for (int bs = 1; bs < BodySegments; bs++)
    {
      History.Add(new Point(
          HeadPosition.X - bs * SegmentSize,
          HeadPosition.Y));
    }

    UpdateSnakeHead(canvas, LeftEye, RightEye);
  }


  public void Move()
  {
    if (History.Count == 0)
      History.Add(HeadPosition);

    // Direction is a unit vector, so this moves by SnakeSpeed pixels.
    HeadPosition += Direction * SnakeSpeed;
    History.Insert(0, HeadPosition);

    double requiredDistance =
        Math.Max(0, BodySegments - 1) * SegmentSize;

    double totalDistance = 0;

    for (int i = 1; i < History.Count; i++)
      totalDistance += Distance(History[i - 1], History[i]);

    // Keep the oldest point needed for interpolation.
    while (History.Count > 2)
    {
      double lastSegmentDistance =
          Distance(History[^2], History[^1]);

      if (totalDistance - lastSegmentDistance < requiredDistance)
        break;

      totalDistance -= lastSegmentDistance;
      History.RemoveAt(History.Count - 1);
    }

  }


  public void UpdateSnake(
      ModelOfCanvas canvas,
      Ellipse LeftEye,
      Ellipse RightEye)
  {
    if (canvas.Width <= 30 ||
        canvas.Height <= 30 ||
        BodySegments <= 0 ||
        History.Count == 0 ||
        SnakePaths.Count < 9)
    {
      return;
    }

    AddBodyWidth();
    foreach (Polyline path in SnakePaths)
    {
      path.StrokeThickness = BodyWidth;
      path.IsVisible = true;
    }

    var points = new Points();

    for (int bs = 0; bs < BodySegments; bs++)
    {
      Point position = GetHistoryPoint(bs * SegmentSize);

      points.Add(new Point(
          position.X + SegmentSize / 2,
          position.Y + SegmentSize / 2));
    }

    // Move the main copy back into the visible canvas.
    double baseOffsetX =
        -Math.Floor(points[0].X / canvas.Width) * canvas.Width;

    double baseOffsetY =
        -Math.Floor(points[0].Y / canvas.Height) * canvas.Height;

    int pathIndex = 0;

    for (int y = -1; y <= 1; y++)
    {
      for (int x = -1; x <= 1; x++)
      {
        Points translatedPoints = [];

        foreach (Point point in points)
        {
          translatedPoints.Add(new Point(
              point.X + baseOffsetX + x * canvas.Width,
              point.Y + baseOffsetY + y * canvas.Height));
        }

        Polyline path = SnakePaths[pathIndex++];
        path.Points = translatedPoints;
        path.IsVisible = true;
      }
    }

    UpdateSnakeHead(canvas, LeftEye, RightEye);
  }


  public void UpdateSnakeHead(
      ModelOfCanvas canvas,
      Ellipse LeftEye,
      Ellipse RightEye)
  {
    static double Wrap(double value, double size)
    {
      double result = value % size;
      return result < 0 ? result + size : result;
    }

    // Use the same point used by the first body segment.
    Point firstSegment = GetHistoryPoint(0);

    Point bodyCenter = new(
        Wrap(firstSegment.X, canvas.Width) + SegmentSize / 2,
        Wrap(firstSegment.Y, canvas.Height) + SegmentSize / 2);

    // Move the rear of the head slightly into the body.
    Point headCenter = bodyCenter +
                       Direction * (HeadLength / 2);

    HeadRenderPosition = new Point(
        headCenter.X - HeadLength / 2,
        headCenter.Y - HeadWidth / 2);

    HeadRotation = Math.Atan2(Direction.Y, Direction.X) * 180 / Math.PI;

    Vector side = new(-Direction.Y, Direction.X);
    Point eyeBase = headCenter + Direction * (HeadLength * 0.25);

    PositionEye(LeftEye, eyeBase + side * (HeadWidth * 0.25));
    PositionEye(RightEye, eyeBase - side * (HeadWidth * 0.25));
  }


  private static void PositionEye(Ellipse? eye, Point center)
  {
    if (eye is null)
      return;

    Canvas.SetLeft(eye, center.X - eye.Width / 2);
    Canvas.SetTop(eye, center.Y - eye.Height / 2);
  }


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


  public bool IsSelfCollision(ModelOfCanvas canvas)
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
          canvas.Width);

      double dy = WrappedDifference(
          HeadPosition.Y,
          bodyPoint.Y,
          canvas.Height);

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


  private static double Distance(Point first, Point second)
  {
    double dx = first.X - second.X;
    double dy = first.Y - second.Y;

    return Math.Sqrt(dx * dx + dy * dy);
  }


  public void RotateLeft() =>
      Direction = new Vector(Direction.Y, -Direction.X);

  public void RotateRight() =>
      Direction = new Vector(-Direction.Y, Direction.X);

  public void AddLength() => BodySegments++;

  public void AddBodyWidth()
  {
    BodyWidth = StartSegmentWidth + (int)(Math.Max(0, BodySegments - 12) * 0.5);
    SetHeadSizes();
  }

  public void SetHeadSizes()
  {
    HeadWidth = BodyWidth * 4 / 3;
    HeadLength = BodyWidth * 2;
  }

  public void AddSpeed(int s) => SnakeSpeed += s * 0.01;

}
