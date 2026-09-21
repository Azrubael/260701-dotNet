using System;
using System.Collections.Generic;
using System.Linq;

namespace _260914_tetris.Models;

public class GameBoard
{
  private readonly CellState[,] grid;
  private readonly int width;
  private readonly int height;
  public event Action? LineCleared;

  public GameBoard(int width = 10, int height = 25)
  {
    this.width = width;
    this.height = height;
    grid = new CellState[width, height];

    // Initialize all cells as empty
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        grid[x, y] = CellState.Empty;
      }
    }
  }

  public int Width => width;
  public int Height => height;

  /// <summary>
  /// Gets the cell state at (x, y).
  /// </summary>
  public CellState GetCell(int x, int y)
  {
    if (x < 0 || x >= width || y < 0 || y >= height)
      return CellState.Empty;

    return grid[x, y];
  }

  /// <summary>
  /// Sets the cell state at (x, y).
  /// </summary>
  public void SetCell(int x, int y, CellState state)
  {
    if (x < 0 || x >= width || y < 0 || y >= height)
      return;

    grid[x, y] = state;
  }

  /// <summary>
  /// Clears a single row and shifts rows above downward.
  /// </summary>
  public void ClearLine(int row)
  {
    if (row < 0 || row >= height)
      return;

    // Shift all rows above down by one
    for (int y = row; y > 0; y--)
    {
      for (int x = 0; x < width; x++)
      {
        grid[x, y] = grid[x, y - 1];
      }
    }

    // Clear the top row
    for (int x = 0; x < width; x++)
    {
      grid[x, 0] = CellState.Empty;
    }

    LineCleared?.Invoke();
  }

  /// <summary>
  /// Clears multiple rows and shifts accordingly.
  /// </summary>
/*
  public void ClearLines(IEnumerable<int> rows)
  {
    // Sort in descending order to clear bottom rows first (prevents index shifting issues)
    var sortedRows = rows.OrderByDescending(r => r).ToList();

    foreach (int row in sortedRows)
    {
      ClearLine(row);
    }
  }
*/

  public void ClearLines(IEnumerable<int> rows)
  {
    var rowsToClear = rows
      .Where(row => row >= 0 && row < height)
      .Distinct()
      .ToHashSet();

    if (rowsToClear.Count == 0)
      return;

    int destinationRow = 0;

    // Copy rows that should remain toward the bottom.
    for (int sourceRow = 0; sourceRow < height; sourceRow++)
    {
      if (rowsToClear.Contains(sourceRow))
        continue;

      for (int x = 0; x < width; x++)
      {
        grid[x, destinationRow] = grid[x, sourceRow];
      }

      destinationRow++;
    }

    // Clear the rows left at the top.
    for (int y = destinationRow; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        grid[x, y] = CellState.Empty;
      }
    }

    // Preserve one notification per cleared line.
    for (int i = 0; i < rowsToClear.Count; i++)
    {
      LineCleared?.Invoke();
    }
  }


  /// <summary>
  /// Returns a snapshot of the entire board state.
  /// Useful for rendering or serialization.
  /// </summary>
  public CellState[,] GetSnapshot()
  {
    var snapshot = new CellState[width, height];
    Array.Copy(grid, snapshot, grid.Length);
    return snapshot;
  }

  /// <summary>
  /// Resets the board to empty state.
  /// </summary>
  public void Clear()
  {
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        grid[x, y] = CellState.Empty;
      }
    }
  }
}
