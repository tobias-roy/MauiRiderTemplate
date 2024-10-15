namespace TipCalculator.Views;

public partial class MainPage : ContentPage
{
    private double _bill;
    private bool _roundedButtonClicked = false;
    private double _tipAmount;
    private double _tipPercentage;
    private double _total;

    public MainPage()
    {
        InitializeComponent();
    }
}