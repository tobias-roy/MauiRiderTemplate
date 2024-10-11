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
        SelectDefaultStartPageBtn.Clicked += OnSelectDefaultStartPageBtnClicked;
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
    
    public async void OnSelectDefaultStartPageBtnClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Select default startpage", 
            "Cancel", 
            null, 
        "About", 
            "Calculator", 
            "Color Grid", 
            "Feedback", 
            "Grid Cell", 
            "Tip Calculator", 
            "Restaurants", 
            "Rotating Text");
        switch (action)
        {
            case "About":
                Preferences.Set("DefaultStartPage", "/about");
                break;
            case "Calculator":
                Preferences.Set("DefaultStartPage", "/calculator");
                break;
            case "Color Grid":
                Preferences.Set("DefaultStartPage", "/colorgrid");
                break;
            case "Feedback":
                Preferences.Set("DefaultStartPage", "/feedback");
                break;
            case "Grid Cell":
                Preferences.Set("DefaultStartPage", "/gridcell");
                break;
            case "Tip Calculator":
                Preferences.Set("DefaultStartPage", "/tipcalculator");
                break;
            case "Restaurants":
                Preferences.Set("DefaultStartPage", "/restaurants");
                break;
            case "Rotating Text":
                Preferences.Set("DefaultStartPage", "/rotatingtext");
                break;
        }
        if (action == "Cancel")
        {
            Preferences.Set("DefaultStartPage", "/tipcalculator");
            
        }
        
    }
}