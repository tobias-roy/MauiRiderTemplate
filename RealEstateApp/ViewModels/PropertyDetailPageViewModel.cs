using System.Windows.Input;
using RealEstateApp.Models;
using RealEstateApp.Services;
using RealEstateApp.Views;

namespace RealEstateApp.ViewModels;

[QueryProperty(nameof(PropertyListItem), "MyPropertyListItem")]
public class PropertyDetailPageViewModel : BaseViewModel
{
    private readonly IPropertyService service;


    private Agent agent;

    private Command editPropertyCommand;

    private Property property;


    private PropertyListItem propertyListItem;

    public PropertyDetailPageViewModel(IPropertyService service)
    {
        this.service = service;
    }

    public Property Property
    {
        get => property;
        set => SetProperty(ref property, value);
    }

    public Agent Agent
    {
        get => agent;
        set => SetProperty(ref agent, value);
    }

    public PropertyListItem PropertyListItem
    {
        set
        {
            SetProperty(ref propertyListItem, value);

            Property = propertyListItem.Property;
            Agent = service.GetAgents().FirstOrDefault(x => x.Id == Property.AgentId);
        }
    }

    public ICommand EditPropertyCommand => editPropertyCommand ??= new Command(async () => await GotoEditProperty());

    private async Task GotoEditProperty()
    {
        await Shell.Current.GoToAsync($"{nameof(AddEditPropertyPage)}?mode=editproperty", true,
            new Dictionary<string, object>
            {
                { "MyProperty", property }
            });
    }
}