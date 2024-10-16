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
    private Command playDescriptionCommand;
    private Command stopPlayDescriptionCommand;
    private Property property;
    private PropertyListItem propertyListItem;
    public bool CanPlay { get; set; } = true;
    public bool CanStopPlay { get; set; } = false;
    CancellationTokenSource cts;
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
    
    public ICommand PlayDescriptionCommand => playDescriptionCommand ??= new Command(async () => await PlayDescriptionTextToSpeech());
    private async Task PlayDescriptionTextToSpeech()
    {
        cts = new CancellationTokenSource();
        CanPlay = false;
        CanStopPlay = true;
        OnPropertyChanged(nameof(CanPlay));
        OnPropertyChanged(nameof(CanStopPlay));
        
        IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();
        SpeechOptions options = new SpeechOptions()
        {
            Locale = locales.Where(locales => locales.Language == "en-IN").FirstOrDefault(),
        };
        
        await TextToSpeech.Default.SpeakAsync(Property.Description, options, cancelToken: cts.Token);
        
        CanPlay = true;
        CanStopPlay = false;
        OnPropertyChanged(nameof(CanPlay));
        OnPropertyChanged(nameof(CanStopPlay));
    }
    
    public ICommand StopPlayDescriptionCommand => stopPlayDescriptionCommand ??= new Command(CancelSpeech);
    public void CancelSpeech()
    {
        CanPlay = true;
        CanStopPlay = false;
        OnPropertyChanged(nameof(CanPlay));
        OnPropertyChanged(nameof(CanStopPlay));
        
        if (cts?.IsCancellationRequested ?? true)
            return;
    
        cts.Cancel();
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