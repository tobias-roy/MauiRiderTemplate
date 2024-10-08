
namespace TipCalculator;

public partial class GridTipCalculator : ContentPage
{
    private double _bill;
    private double _tipPercentage;
    private double _tipAmount;
    private double _total;
    private bool _roundedButtonClicked = false;
    
    public GridTipCalculator()
    {
        InitializeComponent();
        
        TipSlider.ValueChanged += CalculateTip;
        BillEntry.TextChanged += CalculateTip;
        LowPercentageBtn.Clicked += OnLowPercentageBtnClicked;
        HighPercentageBtn.Clicked += OnHighPercentageBtnClicked;
        RoundDownBtn.Clicked += OnRoundDownBtnClicked;
        RoundUpBtn.Clicked += OnRoundUpBtnClicked;
    }

    private void GetAndConvertBill()
    {
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
        _tipPercentage = Math.Round(TipSlider.Value);
        _tipAmount = Math.Round(_bill * TipSlider.Value / 100);
        _total = Math.Round(_bill + (_bill * TipSlider.Value / 100));
        OutputValues();
    }

    private void OnLowPercentageBtnClicked(object? sender, EventArgs e)
    {
        TipSlider.Value = 15;
        CalculateTip(sender, e);
    }
    
    private void OnHighPercentageBtnClicked(object? sender, EventArgs e)
    {
        TipSlider.Value = 20;
        CalculateTip(sender, e);
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