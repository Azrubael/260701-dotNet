The `System.Diagnostics.PerformanceCounter` class requires a separate NuGet package to be installed. The PerformanceCounter type is not included in the base .NET framework by default for modern .NET versions.

They need to add a reference to the `System.Diagnostics.PerformanceCounter` NuGet package to their project.

I should show them how to fix this by adding the NuGet package reference.You need to add the **System.Diagnostics.PerformanceCounter** NuGet package to your project. Here's how to fix it:

## Solution

### Option 1: Using .NET CLI (Simplest)

Run this command in your `utility_free` project directory:

```bash
dotnet add package System.Diagnostics.PerformanceCounter
```

---

### Option 2: Edit `.csproj` Manually

Open your `utility_free.csproj` file and add this to the `<ItemGroup>` section:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <LangVersion>12</LangVersion>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="System.Diagnostics.PerformanceCounter" Version="8.0.0" />
  </ItemGroup>

</Project>
```

Then restore the packages:

```bash
dotnet restore
```

## Run the project on debugging mode with keys
```csharp
dotnet run --configuration Debug -- -h
```


## Assembly the project into a small Windows binary
```csharp
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true
```