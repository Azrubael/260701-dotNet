using System;
using System.Linq;

namespace _260921_tetris.Models;


public class GameLogicEngine
{
  private readonly GameBoard gameBoard;
  private readonly CollisionDetector collisionDetector;
  public CellState[,] BoardSnapshot => gameBoard.GetSnapshot();
  private Tetromino currentPiece;
  public Tetromino CurrentPiece => currentPiece;
  public bool IsGameOver { get; private set; }
  private static readonly Random random = new();
  public event Action? LineCleared;
  public Tetromino NextPiece { get; private set; }


  public GameLogicEngine(int boardWidth, int boardHeight)
  {
    gameBoard = new GameBoard(boardWidth, boardHeight);
    collisionDetector = new CollisionDetector(gameBoard, boardWidth, boardHeight);

    // Initialize with first piece
    currentPiece = SpawnNewPiece();
    NextPiece = SpawnNewPiece();
    gameBoard.LineCleared += () => LineCleared?.Invoke();
  }


  /// <summary>
  /// Spawns a new tetromino at the top center of the board.
  /// </summary>
  public static Tetromino SpawnNewPiece()
  {
    var types = Enum.GetValues<TetrominoType>();
    var randomType = types[random.Next(types.Length)];

    // Spawn at top center (x = 5, y = 0)
    return new Tetromino(randomType, 5, 0);
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


  public bool CheckIsGameOver(Tetromino newPiece)
  {
    // Game over if new piece cannot spawn
    return !collisionDetector.CanPlace(newPiece);
  }


  /// <summary>
  /// Checks if the tetromino dopped and locked
  /// </summary>
  /// <returns></returns>
  public bool LockPiece()
  {
    var cellState = GetCellState(currentPiece.Type);

    if (cellState == CellState.Empty)
      return false;

    foreach (var (x, y) in currentPiece.GetCells())
      gameBoard.SetCell(x, y, cellState);

    return true;
  }


  public bool DropPiece()
  {
    while (TryMovePiece(0, 1))
    {
    }
    if (!LockPiece())
    {
      IsGameOver = true;
      return false;
    }
    ClearCompleteLines();

    // The preview becomes the active piece.
    var newCurrentPiece = NextPiece;

    if (CheckIsGameOver(newCurrentPiece))
    {
      IsGameOver = true;
      return false;
    }

    currentPiece = newCurrentPiece;
    // Generate a replacement preview.
    NextPiece = SpawnNewPiece();
    return true;
  }


  public void ClearCompleteLines()
  {
    var linesToClear = collisionDetector.GetCompleteLines().ToList();

    foreach (int row in linesToClear.OrderByDescending(r => r))
    {
      gameBoard.ClearLine(row);
    }
  }


  public static CellState GetCellState(TetrominoType type)
  {
    return type switch
    {
      TetrominoType.I => CellState.I,
      TetrominoType.L => CellState.L,
      TetrominoType.Г => CellState.Г,
      TetrominoType.O => CellState.O,
      TetrominoType.T => CellState.T,
      TetrominoType.S => CellState.S,
      TetrominoType.Z => CellState.Z,
      _ => CellState.Empty
    };
  }

}
