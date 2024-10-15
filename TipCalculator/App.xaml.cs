using System.Globalization;
using TipCalculator.Views;

namespace TipCalculator;

public partial class App : Application
{
    public App()
    {
        Routing.RegisterRoute("detailspage", typeof(DetailsPage));
        Routing.RegisterRoute("about", typeof(AboutPage));
        Routing.RegisterRoute("colorgrid", typeof(ColorGridExamplePage));
        Routing.RegisterRoute("feedback", typeof(FeedBackPage));
        Routing.RegisterRoute("gridcell", typeof(GridCellTestPage));
        Routing.RegisterRoute("tipcalculator", typeof(TipCalculatorPage));
        Routing.RegisterRoute("restaurants", typeof(RestaurantsPage));
        Routing.RegisterRoute("rotatingtext", typeof(RotatingTextPage));
        Routing.RegisterRoute("calculator", typeof(CalculatorPage));

        InitializeComponent();
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("da-DK");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("da-DK");
        RerouteOnStart();
    }

    private async void RerouteOnStart()
    {
        var defaultStartPage = Preferences.Get("DefaultStartPage", "/tipcalculator");
        MainPage = new AppShell();
        await Shell.Current.GoToAsync(defaultStartPage);
    }
}