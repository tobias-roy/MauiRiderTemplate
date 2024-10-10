using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipCalculator.Views;

public partial class SettingsPage : ContentPage
{
    public bool IsDarkModeOn { get; set; } = Application.Current.UserAppTheme == AppTheme.Dark;
    private Double _defaultDefaultTipPercentage;
    public double DefaultTipPercentage
    {
        get => _defaultDefaultTipPercentage;
        set
        {
            _defaultDefaultTipPercentage = value;
            OnPropertyChanged();
            Preferences.Set("TipPercentage", value);
        }
    }   
    
    public SettingsPage()
    {
        _defaultDefaultTipPercentage = Preferences.Get("TipPercentage", 15);
        InitializeComponent();
        BindingContext = this;
    }

    private void OnToggled(object? sender, ToggledEventArgs e)
    {
        if(e.Value)
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
        }
        else
        {
            Application.Current.UserAppTheme = AppTheme.Light;
        }
    }
}