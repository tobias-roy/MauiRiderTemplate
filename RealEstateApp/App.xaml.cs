using RealEstateApp.Views;

namespace RealEstateApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
        Connectivity.ConnectivityChanged += ConnectivityConnectivityChanged;
        App.Current.UserAppTheme = AppTheme.Light;
        Routing.RegisterRoute("compass", typeof(CompassPage));
    }
    
    private async void ConnectivityConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        var access = e.NetworkAccess;
        if (access == NetworkAccess.Internet)
        {
            await Shell.Current.CurrentPage.DisplayAlert("Connectivity", "You are connected to the internet", "OK");
        }
        else
        {
            await Shell.Current.CurrentPage.DisplayAlert("Connectivity", "You have lost internet connection", "OK");
        }
    }
}

