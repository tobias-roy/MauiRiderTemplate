using System.Collections.ObjectModel;
using System.Windows.Input;
using RealEstateApp.Models;
using RealEstateApp.Services;

namespace RealEstateApp.ViewModels;

[QueryProperty(nameof(Mode), "mode")]
[QueryProperty(nameof(Property), "MyProperty")]
public class AddEditPropertyPageViewModel : BaseViewModel
{
    private readonly IPropertyService service;
    private Command cancelSaveCommand;
    private Command savePropertyCommand;
    private Command getCurrentLocationCommand;
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;

    public AddEditPropertyPageViewModel(IPropertyService service)
    {
        this.service = service;
        Agents = new ObservableCollection<Agent>(service.GetAgents());
    }

    public Location UserLocation { get; } = new();

    public string Mode { get; set; }
    public ICommand SavePropertyCommand => savePropertyCommand ??= new Command(async () => await SaveProperty());
    public ICommand CancelSaveCommand =>
        cancelSaveCommand ??= new Command(async () => await Shell.Current.GoToAsync(".."));

    private async Task SaveProperty()
    {
        if (IsValid() == false)
        {
            StatusMessage = "Please fill in all required fields";
            StatusColor = Colors.Red;
        }
        else
        {
            service.SaveProperty(Property);
            await Shell.Current.GoToAsync("///propertylist");
        }
    }

    public bool IsValid()
    {
        if (string.IsNullOrEmpty(Property.Address)
            || Property.Beds == null
            || Property.Price == null
            || Property.AgentId == null)
            return false;
        return true;
    }
    

    public ICommand GetCurrentLocationCommand => getCurrentLocationCommand ??= new Command(async () => await GetCurrentLocation());
    public async Task GetCurrentLocation()
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
    
    public async Task<string> GetCachedLocation()
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

    public void CancelRequest()
    {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
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