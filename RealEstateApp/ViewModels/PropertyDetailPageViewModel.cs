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
    private CancellationTokenSource cts;
    private Command editPropertyCommand;
    private Command onEmailTappedCommand;
    private Command onPhoneTappedCommand;
    private Command playDescriptionCommand;
    private Property property;
    private PropertyListItem propertyListItem;
    private Command stopPlayDescriptionCommand;

    public PropertyDetailPageViewModel(IPropertyService service)
    {
        this.service = service;
    }

    public bool CanPlay { get; set; } = true;
    public bool CanStopPlay { get; set; }

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

    public ICommand PlayDescriptionCommand =>
        playDescriptionCommand ??= new Command(async () => await PlayDescriptionTextToSpeech());

    public ICommand StopPlayDescriptionCommand => stopPlayDescriptionCommand ??= new Command(CancelSpeech);

    public ICommand EditPropertyCommand => editPropertyCommand ??= new Command(async () => await GotoEditProperty());

    public ICommand OnEmailTappedCommand => onEmailTappedCommand ??= new Command(async () => await OnEmailTapped());
    public ICommand OnPhoneTappedCommand => onPhoneTappedCommand ??= new Command(async () => await OnPhoneTapped());

    private async Task PlayDescriptionTextToSpeech()
    {
        cts = new CancellationTokenSource();
        CanPlay = false;
        CanStopPlay = true;
        OnPropertyChanged(nameof(CanPlay));
        OnPropertyChanged(nameof(CanStopPlay));

        var locales = await TextToSpeech.Default.GetLocalesAsync();
        var options = new SpeechOptions
        {
            Locale = locales.Where(locales => locales.Language == "en-IN").FirstOrDefault()
        };

        await TextToSpeech.Default.SpeakAsync(Property.Description, options, cts.Token);

        CanPlay = true;
        CanStopPlay = false;
        OnPropertyChanged(nameof(CanPlay));
        OnPropertyChanged(nameof(CanStopPlay));
    }

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

    private async Task GotoEditProperty()
    {
        await Shell.Current.GoToAsync($"{nameof(AddEditPropertyPage)}?mode=editproperty", true,
            new Dictionary<string, object>
            {
                { "MyProperty", property }
            });
    }

    private async Task OnEmailTapped()
    {
        if (Email.Default.IsComposeSupported)
        {
            var subject = "Hello friends!";
            var body = "It was great to see you last weekend.";
            string[] recipients = { property.Vendor.Email };

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                BodyFormat = EmailBodyFormat.PlainText,
                To = new List<string>(recipients)
            };

            await Email.Default.ComposeAsync(message);
        }
    }

    private async Task OnPhoneTapped()
    {
        try
        {
            PhoneDialer.Open($"{property.Vendor.Phone}");
        }
        catch (FeatureNotSupportedException ex)
        {
            await Shell.Current.CurrentPage.DisplayAlert("Error", "Phone dialer is not supported on this device", "OK");
        }
        catch (Exception ex)
        {
        }
    }
}