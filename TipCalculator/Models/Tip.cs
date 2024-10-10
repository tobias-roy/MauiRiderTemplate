using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace TipCalculator.Models;

public class Tip : INotifyPropertyChanged
{
    private string? _billAmount;
    public string? BillAmount { get => _billAmount;
        set
        {
            _billAmount = value;
            OnPropertyChanged();
        }
    }
    
    private string? _tipAmount;
    public string? TipAmount { get => _tipAmount;
        set
        {
            _tipAmount = value;
            OnPropertyChanged();
        }
    }
    
    private string? _totalAmount;
    public string? TotalAmount { get => _totalAmount;
        set
        {
            _totalAmount = value;
            OnPropertyChanged();
        }
    }
    
    private double? _tipPct;
    public double? TipPct { get => _tipPct;
        set
        {
            _tipPct = value;
            OnPropertyChanged();
        }
    }

    private string SanitizeTipString()
    {
       return Regex.Replace(TipAmount, "[a-z]+.|([$€])", "");
    }
    
    public void CalculateTip()
    {
        if(BillAmount == null) return;
        if(BillAmount == String.Empty) return;
        TipAmount = (decimal.Parse(BillAmount) * (decimal)TipPct / 100).ToString("C", CultureInfo.DefaultThreadCurrentCulture);
        TotalAmount = (decimal.Parse(BillAmount) + (decimal.Parse(BillAmount) * (decimal)TipPct / 100)).ToString("C", CultureInfo.DefaultThreadCurrentCulture);
    }

    public void CalculateRoundedTip(int direction)
    {
        var _tipString = SanitizeTipString();
        var _tipAmount = double.Parse(_tipString);
        _tipAmount = direction == 0 ? Math.Floor(_tipAmount / 10) * 10 : Math.Ceiling(_tipAmount / 10) * 10;
        TipAmount = _tipAmount.ToString("C", CultureInfo.DefaultThreadCurrentCulture);
        TotalAmount = (decimal.Parse(BillAmount != null ? BillAmount : "0") + (decimal)_tipAmount).ToString("C", CultureInfo.DefaultThreadCurrentCulture);
        TipPct = _tipAmount / double.Parse(BillAmount != null ? BillAmount : "0") * 100;
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}