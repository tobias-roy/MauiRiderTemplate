using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace TipCalculator.Models;

public class Tip : INotifyPropertyChanged
{
    private string? _billAmount;

    private string? _tipAmount;

    private double? _tipPct;

    private string? _totalAmount;

    public string? BillAmount
    {
        get => _billAmount;
        set
        {
            _billAmount = value;
            OnPropertyChanged();
        }
    }

    public string? TipAmount
    {
        get => _tipAmount;
        set
        {
            _tipAmount = value;
            OnPropertyChanged();
        }
    }

    public string? TotalAmount
    {
        get => _totalAmount;
        set
        {
            _totalAmount = value;
            OnPropertyChanged();
        }
    }

    public double? TipPct
    {
        get => _tipPct;
        set
        {
            _tipPct = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private string SanitizeTipString()
    {
        return Regex.Replace(TipAmount, "[a-z]+.|([$€])", "");
    }

    public void CalculateTip()
    {
        if (BillAmount == null) return;
        if (BillAmount == string.Empty) return;
        TipAmount = (decimal.Parse(BillAmount) * (decimal)TipPct / 100).ToString("C",
            CultureInfo.DefaultThreadCurrentCulture);
        TotalAmount =
            (decimal.Parse(BillAmount) + decimal.Parse(BillAmount) * (decimal)TipPct / 100).ToString("C",
                CultureInfo.DefaultThreadCurrentCulture);
    }

    public void CalculateRoundedTip(int direction)
    {
        var _tipString = SanitizeTipString();
        var _tipAmount = double.Parse(_tipString);
        _tipAmount = direction == 0 ? Math.Floor(_tipAmount / 10) * 10 : Math.Ceiling(_tipAmount / 10) * 10;
        TipAmount = _tipAmount.ToString("C", CultureInfo.DefaultThreadCurrentCulture);
        TotalAmount =
            (decimal.Parse(BillAmount != null ? BillAmount : "0") + (decimal)_tipAmount).ToString("C",
                CultureInfo.DefaultThreadCurrentCulture);
        TipPct = _tipAmount / double.Parse(BillAmount != null ? BillAmount : "0") * 100;
    }

    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}