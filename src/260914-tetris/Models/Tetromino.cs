using System;

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
            TetrominoType.A =>      // I-piece
            [
                [(0,0), (0,1), (0,2), (0,3)],
                [(0,0), (1,0), (2,0), (3,0)],
                [(0,0), (0,1), (0,2), (0,3)],
                [(0,0), (1,0), (2,0), (3,0)]
            ],
            TetrominoType.B =>      // L-piece
            [
                [(0,0), (0,1), (0,2), (1,0)],
                [(0,0), (1,0), (2,0), (2,1)],
                [(0,2), (1,0), (1,1), (1,2)],
                [(0,0), (0,1), (1,1), (2,1)]
            ],
            TetrominoType.C =>      // J-piece
            [
                [(0,0), (0,1), (0,2), (1,2)],
                [(0,1), (1,1), (2,0), (2,1)],
                [(0,0), (1,0), (1,1), (1,2)],
                [(0,0), (0,1), (1,1), (2,1)]
            ],
            TetrominoType.D =>      // O-piece (square, no rotation change)
            [
                [(0,0), (0,1), (1,0), (1,1)],
                [(0,0), (0,1), (1,0), (1,1)],
                [(0,0), (0,1), (1,0), (1,1)],
                [(0,0), (0,1), (1,0), (1,1)]
            ],
            TetrominoType.E =>      // T-piece
            [
                [(0,0), (1,0), (2,0), (1,1)],
                [(0,0), (0,1), (1,0), (2,0)],
                [(0,1), (1,0), (1,1), (2,1)],
                [(0,0), (1,0), (1,1), (2,0)]
            ],
            TetrominoType.F =>      // S-piece
            [
                [(0,1), (0,2), (1,0), (1,1)],
                [(0,0), (1,0), (1,1), (2,1)],
                [(0,1), (0,2), (1,0), (1,1)],
                [(0,0), (1,0), (1,1), (2,1)]
            ],
            TetrominoType.G =>    // Z-piece
            [
                [(0,0), (0,1), (1,1), (1,2)],
                [(0,1), (1,0), (1,1), (2,0)],
                [(0,0), (0,1), (1,1), (1,2)],
                [(0,1), (1,0), (1,1), (2,0)]
            ],
            _ => throw new ArgumentException("Unknown tetromino type")
        };
    }
}

public enum TetrominoType { A, B, C, D, E, F, G }