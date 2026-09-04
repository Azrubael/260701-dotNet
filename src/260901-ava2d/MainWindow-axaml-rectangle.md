```axaml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:vm="using:_260901_ava2d.ViewModels"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        mc:Ignorable="d" d:DesignWidth="800" d:DesignHeight="600"
        x:Class="_260901_ava2d.Views.MainWindow"
        x:DataType="vm:MainViewModel"
        Icon="/Assets/avalonia-logo.ico"
        Title="Avalonia Snake!"
        Width="600" Height="550"
        CanResize="False"
        Focusable="True"
        KeyDown="OnWindowKeyDown"
        WindowStartupLocation="CenterScreen">

  <Grid Background="lightgray">
    <Canvas x:Name="GameCanvas"
            Width="600"
            Height="480"
            Background="lightgray"
            IsHitTestVisible="False"
            HorizontalAlignment="Center"
            VerticalAlignment="Center">

      <Polyline x:Name="SnakePath"
                Stroke="#617E50"
                StrokeThickness="8"
                StrokeLineCap="Round"
                StrokeJoin="Round"
                IsVisible="False" />

      <Rectangle x:Name="Snake0"
                 Width="15"
                 Height="15"
                 Fill="Black"
                 IsVisible="False" />
      <Rectangle x:Name="Snake1"
                 Width="15"
                 Height="15"
                 Fill="#617E50"
                 IsVisible="False" />
      <Rectangle x:Name="Snake2"
                 Width="15"
                 Height="15"
                 Fill="#617E50"
                 IsVisible="False" />
      <Rectangle x:Name="Snake3"
                 Width="15"
                 Height="15"
                 Fill="#617E50"
                 IsVisible="False" />
      <Rectangle x:Name="Snake4"
                 Width="15"
                 Height="15"
                 Fill="#617E50"
                 IsVisible="False" />


    </Canvas>

    <Menu HorizontalAlignment="Left" VerticalAlignment="Top">
      <MenuItem Header="_Game">
        <MenuItem Header="_New" Click="OnNewGameClick" />
        <MenuItem Header="_Exit" Click="OnExitClick" />
      </MenuItem>

      <MenuItem Header="_Help">
        <MenuItem Header="FAQ" />
        <MenuItem Header="About" />
      </MenuItem>
    </Menu>
  </Grid>

  <!-- <Design.DataContext> -->
  <!-- This only sets the DataContext for the previewer in an IDE,
  to set the actual DataContext for runtime, set the DataContext property in code (look at App.axaml.cs) -->
  <!-- <vm:MainViewModel />
  </Design.DataContext> -->

</Window>
```