using CommunityToolkit.Mvvm.ComponentModel;

namespace _260820_ava3d.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";
}
