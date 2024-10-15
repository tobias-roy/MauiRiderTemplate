using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using RealEstateApp.Models;
using RealEstateApp.Services;
using RealEstateApp.Views;

namespace RealEstateApp.ViewModels;

public class PropertyListPageViewModel : BaseViewModel
{
    private readonly IPropertyService service;

    private Command getPropertiesCommand;

    private Command goToAddPropertyCommand;

    private Command goToDetailsCommand;

    private bool isRefreshing;

    public PropertyListPageViewModel(IPropertyService service)
    {
        Title = "Property List";
        this.service = service;
    }

    public ObservableCollection<PropertyListItem> PropertiesCollection { get; } = new();

    public bool IsRefreshing
    {
        get => isRefreshing;
        set => SetProperty(ref isRefreshing, value);
    }

    public ICommand GetPropertiesCommand =>
        getPropertiesCommand ??= new Command(async () => await GetPropertiesAsync());

    public ICommand GoToDetailsCommand => goToDetailsCommand ??=
        new Command<PropertyListItem>(async propertyListItem => await GoToDetails(propertyListItem));

    public ICommand GoToAddPropertyCommand =>
        goToAddPropertyCommand ??= new Command(async () => await GotoAddProperty());

    private async Task GetPropertiesAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;

            var properties = service.GetProperties();

            if (PropertiesCollection.Count != 0)
                PropertiesCollection.Clear();

            foreach (var property in properties)
                PropertiesCollection.Add(new PropertyListItem(property));
        }
        catch (Exception ex)
        {
            IsRefreshing = false;
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    private async Task GoToDetails(PropertyListItem propertyListItem)
    {
        if (propertyListItem == null)
            return;

        await Shell.Current.GoToAsync(nameof(PropertyDetailPage), true, new Dictionary<string, object>
        {
            { "MyPropertyListItem", propertyListItem }
        });
    }

    private async Task GotoAddProperty()
    {
        await Shell.Current.GoToAsync($"{nameof(AddEditPropertyPage)}?mode=newproperty", true,
            new Dictionary<string, object>
            {
                { "MyProperty", new Property() }
            });
    }
}