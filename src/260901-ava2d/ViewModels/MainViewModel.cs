using CommunityToolkit.Mvvm.ComponentModel;

namespace _260901_ava2d.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";
}
