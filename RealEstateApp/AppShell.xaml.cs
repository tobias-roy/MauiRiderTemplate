using RealEstateApp.Views;
using Application = Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific.Application;

namespace RealEstateApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(PropertyDetailPage), typeof(PropertyDetailPage));
        Routing.RegisterRoute(nameof(AddEditPropertyPage), typeof(AddEditPropertyPage));
    }
}