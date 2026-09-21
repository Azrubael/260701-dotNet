To create a ne plain template project:

```powershell
dotnet new avalonia.app
```

If something get wrong
```powershell
Get-Process tetris -ErrorAction SilentlyContinue | Stop-Process -Force
Stop-Process -Id 25576 -Force
```

```powershell
Remove-Item .\bin, .\obj, .\publish -Recurse -Force -ErrorAction SilentlyContinue
dotnet clean

dotnet publish .\260914-tetris.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  -p:EnableCompressionInSingleFile=true `
  -p:PublishTrimmed=false `
  -p:DebugType=None `
  -p:DebugSymbols=false `
  -o .\publish\win-x64
```