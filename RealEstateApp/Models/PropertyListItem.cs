using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RealEstateApp.Models;

public class PropertyListItem : INotifyPropertyChanged
{
    private Property _property;

    public PropertyListItem(Property property)
    {
        Property = property;
    }

    public Property Property
    {
        get => _property;
        set
        {
            _property = value;
            OnPropertyChanged();
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}