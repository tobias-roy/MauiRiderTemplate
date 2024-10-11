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
    
    //Refactor this to use a list of pages and a dictionary to map the selected page to the route
    Dictionary<String, String> pages = new Dictionary<String, String>
    {
        {"About", "/about"},
        {"Calculator", "/calculator"},
        {"Color Grid", "/colorgrid"},
        {"Feedback", "/feedback"},
        {"Grid Cell", "/gridcell"},
        {"Tip Calculator", "/tipcalculator"},
        {"Restaurants", "/restaurants"},
        {"Rotating Text", "/rotatingtext"}
    };
    
    public async void OnSelectDefaultStartPageBtnClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Select default startpage", 
            "Cancel", 
            null, 
            pages.Keys.ToArray());

        Preferences.Set("DefaultStartPage", pages[action]);        
        if (action == "Cancel")
        {
            Preferences.Set("DefaultStartPage", "/tipcalculator");
        }
        
    }
}