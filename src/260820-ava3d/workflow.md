## Starting an Avalonia UI Project for 3D PDF Viewing

1. Install the .NET SDK and Avalonia Templates
Ensure you have the latest .NET SDK installed (likely .NET 10 for C# 14). Install the Avalonia project templates via the command line to enable quick project generation:
`dotnet new install Avalonia.Templates`

2. Create the Project
Generate a new Avalonia MVVM application. Using the MVVM (Model-View-ViewModel) pattern is recommended for 3D applications to separate the complex rendering logic from the UI:
`dotnet new avalonia.mvvm -o ThreeDPdfViewer`

3. Enable C# 14 Features
Since C# 14 is the latest version, you may need to explicitly set the language version in your `.csproj` file to enable preview features if the SDK does not default to it:
```xml
<PropertyGroup>
  <OutputType>WinExe</OutputType>
  <TargetFramework>net10.0</TargetFramework>
  <LangVersion>preview</LangVersion>
</PropertyGroup>
```

4. Resolve 3D PDF Rendering
Reading 3D models from PDFs is a complex task because 3D PDFs typically use the U3D (Universal 3D) or PRC (Product Representation Compact) formats embedded within the PDF structure. Standard PDF libraries like PdfSharp or iText only handle 2D content. You have two primary paths:

* Commercial SDKs: Tools like Apryse (formerly PDFtron) or Foxit provide native support for 3D PDF rendering and can be integrated into Avalonia via a native window handle or a wrapper.
* Open Source Path: Use a library to parse the PDF and extract the U3D/PRC stream, then use a 3D rendering engine. For Avalonia, the most efficient way to render 3D content is via the `OpenGlControlBase` class, which allows you to write raw OpenGL code or integrate a library like Silk.NET.

5. Implement the 3D Viewport
To display the model in Avalonia, create a custom control that inherits from `OpenGlControlBase`. This ensures the 3D content renders consistently across both Windows 11 (via DirectX/OpenGL) and Ubuntu 24 (via OpenGL/Vulkan):

```csharp
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;

public class Pdf3DViewport : OpenGlControlBase
{
    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        // 1. Clear the buffer
        gl.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        gl.Clear(GlConsts.GL_COLOR_BUFFER_BIT | GlConsts.GL_DEPTH_BUFFER_BIT);

        // 2. Logic to render the extracted U3D/PRC mesh goes here
    }
}
```

6. Configure Cross-Platform Compatibility
To ensure the project runs on Ubuntu 24, include the necessary Linux dependencies in your deployment. Ubuntu may require `libdl` and specific OpenGL drivers. You can test the Linux build from Windows using a Docker container or by publishing the app:
`dotnet publish -r linux-x64 --self-contained`


###########################################

To create this minimal application, you need two primary files: the XAML file for the layout and the C# code-behind file for the logic. This example uses a `DockPanel` to keep the menu at the top and allow the viewport to fill the remaining screen space.

### MainWindow.axaml
This file defines the visual structure. The `Menu` control creates the pull-down navigation, and a `Panel` with a white background serves as the placeholder for your 3D rendering engine.

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        x:Class="ThreeDPdfViewer.MainWindow"
        Title="3D PDF Viewer"
        Width="800" Height="600">

    <DockPanel>
        <!-- Top Navigation Menu -->
        <Menu DockPanel.Dock="Top">
            <MenuItem Header="_File">
                <MenuItem Header="_Open" Click="OnOpenClick" />
                <MenuItem Header="_Exit" Click="OnExitClick" />
            </MenuItem>
            
            <MenuItem Header="_Manipulate">
                <MenuItem Header="Zoom In" />
                <MenuItem Header="Zoom Out" />
                <MenuItem Header="Pan" />
                <MenuItem Header="Rotate X" />
                <MenuItem Header="Rotate Y" />
                <MenuItem Header="Rotate Z" />
            </MenuItem>
            
            <MenuItem Header="_Help">
                <MenuItem Header="FAQ" />
                <MenuItem Header="About" />
            </MenuItem>
        </Menu>

        <!-- 3D Render Area -->
        <Panel Background="White">
            <TextBlock Text="3D Render Viewport" 
                       HorizontalAlignment="Center" 
                       VerticalAlignment="Center" 
                       Foreground="LightGray" />
        </Panel>
    </DockPanel>
</Window>
```

### MainWindow.axaml.cs
The code-behind handles the interaction logic. In a full MVVM application, these would be commands in a ViewModel, but for a minimal example, direct event handlers are used.

```csharp
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ThreeDPdfViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Handles the 'Open' menu item
        private void OnOpenClick(object sender, RoutedEventArgs e)
        {
            // Logic to trigger FileOpenDialog goes here
        }

        // Handles the 'Exit' menu item
        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
```

### Integration Notes
To turn the white `Panel` into a functional 3D window, you would replace `<Panel Background="White">` in the XAML with your custom OpenGL control created in the previous step:

1. Define your control namespace in the Window tag: `xmlns:local="clr-namespace:ThreeDPdfViewer"`.
2. Replace the Panel with: `<local:Pdf3DViewport />`.
3. Ensure the `Pdf3DViewport` class sets its own background color or clear color within the `OnOpenGlRender` method to maintain the white background.
