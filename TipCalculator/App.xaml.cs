using System.Globalization;

namespace TipCalculator;

public partial class App : Application
{
    public App()
    { 
        //Setup routes for the entire app
        Routing.RegisterRoute("detailspage", typeof(Views.DetailsPage));
        Routing.RegisterRoute("about", typeof(Views.AboutPage));
        Routing.RegisterRoute("colorgrid", typeof(Views.ColorGridExample));
        Routing.RegisterRoute("feedback", typeof(Views.FeedBackPage));
        Routing.RegisterRoute("gridcell", typeof(Views.GridCellTestPage));
        Routing.RegisterRoute("tipcalculator", typeof(Views.GridTipCalculator));
        Routing.RegisterRoute("restaurants", typeof(Views.RestaurantsPage));
        Routing.RegisterRoute("rotatingtext", typeof(Views.RotatingText));

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