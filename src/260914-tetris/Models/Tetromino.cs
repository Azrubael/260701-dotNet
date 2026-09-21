using System;
using Avalonia.Media;

namespace _260914_tetris.Models;

public class Tetromino(TetrominoType type, int startX = 0, int startY = 0)
{
  private readonly (int x, int y)[][] rotationStates = InitializeRotations(type);
  private int currentRotation = 0;

  public int X { get; set; } = startX;
  public int Y { get; set; } = startY;
  public TetrominoType Type { get; } = type;

  public void RotateRight()
  {
    currentRotation = (currentRotation + 1) % 4;
  }

  public void RotateLeft()
  {   // Equivalent to (current - 1 + 4) % 4
    currentRotation = (currentRotation + 3) % 4;
  }

  public (int x, int y)[] GetCells()
  {
    var localCells = rotationStates[currentRotation];
    var worldCells = new (int, int)[localCells.Length];

    for (int i = 0; i < localCells.Length; i++)
    {
      worldCells[i] = (X + localCells[i].x, Y + localCells[i].y);
    }

    return worldCells;
  }

  public int CurrentRotation => currentRotation;

  private static (int x, int y)[][] InitializeRotations(TetrominoType type)
  {
    return type switch
    {
      TetrominoType.I =>      // I-piece
      [
          [(0,0), (0,1), (0,2), (0,3)],
          [(0,0), (1,0), (2,0), (3,0)],
          [(0,0), (0,1), (0,2), (0,3)],
          [(0,0), (1,0), (2,0), (3,0)]
      ],
      TetrominoType.L =>      // L-piece
      [
          [(0,0), (0,1), (0,2), (1,0)],
          [(0,0), (0,1), (1,1), (2,1)],
          [(0,2), (1,2), (1,1), (1,0)],
          [(0,0), (1,0), (2,0), (2,1)]
      ],
      TetrominoType.Г =>      // Г-piece
      [
          [(0,0), (0,1), (0,2), (1,2)],
          [(0,1), (1,1), (2,1), (2,0)],
          [(0,0), (1,0), (1,1), (1,2)],
          [(0,0), (0,1), (1,0), (2,0)]
      ],
      TetrominoType.O =>      // []-piece
      [
          [(0,0), (0,1), (1,0), (1,1)],
          [(0,0), (0,1), (1,0), (1,1)],
          [(0,0), (0,1), (1,0), (1,1)],
          [(0,0), (0,1), (1,0), (1,1)]
      ],
      TetrominoType.T =>      // T-piece
      [
          [(0,0), (1,0), (2,0), (1,1)],
          [(0,0), (0,1), (0,2), (1,1)],
          [(0,1), (1,1), (2,1), (1,0)],
          [(0,1), (1,0), (1,1), (1,2)]
      ],
      TetrominoType.S =>      // S-piece
      [
          [(0,1), (0,2), (1,0), (1,1)],
          [(0,0), (1,0), (1,1), (2,1)],
          [(0,1), (0,2), (1,0), (1,1)],
          [(0,0), (1,0), (1,1), (2,1)]
      ],
      TetrominoType.Z =>    // Z-piece
      [
          [(0,0), (0,1), (1,1), (1,2)],
          [(0,1), (1,0), (1,1), (2,0)],
          [(0,0), (0,1), (1,1), (1,2)],
          [(0,1), (1,0), (1,1), (2,0)]
      ],
      _ => throw new ArgumentException("Unknown tetromino type")
    };
  }


  public static IBrush GetColor(TetrominoType type)
  {
    return type switch
    {
      TetrominoType.I => Brushes.BlueViolet,
      TetrominoType.L => Brushes.Orange,
      TetrominoType.Г => Brushes.Blue,
      TetrominoType.O => Brushes.Yellow,
      TetrominoType.T => Brushes.Pink,
      TetrominoType.S => Brushes.Magenta,
      TetrominoType.Z => Brushes.Red,
      _ => Brushes.White
    };
  }


  public static IBrush GetCellColor(CellState state)
  {
    return state switch
    {
      CellState.I => GetColor(TetrominoType.I),
      CellState.L => GetColor(TetrominoType.L),
      CellState.Г => GetColor(TetrominoType.Г),
      CellState.O => GetColor(TetrominoType.O),
      CellState.T => GetColor(TetrominoType.T),
      CellState.S => GetColor(TetrominoType.S),
      CellState.Z => GetColor(TetrominoType.Z),
      _ => Brushes.White
    };
  }

}

public enum TetrominoType { I, L, Г, O, T, S, Z }