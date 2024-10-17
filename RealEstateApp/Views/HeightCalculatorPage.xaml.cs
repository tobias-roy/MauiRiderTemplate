using RealEstateApp.ViewModels;

namespace RealEstateApp.Views;

public partial class HeightCalculatorPage : ContentPage
{
    public HeightCalculatorPage(HeightCalculatorViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}