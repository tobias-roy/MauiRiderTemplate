
using System.Globalization;

namespace TipCalculator.Views;

public partial class GridTipCalculator : ContentPage
{
    private double _bill;
    private double _tipPercentage;
    private double _tipAmount;
    private double _total;
    private bool _roundedButtonClicked = false;
    private int _cultureCounter = 0;
    
    public GridTipCalculator()
    {
        InitializeComponent();
        
        TipSlider.ValueChanged += CalculateTip;
        BillEntry.TextChanged += CalculateTip;
        LowPercentageBtn.Clicked += OnLowPercentageBtnClicked;
        HighPercentageBtn.Clicked += OnHighPercentageBtnClicked;
        RoundDownBtn.Clicked += OnRoundDownBtnClicked;
        RoundUpBtn.Clicked += OnRoundUpBtnClicked;
        CurrencyBtn.Clicked += ChangeCultureInfo;
        SelectCurrencyBtn.Clicked += SelectChangeCultureInfo;
    }

    private async void GetAndConvertBill()
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(BillEntry.Text, "^[0-9]*$"))
        {
            BillEntry.Text = BillEntry.Text.Remove(BillEntry.Text.Length - 1, 1);
            await DisplayAlert("Error", "Only numbers are allowed", "OK");
            return;
        }
        _bill = BillEntry.Text == string.Empty ? 0 : Convert.ToDouble(BillEntry.Text);
    }

    private void OutputValues()
    {
        TipPercentageLabel.Text = $"{_tipPercentage}%";
        TipAmountLabel.Text = $"{_tipAmount:C}";
        TotalLabel.Text = $"{_total:C}";
    }
    
    private void CalculateTip(object? sender, EventArgs e)
    {
        if(_roundedButtonClicked)
        {
            _roundedButtonClicked = false;
            return;
        }
        GetAndConvertBill();
        if(BillEntry.Text != string.Empty)
        {
            _tipPercentage = Math.Round(TipSlider.Value);
            _tipAmount = Math.Round(_bill * TipSlider.Value / 100);
            _total = Math.Round(_bill + (_bill * TipSlider.Value / 100));
            OutputValues();
        }
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
                OutputValues();
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
    }
    
    private void ChangeCulture(string culture)
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
        OutputValues();
    }

    private async void OnLowPercentageBtnClicked(object? sender, EventArgs e)
    {
        TipSlider.Value = 15;
        CalculateTip(sender, e);
        await DisplayAlert("Fixed tip", "You have chosen a fixed tip of 15%", "OK");
    }
    
    private async void OnHighPercentageBtnClicked(object? sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Fixed tip", "You have chosen a fixed tip of 20% are you sure you want to apply this?", "Yes", "No");
        if (answer)
        {
            TipSlider.Value = 20;
            CalculateTip(sender, e);
        }
    }
    
    private void OnRoundDownBtnClicked(object? sender, EventArgs e)
    {
        GetAndConvertBill();
        _tipAmount = Math.Round(_bill * TipSlider.Value / 100, MidpointRounding.AwayFromZero);
        _tipAmount = _tipAmount % 10 == 0 ? _tipAmount : Math.Floor(_tipAmount / 10) * 10;
        _roundedButtonClicked = true;
        TipSlider.Value = Math.Round(_tipAmount / _bill * 100);
        OutputValues();
    }
    
    private void OnRoundUpBtnClicked(object? sender, EventArgs e)
    {
        GetAndConvertBill();
        _tipAmount = Math.Round(_bill * TipSlider.Value / 100, MidpointRounding.AwayFromZero);
        _tipAmount = _tipAmount % 10 == 0 ? _tipAmount : Math.Ceiling(_tipAmount / 10) * 10;
        _roundedButtonClicked = true;
        TipSlider.Value = Math.Round(_tipAmount / _bill * 100);
        OutputValues();
    }
}