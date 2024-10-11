
namespace TipCalculator.Views;

public partial class MainPage : ContentPage
{
    private double _bill;
    private double _tipPercentage;
    private double _tipAmount;
    private double _total;
    private bool _roundedButtonClicked = false;
    
    public MainPage()
    {
        InitializeComponent();
        
    }
}