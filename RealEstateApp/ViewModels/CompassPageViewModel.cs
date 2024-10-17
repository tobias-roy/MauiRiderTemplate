using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using RealEstateApp.Models;
using RealEstateApp.Services;
using RealEstateApp.Views;
using Switch = Microsoft.Maui.Controls.Switch;

namespace RealEstateApp.ViewModels;

[QueryProperty(nameof(Property), nameof(Property))]
public class CompassPageViewModel : BaseViewModel, INotifyPropertyChanged
{
    private Property _property;
    public Property Property
    {
        get => _property;
        set => SetProperty(ref _property, value);
    }

    public string CurrentAspect { get; set; }
    public double RotationAngle { get; set; }
    public double CurrentHeading { get; set; }

    private Command backCommand;
    private Command onDissapearingCommand;
    public ICommand BackCommand => backCommand ??= new Command(async () => await Shell.Current.GoToAsync(".."));
    
    public CompassPageViewModel()
    {
        Compass.Default.ReadingChanged += Compass_ReadingChanged;
        ToggleCompass();
    }
    
    private void ToggleCompass()
    {
        if (Compass.Default.IsSupported)
        {
            if (!Compass.Default.IsMonitoring)
            {
                Compass.Default.ReadingChanged += Compass_ReadingChanged;
                Compass.Default.Start(SensorSpeed.UI);
            }
        }
        OnPropertyChanged(nameof(Property));
    }

    public ICommand OnDissapearingCommand => onDissapearingCommand ??= new Command(async () => await OnDissapearing());
    async Task OnDissapearing()
    {
        if (Compass.Default.IsSupported)
        {
            if (Compass.Default.IsMonitoring)
            {
                Compass.Default.ReadingChanged -= Compass_ReadingChanged;
                Compass.Default.Stop();
            }
        }
        OnPropertyChanged(nameof(Property));
    }

    private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
    {
        CurrentHeading = e.Reading.HeadingMagneticNorth;
        RotationAngle = -e.Reading.HeadingMagneticNorth;
        Property.Aspect = CurrentAspect = CurrentHeading switch
        {
            >= 0 and < 45 or >= 315 and <= 360 => "North", // Dækker både 0-45 og 315-360 grader
            >= 45 and < 135 => "East",                      // Dækker 45-135 grader
            >= 135 and < 225 => "South",                    // Dækker 135-225 grader
            >= 225 and < 315 => "West",                     // Dækker 225-315 grader
            _ => "Unknown"                                  // Fallback i tilfælde af at intet matcher
        };
        OnPropertyChanged(nameof(CurrentHeading));
        OnPropertyChanged(nameof(RotationAngle));
        OnPropertyChanged(nameof(CurrentAspect));
    }
}