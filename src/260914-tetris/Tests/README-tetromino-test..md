Here's how to create and run the unit tests:

## **STEP 1: Create a Test Project**

```bash
dotnet new xunit -n YourProject.Tests
cd YourProject.Tests
dotnet add reference ../YourProject/YourProject.csproj
```

---

## **STEP 2: Add the Test File**

Create `TetrominoTests.cs` in the test project and paste the test code from the previous response.

---

## **STEP 3: Run Tests**

### **From Command Line**
```bash
# Run all tests
dotnet test

# Run with verbose output
dotnet test --verbosity detailed

# Run specific test file
dotnet test --filter ClassName=TetrominoTests

# Run specific test method
dotnet test --filter Name=Constructor_SetPositionAndType
```

### **From Visual Studio**
1. Open **Test Explorer** → View → Test Explorer
2. Click **Run All Tests** (green play button)
3. View results in the Test Explorer panel

### **From Visual Studio Code**
1. Install the **Test Explorer UI** extension
2. Open Test Explorer sidebar
3. Click the run icon next to test names

---

## **STEP 4: Interpret Results**

**Passed Test** (green ✓)
```
TetrominoTests.Constructor_SetPositionAndType PASSED
```

**Failed Test** (red ✗)
```
TetrominoTests.RotateRight_IncrementRotationState FAILED
Expected: (2, 1)
Actual:   (1, 2)
```

---

## **STEP 5: Debug a Failing Test**

Add a breakpoint in the test and run with debugger:

```csharp
[Fact]
public void GetCells_ReturnsCorrectWorldCoordinates()
{
    // Arrange
    var tetromino = new Tetromino(TetrominoType.I, 3, 2);

    // Act
    var cells = tetromino.GetCells().ToList();  // ← Breakpoint here

    // Assert
    Assert.NotEmpty(cells);
    foreach (var (x, y) in cells)
    {
        System.Diagnostics.Debug.WriteLine($"Cell: ({x}, {y})");
    }
}
```

Then run with **Debug Test** (right-click test → Debug Selected Tests).

---

## **EXAMPLE WORKFLOW**

1. Run tests:
   ```bash
   dotnet test
   ```

2. See failures:
   ```
   FAILED: RotateRight_IncrementRotationState
   Expected cells: [(0,0), (1,0), (2,0), (3,0)]
   Actual cells:   [(0,0), (0,1), (0,2), (0,3)]
   ```

3. Add debug output to `Tetromino.GetCells()` to inspect the rotation logic
4. Fix the bug in `Tetromino` class
5. Re-run tests until all pass ✓

---

**Debugging Tip:** Run a single failing test in isolation to focus your debugging:
```bash
dotnet test --filter Name=RotateRight_IncrementRotationState
```