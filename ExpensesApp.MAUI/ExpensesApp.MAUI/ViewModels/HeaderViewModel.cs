using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ExpensesApp.MAUI.ViewModels;

public partial class HeaderViewModel : ObservableObject
{
    [RelayCommand]
    private async Task OpenProfile()
    {
        await Application.Current.MainPage.DisplayAlert("Profile", "Coming soon!", "Ok");
    }
}