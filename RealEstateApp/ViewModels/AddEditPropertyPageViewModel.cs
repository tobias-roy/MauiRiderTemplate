using System.Collections.ObjectModel;
using System.Windows.Input;
using RealEstateApp.Models;
using RealEstateApp.Services;
using RealEstateApp.Views;

namespace RealEstateApp.ViewModels;

[QueryProperty(nameof(Mode), "mode")]
[QueryProperty(nameof(Property), "MyProperty")]
public class AddEditPropertyPageViewModel : BaseViewModel
{
    private readonly IPropertyService service;
    private Command cancelSaveCommand;
    private Command savePropertyCommand;
    private Command getCurrentLocationCommand;
    private Command getLocationFromAddressCommand;
    private Command checkConnectivityCommand;
    private Command setConnectivityCommand;
    private Command activateFlashlightCommand;
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;
    private bool _flashLightOn;
    public Location UserLocation { get; } = new();
    public bool HasService { get; set; }
    public string Mode { get; set; }

    public AddEditPropertyPageViewModel(IPropertyService service)
    {
        this.service = service;
        Agents = new ObservableCollection<Agent>(service.GetAgents());
        Battery.Default.BatteryInfoChanged += OnBatteryInfoChanged;
    }
    
    private void OnBatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
    {
        if (e.ChargeLevel >= 20)
        {
            StatusMessage = "Battery level is low";
            StatusColor = Colors.Red;
        }
        if(e.PowerSource == BatteryPowerSource.AC)
        {
            StatusMessage = "Battery level is low - charging";
            StatusColor = Colors.Yellow;
        }

        if (Battery.Default.EnergySaverStatus == EnergySaverStatus.On)
        {
            StatusMessage = "Energy saver is on";
            StatusColor = Colors.Green;
        }
    }
    
    public ICommand SavePropertyCommand => savePropertyCommand ??= new Command(async () => await SaveProperty());
    public ICommand CancelSaveCommand =>
        cancelSaveCommand ??= new Command(async () => await Shell.Current.GoToAsync(".."));
    private async Task SaveProperty()
    {
        if (IsValid() == false)
        {
            
            StatusMessage = "Please fill in all required fields";
            StatusColor = Colors.Red;
            HapticResponse();
        }
        else
        {
            service.SaveProperty(Property);
            await Shell.Current.GoToAsync("///propertylist");
        }
    }
    bool IsValid()
    {
        if (string.IsNullOrEmpty(Property.Address)
            || Property.Beds == null
            || Property.Price == null
            || Property.AgentId == null)
            return false;
        return true;
    }
    void HapticResponse()
    {
        Vibration.Default.Vibrate();
    }
    public ICommand GetLocationFromAddress => getLocationFromAddressCommand ??= new Command(async () => await GetGpsLocationFromAddress());
    async Task GetGpsLocationFromAddress()
    {
        try
        {
            string address = Property.Address;
            if(address == null || address.Length < 3)
            {
                await Shell.Current.CurrentPage.DisplayAlert("Wrong address", "Please provide a valid address", "OK");
            }

            IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(address);
            Location location = locations?.FirstOrDefault();
            if (location != null)
            {
                UserLocation.Latitude = location.Latitude;
                UserLocation.Longitude = location.Longitude;
                OnPropertyChanged(nameof(UserLocation));
            }
        }

        catch (Exception ex)
        {
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }
    async Task<string> GetCachedLocation()
    {
        try
        {
            var location = await Geolocation.Default.GetLastKnownLocationAsync();
            if (location != null)
            {
                UserLocation.Latitude = location.Latitude;
                UserLocation.Longitude = location.Longitude;
                OnPropertyChanged(nameof(UserLocation));
            }
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
    public ICommand GetCurrentLocationCommand => getCurrentLocationCommand ??= new Command(async () => await GetCurrentLocation());
    async Task GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
            {
                UserLocation.Latitude = location.Latitude;
                UserLocation.Longitude = location.Longitude;
                IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);
                Property.Address =
                    placemarks?.FirstOrDefault()?.FeatureName +
                    ", " +
                    placemarks?.FirstOrDefault()?.PostalCode +
                    " " +
                    placemarks?.FirstOrDefault()?.Locality;
                OnPropertyChanged(nameof(Property));
                OnPropertyChanged(nameof(UserLocation));
            }
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
    public ICommand CheckConnectivityCommand => checkConnectivityCommand ??= new Command(async () => await CheckConnectivity());
    async Task CheckConnectivity()
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.CurrentPage.DisplayAlert("No internet", "Please check your internet connection", "OK");
        }
    }
    public ICommand SetConnectivityCommand => setConnectivityCommand ??= new Command(async () => await SetConnectivity());
    async Task SetConnectivity()
    {
        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            HasService = false;
            OnPropertyChanged(nameof(HasService));
        }
        else
        {
            HasService = true;
            OnPropertyChanged(nameof(HasService));
        }
    }
    public ICommand ActivateFlashlightCommand => activateFlashlightCommand ??= new Command(async () => await ActivateFlashlight());
    async Task ActivateFlashlight()
    {
        if (!_flashLightOn)
        {
            try
            {
                await Flashlight.TurnOnAsync();
                _flashLightOn = true;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to turn on flashlight
            }
        }
        else
        {
            try
            {
                await Flashlight.TurnOffAsync();
                _flashLightOn = false;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to turn on flashlight
            }
        }
    }
    #region PROPERTIES

    public ObservableCollection<Agent> Agents { get; }
    private Property _property;

    public Property Property
    {
        get => _property;
        set
        {
            SetProperty(ref _property, value);
            Title = Mode == "newproperty" ? "Add Property" : "Edit Property";

            if (_property.AgentId != null) SelectedAgent = Agents.FirstOrDefault(x => x.Id == _property?.AgentId);
        }
    }

    private Agent _selectedAgent;

    public Agent SelectedAgent
    {
        get => _selectedAgent;
        set
        {
            if (Property != null)
            {
                _selectedAgent = value;
                Property.AgentId = _selectedAgent?.Id;
            }
        }
    }

    private string statusMessage;

    public string StatusMessage
    {
        get => statusMessage;
        set => SetProperty(ref statusMessage, value);
    }

    private Color statusColor;

    public Color StatusColor
    {
        get => statusColor;
        set => SetProperty(ref statusColor, value);
    }

    #endregion
}