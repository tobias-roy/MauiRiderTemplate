using RealEstateApp.ViewModels;

namespace RealEstateApp.Views;

public partial class AddEditPropertyPage : ContentPage
{
    private readonly AddEditPropertyPageViewModel vm;
    public AddEditPropertyPage(AddEditPropertyPageViewModel vm)
    {
        InitializeComponent();
        this.vm = vm;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        vm.SetConnectivityCommand.Execute(null);
    }
}