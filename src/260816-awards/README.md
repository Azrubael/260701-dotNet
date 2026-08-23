# In your project folder:
```powershell
dotnet add package ClosedXML
```

*ClosedXML*: Excellent for reading/writing values, basic formatting, tables, and many common workflows, with a focus on simplicity.
*EPPlus*: Often stronger when you need finer control or support for more of Excel’s richer features and edge cases (depending on version), such as complex formatting, charts, and certain advanced structures.


## To assembly the project into a single JIT binary for Windows
```powershell
dotnet clean
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
```

Використання параметра -p:PublishAot=true замість PublishTrimmed змусить компілятор C# перетворити код одразу в машинний, як це робить Go. Це значно зменшить розмір файлу та прискорить запуск, оскільки програмі більше не знадобиться JIT-компілятор і велика частина інфраструктури CLR. NativeAOT працює інакше, ніж стандартна компіляція C#. Для цього процесу (лінкування) .NET використовує native linker від Microsoft Visual C++ (MSVC).
## To assembly the project into a tiny binary for Windows with Microsoft Visual C++ (MSVC)
```powershell
dotnet clean
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishAot=true
```


The executable will be located under:
bin\Release\net10.0\win-x64\publish\


The file is named `.csproj`, not `.cspoj`. For an old Windows 10 computer, publish a **self-contained `win-x64` Release** application so .NET does not need to be installed.

````xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <RootNamespace>_260816_awards</RootNamespace>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>

    <PublishReadyToRun>false</PublishReadyToRun>
    <PublishTrimmed>false</PublishTrimmed>

    <Configuration>Release</Configuration>
    <SelfContained>false</SelfContained>
    <PublishSingleFile>true</PublishSingleFile>
    <EnableCompressionInSingleFile>false</EnableCompressionInSingleFile>
    <DebugType>None</DebugType>
    <DebugSymbols>false</DebugSymbols>
    <InvariantGlobalization>false</InvariantGlobalization>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="ClosedXML" Version="0.105.1" />
  </ItemGroup>

</Project>
```

Publish it with:

````powershell
dotnet clean
dotnet publish -c Release -r win-x64
````

The executable will be located under:

```text
bin\Release\net10.0\win-x64\publish\
```

### Important notes
In *.csproj
- `PublishTrimmed` should remain `false`; `ClosedXML` may rely on reflection.
- `PublishReadyToRun` is disabled because it increases binary size.
- `InvariantGlobalization` must remain `false` because the application uses Ukrainian text and date formatting.
- `win-x64` requires 64-bit Windows. For 32-bit Windows 10, use `win-x86`, but modern .NET support may be limited.
- If the computer uses an especially old Windows 10 build and `.NET 10` does not run, target `net8.0` instead and install the .NET 8 SDK.