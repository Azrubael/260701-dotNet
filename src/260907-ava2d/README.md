From the directory containing your C# project, run:

```powershell
Remove-Item .\bin, .\obj, .\publish -Recurse -Force -ErrorAction SilentlyContinue

dotnet publish .\260907-ava2d.csproj `
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

The executable will be created at:

```text
publish\win-x64\SnakeGame.exe
```

Copy that `.exe` to a Windows 10 or Windows 11 **64-bit** computer. The .NET runtime will be included, so the target computer does not need .NET installed.

For a true single-file executable, your PNG files must be embedded as Avalonia resources rather than copied beside the executable. In `SnakeGame.csproj`, ensure you have:

```xml
<ItemGroup>
  <AvaloniaResource Include="Assets\**" />
</ItemGroup>
```

Reference an embedded image in Avalonia using an `avares://` URI, for example:

```csharp
using Avalonia.Media.Imaging;

var image = new Bitmap(
    new Uri("avares://SnakeGame/Assets/snake.png"));
```

Replace `SnakeGame` with the assembly name if it differs from the project name.

If you use the image in XAML:

```xml
<Image Source="avares://SnakeGame/Assets/snake.png" />
```

You can also place the publish settings in the project file:

```xml
<PropertyGroup>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  <SelfContained>true</SelfContained>
  <PublishSingleFile>true</PublishSingleFile>
  <IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
  <PublishTrimmed>false</PublishTrimmed>
</PropertyGroup>
```

Then publish with:

```powershell
dotnet publish .\SnakeGame.csproj -c Release -o .\publish\win-x64
```

`PublishTrimmed=false` is recommended for Avalonia applications because trimming can remove code used through reflection. `IncludeNativeLibrariesForSelfExtract=true` allows native Avalonia components to be extracted temporarily at runtime while still distributing one `.exe` file.


### Reserve
```cproj
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ApplicationManifest>app.manifest</ApplicationManifest>
    <RootNamespace>_260907_ava2d</RootNamespace>
    <AssemblyName>_260907_ava2d</AssemblyName>
    <AvaloniaUseCompiledBindingsByDefault>true</AvaloniaUseCompiledBindingsByDefault>
  </PropertyGroup>

  <ItemGroup>
    <Folder Include="Models\" />
    <Folder Include="ViewModels\" />
    <Folder Include="Views\" />
    <AvaloniaResource Include="Assets\**" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Avalonia" Version="12.1.1" />
    <PackageReference Include="Avalonia.Desktop" Version="12.1.1" />
    <PackageReference Include="Avalonia.Themes.Fluent" Version="12.1.1" />
    <PackageReference Include="Avalonia.Fonts.Inter" Version="12.1.1" />
    <PackageReference Include="AvaloniaUI.DiagnosticsSupport" Version="2.2.3">
      <IncludeAssets Condition="'$(Configuration)' != 'Debug'">None</IncludeAssets>
      <PrivateAssets Condition="'$(Configuration)' != 'Debug'">All</PrivateAssets>
    </PackageReference>
    <PackageReference Include="CommunityToolkit.Mvvm" Version="8.4.2" />
  </ItemGroup>
</Project>
```

### Reserve AI
```cproj
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
	<OutputType>WinExe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ApplicationManifest>app.manifest</ApplicationManifest>
    <RootNamespace>_260907_ava2d</RootNamespace>
    <AssemblyName>snake</AssemblyName>
	<PublishSingleFile>true</PublishSingleFile>
	<SelfContained>true</SelfContained>
	<IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
	<IncludeAllContentForSelfExtract>true</IncludeAllContentForSelfExtract>
	<PublishTrimmed>false</PublishTrimmed>
	<DebugType>None</DebugType>
	<DebugSymbols>false</DebugSymbols>
  </PropertyGroup>

  <ItemGroup>
    <Folder Include="Models\" />
    <Folder Include="ViewModels\" />
    <Folder Include="Views\" />
    <AvaloniaResource Include="Assets\**" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Avalonia" Version="12.1.1" />
    <PackageReference Include="Avalonia.Desktop" Version="12.1.1" />
    <PackageReference Include="Avalonia.Themes.Fluent" Version="12.1.1" />
    <PackageReference Include="Avalonia.Fonts.Inter" Version="12.1.1" />
    <PackageReference Include="AvaloniaUI.DiagnosticsSupport" Version="2.2.3">
      <IncludeAssets Condition="'$(Configuration)' != 'Debug'">None</IncludeAssets>
      <PrivateAssets Condition="'$(Configuration)' != 'Debug'">All</PrivateAssets>
    </PackageReference>
    <PackageReference Include="CommunityToolkit.Mvvm" Version="8.4.2" />
  </ItemGroup>
</Project>

```