## 2026-07-25
-----------------

A company might have a strategy in place for what packages are OK to use and where to find them.
You can learn more about a package before installing it by going to https://www.nuget.org/packages/<package name>

To ensure that you can use a package, all dependencies are crawled and downloaded when you run the dotnet add package <package name> command.

A typical installation command looks like this:
`dotnet add package <name of package>`
Install global tools by using the command
`dotnet tool install <name of package>`
Install templates by using the command
`dotnet new -i <name of package>`

The installed packages are listed in the *dependencies* section of your *.csproj* file.
If you want to see what packages are in the folder, you can enter
`dotnet list package`
This command lists only the top-level packages, and not dependencies of those packages that we call transitive packages.
```pwsh
dotnet list package --include-transitive

Restore complete (0,3s)
Build succeeded in 0,5s
Project 'utility_free' has the following package references
   [net10.0]:
   Top-level Package                            Requested   Resolved
   > System.Diagnostics.PerformanceCounter      10.0.10     10.0.10

   Transitive Package                                Resolved
   > System.Configuration.ConfigurationManager       10.0.10
   > System.Diagnostics.EventLog                     10.0.10
   > System.Security.Cryptography.ProtectedData      10.0.10
```
You can manually restore dependencies, and project-specific tools that are specified in the project file, by running command the
`dotnet restore`
NuGet restore runs _implicitly_, if necessary, when you run commands like `new`, `build`, and `run`.

`dotnet remove package <name of dependency>`
It removes the package from your project's .csproj file.


*Semantic versioning update approach*:
`Major version`: I'm OK with updating to the latest major version as soon as it's out. I accept the fact that I might need to change code on my end.
`Minor version`: I'm OK with a new feature being added. I'm not OK with code that breaks.
`Patch version`: The only updates I'm OK with are bug fixes.

When you're using a floating version, NuGet resolves the latest version of a package that matches the version pattern.
```
1.0 	x >= 1.0 	Minimum version, inclusive
(1.0,) 	x > 1.0 	Minimum version, exclusive
[1.0] 	x == 1.0 	Exact version match
(,1.0] 	x ≤ 1.0 	Maximum version, inclusive
(,1.0) 	x < 1.0 	Maximum version, exclusive
[1.0,2.0] 	1.0 ≤ x ≤ 2.0 	Exact range, inclusive
(1.0,2.0) 	1.0 < x < 2.0 	Exact range, exclusive
[1.0,2.0) 	1.0 ≤ x < 2.0 	Mixed inclusive minimum and exclusive maximum version
(1.0) 	invalid 	invalid
```
NuGet also supports using a floating version notation for major, minor, patch, and prerelease suffix parts of the number. This notation is an asterisk (*). For example, the version specification 6.0.* says "use the latest 6.0.x version."

Here are some examples that can configure for major, minor, or patch version:
XML
```xml
<!-- Accepts any version 6.1 and later. -->
<PackageReference Include="ExamplePackage" Version="6.1" />

<!-- Accepts any 6.x.y version. -->
<PackageReference Include="ExamplePackage" Version="6.*" />
<PackageReference Include="ExamplePackage" Version="[6,7)" />

<!-- Accepts any later version, but not including 4.1.3. Could be
     used to guarantee a dependency with a specific bug fix. -->
<PackageReference Include="ExamplePackage" Version="(4.1.3,)" />

<!-- Accepts any version earlier than 5.x, which might be used to prevent pulling in a later
     version of a dependency that changed its interface. However, we don't recommend this form because determining the earliest version can be difficult. -->
<PackageReference Include="ExamplePackage" Version="(,5.0)" />

<!-- Accepts any 1.x or 2.x version, but not 0.x or 3.x and later. -->
<PackageReference Include="ExamplePackage" Version="[1,3)" />

<!-- Accepts 1.3.2 up to 1.4.x, but not 1.5 and later. -->
<PackageReference Include="ExamplePackage" Version="[1.3.2,1.5)" />
```

Find and update outdated packages
`dotnet list package --outdated`
`dotnet add package <package name>` // If you run this command, it tries to update to the latest version. Optionally, you can do:
`dotnet add package <package name> --version=<version number/range>`


**Exercise - Install and update the packages**
```pwsh
PS D:\Project\code\dotNet\src> dotnet --list-sdks
10.0.301 [C:\Program Files\dotnet\sdk]

PS D:\Project\code\dotNet\src> mkdir TutorDotNetDependencies
PS D:\Project\code\dotNet\src> cd TutorDotNetDependencies
PS D:\Project\code\dotNet\src\TutorDotNetDependencies> dotnet new console --framework net10.0
PS D:\Project\code\dotNet\src\TutorDotNetDependencies> dotnet run
Hello, World!

PS D:\Project\code\dotNet\src\TutorDotNetDependencies> dotnet add package Humanizer --version 2.7.9
	...
PS D:\Project\code\dotNet\src\TutorDotNetDependencies> type TutorDotNetDependencies.csproj
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Humanizer" Version="2.7.9" />
  </ItemGroup>

</Project>


PS D:\Project\code\dotNet\src\TutorDotNetDependencies> vim Project.cs
PS D:\Project\code\dotNet\src\TutorDotNetDependencies> type Project.cs
using Humanizer;
static void HumanizeQuantities()
{
    Console.WriteLine("case".ToQuantity(0));
    Console.WriteLine("case".ToQuantity(1));
    Console.WriteLine("case".ToQuantity(5));
}
static void HumanizeDates()
{
    Console.WriteLine(DateTime.UtcNow.AddHours(-24).Humanize());
    Console.WriteLine(DateTime.UtcNow.AddHours(-2).Humanize());
    Console.WriteLine(TimeSpan.FromDays(1).Humanize());
    Console.WriteLine(TimeSpan.FromDays(16).Humanize());
}
Console.WriteLine("Quantities:");
HumanizeQuantities();

Console.WriteLine("\nDate/Time Manipulation:");
HumanizeDates();


PS D:\Project\code\dotNet\src\TutorDotNetDependencies> dotnet run
Quantities:
0 cases
1 case
5 cases

Date/Time Manipulation:
вчора
2 години тому
один день
2 тижні


PS D:\Project\code\dotNet\src\TutorDotNetDependencies> dotnet list package --outdated
Restore complete (0,3s)

Build succeeded in 0,5s
The following sources were used:
   https://api.nuget.org/v3/index.json

Project `TutorDotNetDependencies` has the following updates to its packages
   [net10.0]:
   Top-level Package      Requested   Resolved   Latest
   > Humanizer            2.7.9       2.7.9      3.0.10


PS D:\Project\code\dotNet\src\TutorDotNetDependencies> dotnet add package Humanizer
PS D:\Project\code\dotNet\src\TutorDotNetDependencies> type TutorDotNetDependencies.csproj
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Humanizer" Version="3.0.10" />
  </ItemGroup>

</Project>


PS D:\Project\code\dotNet\src\TutorDotNetDependencies> dotnet run
Quantities:
0 cases
1 case
5 cases

Date/Time Manipulation:
вчора
2 години тому
1 день
2 тижні
```