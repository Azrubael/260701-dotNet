using System;
using System.Linq;

namespace _260914_tetris.Models;


public class GameLogicEngine
{
  private readonly GameBoard gameBoard;
  private readonly CollisionDetector collisionDetector;
  private readonly Tetromino currentPiece;
  public Tetromino CurrentPiece => currentPiece;


  public GameLogicEngine(int boardWidth = 10, int boardHeight = 20)
  {
    gameBoard = new GameBoard(boardWidth, boardHeight);
    collisionDetector = new CollisionDetector(gameBoard, boardWidth, boardHeight);

    // Initialize with first piece
    currentPiece = SpawnNewPiece();
  }


  /// <summary>
  /// Spawns a new tetromino at the top center of the board.
  /// </summary>
  private static Tetromino SpawnNewPiece()
  {
    var random = new Random();
    var types = Enum.GetValues<TetrominoType>().Cast<TetrominoType>().ToArray();
    var randomType = types[random.Next(types.Length)];

    // Spawn at top center (x = 4, y = 0)
    return new Tetromino(randomType, 4, 0);
  }



  public bool TryMovePiece(int deltaX, int deltaY)
  {
    // Attempt movement
    currentPiece.X += deltaX;
    currentPiece.Y += deltaY;

    // Revert if collision detected
    if (!collisionDetector.CanPlace(currentPiece))
    {
      currentPiece.X -= deltaX;
      currentPiece.Y -= deltaY;
      return false;
    }

    return true;
  }


  public bool TryRotate(bool clockwise)
  {
    // Save current state
    // int originalRotation = currentPiece.CurrentRotation;

    // Attempt rotation
    if (clockwise)
      currentPiece.RotateRight();
    else
      currentPiece.RotateLeft();

    // Revert if collision detected
    if (!collisionDetector.CanPlace(currentPiece))
    {
      // Rotate back
      if (clockwise)
        currentPiece.RotateLeft();
      else
        currentPiece.RotateRight();
      return false;
    }

    return true;
  }


  public bool IsGameOver(Tetromino newPiece)
  {
    // Game over if new piece cannot spawn
    return !collisionDetector.CanPlace(newPiece);
  }


  public void LockPiece()
  {
    var cells = currentPiece.GetCells();
    var cellColor = Enum.Parse<CellState>(currentPiece.Type.ToString());

    foreach (var (x, y) in cells)
    {
      gameBoard.SetCell(x, y, cellColor);
    }
  }


  public void ClearCompleteLines()
  {
    var linesToClear = collisionDetector.GetCompleteLines().ToList();

    foreach (int row in linesToClear.OrderByDescending(r => r))
    {
      gameBoard.ClearLine(row);
    }
  }
}
