https://docs.avaloniaui.net/docs/get-started/install-avalonia

dotnet --version
dotnet --list-sdks
8.0.423 [C:\Program Files\dotnet\sdk]
10.0.301 [C:\Program Files\dotnet\sdk]

# Avalonia uses standard .NET templates to create new projects. Install them by running:
dotnet new install Avalonia.Templates

# You can check which templates are already installed at any time by running:
dotnet new list
These templates matched your input:

Template Name                                 Short Name                    Language    Tags
--------------------------------------------  ----------------------------  ----------  ------------------------------------------------
API Controller                                apicontroller                 [C#]        Web/ASP.NET
ASP.NET Core Empty                            web                           [C#],F#     Web/Empty
ASP.NET Core gRPC Service                     grpc                          [C#]        Web/gRPC/API/Service
ASP.NET Core Web API                          webapi                        [C#],F#     Web/Web API/API/Service
ASP.NET Core Web API (native AOT)             webapiaot                     [C#]        Web/Web API/API/Service
ASP.NET Core Web App (Model-View-Controller)  mvc                           [C#],F#     Web/MVC
ASP.NET Core Web App (Razor Pages)            webapp,razor                  [C#]        Web/MVC/Razor Pages
Avalonia .NET App                             avalonia.app                  [C#],F#     Desktop/Xaml/Avalonia/Windows/Linux/macOS
Avalonia .NET MVVM App                        avalonia.mvvm                 [C#],F#     Desktop/Xaml/Avalonia/Windows/Linux/macOS
Avalonia Cross Platform Application           avalonia.xplat                [C#],F#     Desktop/Xaml/Avalonia/Browser/Mobile/Android/iOS
Blazor Web App                                blazor                        [C#]        Web/Blazor/WebAssembly
Blazor WebAssembly Standalone App             blazorwasm                    [C#]        Web/Blazor/WebAssembly/PWA
Class Library                                 classlib                      [C#],F#,VB  Common/Library
Console App                                   console                       [C#],F#,VB  Common/Console
dotnet gitattributes file                     gitattributes,.gitattributes              Config
dotnet gitignore file                         gitignore,.gitignore                      Config
Dotnet local tool manifest file               tool-manifest                             Config
EditorConfig file                             editorconfig,.editorconfig                Config
global.json file                              globaljson,global.json                    Config
MSBuild Directory.Build.props file            buildprops                                MSBuild/props
MSBuild Directory.Build.targets file          buildtargets                              MSBuild/props
MSBuild Directory.Packages.props file         packagesprops                             MSBuild/packages/props/CPM
MSTest Playwright Test Project                mstest-playwright             [C#]        Test/MSTest/Playwright/Desktop/Web
MSTest Test Class                             mstest-class                  [C#],F#,VB  Test/MSTest
MSTest Test Project                           mstest                        [C#],F#,VB  Test/MSTest/Desktop/Web
MVC Controller                                mvccontroller                 [C#]        Web/ASP.NET
MVC ViewImports                               viewimports                   [C#]        Web/ASP.NET
MVC ViewStart                                 viewstart                     [C#]        Web/ASP.NET
NuGet Config                                  nugetconfig,nuget.config                  Config
NUnit Playwright Test Project                 nunit-playwright              [C#]        Test/NUnit/Playwright/Desktop/Web
NUnit Test Item                               nunit-test                    [C#],F#,VB  Test/NUnit
NUnit Test Project                            nunit                         [C#],F#,VB  Test/NUnit/Desktop/Web
Protocol Buffer File                          proto                                     Web/gRPC
Razor Class Library                           razorclasslib                 [C#]        Web/Razor/Library
Razor Component                               razorcomponent                [C#]        Web/ASP.NET
Razor Page                                    page                          [C#]        Web/ASP.NET
Razor View                                    view                          [C#]        Web/ASP.NET
Solution File                                 sln,solution                              Solution
Solution Filter File                          slnf,solutionfilter                       Solution
Web Config                                    webconfig                                 Config
Windows Forms App                             winforms                      [C#],VB     Common/WinForms
Windows Forms Class Library                   winformslib                   [C#],VB     Common/WinForms
Windows Forms Control Library                 winformscontrollib            [C#],VB     Common/WinForms
Worker Service                                worker                        [C#],F#     Common/Worker/Web
WPF Application                               wpf                           [C#],VB     Common/WPF
WPF Class Library                             wpflib                        [C#],VB     Common/WPF
WPF Custom Control Library                    wpfcustomcontrollib           [C#],VB     Common/WPF
WPF User Control Library                      wpfusercontrollib             [C#],VB     Common/WPF
xUnit Test Project                            xunit                         [C#],F#,VB  Test/xUnit/Desktop/Web

# If you installed the templates previously, you can update them to the latest version:
dotnet new update

# Create a quick test project to confirm everything is working:
cd <workingPath>
dotnet new avalonia.app
dotnet run
dotnet build


# The simplest MainWindow.axaml
```axaml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        mc:Ignorable="d" d:DesignWidth="320" d:DesignHeight="480"
        x:Class="avalonia1_tutorial.MainWindow"
        Title="Temperature converter with Avalonia UI"
        Width="320"
        Height="480">

  <StackPanel Margin="20" Spacing="10">
    <Border Margin="5" CornerRadius="10" Background="LightBlue">
      <TextBlock Margin="5"
                 HorizontalAlignment="Center"
                 FontSize="16"
                 Text="Temperature Converter">
      </TextBlock>
    </Border>
    <TextBlock Text="Celsius:"/>
    <TextBox Text="0" Name="Celsius"/>

    <TextBlock Text="Fahrenheit:"/>
    <TextBox Text="0" Name="Fahrenheit"/>

    <Button Click="Button_OnClick" HorizontalAlignment="Center" Margin="0 10 0 0">Calculate</Button>
  </StackPanel>
</Window>
```