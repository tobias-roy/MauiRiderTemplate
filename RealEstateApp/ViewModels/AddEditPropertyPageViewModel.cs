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

    public AddEditPropertyPageViewModel(IPropertyService service)
    {
        this.service = service;
        Agents = new ObservableCollection<Agent>(service.GetAgents());
    }

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