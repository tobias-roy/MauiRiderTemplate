using System.ComponentModel;
using RealEstateApp.Models;
using RealEstateApp.ViewModels;

namespace RealEstateApp.Views;

public partial class CompassPage : ContentPage
{
    private readonly CompassPageViewModel vm;
    public CompassPage(CompassPageViewModel vm)
    {
        InitializeComponent();
        this.vm = vm;
        BindingContext = vm;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        vm.OnDissapearingCommand.Execute(null);
    }
}