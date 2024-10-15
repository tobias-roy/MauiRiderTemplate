namespace TipCalculator.Views;

public partial class SettingsPage : ContentPage
{
    private double _defaultDefaultTipPercentage;

    //Refactor this to use a list of pages and a dictionary to map the selected page to the route
    private readonly Dictionary<string, string> pages = new()
    {
        { "About", "/about" },
        { "Calculator", "/calculator" },
        { "Color Grid", "/colorgrid" },
        { "Feedback", "/feedback" },
        { "Grid Cell", "/gridcell" },
        { "Tip Calculator", "/tipcalculator" },
        { "Restaurants", "/restaurants" },
        { "Rotating Text", "/rotatingtext" }
    };

    public SettingsPage()
    {
        _defaultDefaultTipPercentage = Preferences.Get("TipPercentage", 15);
        InitializeComponent();
        BindingContext = this;
        SelectDefaultStartPageBtn.Clicked += OnSelectDefaultStartPageBtnClicked;
    }

    public bool IsDarkModeOn { get; set; } = Application.Current.UserAppTheme == AppTheme.Dark;

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

    private void OnToggled(object? sender, ToggledEventArgs e)
    {
        if (e.Value)
            Application.Current.UserAppTheme = AppTheme.Dark;
        else
            Application.Current.UserAppTheme = AppTheme.Light;
    }

    public async void OnSelectDefaultStartPageBtnClicked(object sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Select default startpage",
            "Cancel",
            null,
            pages.Keys.ToArray());

        Preferences.Set("DefaultStartPage", pages[action]);
        if (action == "Cancel") Preferences.Set("DefaultStartPage", "/tipcalculator");
    }
}