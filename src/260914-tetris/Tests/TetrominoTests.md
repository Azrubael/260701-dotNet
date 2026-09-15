```csharp
using Xunit;
using _260914_tetris.Models;

public class TetrominoTests
{
    [Fact]
    public void Constructor_SetPositionAndType()
    {
        // Arrange & Act
        var tetromino = new Tetromino(TetrominoType.I, 5, 3);

        // Assert
        Assert.Equal(5, tetromino.X);
        Assert.Equal(3, tetromino.Y);
        Assert.Equal(TetrominoType.I, tetromino.Type);
    }

    [Fact]
    public void RotateRight_IncrementRotationState()
    {
        // Arrange
        var tetromino = new Tetromino(TetrominoType.T, 0, 0);
        var initialCells = tetromino.GetCells().ToList();

        // Act
        tetromino.RotateRight();
        var rotatedCells = tetromino.GetCells().ToList();

        // Assert
        Assert.NotEqual(initialCells, rotatedCells);
    }

    [Fact]
    public void RotateLeft_DecrementRotationState()
    {
        // Arrange
        var tetromino = new Tetromino(TetrominoType.T, 0, 0);
        var initialCells = tetromino.GetCells().ToList();

        // Act
        tetromino.RotateRight();
        tetromino.RotateLeft();
        var cellsAfterRotations = tetromino.GetCells().ToList();

        // Assert
        Assert.Equal(initialCells, cellsAfterRotations);
    }

    [Fact]
    public void RotateRight_WrapAroundAfter4Rotations()
    {
        // Arrange
        var tetromino = new Tetromino(TetrominoType.S, 0, 0);
        var initialCells = tetromino.GetCells().ToList();

        // Act
        for (int i = 0; i < 4; i++)
        {
            tetromino.RotateRight();
        }
        var cellsAfter4Rotations = tetromino.GetCells().ToList();

        // Assert
        Assert.Equal(initialCells, cellsAfter4Rotations);
    }

    [Fact]
    public void RotateLeft_WrapAroundAfter4Rotations()
    {
        // Arrange
        var tetromino = new Tetromino(TetrominoType.Z, 0, 0);
        var initialCells = tetromino.GetCells().ToList();

        // Act
        for (int i = 0; i < 4; i++)
        {
            tetromino.RotateLeft();
        }
        var cellsAfter4Rotations = tetromino.GetCells().ToList();

        // Assert
        Assert.Equal(initialCells, cellsAfter4Rotations);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 10)]
    [InlineData(-2, 15)]
    public void SetPosition_UpdatesXAndY(int x, int y)
    {
        // Arrange
        var tetromino = new Tetromino(TetrominoType.I, 0, 0);

        // Act
        tetromino.X = x;
        tetromino.Y = y;

        // Assert
        Assert.Equal(x, tetromino.X);
        Assert.Equal(y, tetromino.Y);
    }

    [Fact]
    public void GetCells_ReturnsCorrectWorldCoordinates()
    {
        // Arrange
        var tetromino = new Tetromino(TetrominoType.I, 3, 2);

        // Act
        var cells = tetromino.GetCells().ToList();

        // Assert
        Assert.NotEmpty(cells);

        // All cells should be offset by tetromino position
        foreach (var (x, y) in cells)
        {
            Assert.True(x >= 3, "X coordinate should be >= tetromino.X");
            Assert.True(y >= 2, "Y coordinate should be >= tetromino.Y");
        }
    }

    [Theory]
    [InlineData(TetrominoType.I)]
    [InlineData(TetrominoType.O)]
    [InlineData(TetrominoType.T)]
    [InlineData(TetrominoType.S)]
    [InlineData(TetrominoType.Z)]
    [InlineData(TetrominoType.J)]
    [InlineData(TetrominoType.L)]
    public void Constructor_AllTetrominoTypes_ReturnValidCells(TetrominoType type)
    {
        // Arrange & Act
        var tetromino = new Tetromino(type, 0, 0);
        var cells = tetromino.GetCells().ToList();

        // Assert
        Assert.NotEmpty(cells);
        Assert.Equal(4, cells.Count); // All tetrominoes have exactly 4 cells
    }

    [Fact]
    public void GetCells_DifferentRotations_ProduceDifferentShapes()
    {
        // Arrange
        var tetromino1 = new Tetromino(TetrominoType.T, 0, 0);
        var tetromino2 = new Tetromino(TetrominoType.T, 0, 0);

        tetromino2.RotateRight();

        // Act
        var cells1 = tetromino1.GetCells().ToHashSet();
        var cells2 = tetromino2.GetCells().ToHashSet();

        // Assert
        Assert.NotEqual(cells1, cells2);
    }
}
```