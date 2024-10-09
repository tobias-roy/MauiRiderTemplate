namespace TipCalculator;

public partial class AppShell : Shell
{
    public AppShell()
    {
        Routing.RegisterRoute("detailspage", typeof(DetailsPage));
        InitializeComponent();
    }
}