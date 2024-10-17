using System.Collections.ObjectModel;
using System.Windows.Input;
using RealEstateApp.Models;

namespace RealEstateApp.ViewModels;

public class HeightCalculatorViewModel : BaseViewModel
{
    private readonly double _seaLevelPressure = 1012.8;
    private Command _deleteMeasurementCommand;
    private Command _onDissapearingCommand;
    private Command _saveBaromenterMeasurementCommand;

    public HeightCalculatorViewModel()
    {
        InitiateBarometer();
    }

    public double CurrentPressure { get; set; }
    public double CurrentHeight { get; set; }
    public string SaveLabel { get; set; }
    public ObservableCollection<BarometerMeasurement> BarometerMeasurementCollection { get; set; } = new();

    public ICommand DeleteMeasurementCommand => _deleteMeasurementCommand ??=
        new Command<BarometerMeasurement>(measurement => { BarometerMeasurementCollection.Remove(measurement); });

    public ICommand OnDissapearingCommand => _onDissapearingCommand ??= new Command(() =>
    {
        if (Barometer.Default.IsMonitoring)
        {
            Barometer.Default.Stop();
            Barometer.Default.ReadingChanged -= Barometer_ReadingChanged;
        }
    });

    public ICommand SaveBaromenterMeasurementCommand => _saveBaromenterMeasurementCommand ??= new Command(() =>
    {
        BarometerMeasurementCollection.Add(new BarometerMeasurement
        {
            Pressure = CurrentPressure,
            Altitude = CurrentHeight,
            Label = SaveLabel,
            HeightChange = BarometerMeasurementCollection.FirstOrDefault() != null
                ? CurrentHeight - BarometerMeasurementCollection.Last().Altitude
                : 0
        });
        SaveLabel = string.Empty;
        OnPropertyChanged(nameof(SaveLabel));
    });

    private async void InitiateBarometer()
    {
        var status = await Permissions.RequestAsync<Permissions.Sensors>();
        if (status != PermissionStatus.Granted)
        {
            await Shell.Current.CurrentPage.DisplayAlert("Permission Denied",
                "The app needs permission to access the barometer", "OK");
            return;
        }

        if (Barometer.Default.IsSupported)
            if (!Barometer.Default.IsMonitoring)
            {
                Barometer.Default.ReadingChanged += Barometer_ReadingChanged;
                Barometer.Default.Start(SensorSpeed.UI);
            }
    }

    private void Barometer_ReadingChanged(object sender, BarometerChangedEventArgs e)
    {
        CurrentPressure = e.Reading.PressureInHectopascals;
        OnPropertyChanged(nameof(CurrentPressure));
        CurrentHeight = CalculateAltitudeInMeters();
        OnPropertyChanged(nameof(CurrentHeight));
    }

    private double CalculateAltitudeInMeters()
    {
        return 44307.694 * (1 - Math.Pow(CurrentPressure / _seaLevelPressure, 0.190284));
    }
}