using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TipCalculator.Models;

public class Restaurant : INotifyPropertyChanged
{
    private string? _name;
    public string? Name { get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }
    
    private string? _tipPct;
    public string? TipPct { get => _tipPct;
        set
        {
            _tipPct = value;
            OnPropertyChanged();
        }
    }
    
    private string? _imageUrl;
    public string? ImageUrl { get => _imageUrl;
        set
        {
            _imageUrl = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}