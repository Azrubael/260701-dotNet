using System.Collections.Generic;

namespace _260914_tetris.Models;

public class CollisionDetector(GameBoard board, int width = 10, int height = 20)
{
    private readonly int boardWidth = width;
    private readonly int boardHeight = height;
    private readonly GameBoard gameBoard = board;

  /// <summary>
  /// Determines if a tetromino can be placed at its current position without collision.
  /// </summary>
  public bool CanPlace(Tetromino tetromino)
    {
        var cells = tetromino.GetCells();

        foreach (var (x, y) in cells)
        {
            // Wall collision: outside board boundaries
            if (x < 0 || x >= boardWidth || y < 0 || y >= boardHeight)
                return false;

            // Piece collision: cell already occupied
            if (gameBoard.GetCell(x, y) != CellState.Empty)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if tetromino collides with walls only.
    /// </summary>
    public bool HasWallCollision(Tetromino tetromino)
    {
        var cells = tetromino.GetCells();

        foreach (var (x, y) in cells)
        {
            if (x < 0 || x >= boardWidth || y < 0 || y >= boardHeight)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if tetromino collides with locked pieces only.
    /// </summary>
    public bool HasPieceCollision(Tetromino tetromino)
    {
        var cells = tetromino.GetCells();

        foreach (var (x, y) in cells)
        {
            // Only check collision with placed pieces, ignore wall bounds
            if (y >= 0 && y < boardHeight && x >= 0 && x < boardWidth)
            {
                if (gameBoard.GetCell(x, y) != CellState.Empty)
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if a row is completely filled.
    /// </summary>
    public bool IsLineFull(int row)
    {
        if (row < 0 || row >= boardHeight)
            return false;

        for (int x = 0; x < boardWidth; x++)
        {
            if (gameBoard.GetCell(x, row) == CellState.Empty)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Finds all complete rows.
    /// </summary>
    public IEnumerable<int> GetCompleteLines()
    {
        for (int row = 0; row < boardHeight; row++)
        {
            if (IsLineFull(row))
                yield return row;
        }
    }
}

public enum CellState
{
    Empty,
    I, O, T, S, Z, J, L  // Color-coded by piece type
}
