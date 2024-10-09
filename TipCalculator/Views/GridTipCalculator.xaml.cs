
using System.Globalization;
using System.Text.RegularExpressions;
using TipCalculator.Models;

namespace TipCalculator.Views;

public partial class GridTipCalculator : ContentPage
{
    private bool _roundedButtonClicked = false;
    private int _cultureCounter = 0;
    private Tip _tip;
    
    public GridTipCalculator()
    {
        InitializeComponent();
        
        _tip = new Tip
        {
            BillAmount = "",
            TipAmount = "0.00",
            TotalAmount = "0.00",
            TipPct = 15
        };
        BindingContext = _tip;
        
        BillEntry.TextChanged += CheckUserBillInput;
        TipSlider.ValueChanged += (sender, e) => _tip.CalculateTip();
        LowPercentageBtn.Clicked += OnLowPercentageBtnClicked;
        HighPercentageBtn.Clicked += OnHighPercentageBtnClicked;
        RoundDownBtn.Clicked += OnRoundDownBtnClicked;
        RoundUpBtn.Clicked += OnRoundUpBtnClicked;
        CurrencyBtn.Clicked += ChangeCultureInfo;
        SelectCurrencyBtn.Clicked += SelectChangeCultureInfo;
    }

    private async void CheckUserBillInput(object? sender, EventArgs e)
    {
        if (!Regex.IsMatch(BillEntry.Text, "^[0-9]*$"))
        {
            BillEntry.Text = BillEntry.Text.Remove(BillEntry.Text.Length - 1, 1);
            await DisplayAlert("Error", "Only numbers are allowed", "OK");
        }
        _tip.CalculateTip();
    }
    private void ChangeCultureInfo(object? sender, EventArgs e)
    {
        switch (_cultureCounter)
        {
            case 0:
                ChangeCulture("de-DE");
                _cultureCounter++;
                break;
            case 1:
                ChangeCulture("en-US");
                _cultureCounter++;
                break;
            case 2:
                ChangeCulture("da-DK");
                _cultureCounter = 0;
                break;
            default:
                _cultureCounter = 0;
                break;
        }   
    }
    private async void SelectChangeCultureInfo(object? sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Select currency", "Cancel", null, "Danish Krone", "Euro", "US Dollar");
        switch (action)
        {
            case "Euro":
                ChangeCulture("de-DE");
                _cultureCounter++;
                break;
            case "US Dollar":
                ChangeCulture("en-US");
                _cultureCounter++;
                break;
            case "Danish Krone":
                ChangeCulture("da-DK");
                _cultureCounter = 0;
                break;
            case "Cancel":
                break;
        }   
        _tip.CalculateTip();
    }
    private void ChangeCulture(string culture)
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
        _tip.CalculateTip();
    }
    private async void OnLowPercentageBtnClicked(object? sender, EventArgs e)
    {
        TipSlider.Value = 15;
        _tip.CalculateTip();        
        await DisplayAlert("Fixed tip", "You have chosen a fixed tip of 15%", "OK");
    }
    private async void OnHighPercentageBtnClicked(object? sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Fixed tip", "You have chosen a fixed tip of 20% are you sure you want to apply this?", "Yes", "No");
        if (answer)
        {
            TipSlider.Value = 20;
            _tip.CalculateTip();        
        }
    }
    private void OnRoundDownBtnClicked(object? sender, EventArgs e)
    {
        _tip.CalculateRoundedTip(0);
    }
    private void OnRoundUpBtnClicked(object? sender, EventArgs e)
    {
        _tip.CalculateRoundedTip(1);

    }
}