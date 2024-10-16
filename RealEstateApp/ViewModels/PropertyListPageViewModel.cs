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

    private Command sortPropertiesCommand;

    private bool isRefreshing;

    private Location userLocation = new();
    private bool _isCheckingLocation;
    private CancellationTokenSource _cancelTokenSource;

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
    
    public ICommand SortPropertiesCommand => sortPropertiesCommand ??= new Command( async () => await SortProperties());

    private async Task GetPropertiesAsync()
    {
        List<Property> UnsortedProperties = new();
        await GetCurrentLocation();
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;

            var properties = service.GetProperties();

            if (PropertiesCollection.Count != 0)
                PropertiesCollection.Clear();

            foreach (var property in properties)
            {
                property.Distance = CalculateDistance(property);
                UnsortedProperties.Add(property);
            }
            var SortedProperties = UnsortedProperties.OrderByDescending(p => p.Distance).ToList();
            foreach (var property in SortedProperties)
            {
                PropertiesCollection.Add(new PropertyListItem(property));
            }
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

    private async Task SortProperties()
    {
        List<Property> UnsortedProperties = new();
        foreach (var property in PropertiesCollection)
        {
            UnsortedProperties.Add(property.Property);
        }
        if (PropertiesCollection.FirstOrDefault().Property.Distance > PropertiesCollection.Last().Property.Distance)
        {
            PropertiesCollection.Clear();
            var SortedProperties = UnsortedProperties.OrderBy(p => p.Distance).ToList();
            foreach (var property in SortedProperties)
            {
                PropertiesCollection.Add(new PropertyListItem(property));
            }
        }
        else
        {
            PropertiesCollection.Clear();
            var SortedProperties = UnsortedProperties.OrderByDescending(p => p.Distance).ToList();
            foreach (var property in SortedProperties)
            {
                PropertiesCollection.Add(new PropertyListItem(property));
            }
        }
    }

    private double CalculateDistance(Property property)
    {
        var propertyLocation = new Location((double)property.Latitude, (double)property.Longitude);
        return propertyLocation.CalculateDistance(userLocation, DistanceUnits.Kilometers);
    }
    
    public async Task GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            userLocation = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
        }

        catch (Exception ex)
        {
            GetCachedLocation();
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }
    
    private async Task<string> GetCachedLocation()
    {
        try
        {
            userLocation = await Geolocation.Default.GetLastKnownLocationAsync();
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
        }
        catch (Exception ex)
        {
            // Unable to get location
        }

        return "None";
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